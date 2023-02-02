using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoulsFormats;
using DSPorterUtil;

namespace DSRPorter
{
    public class TexturePorter
    {
        private readonly DSPorter _porter;
        private Dictionary<string, HashSet<TPF>> tpfCache = new();

        public TexturePorter(DSPorter porter)
        {
            _porter = porter;
        }

        private TPF? CreateTPF(BND3 bnd)
        {
            HashSet<FLVER2.Texture> textures = GetFlverTextures(bnd);
            return CreateTPF(textures);
        }

        private bool AddMatchingTextures(string matchName, TPF sourceTPF, TPF? targetTPF)
        {
            foreach (var tpfTex in sourceTPF.Textures)
            {
                if (tpfTex.Name == matchName)
                {
                    targetTPF ??= new TPF()
                    {
                        Compression = DCX.Type.None,
                        Encoding = sourceTPF.Encoding,
                        Flag2 = sourceTPF.Flag2,
                        Platform = sourceTPF.Platform,
                    };
                    targetTPF.Textures.Add(tpfTex);
                    return true;
                }
            }
            return false;
        }

        private TPF? CreateTPF(HashSet<FLVER2.Texture> textures)
        {
            TPF? targetTPF = null;
            foreach (var tex in textures)
            {
                var split = tex.Path.Split("\\");
                var fileName = Path.GetFileNameWithoutExtension(split.Last());
                if (fileName.StartsWith('m'))
                {
                    string mapFolder = fileName.Split("_")[0];
                    if (mapFolder == "m19")
                        mapFolder = "m18"; // Asylum used to be m19
                    var bhdDirectory = $@"{_porter.DataPath_DSR}\map\{mapFolder}";
                    foreach (var bhd in Directory.GetFiles(bhdDirectory, "*.tpfbhd"))
                    {
                        restart:
                        if (tpfCache.TryGetValue(bhd, out HashSet<TPF>? tpfs))
                        {
                            // TPF cache exists, search through it for the right texture.
                            foreach (var sourceTPF in tpfs)
                            {
                                foreach (var tpfTex in sourceTPF.Textures)
                                {
                                    if (tpfTex.Name == fileName)
                                    {
                                        targetTPF ??= new TPF()
                                        {
                                            Compression = DCX.Type.None,
                                            Encoding = sourceTPF.Encoding,
                                            Flag2 = sourceTPF.Flag2,
                                            Platform = sourceTPF.Platform,
                                        };
                                        targetTPF.Textures.Add(tpfTex);
                                        goto next;
                                    }
                                }
                            }
                        }
                        else
                        {
                            // TPF cache doesn't exist, build it.
                            tpfCache.Add(bhd, new HashSet<TPF>());
                            var bx = BXF3.Read(bhd, bhd.Replace(".tpfbhd", ".tpfbdt"));
                            foreach (var file in bx.Files)
                            {
                                if (TPF.Is(file.Bytes))
                                {
                                    var sourceTPF = TPF.Read(file.Bytes);
                                    tpfCache[bhd].Add(sourceTPF);
                                }
                            }
                            goto restart;
                        }
                    }
                }
                next:;
            }
            return targetTPF;
        }

        private HashSet<FLVER2.Texture> GetFlverTextures(BND3 bnd)
        {
            HashSet<FLVER2.Texture> flverTextures = new();
            GetFlvers(flverTextures, bnd);
            return flverTextures;
        }

        private void GetFlvers(HashSet<FLVER2.Texture> flverTextures, BND3 bnd)
        {
            foreach (var file in bnd.Files)
            {
                if (BND3.Is(file.Bytes))
                {
                    GetFlvers(flverTextures, BND3.Read(file.Bytes));
                }
                else if (FLVER2.Is(file.Bytes))
                {
                    var flver = FLVER2.Read(file.Bytes);
                    foreach (var mat in flver.Materials)
                    {
                        flverTextures.UnionWith(mat.Textures);
                    }
                }
            }
        }

        public void ModifyObjbnd(string modelName)
        {
            string bndPath = $@"{_porter.DataPath_Output}\obj\{modelName}.objbnd.dcx";
            string dataPath = _porter.DataPath_Output;

            if (!File.Exists(bndPath))
            {
                bndPath = $@"{_porter.DataPath_DSR}\obj\{modelName}.objbnd.dcx";
                dataPath = _porter.DataPath_DSR;
            }

            if (!File.Exists(bndPath))
            {
                _porter.OutputLog.Add($"Couldn't find \"{bndPath}\" in DSR data, skipped self-contained texture processing.");
                return;
            }

            BND3 objBND = BND3.Read(bndPath);
            foreach (var file in objBND.Files)
            {
                if (TPF.Is(file.Bytes))
                    return;
                if (file.ID == 100)
                    throw new NotImplementedException($"Unhandled issue: \"{bndPath}\" has a file with an ID of 100 ({file.Name}).");
            }

            TPF? newObjTPF = CreateTPF(objBND);

            if (newObjTPF != null && newObjTPF.Textures.Any())
            {
                BinderFile binder = new()
                {
                    Name = $@"obj\{modelName}\{modelName}.tpf",
                    ID = 100,
                    Bytes = newObjTPF.Write()
                };
                objBND.Files.Insert(0, binder);
                Util.WritePortedSoulsFile(objBND, dataPath, bndPath, _porter.CompressionType);
            }
        }
    }
}
