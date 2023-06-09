using System.Text;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Numerics;
using DSPorterUtil;
using SoulsFormats;
using SoulsFormatsExtensions;
using SoulsAssetPipeline.Animation;

/*
 THINGS THAT NEED TO BE CONVEYED TO USERS
 * Porting DrawParam should be done in conjunction with MSB porting due to DSR's new IDs
 * modified lua must be decompiled since 32 bit lua can't be decompiled (and thus converted)
    * any compiled lua will just be replaced with DSR lua, unless the DSR lua cannot be found in which case program will complain.
 * scaled collision in MSB
 */



namespace DSRPorter
{
    public partial class DSPorter
    {
        public string DataPath_PTDE_Mod = "";
        public string DataPath_PTDE_Vanilla = "";
        public string DataPath_DSR = "";

        public readonly TAE.Template TaeTemplate = TAE.Template.ReadXMLFile(@$"{Directory.GetCurrentDirectory()}\Resources\TAE.Template.DS1.xml");
        public readonly string DataPath_Output = $@"{Directory.GetCurrentDirectory()}\Output";
        public readonly string LuaCompilationPath = $@"{Directory.GetCurrentDirectory()}\Resources\lua";
        public readonly string OutputOverwritePath = $@"{Directory.GetCurrentDirectory()}\Output Overwrite";
        public readonly DCX.Type CompressionType = DCX.Type.DCX_DFLT_10000_24_9;

        private ConcurrentBag<PARAMDEF> _paramdefs_ptde;
        private ConcurrentBag<PARAMDEF> _paramdefs_dsr;
        private readonly List<ScaledObject> _scaledObjects = new();
        private readonly HashSet<string> _objsToPort = new();
        private readonly ProgressBar _progressBar;

        public ConcurrentBag<string> OutputLog = new();
        public System.Runtime.ExceptionServices.ExceptionDispatchInfo? PorterException = null;

        public const bool EnableScaledObjectAdjustments = false;
        public const bool UseDsrTextures = true;
        public const bool UseDSRToneMapBankValues = true;
        public bool DescaleMSBObjects = true;

        public DSPorter(ProgressBar progressBar)
        {
            _progressBar = progressBar;

            if (DSPorterSettings.Is_SOTE)
            {
                OutputOverwritePath = @"Y:\Projects Y\Modding\DSR\DSR port input overwrite";
            }

            if (DSPorterSettings.Is_SOTE)
            {
                FFX_Whitelist = LoadTextResource_FFX("FFX Whitelist SOTE.txt");
                FFX_Blacklist = LoadTextResource_FFX("FFX Blacklist SOTE.txt");
            }
            else
            {
                FFX_Whitelist = LoadTextResource_FFX("FFX Whitelist.txt");
                FFX_Blacklist = LoadTextResource_FFX("FFX Blacklist.txt");
            }

            MsbScaledObj_Whitelist = LoadTextResource_MsbScaledObjs();
            MSB_RenderGroupModifiers = LoadTextResource_RenderGroupModifiers();
        }

        public readonly Dictionary<string, List<string>> MsbScaledObj_Whitelist;

        private readonly List<long> _ffxTpfWhitelist = new()
        {
            // TPF for FFX
            // Not implemented.
        };


        public readonly Dictionary<string, List<RenderGroupModifier>> MSB_RenderGroupModifiers;
        public class RenderGroupModifier
        {
            public GroupTypeEnum GroupType;
            public int Index;
            public string PartName;
            public uint OldValue;
            public uint NewValue;
            public enum GroupTypeEnum
            { 
                DrawGroup = 0,
                DispGroup = 1
            }

            public RenderGroupModifier()
            { }
        }
        private Dictionary<string, List<RenderGroupModifier>> LoadTextResource_RenderGroupModifiers()
        {
            string pathName = $@"MSB render group improvements.txt";
            List<string[]> resList = Util.LoadTextResource($@"{Directory.GetCurrentDirectory()}\Resources\{pathName}", 6);
            Dictionary<string, List<RenderGroupModifier>> dict = new();
            foreach (var res in resList)
            {
                RenderGroupModifier mod = new();
                dict.TryAdd(res[0], new List<RenderGroupModifier>());

                try
                {
                    mod.GroupType = res[1] switch
                    {
                        "draw" => RenderGroupModifier.GroupTypeEnum.DrawGroup,
                        "disp" => RenderGroupModifier.GroupTypeEnum.DispGroup,
                        _ => throw new InvalidDataException(),
                    };
                    mod.Index = int.Parse(res[2]);
                    mod.PartName = res[3];
                    mod.OldValue = uint.Parse(res[4]);
                    mod.NewValue = uint.Parse(res[5]);
                    dict[res[0]].Add(mod);
                }
                catch
                {
                    throw new Exception($"Text resource load error: {pathName}.\n{string.Join("||", res)} has invalid formatting.");
                }
            }

            return dict;
        }

        public readonly List<long> FFX_Whitelist = new();
        public readonly List<long> FFX_Blacklist = new();
        private List<long> LoadTextResource_FFX(string pathName)
        {
            List<string[]> resList = Util.LoadTextResource($@"{Directory.GetCurrentDirectory()}\Resources\{pathName}", 1);
            List<long> output = new();
            foreach (var res in resList)
            {
                long id;
                try
                {
                    id = long.Parse(res[0]);
                }
                catch
                {
                    throw new Exception($"Text resource load error: {pathName}.\n{res[0]} is not a valid FFX ID.");
                }
                output.Add(id);
            }
            return output;
        }


        public readonly List<ScaledObject> SOTE_ScaledObjectList = new()
        {
            //new ScaledObject("o1413", "o1422", new Vector3( 1.0f,    1.5f,   1.0f)), // taurus arena blockers. removed in favor of enemy-only collision
            //new ScaledObject("o1413", "o1423", new Vector3( 1.0f,    2.0f,   1.0f)), // taurus arena blockers. removed in favor of enemy-only collision
            new ScaledObject("o1312", "o1324", new Vector3( 1.36f,   1.83f,  0.95f)), // m12_00 door
            new ScaledObject("o8300", "o8301", new Vector3( 0.5f,    0.5f,   0.5f)), // m12_01 NG+ door
            new ScaledObject("o2403", "o2404", new Vector3( 1.5f,    1.5f,   1.5f)), // m12_01 respite lantern healer
            new ScaledObject("o3010", "o3012", new Vector3( 0.5f,    0.5f,   0.5f)), // m12_01 respite pain device
            //
            //new ScaledObject("o1419", "o1424", new Vector3( 0.5f,    1.0f,   1.0f)), // Adjusted due to o1413 removals
            new ScaledObject("o1419", "o1422", new Vector3( 0.5f,    1.0f,   1.0f)), // m13_02 fog gate?
            //
            new ScaledObject("o4510", "o4511", new Vector3( 1.6f,    1.6f,   1.6f)), // m14_01 
            //new ScaledObject("o4830", "o4831", new Vector3(16.0f,   16.0f,  16.0f)), // giant demon statue turrets. Removed because I want collision to NOT scale with those since they interfere with the turret.
            new ScaledObject("o8540", "o8545", new Vector3( 2.0f,    2.0f,   2.0f)),
            new ScaledObject("o6700", "o6701", new Vector3( 2.3f,    1.5f,   1.0f)),
            new ScaledObject("o8051", "o8052", new Vector3( 1.0f,    1.0f,   4.0f)),
            new ScaledObject("o8300", "o8302", new Vector3( 0.25f,   0.15f,  0.25f)),
            new ScaledObject("o4610", "o4612", new Vector3( 0.75f,   0.75f,  0.75f)),
            new ScaledObject("o4700", "o4702", new Vector3( 1.1f,    1.2f,   1.1f)),
            new ScaledObject("o1317", "o1325", new Vector3( 1.05f,   1.0f,   1.0f)),
            new ScaledObject("o1305", "o1307", new Vector3( 1.08f,   1.0f,   1.0f)),
        };

        private void DSRPorter_EMEVD()
        {
            var paths = Directory.GetFiles($@"{DataPath_PTDE_Mod}\event", "*.emevd");
            if (paths.Length == 0)
                return;
            foreach (var path in paths)
            {
                var emevd = EMEVD.Read(path);
                Util.WritePortedSoulsFile(emevd, DataPath_PTDE_Mod, path, CompressionType);
            }
            Debug.WriteLine("Finished: EMEVD");
            OutputLog.Add($@"Finished: event\*.emevd");
        }

        private bool _MSBFinished = false;
        private void DSRPorter_MSB()
        {
            var paths = Directory.GetFiles($@"{DataPath_PTDE_Mod}\map\mapstudio", "*.msb");
            if (paths.Length == 0)
                return;

            foreach (var path in paths)
            {
                string msbName = Path.GetFileNameWithoutExtension(path);
                var msb = MSB1.Read(path);
                MSB1 msb_vanilla = MSB1.Read(path.Replace(DataPath_PTDE_Mod, DataPath_PTDE_Vanilla));
                MsbScaledObj_Whitelist.TryGetValue(msbName, out List<string>? scalingExceptionList);
                MSB_RenderGroupModifiers.TryGetValue(msbName, out List<RenderGroupModifier>? RenderGroupModifiers);

                foreach (var model in msb.Models.Objects)
                {
                    if (msb_vanilla.Models.Objects.Find(e => e.Name == model.Name) == null)
                    {
                        if (DSPorterSettings.Is_SOTE)
                        {
                            if (model.Name.Contains("4203"))
                            {
                                // Non-existant model I shoved into m18_01 model param somehow.
                                continue;
                            }
                        }
                        _objsToPort.Add(model.Name);
                    }
                }

                foreach (var p in msb.Parts.GetEntries())
                {
                    if (RenderGroupModifiers != null)
                    {
                        // dispgroup/drawgroup overrides
                        if(DSPorterSettings.RenderGroupImprovements)
                        {
                            RenderGroupModifier? rgm = RenderGroupModifiers.Find(e => e.PartName == p.Name);
                            if (rgm != null)
                            {
                                if (rgm.GroupType == RenderGroupModifier.GroupTypeEnum.DrawGroup)
                                {
                                    if (p.DrawGroups[rgm.Index] == rgm.OldValue)
                                    {
                                        p.DrawGroups[rgm.Index] = rgm.NewValue;
                                    }
                                }
                                else if (rgm.GroupType == RenderGroupModifier.GroupTypeEnum.DispGroup)
                                {
                                    if (p.DispGroups[rgm.Index] == rgm.OldValue)
                                    {
                                        p.DispGroups[rgm.Index] = rgm.NewValue;
                                    }
                                }
                                else
                                {
                                    throw new NotSupportedException();
                                }
                            }
                        }
                    }

                    if (p is MSB1.Part.MapPiece piece)
                    {
                        if (DSPorterSettings.Is_SOTE)
                        {
                            if (msbName == "m14_01_00_00")
                            {
                                // Lava fields
                                if (p.ModelName == "m8001B1")
                                {
                                    p.LightID = 14;
                                    p.ScatterID = 14;
                                }
                            }
                        }
                    }
                    else if (p is MSB1.Part.Enemy ene)
                    {
                        if (DSPorterSettings.SlimeCeilingFix)
                        {
                            if (msbName == "m10_00_00_00")
                            {
                                // Some slimes in the depths need to be moved downards to not clip in the walls.
                                if (p.Name == "c3200_0006" && p.Position == new Vector3(-122.8f, -70.5f, -21.0f))
                                    p.Position = new Vector3(-122.8f, -70.582f, -21.0f);
                                else if (p.Name == "c3200_0003" && p.Position == new Vector3(-119.8f, -70.5f, -27.5f))
                                    p.Position = new Vector3(-119.8f, -70.593f, -27.5f);
                                else if (p.Name == "c3200_0002" && p.Position == new Vector3(-122.8f, -70.5f, -28.5f))
                                    p.Position = new Vector3(-122.8f, -70.650f, -28.5f);
                            }
                        }
                    }
                    else if (p is MSB1.Part.Collision col)
                    {
                        if (DSPorterSettings.MiscCollisionFixes)
                        {
                            if (msbName == "m10_01_00_00")
                            {
                                if (col.Position == Vector3.Zero)
                                {
                                    if (col.Name == "h1000B1")
                                    {
                                        col.Position = new Vector3(0, 0, -0.1f);
                                    }
                                }
                            }
                            else if (msbName == "m14_00_00_00")
                            {
                                if (col.Name == "h0020B0" && col.NvmGroups[0] == 1048576)
                                {
                                    col.NvmGroups[0] = 2148532224; // 1048576 -> 2148532224
                                }
                            }
                        }
                    }
                    else if (p is MSB1.Part.ConnectCollision connect)
                    {
                        if (DSPorterSettings.MiscCollisionFixes)
                        {
                            if (msbName == "m10_01_00_00")
                            {
                                if (p.Position == Vector3.Zero)
                                {
                                    if (p.Name == "h1023B1_0000")
                                    {
                                        p.Position = new Vector3(0, 0, -1.4f);
                                    }
                                }
                            }
                        }
                    }
                    else if (p is MSB1.Part.Object obj)
                    {
                        if (scalingExceptionList != null && scalingExceptionList.Contains(p.Name))
                        {
                        }
                        else if (DescaleMSBObjects && Util.HasModifiedScaling(p.Scale))
                        {
                            if (!DSPorterSettings.Is_SOTE)
                            {
                                OutputLog.Add($"MSB object \"{msbName}\\{p.Name}\" had its scaling reverted: {p.Scale} -> {Vector3.One}");
                                p.Scale = Vector3.One;
                            }
                            else
                            {
                                ScaledObject? scaledObj = null;
                                if (msbName == "m14_01_00_00")
                                {
                                    // Demon statue turret
                                    if (p.ModelName == "o4830")
                                    {
                                        if (p.Scale == new Vector3(16, 16, 16))
                                        {
                                            // Ignore izalith statue turrets due to scaled collision making the turret not work.
                                            OutputLog.Add($"Ignored Demon Statue Turret ({p.Name}) scaling.");
                                            continue;
                                        }
                                    }
                                    // Ceaseless lava object
                                    else if (p.ModelName == "o4500")
                                    {
                                        p.LightID = 14;
                                        p.ScatterID = 14;
                                    }
                                }

                                foreach (var so in SOTE_ScaledObjectList)
                                {
                                    // SOTE: go through pre-scaled object list to find the corresponding pre-scaled object.
                                    if (so.Matches(p.ModelName, p.Scale))
                                    {
                                        scaledObj = so;
                                        break;
                                    }
                                }
                                if (scaledObj == null)
                                {
                                    // Couldn't find pre-scaled object in the list, something went wrong and needs to be fixed.
                                    throw new Exception($"Couldn't find SOTE pre-scaled object for {msbName}\\{p.Name} {p.Scale}");
                                }

                                if (msb.Models.Objects.Find(e => e.Name == scaledObj.NewModelName) == null)
                                {
                                    // Add pre-scaled object to MSB models.
                                    MSB1.Model.Object model = new()
                                    {
                                        Name = scaledObj.NewModelName,
                                        SibPath = $@"N:\FRPG\data\Model\obj\{scaledObj.NewModelName}\sib\{scaledObj.NewModelName}.sib"
                                    };
                                    msb.Models.Objects.Add(model);
                                }

                                OutputLog.Add($"MSB object \"{msbName}\\{p.Name}\" now uses SOTE pre-scaled object \"{scaledObj.NewModelName}\". MSB scaling was reverted in the process: {p.Scale} -> {Vector3.One}");
                                p.ModelName = scaledObj.NewModelName;
                                p.Scale = Vector3.One;
                            }
                        }
                    }
                }

                if (DSPorterSettings.Is_SOTE)
                {
                    if (msbName == "m15_00_00_00")
                    {
                        // SOTE Sen's Fortress traps
                        var trapffx1 = msb.Events.SFX.Find(e => e.Name == "SFX new trap 1");
                        var trapffx2 = msb.Events.SFX.Find(e => e.Name == "SFX new trap 2");
                        var trapRegion2 = msb.Regions.Regions.Find(e => e.Name == "SFX region new trap 2");

                        if (trapffx1 == null || trapffx2 == null || trapRegion2 == null)
                        {
                            throw new Exception("Couldn't find m15_00_00_00 SOTE new trap FFX or regions by name.");
                        }

                        trapffx1.EffectID = 815;
                        trapffx2.EffectID = 815;
                        trapRegion2.Position = new Vector3(74.524f, 26.275f, 266.305f);
                    }
                }

                Util.WritePortedSoulsFile(msb, DataPath_PTDE_Mod, path);
            }
            Debug.WriteLine("Finished: MSB");
            OutputLog.Add($@"Finished: map\mapstudio\*.msb");
            _MSBFinished = true;
        }

        /// <summary>
        /// Does some important things to bnds to make sure they work
        /// </summary>
        private void FinalizeBND(BND3 bnd)
        {
            List<int> IDs = new();
            foreach (var file in bnd.Files)
            {
                if (IDs.Contains(file.ID))
                {
                    do
                    {
                        file.ID++;
                    }
                    while (IDs.Contains(file.ID));
                }
                IDs.Add(file.ID);
            }
            bnd.Files = bnd.Files.OrderBy(e => e.ID).ToList();
        }

        private void DSRPorter_FFX()
        {
            var paths = Directory.GetFiles($@"{DataPath_PTDE_Mod}\sfx", "*.ffxbnd");
            if (paths.Length == 0)
                return;

            foreach (var path in paths)
            {
                var bnd_PTDE_modded = BND3.Read(path);
                var bnd_PTDE_vanilla = BND3.Read(path.Replace(DataPath_PTDE_Mod, DataPath_PTDE_Vanilla));
                var bnd_DSR_target = BND3.Read(path.Replace(DataPath_PTDE_Mod, DataPath_DSR) + ".dcx");

                foreach (var file_PTDE_modded in bnd_PTDE_modded.Files)
                {
                    long fileID = GetFileIdFromName(file_PTDE_modded.Name);

                    bool is_ffx = FXR1.Is(file_PTDE_modded.Bytes);

                    var file_PTDE_vanilla = bnd_PTDE_vanilla.Files.Find(f => f.Name == file_PTDE_modded.Name);

                    if (file_PTDE_vanilla != null)
                    {
                        // File exists in vanilla, check if it is allowed to be ported.

                        if (_ffxTpfWhitelist.Contains(GetFileIdFromName(file_PTDE_vanilla.Name)))
                        {
                            throw new NotSupportedException();
                        }
                        else
                        {
                            // Check if it was modified
                            if (file_PTDE_vanilla.Bytes.SequenceEqual(file_PTDE_modded.Bytes))
                            {
                                // File was unmodified
                                if (is_ffx)
                                {
                                    if (!FFX_Whitelist.Contains(fileID))
                                    {
                                        // Not present in whitelist, skip this FFX.
                                        continue;
                                    }
                                }
                                else
                                {
                                    // Skip any non-FFX files.
                                    continue;
                                }
                            }
                        }
                    }
                    if (is_ffx)
                    {
                        if (FFX_Blacklist.Contains(fileID))
                        {
                            // Present in blacklist, skip this FFX.
                            continue;
                        }
                    }

                    // File is permitted to be ported to DSR

                    if (is_ffx)
                    {
                        if (IsCoreFFX(file_PTDE_modded))
                        {
                            // Core FFX, must use base DSR version or things will break.
                            continue;
                        }

                        FXR1 ffx_PTDE_modded = FXR1.Read(file_PTDE_modded.Bytes);

                        // Convert to DSR
                        ffx_PTDE_modded.Wide = true;
                        file_PTDE_modded.Bytes = ffx_PTDE_modded.Write();
                    }

                    file_PTDE_modded.Name = file_PTDE_modded.Name.Replace("win32", "x64");
                    var file_dsr_target = bnd_DSR_target.Files.Find(f => f.Name == file_PTDE_modded.Name);
                    var dsrBndName = Path.GetFileName($@"{path}.dcx");
                    var outputName = $"\"{dsrBndName}\\{Path.GetFileName(file_PTDE_modded.Name)}\"";

                    if (file_dsr_target != null)
                    {
                        // DSR has this FFX already.
                        file_dsr_target.Bytes = file_PTDE_modded.Bytes;
                        OutputLog.Add($"FFX: Overwrote FFX file {outputName}");
                    }
                    else
                    {
                        // DSR doesn't have this FFX, ensure ID is unused then add it.
                        while (bnd_DSR_target.Files.Find(f => f.ID == file_PTDE_modded.ID) != null)
                        {
                            file_PTDE_modded.ID++;
                        }
                        bnd_DSR_target.Files.Add(file_PTDE_modded);
                        OutputLog.Add($"FFX: Added FFX file {outputName}");
                    }
                }

                bnd_DSR_target.Files = bnd_DSR_target.Files.OrderBy(e => e.ID).ToList();
                Util.WritePortedSoulsFile(bnd_DSR_target, DataPath_PTDE_Mod, path, CompressionType);
            }

            // Make a second pass for whitelisted FFX in ffxbnds not used by SOTE PTDE
            foreach (var dsrPath in Directory.GetFiles($@"{DataPath_DSR}\sfx", "*.ffxbnd.dcx"))
            {
                if (dsrPath.EndsWith("Patch.ffxbnd.dcx"))
                {
                    // It seems like dupe FFX present in common already take precedence over patch so I don't believe this is necessary.

                    // But if it does turn out to matter: ha ha, idiot. finish this.
                    /*
                    // For patchffxbnd, any present FFXs are expected to be in commonffxbnd instead.
                    BND3 bnd_patch = BND3.Read(dsrPath);
                    BND3 commonBND = BND3.Read($@"{DataPath_Output}\sfx\FRPG_SfxBnd_CommonEffects.ffxbnd.dcx");
                    foreach (var patchFile in bnd_patch.Files.ToList())
                    {
                        long fileID = GetFileIdFromName(patchFile.Name);
                        if (FXR1.Is(patchFile.Bytes) && _ffxWhitelist.Contains(fileID))
                        {
                            // Scan each output FFXBND for this whitelist FFX found in patch ffx. If it isn't present in common, then it probably isn't being properly implemented.
                            bool handledByCommon = false;
                            foreach (var commonFile in commonBND.Files)
                            {
                                if (FXR1.Is(commonFile.Bytes) && GetFileIdFromName(commonFile.Name) == fileID)
                                {
                                    handledByCommon = true;
                                    break;
                                }
                            }
                            if (!handledByCommon)
                            {
                                // TODO: add to output instead
                                throw new NotImplementedException("Not equipped to handle whitelisted FFX in DSR patch.ffxbnd");
                            }
                            bnd_patch.Files.Remove(patchFile);
                        }
                    }
                    Util.WritePortedSoulsFile(bnd_patch, DataPath_DSR, dsrPath, CompressionType);
                    */
                    continue;
                }

                bool portThisBND = false;
                var fileName = Path.GetFileNameWithoutExtension(dsrPath);
                if (Directory.GetFiles($@"{DataPath_Output}\sfx", "*.ffxbnd.dcx").ToList().Find(e => Path.GetFileNameWithoutExtension(e) == fileName) != null)
                {
                    continue;
                }
                BND3 bnd_DSR_target = BND3.Read(dsrPath);
                BND3 bnd_PTDE_vanilla = BND3.Read(dsrPath.Replace(DataPath_DSR, DataPath_PTDE_Vanilla).Replace(".dcx", ""));
                foreach (var file in bnd_DSR_target.Files)
                {
                    long fileID = GetFileIdFromName(file.Name);
                    if (FXR1.Is(file.Bytes) && FFX_Whitelist.Contains(fileID))
                    {
                        var ptde_file = bnd_PTDE_vanilla.Files.Find(e => GetFileIdFromName(e.Name) == fileID);

                        FXR1 ffx_PTDE = FXR1.Read(ptde_file.Bytes);

                        // Convert to DSR
                        ffx_PTDE.Wide = true;
                        file.Bytes = ffx_PTDE.Write();
                        portThisBND = true;
                        OutputLog.Add($"FFX: Ported whitelist FFX \"{file.Name}\"");
                    }
                }
                if (portThisBND)
                {
                    Util.WritePortedSoulsFile(bnd_DSR_target, DataPath_DSR, dsrPath, CompressionType);
                    OutputLog.Add($"FFX: Included \"{fileName}.ffxbnd.dcx\" because it had whitelisted FFX");
                }
            }

            Debug.WriteLine("Finished: FFX");
            OutputLog.Add($@"Finished: sfx\*.ffxbnd");
        }

        private void TransferFmgEntries(BinderFile file_old, BinderFile file_new)
        {
            string? enumName = System.Enum.GetName(typeof(SoulsResources.FmgIDType), file_old.ID);
            if (enumName == null)
            {
                throw new InvalidDataException($"FMG \"{file_old.Name}\" is has an ID of {file_old.ID} which cannot be identified. Please fix or report this.");
            }
            if (enumName.ToLower().Contains("_patch"))
            {
                // Don't transfer any entries for patch FMGs, since DSR has every entry in patch and source FMGs.
                file_new.Bytes = file_old.Bytes;
                return; 
            }
            FMG fmg_new = FMG.Read(file_new.Bytes);
            FMG fmg_old = FMG.Read(file_old.Bytes);
            foreach (var entry_new in fmg_new.Entries)
            {
                // Add any new DSR entries
                var entry_old = fmg_old.Entries.Find(e => e.ID == entry_new.ID);
                if (entry_old == null)
                {
                    fmg_old.Entries.Add(entry_new);
                }
                else if (string.IsNullOrEmpty(entry_old.Text))
                {
                    entry_old.Text = entry_new.Text;
                }
            }
            file_new.Bytes = fmg_old.Write();
        }

        private void DSRPorter_MSGBND()
        {
            var paths = Directory.GetFiles($@"{DataPath_PTDE_Mod}\msg", "*.msgbnd", SearchOption.AllDirectories);
            if (paths.Length == 0)
                return;

            List<BinderFile> files_old = new();
            foreach (var path in paths)
            {
                files_old.AddRange(BND3.Read(path).Files);
            }
            foreach (var path in paths)
            {
                string path_new = path.Replace(DataPath_PTDE_Mod, DataPath_DSR) + ".dcx";
                if (!File.Exists(path_new))
                {
                    MessageBox.Show($"{path} couldn't be found in DSR data.", "Warning");
                    OutputLog.Add($"Skipped \"{path}\" since it couldn't be found in DSR data.");
                    continue;
                }

                var bnd_new = BND3.Read(path_new);
                foreach (var file_new in bnd_new.Files)
                {
                    var file_old = files_old.Find(e => e.ID == file_new.ID);
                    if (file_old == null)
                    {
                        throw new FileNotFoundException($"Couldn't find file ID {file_new.ID} in PTDE MSGBNDs.");
                    }
                    TransferFmgEntries(file_old, file_new);
                }
                Util.WritePortedSoulsFile(bnd_new, DataPath_PTDE_Mod, path, CompressionType);
            }
            Debug.WriteLine("Finished: MSGBND");
            OutputLog.Add($@"Finished: msg\*.msgbnd");
        }

        /*
        /// <summary>
        /// Debug thing to figure out how drawvalues were changed from PTDE to DSR.
        /// </summary>
        public ConcurrentDictionary<string, ConcurrentBag<string>> dict = new(); // dict[cell name][list<values>]
        private void DEBUG_CompareDrawParamRows(string paramName, PARAM.Row row_old, PARAM.Row? row_new)
        {
            if (row_new == null)
                return;
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
                if (oldCell.Value.ToString() != newCell.Value.ToString())
                {
                    dict.TryAdd(oldCell.Def.InternalName, new ConcurrentBag<string>());
                    dict[oldCell.Def.InternalName].Add($"{paramName}: {oldCell.Value} -> {newCell.Value}");
                }
            }
            return;
        }
        */

        private void AdjustDrawParamRow(PARAM param, PARAM.Row row_old, PARAM.Row? row_new)
        {
            if (row_new == null)
                return;

            if (param.ParamType == "LIGHT_SCATTERING_BANK")
            {
                //row_old["sunA"].Value = row_new["sunA"].Value;
            }
            if (param.ParamType == "LIGHT_BANK")
            {
                // Lighting is WAY too dark when using PTDE values for these two fields.
                row_old["envSpc_colA"].Value = row_new["envDif_colA"].Value;
                row_old["envDif_colA"].Value = row_new["envDif_colA"].Value;
            }
        }

        private ConcurrentBag<string> _debugOutput = new();
        private void DSRPorter_TransferParams(string datapath, bool isDrawParam = false)
        {
            var paths = Directory.GetFiles(datapath, "*.parambnd");
            if (paths.Length == 0)
                return;

            while (!_paramdefs_ptde.Any() || !_paramdefs_dsr.Any())
            {
                Thread.Sleep(1000);
            }

            foreach (string bndPath_old in paths)
            {
                string bndPath_new = bndPath_old.Replace(DataPath_PTDE_Mod, $@"{DataPath_DSR}") + ".dcx";
                string bndPath_vanilla = bndPath_old.Replace(DataPath_PTDE_Mod, DataPath_PTDE_Vanilla);

                ConcurrentDictionary<string, PARAM> paramList_old = new();
                ConcurrentDictionary<string, PARAM> paramList_new = new();
                ConcurrentDictionary<string, PARAM> paramList_vanilla = new();

                // Load params
                BND3 bnd_old = BND3.Read(bndPath_old);
                BND3 bnd_new = BND3.Read(bndPath_new);
                BND3 bnd_vanilla; 

                Util.ApplyParamDefs(_paramdefs_ptde, bnd_old.Files, paramList_old);
                Util.ApplyParamDefs(_paramdefs_dsr, bnd_new.Files, paramList_new);

                if (isDrawParam)
                {
                    bnd_vanilla = BND3.Read(bndPath_vanilla);
                    Util.ApplyParamDefs(_paramdefs_ptde, bnd_vanilla.Files, paramList_vanilla);
                }

                Parallel.ForEach(Partitioner.Create(paramList_old), item =>
                {
                    if (paramList_new.ContainsKey(item.Key) == false)
                    {
                        // Couldn't find matching param in other list.
                        if (item.Key == "default_ToneCorrectBank")
                        {
                            // DSR defs can't handle this, but it's all default values in PTDE so I doubt it matters
                        }
                        else if (DSPorterSettings.Is_SOTE
                            && (item.Key.StartsWith("s18_1") || item.Key.StartsWith("m18_1")))
                        {
                            // Those drawparams I added to SOTE but never used
                        }
                        else
                        {
                            throw new FileNotFoundException($"Couldn't find {item.Key} in DSR parambnd.");
                        }
                        return;
                    }

                    PARAM param_old = paramList_old[item.Key];
                    PARAM param_new_target = paramList_new[item.Key];
                    List<PARAM.Row> paramRows_new = param_new_target.Rows.ToList();

                    PARAM? param_vanilla = null;

                    if (isDrawParam)
                    {
                        // Todo: handle newly added DrawParams properly
                        try
                        {
                            param_vanilla = paramList_vanilla[item.Key];
                        }
                        catch
                        { 
                        
                        }
                    }

                    if (param_old.ParamType == "TONE_MAP_BANK")
                    {
                        if (UseDSRToneMapBankValues)
                        {
                            // Don't transfer row data for this param
                            return;
                        }
                    }

                    param_new_target.Rows.Clear();
                    bool orderRows = false;

                    if (param_old.ParamType == "OBJECT_PARAM_ST")
                    {
                        if (EnableScaledObjectAdjustments)
                        {
                            foreach (var scaledObj in _scaledObjects)
                            {
                                var OGRow = param_old[scaledObj.OGModelID];
                                PARAM.Row newObjRow = new(scaledObj.NewModelID, $"Scaled Object {scaledObj.NewModelName}", param_new_target.AppliedParamdef);
                                TransferParamRow(OGRow, newObjRow);
                                param_new_target.Rows.Add(newObjRow);
                            }
                        }
                        else if (DSPorterSettings.Is_SOTE)
                        {
                            orderRows = true;
                            foreach (var scaledObj in SOTE_ScaledObjectList)
                            {
                                var OGRow = param_old[scaledObj.OGModelID];
                                if (OGRow == null)
                                {
                                    // Apparently this isn't an issue.
#if DEBUG
                                    OutputLog.Add($"Couldn't find object param row for {scaledObj.OGModelName}. This probably doesn't matter.");
#endif
                                    //throw new Exception($"Couldn't find object param row for {scaledObj.OGModelName}}");
                                    continue;
                                }
                                PARAM.Row newObjRow = new(scaledObj.NewModelID, $"Scaled Object {scaledObj.NewModelName}", param_new_target.AppliedParamdef);
                                TransferParamRow(OGRow, newObjRow);
                                param_new_target.Rows.Add(newObjRow);
                            }
                        }
                    }
                    else if (param_old.ParamType == "OBJ_ACT_PARAM_ST")
                    {
                        if (EnableScaledObjectAdjustments)
                        {
                            foreach (var scaledObj in _scaledObjects)
                            {
                                var OGRow = param_old[scaledObj.OGModelID];
                                PARAM.Row newObjRow = new(scaledObj.NewModelID, $"Scaled Object {scaledObj.NewModelName}", param_new_target.AppliedParamdef);
                                TransferParamRow(OGRow, newObjRow);
                                param_new_target.Rows.Add(newObjRow);
                            }
                        }
                        else if (DSPorterSettings.Is_SOTE)
                        {
                            orderRows = true;
                            foreach (var scaledObj in SOTE_ScaledObjectList)
                            {
                                var OGRow = param_old[scaledObj.OGModelID];
                                if (OGRow == null)
                                {
                                    continue;
                                }
                                PARAM.Row newObjRow = new(scaledObj.NewModelID, $"Scaled Object {scaledObj.NewModelName}", param_new_target.AppliedParamdef);
                                TransferParamRow(OGRow, newObjRow);
                                param_new_target.Rows.Add(newObjRow);
                            }
                        }
                    }

                    for (var iRow = 0; iRow < param_old.Rows.Count; iRow++)
                    {
                        PARAM.Row row_ptde_modded = param_old.Rows[iRow];
                        PARAM.Row row_dsr_target = new(row_ptde_modded.ID, row_ptde_modded.Name, param_new_target.AppliedParamdef);
                        PARAM.Row? row_dsr_vanilla = param_new_target.Rows.Find(e => e.ID == row_ptde_modded.ID);
                        if (row_dsr_vanilla != null)
                        {
                            for (var iCell = 0; iCell < row_dsr_vanilla.Cells.Count; iCell++)
                            {
                                row_dsr_target.Cells[iCell].Value = row_dsr_vanilla.Cells[iCell].Value;
                            }
                        }
                        if (isDrawParam)
                        {
                            // DrawParam
                            if (param_vanilla == null)
                                break;

                            PARAM.Row? row_new = paramRows_new.Find(r => r.ID == row_ptde_modded.ID);
                            PARAM.Row? row_vanilla = param_vanilla.Rows.Find(r => r.ID == row_ptde_modded.ID);
                            if (row_new == null || row_vanilla == null)
                            {
                                TransferParamRow(row_ptde_modded, row_dsr_target);
                                param_new_target.Rows.Add(row_dsr_target);
                                continue;
                            }

                            if (DSPorterSettings.Is_SOTE && param_old.ParamType == "POINT_LIGHT_BANK")
                            {
                                if (item.Key.StartsWith("m13"))
                                {
                                    // TOTG darkness
                                    if (row_new.ID == 14)
                                    {
                                        row_new["dwindleEnd"].Value = 10.0f; // From 8
                                        row_new["colA"].Value = 180.0f; // From 170
                                    }
                                    else if (row_new.ID == 44)
                                    {
                                        row_new["dwindleEnd"].Value = 14.0f; // From 12
                                    }
                                }
                            }
                            else if (DSPorterSettings.Is_SOTE && param_old.ParamType == "LIGHT_SCATTERING_BANK")
                            {
                                OffsetDrawParamRow(row_ptde_modded, row_new, row_vanilla);
                                if (item.Key.StartsWith("m14"))
                                {
                                    if (row_new.ID == 15)
                                    {
                                        // izalith city
                                        row_new["sunA"].Value = (short)100;
                                        row_new["reflectanceA"].Value = (short)100;
                                        row_new["blendCoef"].Value = (short)70;
                                    }
                                    else if (row_new.ID == 30)
                                    {
                                        // izalith shortcut
                                        row_new["sunA"].Value = (short)150;
                                        row_new["reflectanceA"].Value = (short)100;
                                    }
                                }
                            }
                            else if (DSPorterSettings.Is_SOTE && param_old.ParamType == "LIGHT_BANK")
                            {
                                // EXPERIMENTAL: use least shiny value
                                // Seemingly may want this as an option for non-sote (perhaps only in cases of modified params)!!
                                // envSpc_colA: use the smallest value (shininess)
                                // envDif_colA: use the smallest value (brightness refraction?)
                                short moddedSpcA = (short)row_ptde_modded["envSpc_colA"].Value;
                                short vanillaSpcA = (short)row_vanilla["envSpc_colA"].Value;
                                short dsrSpcA = (short)row_new["envSpc_colA"].Value;
                                short moddedDifA = (short)row_ptde_modded["envDif_colA"].Value;
                                short vanillaDifA = (short)row_vanilla["envDif_colA"].Value;
                                short dsrDifA = (short)row_new["envDif_colA"].Value;
                                OffsetDrawParamRow(row_ptde_modded, row_new, row_vanilla);
                                if (dsrSpcA > moddedSpcA)
                                {
                                    row_new["envSpc_colA"].Value = moddedSpcA;
                                }
                                else
                                {
                                    row_new["envSpc_colA"].Value = dsrSpcA;
                                }

                                // added this v2
                                if (item.Key.StartsWith("m14"))
                                {
                                    if (row_new.ID == 7 || row_new.ID == 14)
                                    {
                                        // lost izalith lava
                                        row_new["colA_u"].Value = (short)((short)row_new["colA_u"].Value * 0.6f);
                                        row_new["colA_d"].Value = (short)((short)row_new["colA_d"].Value * 0.6f);
                                        //row_new["envDif_colA"].Value = (short)row_new["envDif_colA"].Value * 0.4f;

                                        if (dsrDifA > moddedDifA)
                                        {
                                            row_new["envDif_colA"].Value = (short)(moddedDifA * 0.6f);
                                        }
                                        else
                                        {
                                            row_new["envDif_colA"].Value = (short)(dsrDifA * 0.6f);
                                        }

                                    }
                                }
                            }
                            else
                            {
                                // Default
                                OffsetDrawParamRow(row_ptde_modded, row_new, row_vanilla);
                            }
                            param_new_target.Rows.Add(row_new);
                        }
                        else // isGameParam
                        {
                            // GameParam
                            TransferParamRow(row_ptde_modded, row_dsr_target);
                            param_new_target.Rows.Add(row_dsr_target);
                        }
                    }

                    if (DSPorterSettings.Is_SOTE && param_old.ParamType == "EQUIP_PARAM_GOODS_ST")
                    {
                        foreach (var row in param_new_target.Rows)
                        {
                            row["enable_pvp"].Value = (byte)1;
                        }
                    }
                    /*
                    if (DSPorterSettings.IS_SOTE && param_old.ParamType == "BULLET_PARAM_ST")
                    {
                    }
                    */
                    if (orderRows)
                    param_new_target.Rows = param_new_target.Rows.OrderBy(e => e.ID).ToList();
                });

                // Save each param, then the parambnd
                foreach (BinderFile file in bnd_new.Files)
                {
                    string name = Path.GetFileNameWithoutExtension(file.Name);
                    if (paramList_new.ContainsKey(name))
                        file.Bytes = paramList_new[name].Write();
                }

                //File.WriteAllLines("output\\debugOutput.txt", _debugOutput);

                Util.WritePortedSoulsFile(bnd_new, DataPath_PTDE_Mod, bndPath_old, CompressionType);
            }
            return;
        }

        private void DSRPorter_DrawParam()
        {
            string datapath = $@"{DataPath_PTDE_Mod}\param\DrawParam";
            DSRPorter_TransferParams(datapath, true);
            Debug.WriteLine("Finished: DrawParam");
            OutputLog.Add($@"Finished: param\DrawParam\*.parambnd");
        }
        private void DSRPorter_GameParam()
        {
            string datapath = $@"{DataPath_PTDE_Mod}\param\GameParam";
            DSRPorter_TransferParams(datapath, false);
            Debug.WriteLine("Finished: GameParam");
            OutputLog.Add($@"Finished: param\GameParam\*.parambnd");
        }

        private void DSRPorter_ANIBND()
        {
            var paths = Directory.GetFiles($@"{DataPath_PTDE_Mod}\chr", "*.anibnd");
            if (paths.Length == 0)
                return;
            foreach (var bndPath_old in paths)
            {
                string bndPath_new = bndPath_old.Replace(DataPath_PTDE_Mod, $@"{DataPath_DSR}") + ".dcx";
                var bnd_old = BND3.Read(bndPath_old);
                var bnd_new = BND3.Read(bndPath_new);
                ModifyEntityBND(bnd_old, bnd_new);

                Util.WritePortedSoulsFile(bnd_old, DataPath_PTDE_Mod, bndPath_old, CompressionType);
            }
            Debug.WriteLine("Finished: ANIBNDs");
            OutputLog.Add($@"Finished: chr\*.anibnd");
        }

        private void DSRPorter_CHRBND()
        {
            var paths = Directory.GetFiles($@"{DataPath_PTDE_Mod}\chr", "*.chrbnd");
            if (paths.Length == 0)
                return;
            foreach (var bndPath_old in paths)
            {
                string bndPath_new = bndPath_old.Replace(DataPath_PTDE_Mod, $@"{DataPath_DSR}") + ".dcx";
                var bnd_old = BND3.Read(bndPath_old);
                var bnd_new = BND3.Read(bndPath_new);
                ModifyEntityBND(bnd_old, bnd_new);

                Util.WritePortedSoulsFile(bnd_old, DataPath_PTDE_Mod, bndPath_old, CompressionType);
            }
            Debug.WriteLine("Finished: CHRBNDs");
            OutputLog.Add($@"Finished: chr\*.chrbnd");
        }

        private void ModifyTAE(BinderFile taeBinder)
        {
            TAE tae = null!;
            bool write = false;

            if (DSPorterSettings.Is_SOTE &&
                taeBinder.Name.ToLower().Contains("c0000") &&
                taeBinder.Name.ToLower().Contains("a00.tae"))
            {
                tae = TAE.Read(taeBinder.Bytes);
                tae.ApplyTemplate(TaeTemplate);

                // Modify root motion of c0000 jump animation
                var jumpAnim = tae.Animations.Find(e => e.ID == 900); // running jump

                if (jumpAnim != null)
                {
                    foreach (var animEvent in jumpAnim.Events)
                    {
                        if (animEvent.TypeName == "ActivateJumpTableEarly")
                        {
                            if ((short)animEvent.Parameters["JumpTableID_ToActivateEarly"] == 27)
                            {
                                animEvent.EndTime = 9f / 30f; // PTDE: 13/30. vanilla: 9/30
                                write = true;
                                break;
                            }
                        }
                    }
                }
            }
            if (write)
            {
                taeBinder.Bytes = tae.Write();
            }
        }

        /// <summary>
        /// Recursive func for porting bnd files used for objects and characters.
        /// Uses DSR .hkx files, but ports everything else.
        /// </summary>
        private void ModifyEntityBND(BND3 bnd_old, BND3 bnd_new)
        {
            foreach (var file_old in bnd_old.Files.ToList())
            {
                if (file_old.Name.ToLower().EndsWith(".hkx"))
                {
                    // Check if DSR has this new HKX file so it can be logged if necessary.
                    file_old.Name = file_old.Name.Replace("win32", "x64");
                    if (bnd_new.Files.Find(e => e.Name == file_old.Name) == null)
                    {
                        if (DSPorterSettings.Is_SOTE)
                        {
                            if (file_old.Name == @"N:\FRPG\data\Model\chr\c5250\hkxx64\a01_3029.hkx")
                            {
                                // Port new ceaseless animation (not TAE hkx override due to some funny bug). Desired animation is in c5250_c5250.anibnd.dcx
                                file_old.Bytes = BND3.Read($@"{DataPath_DSR}\chr\c5250_c5250.anibnd.dcx").Files.Find(e => e.Name.EndsWith("a01_3004.hkx")).Bytes;
                                continue;
                            }
                            else if (file_old.Name == @"N:\FRPG\data\Model\chr\c5250\hkxx64\a01_3030.hkx")
                            {
                                // Port new ceaseless animation (not TAE hkx override due to some funny bug). Desired animation is in c5250_c5250.anibnd.dcx
                                file_old.Bytes = BND3.Read($@"{DataPath_DSR}\chr\c5250_c5250.anibnd.dcx").Files.Find(e => e.Name.EndsWith("a01_3019.hkx")).Bytes;
                                continue;
                            }
                        }
                        OutputLog.Add($"MANUAL PORT REQUEST: Skipped \"{file_old.Name}\" since it's a newly introduced modded HKX file. This file must be ported manually.");
                    }

                    bnd_old.Files.Remove(file_old);
                    continue;
                }
                if (file_old.Name.ToLower().EndsWith(".chrtpfbhd"))
                {
                    // TODO: to properly support this, scan to see if there was a modified chrtpfBDT in the chr folder.
                    //// If the BDT was unmodified, just use DSR BHD.
                    //// if the BDT was modified, I need to support that with a new function & probably use the moddedPTDE bhd w/ adjustments so it doesnt look like shit.
                    OutputLog.Add($"Overwrote {file_old.Name} with DSR version. If this file was modified, it must be ported manually. (Dev note: I can probably fix this. let me know if you REALLY need it).");
                    bnd_old.Files.Remove(file_old);
                    continue;
                }

                file_old.Name = file_old.Name.Replace("win32", "x64");

                if (BND3.Is(file_old.Bytes))
                {
                    BinderFile? file_new = bnd_new.Files.Find(e => file_old.Name == e.Name);
                    if (file_new == null)
                    {
                        if (DSPorterSettings.Is_SOTE && file_old.Name.Contains("6010"))
                        {
                            return;
                        }
                        else
                        {
                            OutputLog.Add($"MANUAL PORT REQUEST: Skipped \"{file_old.Name}\" since it couldn't be found in DSR data. This file must be ported manually.");
                            continue;
                        }
                    }
                    var modifiedBND = BND3.Read(file_old.Bytes);
                    ModifyEntityBND(modifiedBND, BND3.Read(file_new.Bytes));
                    file_old.Bytes = modifiedBND.Write();
                }
                else if (TPF.Is(file_old.Bytes))
                {
                    if (UseDsrTextures)
                    {
                        OutputLog.Add($"Overwrote {file_old.Name} with DSR version. If this file was modified, it must be ported manually. (Dev note: I can probably fix this. let me know if you REALLY need it).");
                        bnd_old.Files.Remove(file_old);
                    }
                }
                else if (TAE.Is(file_old.Bytes))
                {
                    ModifyTAE(file_old);
                }
            }

            foreach (var file_new in bnd_new.Files)
            {
                if (file_new.Name.ToLower().EndsWith(".hkx"))
                {
                    while (bnd_old.Files.Find(f => f.ID == file_new.ID) != null)
                    {
                        file_new.ID++;
                    }
                    bnd_old.Files.Add(file_new);
                    continue;
                }
                if (file_new.Name.ToLower().EndsWith(".chrtpfbhd"))
                {
                    while (bnd_old.Files.Find(f => f.ID == file_new.ID) != null)
                    {
                        file_new.ID++;
                    }
                    bnd_old.Files.Add(file_new);
                    continue;
                }
                if (TPF.Is(file_new.Bytes))
                {
                    if (UseDsrTextures)
                    {
                        while (bnd_old.Files.Find(f => f.ID == file_new.ID) != null)
                        {
                            file_new.ID++;
                        }
                        bnd_old.Files.Add(file_new);
                        continue;
                    }
                }
            }
            bnd_old.Files = bnd_old.Files.OrderBy(e => e.ID).ToList(); // This matters.  
        }

        private bool _objBNDFinished = false;
        private void DSRPorter_OBJBND()
        {
            var paths = Directory.GetFiles($@"{DataPath_PTDE_Mod}\obj", "*.objbnd");
            if (paths.Length == 0)
                return;
            foreach (var bndPath_old in paths)
            {
                string bndPath_new = bndPath_old.Replace(DataPath_PTDE_Mod, $@"{DataPath_DSR}") + ".dcx";
                var bnd_old = BND3.Read(bndPath_old);
                var bnd_new = BND3.Read(bndPath_new);
                ModifyEntityBND(bnd_old, bnd_new);

                Util.WritePortedSoulsFile(bnd_old, DataPath_PTDE_Mod, bndPath_old, CompressionType);
            }
            Debug.WriteLine("Finished: OBJBND");
            OutputLog.Add($@"Finished: obj\*.objbnd");
            _objBNDFinished = true;
        }

        private void DSRPorter_ESD()
        {
            var paths = Directory.GetFiles($@"{DataPath_PTDE_Mod}\script\talk", "*.talkesdbnd");
            if (paths.Length == 0)
                return;
            foreach (var path in paths)
            {
                var bnd = BND3.Read(path);
                foreach (var file in bnd.Files)
                {
                    file.Name = file.Name.Replace("win32", "x64");
                    var esd = ESD.Read(file.Bytes);
                    esd.LongFormat = true;
                    file.Bytes = esd.Write();
                }
                Util.WritePortedSoulsFile(bnd, DataPath_PTDE_Mod, path, CompressionType);
            }
            OutputLog.Add($@"Finished: script\talk\*.esd");

            foreach (var path in Directory.GetFiles($@"{DataPath_PTDE_Mod}\chr", "*.esd"))
            {
                var esd = ESD.Read(path);
                esd.LongFormat = true;
                Util.WritePortedSoulsFile(esd, DataPath_PTDE_Mod, path, CompressionType);
            }
            Debug.WriteLine("Finished: ESDs");
            OutputLog.Add($@"Finished: chr\*.esd");
        }

        private void DSRPorter_GenericFiles(string directory, string searchPattern)
        {
            var paths = Directory.GetFiles($@"{DataPath_PTDE_Mod}\{directory}", searchPattern);
            if (paths.Length == 0)
                return;
            foreach (var path in paths)
            {
                File.Copy(path, Util.GetOutputPath(DataPath_PTDE_Mod, path, false), true);
            }
            Debug.WriteLine("Finished: Generic Files");
            OutputLog.Add($@"Finished: {directory}\{searchPattern}"); ;
        }

        private void DSRPorter_GenericBNDs(string directory, string searchPattern, bool compress, bool searchInnerFolders = false)
        {
            SearchOption searchOption = searchInnerFolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            var paths = Directory.GetFiles($@"{DataPath_PTDE_Mod}\{directory}", searchPattern, searchOption);
            if (paths.Length == 0)
                return;

            foreach (var path in paths)
            {
                BND3 bnd = BND3.Read(path);
                foreach (var file in bnd.Files)
                {
                    file.Name = file.Name.Replace("win32", "x64");
                }
                if (compress)
                {
                    Util.WritePortedSoulsFile(bnd, DataPath_PTDE_Mod, path, CompressionType);
                }
                else
                {
                    Util.WritePortedSoulsFile(bnd, DataPath_PTDE_Mod, path);
                }
            }
            Debug.WriteLine("Finished: Generic BNDs");
            OutputLog.Add($@"Finished: {directory}\{searchPattern}");
        }
        private byte[] CompileLua(byte[] bytes)
        {
            // Save bytes to working dir
            string inputPath = $@"{LuaCompilationPath}\lua.in";
            File.WriteAllBytes(inputPath, bytes);

            // Use ProcessStartInfo class
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = true;
            startInfo.RedirectStandardInput = false;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.WorkingDirectory = LuaCompilationPath;
            startInfo.FileName = $@"luac50.exe";
            startInfo.Arguments = $" -s -l -o \"lua.out\" \"{inputPath}\"";

            // Start the process with the info we specified.
            // Call WaitForExit and then the using statement will close.
            using (Process exeProcess = Process.Start(startInfo))
            {
                exeProcess.WaitForExit();
            }

            return File.ReadAllBytes($@"{LuaCompilationPath}\lua.out");
        }

        private void DSRPorter_LUABND()
        {
            var files_old = Directory.GetFiles($@"{DataPath_PTDE_Mod}\script", "*.luabnd");
            if (files_old.Length == 0)
                return;

            List<BinderFile> files_DSR_list = new();
            foreach (var file in Directory.GetFiles($@"{DataPath_DSR}\script", "*.luabnd.dcx"))
            {
                files_DSR_list.AddRange(BND3.Read(file).Files);
            }
            foreach (var bndPath_old in files_old)
            {
                var bnd_PTDE_Target = BND3.Read(bndPath_old);
                int count = 0;

                foreach (var file_PTDE_Target in bnd_PTDE_Target.Files.ToList())
                {
                    file_PTDE_Target.Name = file_PTDE_Target.Name.Replace("win32", "x64");
                    if (file_PTDE_Target.Name.ToUpper().EndsWith(".LUA"))
                    {
                        var luaHeader = file_PTDE_Target.Bytes[..4];
                        if (luaHeader.SequenceEqual(new byte[4] { 0x1B, 0x4C, 0x75, 0x61 }))
                        {
                            // This is compiled lua. 32 bit compiled lua cannot be used in DSR, so use DSR instead.
                            BinderFile? file_DSR = files_DSR_list.Find(e => e.Name == file_PTDE_Target.Name);
                            if (file_DSR == null)
                            {
                                MessageBox.Show($"Error: \"{file_PTDE_Target.Name}\" is both compiled and cannot be found in DSR data" +
                                    $"\n\nIf this is a new lua file, please provide decompiled lua instead. Otherwise, fix DSR version being unfindable (name must be identical)."
                                    , "luaBND porting cancelled", MessageBoxButtons.OK);
                                return;
                            }
                            file_PTDE_Target.Bytes = file_DSR.Bytes;
                        }
                        else
                        {
                            // This is decompiled lua. Can be used in DSR safely.
                            if (DSPorterSettings.CompileLua)
                            {
                                try
                                {
                                    file_PTDE_Target.Bytes = CompileLua(file_PTDE_Target.Bytes);
                                }
                                catch(Exception e)
                                {
                                    Debugger.Break();
                                    OutputLog.Add($"Could not compile lua AI: {e.Message}. Decompiled lua used instead.");
                                }
                            }
                            count++;
                        }
                    }
                    else
                    {
                        BinderFile? file_new = files_DSR_list.Find(e => e.Name == file_PTDE_Target.Name);
                        if (file_PTDE_Target.Name.ToUpper().Contains("LUAINFO"))
                        {
                            LUAINFO DSRLuaInfo = LUAINFO.Read(file_new.Bytes);
                            LUAINFO PTDELuaInfo = LUAINFO.Read(file_PTDE_Target.Bytes);
                            foreach (var dsrinfo in DSRLuaInfo.Goals)
                            { 
                                if (PTDELuaInfo.Goals.Find(e => e.Name == dsrinfo.Name) == null)
                                {
                                    PTDELuaInfo.Goals.Add(dsrinfo);
                                }
                            }
                        }
                        else if (file_PTDE_Target.Name.ToUpper().Contains("GNL"))
                        {
                            //LUAGNL DSRLuaGNL = LUAGNL.Read(file_new.Bytes);
                            //LUAGNL PTDELuaGNL = LUAGNL.Read(file_PTDE_Target.Bytes);

                            // Delete all LUAGNL
                            bnd_PTDE_Target.Files.Remove(file_PTDE_Target);

                        }
                        else
                        {
                            OutputLog.Add($@"Unsupported file type was skipped: {Path.GetFileName(bndPath_old)}\{Path.GetFileName(file_PTDE_Target.Name)}");
                        }
                    }
                }

                if (count == 0)
                {
                    if (MessageBox.Show(
                        $"No decompiled lua could be found in {bndPath_old}. Only decompiled lua can be ported!" +
                        $"\n\nCancel operation?",
                        "Error", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        return;
                    }
                }

                Util.WritePortedSoulsFile(bnd_PTDE_Target, DataPath_PTDE_Mod, bndPath_old, CompressionType);
            }

            Debug.WriteLine("Finished: LUABND");
            OutputLog.Add($@"Finished: script\*.luabnd");
        } 

        private void DSRPorter_GenericTPFs(string directory, string searchPattern, bool compress)
        {
            throw new NotSupportedException();
        }

        public void LogUnportedFiles()
        {
            List<string> ptdeFiles = Directory.GetFiles(DataPath_PTDE_Mod, "*", SearchOption.AllDirectories).ToList();
            List<string> portedFiles = Directory.GetFiles(DataPath_Output, "*", SearchOption.AllDirectories).ToList();
            foreach (var ptdeFile in ptdeFiles)
            {
                string ptdeFileName = Path.GetFileName(ptdeFile).Replace(".dcx", "");
                if (portedFiles.Find(l => Path.GetFileName(l).Replace(".dcx", "") == ptdeFileName) == null)
                {
                    OutputLog.Add($"Unsupported file was not ported: \"{ptdeFile.Replace($@"{DataPath_PTDE_Mod}\", "")}\"");
                }
            }
        }

        public void DSRPorter_ObjTextures()
        {
            while (!_MSBFinished || !_objBNDFinished)
            {
                // Wait for MSB to populate list of objects transferred between maps
                Thread.Sleep(1000);
            }

            TexturePorter texPorter = new(this);
            int progressBarTotal = 280;

            if (DSPorterSettings.Is_SOTE)
            {
                progressBarTotal /= 2;
                foreach (var scaledObj in SOTE_ScaledObjectList)
                {
                    texPorter.SelfContainTextures_Objbnd(scaledObj.NewModelName);
                    string outputPath = $@"{DataPath_Output}\obj\{scaledObj.NewModelName}.objbnd.dcx";
                    if (!File.Exists(outputPath))
                    {
                        if (scaledObj.NewModelID == 4612) // root seals
                            continue;
                        File.Copy($@"{DataPath_DSR}\obj\{scaledObj.NewModelName}.objbnd.dcx", outputPath);
                    }
                    _progressBar.Invoke(() => _progressBar.Increment(1 + progressBarTotal / SOTE_ScaledObjectList.Count));
                }
                OutputLog.Add($@"Ported all pre-scaled SOTE objects.");
            }

            OutputLog.Add($@"Adding self-contained textures to objects added to new maps");
            foreach (var obj in _objsToPort)
            {
                texPorter.SelfContainTextures_Objbnd(obj);
                OutputLog.Add($@"Added self-contained textures: {obj}");
                _progressBar.Invoke(() => _progressBar.Increment(1 + progressBarTotal / _objsToPort.Count));
            }
        }

        public void SOTE_MoveScaledObjectAnims()
        {
            var outputPaths = Directory.GetFiles($@"{DataPath_Output}\obj", "*.objbnd.dcx");
            foreach (var sObj in SOTE_ScaledObjectList)
            {
                if (sObj.NewModelID == 4612)
                    continue;
                string bndPath_target = $@"{DataPath_Output}\obj\{sObj.NewModelName}.objbnd.dcx";
                string bndPath_vanilla = $@"{DataPath_DSR}\obj\{sObj.OGModelName}.objbnd.dcx";
                var bnd_target = BND3.Read(bndPath_target);
                var bnd_vanilla = BND3.Read(bndPath_vanilla);

                foreach (var objbndFile in bnd_target.Files)
                {
                    if (objbndFile.Name.EndsWith(".anibnd"))
                    {
                        BND3 mBND = BND3.Read(objbndFile.Bytes);
                        BND3 vBND;
                        BinderFile? vFile = bnd_vanilla.Files.Find(e => e.Name.Replace(sObj.OGModelName, sObj.NewModelName) == objbndFile.Name);
                        if (vFile == null)
                        {
                            // I don't think this triggers ATM.
                            // old comment: this should be the root seal (o4610 -> o4612). apparently vanilla anims work fine for that so don't worry about it.
                            continue;

                        }
                        else
                        {
                            vBND = BND3.Read(vFile.Bytes);
                        }
                        foreach (var f in mBND.Files)
                        {
                            if (f.Name.ToLower().EndsWith(".hkx"))
                            {
                                if (!f.Name.ToLower().StartsWith("skeleton"))
                                {
                                    var vAnim = vBND.Files.Find(e => e.Name.Replace(sObj.OGModelName, sObj.NewModelName) == f.Name);
                                    if (vAnim == null)
                                    {
                                        throw new Exception("SOTE: Animation in modded scaled object objbnd cannot be found in vanilla DSR animations. This shouldn't happen!");
                                    }
                                    f.Bytes = vAnim.Bytes;
                                }
                            }
                        }
                        objbndFile.Bytes = mBND.Write();
                    }
                }


                Util.WritePortedSoulsFile(bnd_target, DataPath_Output, bndPath_target, CompressionType);
            }
            Debug.WriteLine("Finished: SOTE OBJBND VANILLA ANIM TRANSFER");
            OutputLog.Add($@"Finished: obj\*.objbnd");
        }

        public void Run()
        {
            try
            {
                if (Directory.Exists(DataPath_Output))
                {
                    var result = MessageBox.Show("Output folder already exists. Delete all output files before proceeding?", "Delete output folder?", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        result = MessageBox.Show("Send to recycle bin?\n", "Recycle vs delete", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
                            Microsoft.VisualBasic.FileIO.FileSystem.DeleteDirectory(DataPath_Output,
                                Microsoft.VisualBasic.FileIO.UIOption.AllDialogs,
                                Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
                        }
                        else
                        {
                            Microsoft.VisualBasic.FileIO.FileSystem.DeleteDirectory(DataPath_Output,
                                Microsoft.VisualBasic.FileIO.UIOption.AllDialogs,
                                Microsoft.VisualBasic.FileIO.RecycleOption.DeletePermanently);
                        }
                    }
                }

                Directory.CreateDirectory(DataPath_Output);

                OutputLog.Add("Notice: All .hkx files were overwritten with copies from DSR. Modifications for these will not be ported.");
                List<Task> taskList = new();

                taskList.AddRange(new List<Task>()
                {
                    Task.Run(() => DSRPorter_MSB()),
                    Task.Run(() => DSRPorter_CHRBND()),
                    Task.Run(() => DSRPorter_OBJBND()),
                    Task.Run(() => DSRPorter_LUABND()),
                    Task.Run(() => DSRPorter_ANIBND()),
                    Task.Run(() => DSRPorter_ESD()),
                    Task.Run(() => DSRPorter_MSGBND()),
                    Task.Run(() => DSRPorter_EMEVD()),
                    Task.Run(() => DSRPorter_FFX()),

                    Task.Run(() => DSRPorter_GenericFiles(@"sound", "*")),
                    Task.Run(() => DSRPorter_GenericBNDs(@"parts", "*.partsbnd", true)),

                    Task.Run(() => DSRPorter_ObjTextures()),

                    //
                    Task.Run(() => _paramdefs_ptde = Util.LoadParamDefXmls("DS1")),
                    Task.Run(() => _paramdefs_dsr = Util.LoadParamDefXmls("DS1R")),
                    Task.Run(() => DSRPorter_GameParam()),
                    Task.Run(() => DSRPorter_DrawParam()),
                    //
                });

                var taskCount = taskList.Count;
                while (taskList.Any())
                {
                    Task[] taskArray = taskList.ToArray();
                    Task.WaitAny(taskArray);
                    foreach (var task in taskArray)
                    {
                        if (task.IsCompleted)
                        {
                            if (task.Exception != null)
                            {
                                throw task.Exception;
                            }
                            _progressBar.Invoke(() => _progressBar.Increment(1 + 700 / taskCount));
                            taskList.Remove(task);
                        }
                    }
                }

                foreach (var path in Directory.GetFiles(OutputOverwritePath, "*", SearchOption.AllDirectories))
                {
                    var targetPath = $@"{DataPath_Output}\{path.Replace(OutputOverwritePath, "")}";
                    string fileName = Path.GetFileName(path);
                    Directory.CreateDirectory(targetPath.Replace(fileName, ""));
                    File.Copy(path, targetPath, true);
                    OutputLog.Add($"Ported manually prepared file \"{fileName}\"");
                }
                foreach (var obj in SOTE_ScaledObjectList)
                {
                    if (!File.Exists($@"{DataPath_Output}\obj\{obj.NewModelName}.objbnd.dcx"))
                    {
                        OutputLog.Add($"Couldn't find scaled object \"{obj.NewModelName}\" in the output folder!");
                    }
                }
                if (DSPorterSettings.Is_SOTE)
                {
                    SOTE_MoveScaledObjectAnims();
                }

                LogUnportedFiles();

                File.WriteAllLines($@"{DataPath_Output}\Output Log.txt", OutputLog.OrderBy(e => e));
            }
            catch (Exception e)
            {
                // Capture exception to be thrown later.
                PorterException = System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(e);
            }

            _progressBar.Invoke(() => _progressBar.Value = _progressBar.Maximum);
        }
    }
}
