﻿using SoulsFormats;
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
                if (!Util.ScalingIsEqual(Scaling, scaling))
                    return false;
                return true;
            }
        }

        public ScaledObject CreateScaledObject(string modelName, Vector3 scale)
        {
            ScaledObject scaledObj = new(modelName, scale);

            string[] objbndPaths = Directory.GetFiles($@"{_dataPath_DSR}\obj", "*.objbnd.dcx");

            string newObjName;
            int id = scaledObj.OGModelID;
            do
            {
                id++;
                newObjName = $"o{id}";
            }
            while (objbndPaths.Contains($@"{_dataPath_DSR}\obj\{newObjName}.objbnd.dcx"));

            scaledObj.NewModelName = newObjName;

            var oldpath = $@"{_dataPath_DSR}\obj\{scaledObj.OGModelName}.objbnd.dcx";
            var newpath = $@"{_dataPath_DSR}\obj\{scaledObj.NewModelName}.objbnd.dcx";
            BND3 oldBND = BND3.Read(oldpath);

            ChangeBNDFileNames(oldBND, scaledObj.OGModelName, scaledObj.NewModelName);

            Util.WritePortedSoulsFile(oldBND, _dataPath_DSR, newpath, _compressionType);

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
    }
}