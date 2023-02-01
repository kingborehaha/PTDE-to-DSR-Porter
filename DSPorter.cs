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

/*
 * TODO

 THINGS THAT NEED TO BE CONVEYED IN EDITOR
 * Porting DrawParam should be done in conjunction with MSB porting due to DSR's new IDs
 * modified lua must be decompiled since 32 bit lua can't be decompiled (and thus converted)
    * any compiled lua will just be replaced with DSR lua, unless the DSR lua cannot be found in which case program will complain.
 * scaled collision in MSB (maybe program should also report this, with vanilla ones blacklisted)

 DRAW PARAM
 * drawparam rows introduced in DSR are a concern, since if i overwite those, MSB references will be in trouble.
 * * maybe just make manual exceptions for these rows?
 * * maybe manual exceptions that are only allowed when relevent MSBs are not being overwritten?

 msb
 * option to keep DSR drawgroup improvements?
 * Scaled objects. Need to make new objects and scale down their collision to whatever scale I need.
 * * or, go through list and pray there are acceptable alternatives for every single one (yikes. especially for fog wall blockers)


//// SOTE stuff
 EXE
 * use tk's propertyhook
 * maybe remake PTDE's while I'm at it
 main menu
    texture
    intro video replacement
 Random textures?
    Those lecturns in respite were missing textures. Texture issue, or cross-map loading issue? (see if duke's ones look right)
 FFX
 * There are some DSR FFX I don't want to replace, and some FFX I want the PTDE versions of.
    * Maybe do a diff between vanilla PTDE and SOTE to see which FFX I modified, and include those (sans crystal FFX)
    * Then do a playthrough with debug on and if I see any FFX I hate (chaos FFX from ceaseless, other chaos boys come to mind) then I add it to a list which program reads and transfers when present.
 OBJ
 * see what changes i made to these objects in devlogs. root seal is especially concerning, dont remember which new file i made for that (TAE?)
    * re: root seals. i think i scaled all of these, so changes would need to be made to scaled dupes
 LUA
 * need to check if game has enough memory for all that. May need to compile it to 64 bit?
 chrBNDs
 * just titanite demon, right?
 * scaled objects
    m10_01_00_00 - o1413 | o1201_n_blocker_1 - <1, 1.5, 1>
    m10_01_00_00 - o1413 | o1201_n_blocker_2 - <1, 2, 1>
    m10_01_00_00 - o1413 | o1201_n_blocker_3 - <1, 2, 1>
    m12_00_00_01 - o1312 | o_n_door_0001 - <1.36, 1.83, 0.95>
    m12_01_00_00 - o8300 | o_n_NGplus_1 - <0.5, 0.5, 0.5>
    m12_01_00_00 - o2403 | o_n_staterefresh - <1.5, 1.5, 1.5>
    m12_01_00_00 - o3010 | o_n_hurtstone - <0.5, 0.5, 0.5>
    m13_02_00_00 - o1419 | o_n_blocker_ramp - <0.5, 1, 1>
    m14_01_00_00 - o4510 | o4510_n_illusorywall_1 - <1.6, 1.6, 1.6>
    m14_01_00_00 - o4830 | o_n_turret_obj - <16, 16, 16>
    m14_01_00_00 - o4830 | o_n_turret_obj_0001 - <16, 16, 16>
    m15_00_00_00 - o8540 | o_n_blocker_2 - <2, 2, 2>
    m16_00_00_00 - o6700 | o_n_OneWayWall - <2.3, 1.5, 1>
    m18_00_00_00 - o8051 | o8051_n_fog_eclipse_2.1 - <1, 1, 4>
    m18_00_00_00 - o8300 | o8300_n - <0.25, 0.15, 0.25>
    // BOC root seals
    m14_01_00_00 - o4610 | o4610_0000 - <0.75, 0.75, 0.75>
    m14_01_00_00 - o4610 | o4610_0001 - <0.75, 0.75, 0.75>
    m14_01_00_00 - o4610 | o4610_n_tendril_0001 - <0.75, 0.75, 0.75>
    m14_01_00_00 - o4610 | o4610_n_tendril_0002 - <0.75, 0.75, 0.75>
    m14_01_00_00 - o4610 | o4610_n_tendril_0003 - <0.75, 0.75, 0.75>

    //can be worked around
    //little jank
    //replaced obj
        (ceaseless arena gates blocking the little area)
        m14_01_00_00 - o4700 | o4700_n_blocker_1 - <1.1, 1.2, 1.1>
        m14_01_00_00 - o4700 | o4700_n_blocker_2 - <1.1, 1.2, 1.1>

    //these MIGHT be fine?
        //I forget if I disable collision on these during gameplay

    //these are fine:
        //vanilla
        m10_00_00_00 - o1057 | o1057_01 - <1.4999995, 1.5, 1.4999995>
        m10_00_00_00 - o1057 | o1057_04 - <0.8, 0.8, 0.8>
        m10_00_00_00 - o1057 | o1057_08 - <1.4999995, 1.5, 1.4999995>
        m10_00_00_00 - o1057 | o1057_12 - <1.2, 0.7, 0.7>
        m10_00_00_00 - o1057 | o1057_16 - <0.89999974, 0.9, 0.89999974>
        m10_00_00_00 - o1057 | o1057_17 - <0.8, 0.8, 0.8>
        m10_00_00_00 - o1057 | o1057_20 - <0.7999998, 0.8, 0.7999998>
        m10_00_00_00 - o1057 | o1057_22 - <2, 0.8, 0.5>
        // i don't remember
        m18_01_00_00 - o8550 | o8550_0001 - <0.458642, 0.458642, 0.458642>
        // SOTE
       m10_01_00_00 - o1317 | o1317_0001 - <1.05, 1, 1>
 */



namespace DSRPorter
{
    public partial class DSPorter
    {
        private string _dataPath_PTDE = "";
        private string _dataPath_DSR = "";

        private const string _outputPath = "output";
        private const DCX.Type _compressionType = DCX.Type.DCX_DFLT_10000_24_9;

        private ConcurrentBag<PARAMDEF> _paramdefs_ptde = new();
        private ConcurrentBag<PARAMDEF> _paramdefs_dsr = new();
        private List<ScaledObject> _scaledObjects = new();

        private ConcurrentBag<string> _outputLog = new();

        private readonly bool _enableScaledObjectAdjustments = false;
        private readonly bool _Is_SOTE = true;

        private readonly bool _useDSRToneMapBankValues = true;

        private readonly ProgressBar _progressBar;

        public DSPorter(ProgressBar progressBar)
        {
            _progressBar = progressBar;
        }

        private void DSRPorter_EMEVD()
        {
            var paths = Directory.GetFiles($@"{_dataPath_PTDE}\event", "*.emevd");
            if (paths.Length == 0)
                return;
            foreach (var path in paths)
            {
                var emevd = EMEVD.Read(path);
                Util.WritePortedSoulsFile(emevd, _dataPath_PTDE, path, _compressionType);
            }
            Debug.WriteLine("Finished: EMEVD");
            _outputLog.Add($@"Finished: event\*.emevd");
        }

        private bool _MSBFinished = false;
        private void DSRPorter_MSB()
        {
            _MSBFinished = false;
            var paths = Directory.GetFiles($@"{_dataPath_PTDE}\map\mapstudio", "*.msb");
            if (paths.Length == 0)
                return;

            _scaledObjects.Clear();
            foreach (var path in paths)
            {
                string msbName = Path.GetFileNameWithoutExtension(path);
                var msb = MSB1.Read(path);

                foreach (var p in msb.Parts.Objects)
                {
                    if (_enableScaledObjectAdjustments && Util.HasModifiedScaling(p.Scale))
                    {
                        string? newModelName = null;
                        foreach (ScaledObject scaledObj in _scaledObjects)
                        {
                            if (scaledObj.Matches(p.ModelName, p.Scale))
                            {
                                newModelName = scaledObj.NewModelName;
                                p.ModelName = newModelName;
                                break;
                            }
                        }
                        if (newModelName == null)
                        {
                            ScaledObject scaledObj = CreateScaledObject(p.ModelName, p.Scale);
                            newModelName = scaledObj.NewModelName;
                            p.ModelName = newModelName;
                            _scaledObjects.Add(scaledObj);
                        }

                        if (msb.Models.Objects.Find(e => e.Name == newModelName) == null)
                        {
                            MSB1.Model.Object model = new()
                            {
                                Name = newModelName
                            };
                            msb.Models.Objects.Add(model);
                        }
                    }
                }
                foreach (var p in msb.Parts.Collisions)
                {
                    if (true)
                    {
                        if (msbName == "m10_01_00_00")
                        {
                            if (p.Name == "h1023B1_0000")
                                p.Position = new Vector3(0, 0, -1.4f);
                            else if (p.Name == "h1000B1")
                                p.Position = new Vector3(0, 0, -0.1f);
                        }
                        else if (msbName == "m14_00_00_00")
                        {
                            if (p.Name == "h0020B0")
                                p.NvmGroups[0] = 2148532224; // 1048576-> 2148532224
                        }
                    }
                }
                Util.WritePortedSoulsFile(msb, _dataPath_PTDE, path);
            }
            Debug.WriteLine("Finished: MSB");
            _outputLog.Add($@"Finished: map\mapstudio\*.msb");
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
            var paths = Directory.GetFiles($@"{_dataPath_PTDE}\sfx", "*.ffxbnd");
            if (paths.Length == 0)
                return;

            foreach (var path in paths)
            {
                var bnd_old = BND3.Read(path);
                var bnd_new = BND3.Read(path.Replace(_dataPath_PTDE, _dataPath_DSR) + ".dcx");

                foreach (var file_old in bnd_old.Files)
                {
                    if (FXR1.Is(file_old.Bytes))
                    {
                        if (IsCoreFFX(file_old))
                        {
                            // Core FFX, must use base DSR version or things will break.
                            continue;
                        }

                        FXR1 ffx_old = FXR1.Read(file_old.Bytes);
                        ffx_old.Wide = true;
                        file_old.Bytes = ffx_old.Write();
                    }

                    file_old.Name = file_old.Name.Replace("win32", "x64");
                    var file_new = bnd_new.Files.Find(f => f.Name == file_old.Name);

                    if (file_new != null)
                    {
                        file_new.Bytes = file_old.Bytes;
                    }
                    else
                    {
                        while (bnd_new.Files.Find(f => f.ID == file_old.ID) != null)
                        {
                            file_old.ID++;
                        }
                        bnd_new.Files.Add(file_old);
                    }
                }

                bnd_new.Files = bnd_new.Files.OrderBy(e => e.ID).ToList();
                Util.WritePortedSoulsFile(bnd_new, _dataPath_PTDE, path, _compressionType);
            }
            Debug.WriteLine("Finished: FFX");
            _outputLog.Add($@"Finished: sfx\*.ffxbnd");
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
            }
            file_new.Bytes = fmg_old.Write();
        }

        private void DSRPorter_MSGBND()
        {
            var paths = Directory.GetFiles($@"{_dataPath_PTDE}\msg", "*.msgbnd", SearchOption.AllDirectories);
            if (paths.Length == 0)
                return;

            List<BinderFile> files_old = new();
            foreach (var path in paths)
            {
                files_old.AddRange(BND3.Read(path).Files);
            }
            foreach (var path in paths)
            {
                string path_new = path.Replace(_dataPath_PTDE, _dataPath_DSR) + ".dcx";
                if (!File.Exists(path_new))
                {
                    MessageBox.Show($"{path} couldn't be found in DSR data.", "Warning");
                    _outputLog.Add($"Skipped \"{path}\" since it couldn't be found in DSR data.");
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
                    switch (file_new.ID)
                    {
                        /*
                        case (int)SoulsResources.FmgIDType.MenuKeyGuide:
                        case (int)SoulsResources.FmgIDType.MenuKeyGuide_Patch:
                            // Just use DSR entries.
                            break;
                        */
                        default:
                            TransferFmgEntries(file_old, file_new);
                            break;
                    }
                }
                Util.WritePortedSoulsFile(bnd_new, _dataPath_PTDE, path, _compressionType);
            }
            Debug.WriteLine("Finished: MSGBND");
            _outputLog.Add($@"Finished: msg\*.msgbnd");
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

        private void DSRPorter_TransferParams(string datapath, bool isDrawParam = false)
        {
            var paths = Directory.GetFiles(datapath, "*.parambnd");
            if (paths.Length == 0)
                return;

            while (!_MSBFinished || !_paramdefs_ptde.Any() || !_paramdefs_dsr.Any())
            {
                Thread.Sleep(1000);
            }

            foreach (string bndPath_old in paths)
            {
                string bndPath_new = bndPath_old.Replace(_dataPath_PTDE, $@"{_dataPath_DSR}") + ".dcx";

                ConcurrentDictionary<string, PARAM> paramList_old = new();
                ConcurrentDictionary<string, PARAM> paramList_new = new();

                // Load params
                BND3 bnd_old = BND3.Read(bndPath_old);
                BND3 bnd_new = BND3.Read(bndPath_new);

                Util.ApplyParamDefs(_paramdefs_ptde, bnd_old.Files, paramList_old);
                Util.ApplyParamDefs(_paramdefs_dsr, bnd_new.Files, paramList_new);

                Parallel.ForEach(Partitioner.Create(paramList_old), item =>
                {
                    if (paramList_new.ContainsKey(item.Key) == false)
                    {
                        // Couldn't find matching param in other list.
                        if (item.Key == "default_ToneCorrectBank")
                        {
                            // DSR defs can't handle this, but it's all default values in PTDE so I doubt it matters
                        }
                        else if (_Is_SOTE
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

                    if (param_old.ParamType == "TONE_MAP_BANK")
                    {
                        if (_useDSRToneMapBankValues)
                        {
                            // Don't transfer row data for this param
                            return;
                        }
                    }

                    param_new_target.Rows.Clear();

                    if (_enableScaledObjectAdjustments && param_old.ParamType == "OBJECT_PARAM_ST")
                    {
                        foreach (var scaledObj in _scaledObjects)
                        {
                            // TODO: test
                            var OGRow = param_old[scaledObj.OGModelID];
                            PARAM.Row newObjRow = new(scaledObj.NewModelID, $"Scaled Object {scaledObj.NewModelName}", param_new_target.AppliedParamdef);
                            TransferParamRow(OGRow, newObjRow);
                            param_new_target.Rows.Add(newObjRow);
                        }
                    }

                    for (var iRow = 0; iRow < param_old.Rows.Count; iRow++)
                    {
                        PARAM.Row row_old = param_old.Rows[iRow];

                        PARAM.Row row_target = new(row_old.ID, row_old.Name, param_new_target.AppliedParamdef);
                        if (isDrawParam)
                        {
                            PARAM.Row? row_new = paramRows_new.Find(r => r.ID == row_old.ID);
                            //DEBUG_CompareDrawParamRows(item.Key, row_old, row_new);
                            AdjustDrawParamRow(param_old, row_old, row_new);
                        }
                        TransferParamRow(row_old, row_target);
                        param_new_target.Rows.Add(row_target);
                    }
                });
                // Save each param, then the parambnd
                foreach (BinderFile file in bnd_new.Files)
                {
                    string name = Path.GetFileNameWithoutExtension(file.Name);
                    if (paramList_new.ContainsKey(name))
                        file.Bytes = paramList_new[name].Write();
                }

                Util.WritePortedSoulsFile(bnd_new, _dataPath_PTDE, bndPath_old, _compressionType);
            }
            return;
        }

        private void DSRPorter_DrawParam()
        {
            string datapath = $@"{_dataPath_PTDE}\param\DrawParam";
            DSRPorter_TransferParams(datapath, true);
            Debug.WriteLine("Finished: DrawParam");
            _outputLog.Add($@"Finished: param\DrawParam\*.parambnd");
        }
        private void DSRPorter_GameParam()
        {
            string datapath = $@"{_dataPath_PTDE}\param\GameParam";
            DSRPorter_TransferParams(datapath, false);
            Debug.WriteLine("Finished: GameParam");
            _outputLog.Add($@"Finished: param\GameParam\*.parambnd");
        }

        private void DSRPorter_ANIBND()
        {
            var paths = Directory.GetFiles($@"{_dataPath_PTDE}\chr", "*.anibnd");
            if (paths.Length == 0)
                return;
            foreach (var bndPath_old in paths)
            {
                string bndPath_new = bndPath_old.Replace(_dataPath_PTDE, $@"{_dataPath_DSR}") + ".dcx";
                var bnd_old = BND3.Read(bndPath_old);
                var bnd_new = BND3.Read(bndPath_new);
                ModifyEntityBND(bnd_old, bnd_new);

                Util.WritePortedSoulsFile(bnd_old, _dataPath_PTDE, bndPath_old, _compressionType);
            }
            Debug.WriteLine("Finished: ANIBNDs");
            _outputLog.Add($@"Finished: chr\*.anibnd");
        }

        /// <summary>
        /// Recursive func for porting .anibnd and .objbnd
        /// </summary>
        private void ModifyEntityBND(BND3 bnd_old, BND3 bnd_new)
        {
            foreach (var file_old in bnd_old.Files.ToList())
            {
                if (file_old.Name.ToLower().EndsWith(".hkx"))
                {
                    bnd_old.Files.Remove(file_old);
                }
                else
                {
                    file_old.Name = file_old.Name.Replace("win32", "x64");
                }

                if (BND3.Is(file_old.Bytes))
                {
                    BinderFile? file_new = bnd_new.Files.Find(e => file_old.Name == e.Name);
                    if (file_new == null)
                    {
                        //MessageBox.Show($"{file_old.Name} can't be found in DSR data and will be skipped. This file must be ported manually.", "Warning");
                        _outputLog.Add($"Skipped \"{file_old.Name}\" since it couldn't be found in DSR data. This file must be ported manually.");
                        continue;
                    }
                    var modifiedBND = BND3.Read(file_old.Bytes);
                    ModifyEntityBND(modifiedBND, BND3.Read(file_new.Bytes));
                    file_old.Bytes = modifiedBND.Write();
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
                }
            }

            bnd_old.Files = bnd_old.Files.OrderBy(e => e.ID).ToList(); // This matters.  
        }

        private void DSRPorter_OBJBND()
        {
            var paths = Directory.GetFiles($@"{_dataPath_PTDE}\obj", "*.objbnd");
            if (paths.Length == 0)
                return;
            foreach (var bndPath_old in paths)
            {
                string bndPath_new = bndPath_old.Replace(_dataPath_PTDE, $@"{_dataPath_DSR}") + ".dcx";
                var bnd_old = BND3.Read(bndPath_old);
                var bnd_new = BND3.Read(bndPath_new);
                ModifyEntityBND(bnd_old, bnd_new);

                Util.WritePortedSoulsFile(bnd_old, _dataPath_PTDE, bndPath_old, _compressionType);
            }
            Debug.WriteLine("Finished: OBJBND");
            _outputLog.Add($@"Finished: obj\*.objbnd");
        }

        private void DSRPorter_ESD()
        {
            var paths = Directory.GetFiles($@"{_dataPath_PTDE}\script\talk", "*.talkesdbnd");
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
                Util.WritePortedSoulsFile(bnd, _dataPath_PTDE, path, _compressionType);
            }
            _outputLog.Add($@"Finished: script\talk\*.esd");

            foreach (var path in Directory.GetFiles($@"{_dataPath_PTDE}\chr", "*.esd"))
            {
                var esd = ESD.Read(path);
                esd.LongFormat = true;
                Util.WritePortedSoulsFile(esd, _dataPath_PTDE, path, _compressionType);
            }
            Debug.WriteLine("Finished: ESDs");
            _outputLog.Add($@"Finished: chr\*.esd");
        }

        private void DSRPorter_GenericFiles(string directory, string searchPattern)
        {
            var paths = Directory.GetFiles($@"{_dataPath_PTDE}\{directory}", searchPattern);
            if (paths.Length == 0)
                return;
            foreach (var path in paths)
            {
                File.Copy(path, Util.GetOutputPath(_dataPath_PTDE, path, false), true);
            }
            Debug.WriteLine("Finished: Generic Files");
            _outputLog.Add($@"Finished: {directory}\{searchPattern}"); ;
        }   

        private void DSRPorter_GenericBNDs(string directory, string searchPattern, bool compress, bool searchInnerFolders = false)
        {
            SearchOption searchOption = searchInnerFolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            var paths = Directory.GetFiles($@"{_dataPath_PTDE}\{directory}", searchPattern, searchOption);
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
                    Util.WritePortedSoulsFile(bnd, _dataPath_PTDE, path, _compressionType);
                }
                else
                {
                    Util.WritePortedSoulsFile(bnd, _dataPath_PTDE, path);
                }
            }
            Debug.WriteLine("Finished: Generic BNDs");
            _outputLog.Add($@"Finished: {directory}\{searchPattern}");
        }

        private void DSRPorter_LUABND()
        {
            var files_old = Directory.GetFiles($@"{_dataPath_PTDE}\script", "*.luabnd");
            if (files_old.Length == 0)
                return;

            List<BinderFile> files_new = new();
            foreach (var file in Directory.GetFiles($@"{_dataPath_DSR}\script", "*.luabnd.dcx"))
            {
                files_new.AddRange(BND3.Read(file).Files);
            }
            foreach (var bndPath_old in files_old)
            {
                var bnd_old = BND3.Read(bndPath_old);
                int count = 0;

                foreach (var file_old in bnd_old.Files.ToList())
                {
                    file_old.Name = file_old.Name.Replace("win32", "x64");
                    if (file_old.Name.ToLower().EndsWith(".lua"))
                    {
                        var luaHeader = file_old.Bytes[..4];
                        if (luaHeader.SequenceEqual(new byte[4] { 0x1B, 0x4C, 0x75, 0x61 }))
                        {
                            // This is compiled lua. 32 bit compiled lua cannot be used in DSR, so use DSR instead.
                            BinderFile? file_new = files_new.Find(e => e.Name == file_old.Name);
                            if (file_new == null)
                            {
                                MessageBox.Show($"\"{file_old.Name}\" is both compiled and cannot be found in DSR data" +
                                    $"\n\nIf this is a new lua file, please provide decompiled lua instead. Otherwise, fix DSR version being unfindable (name must be identical).", "Error");
                                return;
                            }
                            file_old.Bytes = file_new.Bytes;
                        }
                        else
                        {
                            // This is not compiled lua. Can be used in DSR safely.
                            count++;
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

                Util.WritePortedSoulsFile(bnd_old, _dataPath_PTDE, bndPath_old, _compressionType);
            }

            Debug.WriteLine("Finished: LUABND");
            _outputLog.Add($@"Finished: script\*.luabnd");
        } 

        private void DSRPorter_GenericTPFs(string directory, string searchPattern, bool compress)
        {
            /*
            foreach (var path in Directory.GetFiles($@"{_dataPath_PTDE}\{directory}", searchPattern))
            {
                TPF tpf = TPF.Read(path);
                foreach (TPF.Texture file in tpf)
                {
                    DDS dds = new(file.Bytes);
                    dds.
                    Debugger.Break();
                }
                if (compress)
                {
                    Util.WritePortedSoulsFile(file, _dataPath_PTDE, path, _compressionType);
                }
                else
                {
                    Util.WritePortedSoulsFile(file, _dataPath_PTDE, path);
                }
            }
            Debug.WriteLine("Finished: Generic TPFS");
            _outputLog.Add($@"Finished: {directory}\{searchPattern}");
            */
        }

        public void LogUnportedFiles()
        {
            List<string> ptdeFiles = Directory.GetFiles(_dataPath_PTDE, "*", SearchOption.AllDirectories).ToList();
            List<string> portedFiles = Directory.GetFiles(_outputPath, "*", SearchOption.AllDirectories).ToList();
            foreach (var ptdeFile in ptdeFiles)
            {
                string ptdeFileName = Path.GetFileName(ptdeFile).Replace(".dcx", "");
                if (portedFiles.Find(l => Path.GetFileName(l).Replace(".dcx", "") == ptdeFileName) == null)
                {
                    _outputLog.Add($"Unsupported file was not ported: \"{ptdeFile}\"");
                }
            }
        }
        public void IncrementTaskBar(int i)
        {
            _progressBar.Increment(i);
        }

        public void Run(string ptdePath, string dsrPath)
        {
            Directory.CreateDirectory(_outputPath);
            _dataPath_PTDE = ptdePath;
            _dataPath_DSR = dsrPath;

            List<Task> taskList = new()
            {
                Task.Run(() => DSRPorter_MSB()), // Done, needs more in-game testing though.
                Task.Run(() => DSRPorter_FFX()), // Done, needs more in-game testing though.
                Task.Run(() => DSRPorter_ESD()), // Done
                Task.Run(() => DSRPorter_EMEVD()), // Done
                Task.Run(() => DSRPorter_ANIBND()), // Done, may have per-enemy problems though.
                Task.Run(() => DSRPorter_OBJBND()), // test
                Task.Run(() => DSRPorter_MSGBND()), // Seems mostly good, needs more testing though. Also I should probably still convert button prompts
                Task.Run(() => DSRPorter_LUABND()), // Seems good? Needs more testing.

                Task.Run(() => DSRPorter_GenericFiles(@"map\breakobj", "*.breakobj")),
                Task.Run(() => DSRPorter_GenericFiles(@"sound", "*")),
                Task.Run(() => DSRPorter_GenericBNDs(@"parts", "*.partsbnd", true)), // TODO: make sure these actually work.

                Task.Run(() => _paramdefs_ptde = Util.LoadParamDefXmls("DS1")),
                Task.Run(() => _paramdefs_dsr = Util.LoadParamDefXmls("DS1R")),
                Task.Run(() => DSRPorter_GameParam()), // Done
                Task.Run(() => DSRPorter_DrawParam()) // Done, may need more manual adjustments. Do in-game testing.
            };
            var taskCount = taskList.Count;
            while (taskList.Any())
            {
                Task[] taskArray = taskList.ToArray();
                Task.WaitAny(taskArray);
                foreach (var task in taskArray)
                {
                    if (task.IsCompleted)
                    {
                        _progressBar.Invoke(new Action(() => _progressBar.Increment(1 + (100 / taskCount))));
                        taskList.Remove(task);
                    }
                }
            }
            
            _outputLog.Add("Notice: All .hkx files were overwritten with copies from DSR. Modifications for these will not be ported.");
            LogUnportedFiles();

            File.WriteAllLines($@"{_outputPath}\Output Log.txt", _outputLog.OrderBy(e => e));
            _outputLog.Clear();
            _paramdefs_ptde.Clear();
            _paramdefs_dsr.Clear();
        }
    }
}
