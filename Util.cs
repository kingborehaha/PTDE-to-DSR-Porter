using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using SoulsFormats;

namespace DSPorterUtil
{
    public static class Util
    {
        public static void WritePortedSoulsFile(ISoulsFile file, string datapath, string path, DCX.Type dcxType = DCX.Type.None)
        {
            file.Write(Util.GetOutputPath(datapath, path, dcxType != DCX.Type.None), dcxType);
        }
        public static string GetVirtualPath(string dataPath, string filePath)
        {
            return Path.Join(filePath.Split(dataPath)[1].Split("\\")[..^1]);
        }
        public static string GetOutputPath(string dataPath, string filePath, bool isDCX = true)
        {
            string outputDirectory = $"output\\{GetVirtualPath(dataPath, filePath)}";
            string fileName = Path.GetFileName(filePath);
            string outputPath = $"{outputDirectory}\\{fileName}";
            Directory.CreateDirectory(outputDirectory);
            if (isDCX && !outputPath.EndsWith(".dcx"))
            {
                outputPath += ".dcx";
            }
            return outputPath;
        }
        public static string GetByteArrayString(byte[] field)
        {
            string bytestr = "";
            for (var i = 0; i < field.Length; i++)
            {
                bytestr += field[i];
            }
            bytestr = bytestr[..^1];
            return bytestr + "]";
        }

        public static bool HasModifiedScaling(Vector3 v1)
        {
            return ScalingIsEqual(v1, new Vector3(1.0f, 1.0f, 1.0f));
        }
        public static bool ScalingIsEqual(Vector3 v1, Vector3 v2)
        {
            if (Vector3.Distance(v1, v2) > 0.001f)
            {
                return true;
            }
            return false;
        }


        public static ConcurrentBag<PARAMDEF> LoadParamDefXmls(string gameType)
        {
            ConcurrentBag<PARAMDEF> paramdefs_ptde = new();
            foreach (string path in Directory.GetFiles($"Paramdex\\{gameType}\\Defs", "*.xml", SearchOption.AllDirectories))
            {
                var paramdef = PARAMDEF.XmlDeserialize(path);
                paramdefs_ptde.Add(paramdef);
            }
            return paramdefs_ptde;
        }

        /// <summary>
        /// Apply param defs for list of BinderFiles
        /// </summary>
        public static void ApplyParamDefs(ConcurrentBag<PARAMDEF> paramdefs, List<BinderFile> fileList, ConcurrentDictionary<string, PARAM> paramList)
        {
            ConcurrentBag<string> warningList = new();
            Parallel.ForEach(Partitioner.Create(fileList), file =>
            {
                PARAM? param = null;
                try
                {
                    if (file.Name.Contains("m99") || !file.Name.EndsWith(".param"))
                    {
                        // Not a param
                        return;
                    }

                    string name = Path.GetFileNameWithoutExtension(file.Name);
                    param = PARAM.Read(file.Bytes);
                    param = Util.ApplyDefWithWarnings(param, paramdefs);
                    if (param != null)
                        paramList.TryAdd(name, param);
                }
                catch (InvalidDataException)
                {
                }
            });
        }
  
        public static void ApplyRowNames(ConcurrentDictionary<string, string[]> rowNames, ConcurrentDictionary<string, PARAM> paramList)
        {
            Parallel.ForEach(Partitioner.Create(rowNames), file =>
            {
                if (paramList.TryGetValue(file.Key.Replace(".txt", ""), out PARAM param))
                {
                    foreach (string line in file.Value)
                    {
                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        string[] split = line.Split(' ');
                        int id = int.Parse(split[0]);
                        string name = string.Join(' ', split[1..]);

                        PARAM.Row? row = param[id];
                        if (row != null)
                        {
                            row.Name = name;
                        }
                    }
                }
            });
        }

        public static PARAM? ApplyDef(PARAM param, PARAMDEF paramdef)
        {
            try
            {
                param.ApplyParamdef(paramdef);
                return param;
            }
            catch(InvalidDataException e)
            {
                return null;
            }
        }

        public static PARAM? ApplyDefWithWarnings(PARAM param, ConcurrentBag<PARAMDEF> paramdefs)
        {
            bool matchType = false;
            bool matchDefVersion = false;
            int bestDefVersion = -420;
            long bestRowsize = -69;
            long bestDefRowSize = -999;

            foreach (PARAMDEF paramdef in paramdefs)
            {
                if (param.ParamType == paramdef.ParamType)
                {
                    matchType = true;
                    bestDefVersion = paramdef.DataVersion;
                    if (param.ParamdefDataVersion == paramdef.DataVersion)
                    {
                        matchDefVersion = true;
                        bestRowsize = param.DetectedSize;
                        bestDefRowSize = paramdef.GetRowSize();
                        if (param.DetectedSize == -1 || param.DetectedSize == bestDefRowSize)
                        {
                            return ApplyDef(param, paramdef);
                        }
                    }
                }
            }

            // Def could not be applied.
            if (!matchType && !matchDefVersion)
                throw new InvalidDataException($"Could not apply ParamDef for {param.ParamType}. Valid ParamDef could not be found.");
            else if (matchType && !matchDefVersion)
                throw new InvalidDataException($"Could not apply ParamDef for {param.ParamType}. Cannot find ParamDef version {param.ParamdefDataVersion}.");
            else if (matchType && matchDefVersion)
                throw new InvalidDataException($"Could not apply ParamDef for {param.ParamType}. Row sizes do not match. Param: {bestRowsize}, Def: {bestDefRowSize}.");
            else
                throw new Exception("Unhandled Apply ParamDef error.");
        }

    }

}
