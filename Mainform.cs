using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;

namespace DSRPorter
{
    public partial class MainForm : Form
    {
        public static readonly string Version = Application.ProductVersion;
        public static readonly string ProgramTitle = $"PTDE to DSR porter v{Version}";

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            CheckEnableActivateButton();
            this.Text = ProgramTitle;

            // Set UI settings default values
            Setting_CompileLua.Checked = DSPorterSettings.CompileLua;
            Setting_IsSOTE.Checked = DSPorterSettings.Is_SOTE;
            Setting_SlimeCeilingFix.Checked = DSPorterSettings.SlimeCeilingFix;
            Setting_Misc_DSR_Collision.Checked = DSPorterSettings.MiscCollisionFixes;
            Setting_RenderGroupImprovements.Checked = DSPorterSettings.RenderGroupImprovements;

#if !DEBUG
            Setting_IsSOTE.Visible = false;
            Button_SOTE_set_paths.Visible = false;
#endif
        }

        private void CheckEnableActivateButton()
        {
            Button_Activate.Enabled = false;
            if (!PortingInProcess)
            {
                if (FolderBrowser_PTDE_Mod.SelectedPath != "" && FolderBrowser_PTDE_Vanilla.SelectedPath != "" && FolderBrowser_DSR.SelectedPath != "")
                {
                    Button_Activate.Enabled = true;
                }
            }
            return;
        }

        private void Button_Activate_Click(object sender, EventArgs e)
        {
            Task.Run(() => RunProgram());
        }

        public bool PortingInProcess = false;
        private void RunProgram()
        {
            Button_Activate.Invoke(() => Button_Activate.Enabled = false);
            PortingInProcess = true;

            ProgramProgressBar.Invoke(() => ProgramProgressBar.Value = 0);

            DSPorter porter = new(ProgramProgressBar);
            porter.DataPath_PTDE_Mod = FolderBrowser_PTDE_Mod.SelectedPath;
            porter.DataPath_PTDE_Vanilla = FolderBrowser_PTDE_Vanilla.SelectedPath;
            porter.DataPath_DSR = FolderBrowser_DSR.SelectedPath;
            porter.Run();

            Button_Activate.Invoke(() => Button_Activate.Enabled = true);
            PortingInProcess = false;

            System.Media.SystemSounds.Exclamation.Play();

            if (porter.PorterException != null)
            {
                var result = MessageBox.Show($"Porting process ran into an issue.\n\n" +
                    $"{porter.PorterException.SourceException.Message}\n" +
                    $"{porter.PorterException.SourceException.StackTrace}", "Error", MessageBoxButtons.OK);
                this.Close();
            }
            else
            {
                var result = MessageBox.Show("Finished! Open output folder?", "Finished", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                    Process.Start(@"explorer.exe", $@"{Directory.GetCurrentDirectory()}\output"); // Open up the output folder
            }
        }

        private void Button_Browse_PTDE_Mod_Click(object sender, EventArgs e)
        {
            FolderBrowser_PTDE_Mod.ShowDialog();
            string path = FolderBrowser_PTDE_Mod.SelectedPath;
            if (path != "")
            {
                Text_FileLoaded_PTDE_Mod.Text = $"{path}";
            }
            CheckEnableActivateButton();
        }

        private void Button_Browse_DSR_Click(object sender, EventArgs e)
        {
            FolderBrowser_DSR.ShowDialog();
            string path = FolderBrowser_DSR.SelectedPath;
            if (path != "")
            {
                Text_FileLoaded_DSR_Mod.Text = $"{path}";
            }
            CheckEnableActivateButton();
        }

        private void Button_Browse_PTDE_Vanilla_Click(object sender, EventArgs e)
        {
            FolderBrowser_PTDE_Vanilla.ShowDialog();
            string path = FolderBrowser_PTDE_Vanilla.SelectedPath;
            if (path != "")
            {
                Text_FileLoaded_PTDE_Vanilla.Text = $"{path}";
            }
            CheckEnableActivateButton();
        }

        private void Button_SOTE_set_paths_Click(object sender, EventArgs e)
        {
            string ptdePath_Vanilla = @"V:\VSteamLibrary\steamapps\common\Dark Souls Prepare to Die Edition\DATA vanilla";
            string ptdePath = @"V:\VSteamLibrary\steamapps\common\Dark Souls Prepare to Die Edition\SOTE storage\SOTE DSR port input\DATA";
            string dsrPath = @"Y:\Projects Y\Modding\DSR\DSR port input";
            FolderBrowser_PTDE_Vanilla.SelectedPath = ptdePath_Vanilla;
            FolderBrowser_PTDE_Mod.SelectedPath = ptdePath;
            FolderBrowser_DSR.SelectedPath = dsrPath;
            CheckEnableActivateButton();
        }

        private void Button_Info_Click(object sender, EventArgs e)
        {
            MessageBox.Show("", "Info", MessageBoxButtons.OK);
        }

        private void Setting_CompileLua_CheckedChanged(object sender, EventArgs e)
        {
            DSPorterSettings.CompileLua = Setting_CompileLua.Checked;
        }

        private void Setting_IsSOTE_CheckedChanged(object sender, EventArgs e)
        {
            DSPorterSettings.Is_SOTE = Setting_IsSOTE.Checked;
        }

        private void Setting_SlimeCeilingFix_CheckedChanged(object sender, EventArgs e)
        {
            DSPorterSettings.SlimeCeilingFix = Setting_SlimeCeilingFix.Checked;
        }

        private void Setting_Misc_DSR_Collision_CheckedChanged(object sender, EventArgs e)
        {
            DSPorterSettings.MiscCollisionFixes = Setting_Misc_DSR_Collision.Checked;
        }

        private void Setting_RenderGroupImprovements_CheckedChanged(object sender, EventArgs e)
        {
            DSPorterSettings.RenderGroupImprovements = Setting_RenderGroupImprovements.Checked;
        }

        private void Setting_EmptyEstusFFX_CheckedChanged(object sender, EventArgs e)
        {
            DSPorterSettings.EmptyEstusFFX = Setting_EmptyEstusFFX.Checked;

        }

        private void Setting_m12_01_navmesh_fix_CheckedChanged(object sender, EventArgs e)
        {
            DSPorterSettings.m12_01_AddExtraDSRNavmesh = Setting_m12_01_AddNewNavmesh.Checked;
        }
    }
}