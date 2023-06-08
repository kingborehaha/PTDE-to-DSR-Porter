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

            // Set settings values
            Setting_CompileLua.Checked = DSPorterSettings.CompileLua;
            Setting_IsSOTE.Checked = DSPorterSettings.Is_SOTE;


            if (DSPorterSettings.Is_SOTE)
            {
                MessageBox.Show("Program is currently in SOTE mode. Change \"IS_SOTE\" bool in code or build RELEASE instead of DEBUG.", "SOTE MODE ACTIVE", MessageBoxButtons.OK);
            }
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
            ProgramProgressBar.Invoke(() => ProgramProgressBar.Value = 0);
            PortingInProcess = true;
            Button_Activate.Invoke(() => Button_Activate.Enabled = false);

            DSPorter porter = new(ProgramProgressBar);
            porter.DataPath_PTDE_Mod = FolderBrowser_PTDE_Mod.SelectedPath;
            porter.DataPath_PTDE_Vanilla = FolderBrowser_PTDE_Vanilla.SelectedPath;
            porter.DataPath_DSR = FolderBrowser_DSR.SelectedPath;
            porter.Run();

            Button_Activate.Invoke(() => Button_Activate.Enabled = true);
            PortingInProcess = false;

            System.Media.SystemSounds.Exclamation.Play();

            if (porter.PorterException == null)
            {
                var result = MessageBox.Show("Finished! Open output folder?", "Finished", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                    Process.Start(@"explorer.exe", $@"{Directory.GetCurrentDirectory()}\output"); // Open up the output file
            }
            else
            {
                var result = MessageBox.Show($"Porting process ran into an issue.\n\n" +
                    $"{porter.PorterException.SourceException.Message}\n" +
                    $"{porter.PorterException.SourceException.StackTrace}", "Error", MessageBoxButtons.OK);
            }

            GC.Collect();
        }

        private void cb_log_field_specifics_CheckedChanged(object sender, EventArgs e)
        {
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

        private void debug_button_Click(object sender, EventArgs e)
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
    }
}