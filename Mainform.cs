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
        }

        private void CheckEnableActivateButton()
        {
            if (FolderBrowser_PTDE_Mod.SelectedPath != "" && FolderBrowser_PTDE_Vanilla.SelectedPath != "" && FolderBrowser_DSR.SelectedPath != "")
                Button_Activate.Enabled = true;
            else
                Button_Activate.Enabled = false;
            return;
        }

        private void Button_Activate_Click(object sender, EventArgs e)
        {
            ProgramProgressBar.Value = 0;
            Task.Run(() => RunProgram());
        }

        private void RunProgram()
        {
            Button_Activate.Invoke(() => Button_Activate.Enabled = false);

            DSPorter porter = new(ProgramProgressBar);
            porter.Run(FolderBrowser_PTDE_Mod.SelectedPath, FolderBrowser_DSR.SelectedPath, FolderBrowser_PTDE_Vanilla.SelectedPath);

            Button_Activate.Invoke(() => Button_Activate.Enabled = true);
            
            GC.Collect();

            System.Media.SystemSounds.Exclamation.Play();

            var result = MessageBox.Show("Finished! Open output folder?", "Finished", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
                Process.Start(@"explorer.exe", $@"{Directory.GetCurrentDirectory()}\output"); // Open up the output file
        }

        private void cb_log_field_specifics_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void Button_Browse_PTDE_Mod_Click(object sender, EventArgs e)
        {
            FolderBrowser_PTDE_Mod.ShowDialog();
            CheckEnableActivateButton();
        }

        private void Button_Browse_DSR_Click(object sender, EventArgs e)
        {
            FolderBrowser_DSR.ShowDialog();
            CheckEnableActivateButton();
        }

        private void Button_Browse_PTDE_Vanilla_Click(object sender, EventArgs e)
        {
            FolderBrowser_PTDE_Vanilla.ShowDialog();
            CheckEnableActivateButton();
        }

        private void debug_button_Click(object sender, EventArgs e)
        {
            string ptdePath_Vanilla = @"V:\VSteamLibrary\steamapps\common\Dark Souls Prepare to Die Edition\DATA vanilla packed";
            string ptdePath = @"V:\VSteamLibrary\steamapps\common\Dark Souls Prepare to Die Edition\SOTE storage\Shadow of the Eclipse v2.0.0 DSR input\DATA";
            string dsrPath = @"Y:\Projects Y\Modding\DSR\DSR port input";
            FolderBrowser_PTDE_Vanilla.SelectedPath = ptdePath_Vanilla;
            FolderBrowser_PTDE_Mod.SelectedPath = ptdePath;
            FolderBrowser_DSR.SelectedPath = dsrPath;
            CheckEnableActivateButton();
        }
    }
}