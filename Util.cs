﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using SoulsFormats;
using System.Reflection;
using System.Diagnostics;

namespace DSPorterUtil
{
    public static class Util
    {
        public static void WritePortedSoulsFile(ISoulsFile file, string dataPath, string filePath, DCX.Type dcxType = DCX.Type.None)
        {
            string writePath = GetOutputPath(dataPath, filePath, dcxType != DCX.Type.None);
            file.Write(writePath, dcxType);
        }
        public static string GetVirtualPath(string dataPath, string filePath)
        {
            var fileName = filePath.Split(dataPath)[1];
            var virtualPath = fileName.Split("\\")[..^1];
            return Path.Join(virtualPath);
        }
        public static string GetOutputPath(string dataPath, string filePath, bool isDCX = true)
        {
            string outputDirectory = $"{Directory.GetCurrentDirectory()}\\output\\{GetVirtualPath(dataPath, filePath)}";
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
            if (Vector3.Distance(v1, new Vector3(1.0f, 1.0f, 1.0f)) > 0.001f)
            {
                return true;
            }
            return false;
        }

        public static bool Vector3IsEqual(Vector3 v1, Vector3 v2)
        {
            if (!FloatIsEqual(v1.X, v2.X))
                return false;
            if (!FloatIsEqual(v1.Y, v2.Y))
                return false;
            if (!FloatIsEqual(v1.Z, v2.Z))
                return false;
            return true;
        }

        public static bool FloatIsEqual(float f1, float f2)
        {
            if (Math.Abs(f1 - f2) < 0.001f)
            {
                return true;
            }
            return false;
        }

        public static ConcurrentBag<PARAMDEF> LoadParamDefXmls(string gameType)
        {
            ConcurrentBag<PARAMDEF> paramdefs_ptde = new();
            foreach (string path in Directory.GetFiles($"{Directory.GetCurrentDirectory()}\\Resources\\Paramdex\\{gameType}\\Defs", "*.xml", SearchOption.AllDirectories))
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

        /// <summary>
        /// Search an object's properties and return whichever object has the targeted property.
        /// </summary>
        /// <returns>Object that has the property, otherwise null.</returns>
        public static object? FindPropertyObject(PropertyInfo prop, object obj, int classIndex = -1)
        {
            foreach (var p in obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (p.MetadataToken == prop.MetadataToken)
                    return obj;

                if (p.PropertyType.IsNested)
                {
                    var retObj = FindPropertyObject(prop, p.GetValue(obj));
                    if (retObj != null)
                        return retObj;
                }
                else if (p.PropertyType.IsArray)
                {
                    var pType = p.PropertyType.GetElementType();
                    if (pType.IsNested)
                    {
                        Array array = (Array)p.GetValue(obj);
                        if (classIndex != -1)
                        {
                            var retObj = FindPropertyObject(prop, array.GetValue(classIndex));
                            if (retObj != null)
                                return retObj;
                        }
                        else
                        {
                            foreach (var arrayObj in array)
                            {
                                var retObj = FindPropertyObject(prop, arrayObj);
                                if (retObj != null)
                                    return retObj;
                            }
                        }
                    }
                }
            }
            return null;
        }

        public static object? GetPropertyValue(PropertyInfo prop, object obj)
        {
            return prop.GetValue(FindPropertyObject(prop, obj));
        }

        /// <summary>
        /// Loads a text resource.
        /// </summary>
        public static List<string[]> LoadTextResource(string path, int elementNum)
        {
            string[] file = File.ReadAllLines(path);
            List<string[]> output = new();
            for (var i = 0; i < file.Length; i++)
            {
                var line = file[i];
                if (line.Contains("//"))
                {
                    var index = line.IndexOf("//");
                    line = line.Remove(index);
                }
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                
                var split = line.Split("||");
                if (split.Length != elementNum)
                {
                    throw new Exception($"Text resource load error: \"{path}\" (Line {i + 1} has invalid formatting)");
                }
                output.Add(split);
            }
            return output;
        }
    }

}
