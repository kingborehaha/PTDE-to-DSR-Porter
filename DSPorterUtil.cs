using SoulsFormats;
using SoulsFormatsExtensions;
using DSPorterUtil;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Numerics;

namespace DSRPorter
{
    public partial class DSPorter
    {
        private static void TransferParamRow(PARAM.Row row_old, PARAM.Row row_new)
        {
            for (var iField = 0; iField < row_old.Cells.Count; iField++)
            {
                var oldCell = row_old.Cells[iField];
                var newCell = row_new.Cells[iField];

                if (oldCell.Def.DisplayType == PARAMDEF.DefType.dummy8)
                {
                    // Skip over any padding.
                    continue;
                }

                if (oldCell.Def.InternalName != newCell.Def.InternalName)
                {
                    // Fields don't match, this is a mixed-def check. Try to find correct field to compare (if it exists)
                    newCell = row_new.Cells.FirstOrDefault(c => c.Def.InternalName == oldCell.Def.InternalName);
                    if (newCell == null)
                    {
                        throw new Exception($"Couldn't find {oldCell.Def.InternalName} in new paramdef. Modify Paramdef field names to fix.");
                    }
                }
                newCell.Value = oldCell.Value;
            }
            return;
        }

        private static void OffsetDrawParamRow(PARAM.Row row_ptde_modded, PARAM.Row row_dsr, PARAM.Row row_vanilla)
        {
            for (var iField = 0; iField < row_ptde_modded.Cells.Count; iField++)
            {
                var ptdeVanillaCell = row_vanilla.Cells[iField];
                var ptdeModdedCell = row_ptde_modded.Cells[iField];
                var dsrCell = row_dsr.Cells[iField];

                if (dsrCell.Def.DisplayType == PARAMDEF.DefType.dummy8)
                {
                    // Skip over any padding.
                    continue;
                }

                if (ptdeModdedCell.Def.InternalName != dsrCell.Def.InternalName)
                {
                    // Fields don't match, this is a mixed-def check. Try to find correct field to compare (if it exists)
                    dsrCell = row_dsr.Cells.FirstOrDefault(c => c.Def.InternalName == ptdeModdedCell.Def.InternalName);
                    if (dsrCell == null)
                    {
                        throw new Exception($"Couldn't find {ptdeModdedCell.Def.InternalName} in new paramdef. Modify Paramdef field names to fix.");
                    }
                }

                if ((dynamic)ptdeModdedCell.Value != (dynamic)ptdeVanillaCell.Value)
                {
                    dynamic dsrVal = dsrCell.Value;
                    dynamic vanillaVal = ptdeVanillaCell.Value;
                    dynamic moddedVal = ptdeModdedCell.Value;

                    if (DSPorterSettings.Is_SOTE && dsrCell.Def.InternalName.ToLower().Contains("dwindle"))
                    {
                        dsrCell.Value = moddedVal;
                        continue;
                    }

                    dynamic offsetMult;
                    if (vanillaVal == 0)
                    {
                        if (dsrVal == 0)
                        {
                            offsetMult = 1;
                        }
                        else
                        {
                            dynamic offsetAdd = dsrVal - vanillaVal;
                            dsrCell.Value = moddedVal + offsetAdd;
                            continue;
                        }
                    }
                    else
                    {
                        offsetMult = dsrVal / vanillaVal;
                    }

                    dsrCell.Value = (dynamic)(moddedVal * offsetMult);
                    if (offsetMult != 1)
                    {
                        { }
                    }

                }
            }
            return;
        }

        public void ChangeBNDFileNames(BND3 bnd, string oldStr, string newStr)
        {
            for (var i = 0; i < bnd.Files.Count; i++)
            {
                var file = bnd.Files[i];
                if (BND3.Is(file.Bytes))
                {
                    BND3 newfile = BND3.Read(file.Bytes);
                    ChangeBNDFileNames(newfile, oldStr, newStr);
                    file.Bytes = newfile.Write();
                }
                file.Name = file.Name.Replace(oldStr, newStr);
            }
        }

        public class ScaledObject
        {
            public string OGModelName = "";
            public string NewModelName = "";
            public Vector3 Scaling;
            public int OGModelID => int.Parse(OGModelName.Replace("o", ""));
            public int NewModelID => int.Parse(NewModelName.Replace("o", ""));

            public ScaledObject(string oldModelName, Vector3 scaling)
            {
                OGModelName = oldModelName;
                Scaling = scaling;
            }
            public ScaledObject(string oldModelName, string newModelName, Vector3 scaling)
            {
                OGModelName = oldModelName;
                NewModelName = newModelName;
                Scaling = scaling;
            }

            public bool Matches(string modelName, Vector3 scaling)
            {
                if (OGModelName != modelName)
                    return false;
                if (!Util.Vector3IsEqual(Scaling, scaling))
                    return false;
                return true;
            }
        }

        public ScaledObject CreateScaledObject(string modelName, Vector3 scale)
        {
            ScaledObject scaledObj = new(modelName, scale);

            string[] objbndPaths = Directory.GetFiles($@"{DataPath_DSR}\obj", "*.objbnd.dcx");

            string newObjName;
            int id = scaledObj.OGModelID;
            do
            {
                id++;
                newObjName = $"o{id}";
            }
            while (objbndPaths.Contains($@"{DataPath_DSR}\obj\{newObjName}.objbnd.dcx"));

            scaledObj.NewModelName = newObjName;

            var oldpath = $@"{DataPath_DSR}\obj\{scaledObj.OGModelName}.objbnd.dcx";
            var newpath = $@"{DataPath_DSR}\obj\{scaledObj.NewModelName}.objbnd.dcx";
            BND3 bnd = BND3.Read(oldpath);

            foreach (var file in bnd.Files)
            {
                if (FLVER2.Is(file.Bytes))
                {
                    FLVER2 flver = FLVER2.Read(file.Bytes);
                    foreach (var bone in flver.Bones)
                    {
                        bone.Scale = scale;
                    }
                    file.Bytes = flver.Write();
                }
            }

            ChangeBNDFileNames(bnd, scaledObj.OGModelName, scaledObj.NewModelName);

            Util.WritePortedSoulsFile(bnd, DataPath_DSR, newpath, CompressionType);

            return scaledObj;
        }

        public bool IsCoreFFX(BinderFile binder)
        {
            // Some FFX are super important and cannot be changed without breaking the game.
            var id = GetFileIdFromName(binder.Name);
            if (id <= 2999 && id >= 2000)
                return true;
            return false;
        }

        public long GetFileIdFromName(string name)
        {
            string fileName = Path.GetFileNameWithoutExtension(name);
            long id = long.Parse(string.Join("", fileName.Where(c => char.IsDigit(c))));
            return id;
        }

        public Dictionary<string, List<string>> LoadTextResource_MsbScaledObjs()
        {
            List<string[]> resources = Util.LoadTextResource($@"{Directory.GetCurrentDirectory()}\Resources\MSB Scaled Object Whitelist.txt", 2);
            Dictionary<string, List<string>> output = new();
            foreach (var resource in resources)
            {
                var map = resource[0];
                var name = resource[1];
                output.TryAdd(map, new List<string>());
                output[map].Add(name);
            }
            return output;
        }

        /// <summary>
        /// Compares two files from two provided paths and calculates if they are different.
        /// </summary>
        /// <returns>True if files are identical, false otherwise.</returns>
        public static bool CompareFiles(string filePath1, string filePath2)
        {
            FileInfo fInfo1 = new(filePath1);
            FileInfo fInfo2 = new(filePath2);
            if (fInfo1.Length != fInfo2.Length)
                return false;

            byte[] b1 = File.ReadAllBytes(fInfo1.FullName);
            byte[] b2 = File.ReadAllBytes(fInfo2.FullName);

            return b1.SequenceEqual(b2);
        }
    }
}
