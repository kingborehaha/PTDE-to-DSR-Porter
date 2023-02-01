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
        public static readonly string ProgramTitle = $"PTDE to DSR porter";
        public DSPorter _dsPorter;

        public MainForm()
        {
            InitializeComponent();
            this.Text = ProgramTitle;
            _dsPorter = new(ProgramProgressBar);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (cb_log_field_specifics.Checked)
                cb_fields_share_row.Enabled = true;
            else
                cb_fields_share_row.Enabled = false;
        }

        private void LoadFile(FileDialog fileDialog)
        {
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                CheckEnableActivateButton();
            }
        }
        private void Button_PTDE_Browse(object sender, EventArgs e)
        {
            LoadFile(openFileDialog_old);
            Text_Loaded_PTDE.Text = "Loaded";
            UpdateConsole("Old param selected");
        }

        private void Button_DSR_Browse(object sender, EventArgs e)
        {
            LoadFile(openFileDialog_new);
            Text_Loaded_DSR.Text = "Loaded";
            UpdateConsole("New param selected");
        }

        public void UpdateConsole(string text)
        {
            Application.DoEvents();
        }

        private void CheckEnableActivateButton()
        {
            if (openFileDialog_old.FileName != "" && openFileDialog_new.FileName != "")
                Button_Activate.Enabled = true;
            return;
        }

        private void Button_Activate_Click(object sender, EventArgs e)
        {
            Task.Run(() => RunProgram());
        }

        private void RunProgram()
        {
            string ptdePath = @"V:\VSteamLibrary\steamapps\common\Dark Souls Prepare to Die Edition\SOTE storage\Shadow of the Eclipse v2.0.0 DSR input\DATA";
            string dsrPath = @"Y:\Projects Y\Modding\DSR\DSR port input";

            ProgramProgressBar.Value = 0;

            Button_Activate.Invoke(new Action(() => Button_Activate.Enabled = false));
            
            _dsPorter.Run(ptdePath, dsrPath);

            Button_Activate.Invoke(new Action(() => Button_Activate.Enabled = true));

            GC.Collect();

            MessageBox.Show("Finished!");
        }

        private void cb_log_field_specifics_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_log_field_specifics.Checked)
                cb_fields_share_row.Enabled = true;
            else
                cb_fields_share_row.Enabled = false;
        }

        private void toggle_buttons_logNames()
        {
        }

        private void openFileDialog_old_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            UpdateConsole("Reading Params");
        }

        private void cb_log_name_changes_only_CheckedChanged(object sender, EventArgs e)
        {
            toggle_buttons_logNames();
        }

        private void cb_LogRowNames_CheckedChanged(object sender, EventArgs e)
        {
            toggle_buttons_logNames();
        }

        private void combo_logNameExclusive_SelectedIndexChanged(object sender, EventArgs e)
        {
            toggle_buttons_logNames();
        }

        private void t_VersionOld_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
            openFileDialog_old.FileName = files[0];
            Text_Loaded_PTDE.Text = "Loaded";
            CheckEnableActivateButton();
        }
        private void t_VersionNew_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
            openFileDialog_new.FileName = files[0];
            Text_Loaded_DSR.Text = "Loaded";
            CheckEnableActivateButton();
        }
        private void t_VersionOld_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
            else
                e.Effect = DragDropEffects.None;
        }
        private void t_VersionNew_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
            else
                e.Effect = DragDropEffects.None;
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }
    }
}