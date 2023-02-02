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
        private void TransferParamRow(PARAM.Row row_old, PARAM.Row row_new)
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
        private void OffsetParamRow(PARAM.Row row_old, PARAM.Row row_new, PARAM.Row row_vanilla)
        {
            for (var iField = 0; iField < row_old.Cells.Count; iField++)
            {
                var oldCell = row_old.Cells[iField];
                var newCell = row_new.Cells[iField];
                var vanillaCell = row_vanilla.Cells[iField];

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
                dynamic offsetVal = (dynamic)oldCell.Value - (dynamic)vanillaCell.Value;

                /*
                if (offsetVal != 0)
                    Debugger.Break();
                */

                newCell.Value = (dynamic)newCell.Value + offsetVal;
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

            public ScaledObject(string modelName, Vector3 scaling)
            {
                OGModelName = modelName;
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
            BND3 oldBND = BND3.Read(oldpath);

            ChangeBNDFileNames(oldBND, scaledObj.OGModelName, scaledObj.NewModelName);

            Util.WritePortedSoulsFile(oldBND, DataPath_DSR, newpath, CompressionType);

            // TODO: HKX stuff
            // Also need to create an objParam entry

            return scaledObj;
        }

        private bool IsCoreFFX(BinderFile binder)
        {
            // Some FFX are super important and cannot be changed without breaking the game.
            string fileName = Path.GetFileNameWithoutExtension(binder.Name);
            var id = long.Parse(string.Join("", fileName.Where(c => char.IsDigit(c))));
            if (id <= 2999 && id >= 2000)
                return true;
            return false;
        }

        public Dictionary<string, List<string>> LoadResource_MSBExceptions()
        {
            string[] file = File.ReadAllLines($@"{Directory.GetCurrentDirectory()}\Resources\MSB_Scaled_Obj_Exceptions.txt");
            Dictionary<string, List<string>> output = new();
            foreach (var line in file)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("//"))
                    continue;
                string map = "";
                string name = "";
                try
                {
                    var split = line.Split("||");
                    map = split[0];
                    name = split[1];
                }
                catch
                {
                    throw new Exception($"\"Resources\\MSB Exceptions.txt\" has invalid formatting and cannot be used.");
                }
                output.TryAdd(map, new List<string>());
                output[map].Add(name);
            }
            return output;
        }
    }
}
