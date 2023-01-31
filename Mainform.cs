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
        private DSPorter _dsPorter = new();

        public MainForm()
        {
            InitializeComponent();
            this.Text = ProgramTitle;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (cb_log_field_specifics.Checked)
                cb_fields_share_row.Enabled = true;
            else
                cb_fields_share_row.Enabled = false;
        }

        private void loadFile(FileDialog fileDialog)
        {
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                CheckEnableActivateButton();
            }
        }
        private void b_browse_old_Click(object sender, EventArgs e)
        {
            loadFile(openFileDialog_old);
            t_VersionOld.Text = "Loaded";
            UpdateConsole("Old param selected");
        }

        private void b_browse_new_Click(object sender, EventArgs e)
        {
            loadFile(openFileDialog_new);
            t_VersionNew.Text = "Loaded";
            UpdateConsole("New param selected");
        }

        public void UpdateConsole(string text)
        {
            t_console.Text = text;
            Application.DoEvents();
        }

        private void CheckEnableActivateButton()
        {
            if (openFileDialog_old.FileName != "" && openFileDialog_new.FileName != "")
                b_activate.Enabled = true;
            return;
        }

        private void b_activate_Click(object sender, EventArgs e)
        {
            string ptdePath = @"V:\VSteamLibrary\steamapps\common\Dark Souls Prepare to Die Edition\SOTE storage\Shadow of the Eclipse v2.0.0 DSR input\DATA";
            string dsrPath = @"V:\VSteamLibrary\steamapps\common\DARK SOULS REMASTERED";
            _dsPorter.Run(ptdePath, dsrPath);
        }

        private void toggle_buttons_dupe()
        {
            if (cb_dupe.Checked)
            {
                cb_dupe_no_old.Enabled = true;
                cb_dupe_no_both.Enabled = true;
                toggle_buttons_dupe_no_old();
            }
            else
            {
                cb_dupe_no_old.Enabled = false;
                cb_dupe_no_both.Enabled = false;
            }
        }

        private void toggle_buttons_dupe_no_old()
        {
            if (cb_dupe_no_old.Checked)
            {
                cb_dupe_no_both.Enabled = false;
            }
            else
            {
                cb_dupe_no_both.Enabled = true;
            }
        }
 
        private void cb_dupe_CheckedChanged(object sender, EventArgs e)
        {
            toggle_buttons_dupe();
        }

        private void cb_dupe_no_old_CheckedChanged(object sender, EventArgs e)
        {
            toggle_buttons_dupe_no_old();
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
            t_VersionOld.Text = "Loaded";
            CheckEnableActivateButton();
        }
        private void t_VersionNew_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
            openFileDialog_new.FileName = files[0];
            t_VersionNew.Text = "Loaded";
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
            MessageBox.Show("Bore Pararm Comparison - Made by kingborehaha/george"
                + "\n\nThis tool is for logging differences between param files, which includes regulation.bin, GameParam.parambnd, and individual .param files."
                + "\n\nYou can drag & drop param files onto UI elements to automatically select them, or just click the two \"Open\" buttons to browse."
                + "\n\nThis tool can compare parameters with differing paramdefs. To include addtional paramdefs, place them into the \"Paramdex ALT\" folder, then within in the right game folder."
                , "Info" 
                );
        }
    }
}