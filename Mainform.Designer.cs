﻿namespace DSRPorter
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.openFileDialog_old = new System.Windows.Forms.OpenFileDialog();
            this.Button_Browse_PTDE = new System.Windows.Forms.Button();
            this.Button_Activate = new System.Windows.Forms.Button();
            this.Button_Browse_DSR = new System.Windows.Forms.Button();
            this.openFileDialog_new = new System.Windows.Forms.OpenFileDialog();
            this.cb_dupe = new System.Windows.Forms.CheckBox();
            this.cb_dupe_no_old = new System.Windows.Forms.CheckBox();
            this.cb_dupe_no_both = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Text_Loaded_DSR = new System.Windows.Forms.TextBox();
            this.Text_Loaded_PTDE = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.ProgramProgressBar = new System.Windows.Forms.ProgressBar();
            this.button1 = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.cb_LogAddedRemovedRowCells = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.combo_logNameExclusive = new System.Windows.Forms.ComboBox();
            this.cb_LogRowNames = new System.Windows.Forms.CheckBox();
            this.cb_log_name_changes_only = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cb_log_field_specifics = new System.Windows.Forms.CheckBox();
            this.cb_fields_share_row = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog_old
            // 
            this.openFileDialog_old.Filter = "Parambnd, Regulation, Data0, or .param|*";
            this.openFileDialog_old.Title = "Select old Parambnd or Regulation.bin";
            this.openFileDialog_old.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog_old_FileOk);
            // 
            // Button_Browse_PTDE
            // 
            this.Button_Browse_PTDE.AllowDrop = true;
            this.Button_Browse_PTDE.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Button_Browse_PTDE.Location = new System.Drawing.Point(10, 151);
            this.Button_Browse_PTDE.Name = "Button_Browse_PTDE";
            this.Button_Browse_PTDE.Size = new System.Drawing.Size(74, 24);
            this.Button_Browse_PTDE.TabIndex = 60;
            this.Button_Browse_PTDE.Text = "Browse";
            this.Button_Browse_PTDE.UseVisualStyleBackColor = true;
            this.Button_Browse_PTDE.DragDrop += new System.Windows.Forms.DragEventHandler(this.t_VersionOld_DragDrop);
            this.Button_Browse_PTDE.DragOver += new System.Windows.Forms.DragEventHandler(this.t_VersionOld_DragOver);
            // 
            // Button_Activate
            // 
            this.Button_Activate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Button_Activate.Location = new System.Drawing.Point(283, 152);
            this.Button_Activate.Name = "Button_Activate";
            this.Button_Activate.Size = new System.Drawing.Size(74, 24);
            this.Button_Activate.TabIndex = 61;
            this.Button_Activate.Text = "Port";
            this.Button_Activate.UseVisualStyleBackColor = true;
            this.Button_Activate.Click += new System.EventHandler(this.Button_Activate_Click);
            // 
            // Button_Browse_DSR
            // 
            this.Button_Browse_DSR.AllowDrop = true;
            this.Button_Browse_DSR.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Button_Browse_DSR.Location = new System.Drawing.Point(147, 152);
            this.Button_Browse_DSR.Name = "Button_Browse_DSR";
            this.Button_Browse_DSR.Size = new System.Drawing.Size(74, 24);
            this.Button_Browse_DSR.TabIndex = 63;
            this.Button_Browse_DSR.Text = "Browse";
            this.Button_Browse_DSR.UseVisualStyleBackColor = true;
            this.Button_Browse_DSR.DragDrop += new System.Windows.Forms.DragEventHandler(this.t_VersionNew_DragDrop);
            this.Button_Browse_DSR.DragOver += new System.Windows.Forms.DragEventHandler(this.t_VersionNew_DragOver);
            // 
            // openFileDialog_new
            // 
            this.openFileDialog_new.Filter = "Parambnd, Regulation, Data0, or .param|*";
            this.openFileDialog_new.Title = "Select new Parambnd or Regulation.bin";
            // 
            // cb_dupe
            // 
            this.cb_dupe.AutoSize = true;
            this.cb_dupe.Checked = true;
            this.cb_dupe.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_dupe.Location = new System.Drawing.Point(8, 173);
            this.cb_dupe.Name = "cb_dupe";
            this.cb_dupe.Size = new System.Drawing.Size(95, 19);
            this.cb_dupe.TabIndex = 65;
            this.cb_dupe.Text = "Log dupe IDs";
            this.cb_dupe.UseVisualStyleBackColor = true;
            // 
            // cb_dupe_no_old
            // 
            this.cb_dupe_no_old.AutoSize = true;
            this.cb_dupe_no_old.Location = new System.Drawing.Point(8, 198);
            this.cb_dupe_no_old.Name = "cb_dupe_no_old";
            this.cb_dupe_no_old.Size = new System.Drawing.Size(279, 19);
            this.cb_dupe_no_old.TabIndex = 67;
            this.cb_dupe_no_old.Text = "Don\'t log dupe IDs found in OLD (log all in new)\r\n";
            this.cb_dupe_no_old.UseVisualStyleBackColor = true;
            // 
            // cb_dupe_no_both
            // 
            this.cb_dupe_no_both.AutoSize = true;
            this.cb_dupe_no_both.Checked = true;
            this.cb_dupe_no_both.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_dupe_no_both.Location = new System.Drawing.Point(8, 223);
            this.cb_dupe_no_both.Name = "cb_dupe_no_both";
            this.cb_dupe_no_both.Size = new System.Drawing.Size(326, 19);
            this.cb_dupe_no_both.TabIndex = 66;
            this.cb_dupe_no_both.Text = "Don\'t log dupe IDs found in BOTH (unique new/old only)";
            this.cb_dupe_no_both.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(8, 149);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 21);
            this.label1.TabIndex = 71;
            this.label1.Text = "Dupe IDs";
            // 
            // Text_Loaded_DSR
            // 
            this.Text_Loaded_DSR.AllowDrop = true;
            this.Text_Loaded_DSR.Location = new System.Drawing.Point(147, 117);
            this.Text_Loaded_DSR.Name = "Text_Loaded_DSR";
            this.Text_Loaded_DSR.ReadOnly = true;
            this.Text_Loaded_DSR.Size = new System.Drawing.Size(74, 23);
            this.Text_Loaded_DSR.TabIndex = 72;
            this.Text_Loaded_DSR.Text = "Unloaded";
            this.Text_Loaded_DSR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Text_Loaded_DSR.DragDrop += new System.Windows.Forms.DragEventHandler(this.t_VersionNew_DragDrop);
            this.Text_Loaded_DSR.DragOver += new System.Windows.Forms.DragEventHandler(this.t_VersionNew_DragOver);
            // 
            // Text_Loaded_PTDE
            // 
            this.Text_Loaded_PTDE.AllowDrop = true;
            this.Text_Loaded_PTDE.Location = new System.Drawing.Point(10, 117);
            this.Text_Loaded_PTDE.Name = "Text_Loaded_PTDE";
            this.Text_Loaded_PTDE.ReadOnly = true;
            this.Text_Loaded_PTDE.Size = new System.Drawing.Size(74, 23);
            this.Text_Loaded_PTDE.TabIndex = 73;
            this.Text_Loaded_PTDE.Text = "Unloaded";
            this.Text_Loaded_PTDE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Text_Loaded_PTDE.DragDrop += new System.Windows.Forms.DragEventHandler(this.t_VersionOld_DragDrop);
            this.Text_Loaded_PTDE.DragOver += new System.Windows.Forms.DragEventHandler(this.t_VersionOld_DragOver);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 99);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 15);
            this.label2.TabIndex = 74;
            this.label2.Text = "PTDE Mod Files";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(145, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 15);
            this.label3.TabIndex = 75;
            this.label3.Text = "DSR Mod Files";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label5.Location = new System.Drawing.Point(8, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 21);
            this.label5.TabIndex = 81;
            this.label5.Text = "Row Names";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(373, 353);
            this.tabControl1.TabIndex = 82;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ProgramProgressBar);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.Button_Browse_PTDE);
            this.tabPage1.Controls.Add(this.Button_Activate);
            this.tabPage1.Controls.Add(this.Button_Browse_DSR);
            this.tabPage1.Controls.Add(this.Text_Loaded_DSR);
            this.tabPage1.Controls.Add(this.Text_Loaded_PTDE);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(365, 325);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Main";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // ProgramProgressBar
            // 
            this.ProgramProgressBar.Location = new System.Drawing.Point(6, 296);
            this.ProgramProgressBar.Name = "ProgramProgressBar";
            this.ProgramProgressBar.Size = new System.Drawing.Size(353, 23);
            this.ProgramProgressBar.TabIndex = 80;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(314, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(45, 23);
            this.button1.TabIndex = 79;
            this.button1.Text = "Info";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.cb_LogAddedRemovedRowCells);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.combo_logNameExclusive);
            this.tabPage2.Controls.Add(this.cb_LogRowNames);
            this.tabPage2.Controls.Add(this.cb_log_name_changes_only);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.cb_log_field_specifics);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.cb_dupe);
            this.tabPage2.Controls.Add(this.cb_dupe_no_both);
            this.tabPage2.Controls.Add(this.cb_dupe_no_old);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.cb_fields_share_row);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(365, 325);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Options";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // cb_LogAddedRemovedRowCells
            // 
            this.cb_LogAddedRemovedRowCells.AutoSize = true;
            this.cb_LogAddedRemovedRowCells.Location = new System.Drawing.Point(8, 275);
            this.cb_LogAddedRemovedRowCells.Name = "cb_LogAddedRemovedRowCells";
            this.cb_LogAddedRemovedRowCells.Size = new System.Drawing.Size(245, 19);
            this.cb_LogAddedRemovedRowCells.TabIndex = 89;
            this.cb_LogAddedRemovedRowCells.Text = "Log all field info for added/removed rows";
            this.cb_LogAddedRemovedRowCells.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label7.Location = new System.Drawing.Point(10, 251);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 21);
            this.label7.TabIndex = 88;
            this.label7.Text = "Misc";
            // 
            // combo_logNameExclusive
            // 
            this.combo_logNameExclusive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_logNameExclusive.FormattingEnabled = true;
            this.combo_logNameExclusive.Items.AddRange(new object[] {
            "Import row names from Paramdex",
            "Log Old vs New differences",
            "Use names from New",
            "Use names from Old"});
            this.combo_logNameExclusive.Location = new System.Drawing.Point(113, 100);
            this.combo_logNameExclusive.Name = "combo_logNameExclusive";
            this.combo_logNameExclusive.Size = new System.Drawing.Size(221, 23);
            this.combo_logNameExclusive.TabIndex = 87;
            this.combo_logNameExclusive.SelectedIndexChanged += new System.EventHandler(this.combo_logNameExclusive_SelectedIndexChanged);
            // 
            // cb_LogRowNames
            // 
            this.cb_LogRowNames.AutoSize = true;
            this.cb_LogRowNames.Checked = true;
            this.cb_LogRowNames.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_LogRowNames.Location = new System.Drawing.Point(6, 102);
            this.cb_LogRowNames.Name = "cb_LogRowNames";
            this.cb_LogRowNames.Size = new System.Drawing.Size(107, 19);
            this.cb_LogRowNames.TabIndex = 86;
            this.cb_LogRowNames.Text = "Log row names";
            this.cb_LogRowNames.UseVisualStyleBackColor = true;
            this.cb_LogRowNames.CheckedChanged += new System.EventHandler(this.cb_LogRowNames_CheckedChanged);
            // 
            // cb_log_name_changes_only
            // 
            this.cb_log_name_changes_only.AutoSize = true;
            this.cb_log_name_changes_only.Location = new System.Drawing.Point(6, 127);
            this.cb_log_name_changes_only.Name = "cb_log_name_changes_only";
            this.cb_log_name_changes_only.Size = new System.Drawing.Size(242, 19);
            this.cb_log_name_changes_only.TabIndex = 83;
            this.cb_log_name_changes_only.Text = "Don\'t log row names that are unchanged";
            this.cb_log_name_changes_only.UseVisualStyleBackColor = true;
            this.cb_log_name_changes_only.CheckedChanged += new System.EventHandler(this.cb_log_name_changes_only_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label6.Location = new System.Drawing.Point(6, 3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 21);
            this.label6.TabIndex = 82;
            this.label6.Text = "Formatting";
            // 
            // cb_log_field_specifics
            // 
            this.cb_log_field_specifics.AutoSize = true;
            this.cb_log_field_specifics.Checked = true;
            this.cb_log_field_specifics.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_log_field_specifics.Location = new System.Drawing.Point(6, 27);
            this.cb_log_field_specifics.Name = "cb_log_field_specifics";
            this.cb_log_field_specifics.Size = new System.Drawing.Size(162, 19);
            this.cb_log_field_specifics.TabIndex = 69;
            this.cb_log_field_specifics.Text = "Log specific field changes";
            this.cb_log_field_specifics.UseVisualStyleBackColor = true;
            this.cb_log_field_specifics.CheckedChanged += new System.EventHandler(this.cb_log_field_specifics_CheckedChanged);
            // 
            // cb_fields_share_row
            // 
            this.cb_fields_share_row.AutoSize = true;
            this.cb_fields_share_row.Location = new System.Drawing.Point(6, 52);
            this.cb_fields_share_row.Name = "cb_fields_share_row";
            this.cb_fields_share_row.Size = new System.Drawing.Size(198, 19);
            this.cb_fields_share_row.TabIndex = 70;
            this.cb_fields_share_row.Text = "Log row changes in 1 shared line";
            this.cb_fields_share_row.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(373, 353);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Bore Param Comparison";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private OpenFileDialog openFileDialog_old;
        private Button Button_Browse_PTDE;
        private Button Button_Activate;
        private Button Button_Browse_DSR;
        private OpenFileDialog openFileDialog_new;
        private CheckBox cb_dupe;
        private CheckBox cb_dupe_no_old;
        private CheckBox cb_dupe_no_both;
        private Label label1;
        private TextBox Text_Loaded_DSR;
        private TextBox Text_Loaded_PTDE;
        private Label label2;
        private Label label3;
        private Label label5;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private CheckBox cb_log_field_specifics;
        private CheckBox cb_fields_share_row;
        private Label label6;
        private CheckBox cb_log_name_changes_only;
        private CheckBox cb_LogRowNames;
        private ComboBox combo_logNameExclusive;
        private CheckBox cb_LogAddedRemovedRowCells;
        private Label label7;
        private Button button1;
        private ProgressBar ProgramProgressBar;
    }
}