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
            Button_Browse_PTDE_Mod = new Button();
            Button_Activate = new Button();
            Button_Browse_DSR = new Button();
            label2 = new Label();
            label3 = new Label();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            Setting_IsSOTE = new CheckBox();
            Text_FileLoaded_DSR_Mod = new TextBox();
            Text_FileLoaded_PTDE_Vanilla = new TextBox();
            Text_FileLoaded_PTDE_Mod = new TextBox();
            Button_SOTE_set_paths = new Button();
            Button_Browse_PTDE_Vanilla = new Button();
            label4 = new Label();
            ProgramProgressBar = new ProgressBar();
            Button_Info = new Button();
            tabPage2 = new TabPage();
            FolderBrowser_PTDE_Mod = new FolderBrowserDialog();
            FolderBrowser_PTDE_Vanilla = new FolderBrowserDialog();
            FolderBrowser_DSR = new FolderBrowserDialog();
            Setting_CompileLua = new CheckBox();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            SuspendLayout();
            // 
            // Button_Browse_PTDE_Mod
            // 
            Button_Browse_PTDE_Mod.AllowDrop = true;
            Button_Browse_PTDE_Mod.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            Button_Browse_PTDE_Mod.Location = new Point(10, 111);
            Button_Browse_PTDE_Mod.Name = "Button_Browse_PTDE_Mod";
            Button_Browse_PTDE_Mod.Size = new Size(74, 24);
            Button_Browse_PTDE_Mod.TabIndex = 60;
            Button_Browse_PTDE_Mod.Text = "Browse";
            Button_Browse_PTDE_Mod.UseVisualStyleBackColor = true;
            Button_Browse_PTDE_Mod.Click += Button_Browse_PTDE_Mod_Click;
            // 
            // Button_Activate
            // 
            Button_Activate.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            Button_Activate.Location = new Point(283, 266);
            Button_Activate.Name = "Button_Activate";
            Button_Activate.Size = new Size(74, 24);
            Button_Activate.TabIndex = 61;
            Button_Activate.Text = "Port";
            Button_Activate.UseVisualStyleBackColor = true;
            Button_Activate.Click += Button_Activate_Click;
            // 
            // Button_Browse_DSR
            // 
            Button_Browse_DSR.AllowDrop = true;
            Button_Browse_DSR.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            Button_Browse_DSR.Location = new Point(10, 224);
            Button_Browse_DSR.Name = "Button_Browse_DSR";
            Button_Browse_DSR.Size = new Size(74, 24);
            Button_Browse_DSR.TabIndex = 63;
            Button_Browse_DSR.Text = "Browse";
            Button_Browse_DSR.UseVisualStyleBackColor = true;
            Button_Browse_DSR.Click += Button_Browse_DSR_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(10, 93);
            label2.Name = "label2";
            label2.Size = new Size(88, 15);
            label2.TabIndex = 74;
            label2.Text = "PTDE Mod Files";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(10, 206);
            label3.Name = "label3";
            label3.Size = new Size(82, 15);
            label3.TabIndex = 75;
            label3.Text = "DSR Mod Files";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Margin = new Padding(0);
            tabControl1.Multiline = true;
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(373, 353);
            tabControl1.TabIndex = 82;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(Setting_CompileLua);
            tabPage1.Controls.Add(Setting_IsSOTE);
            tabPage1.Controls.Add(Text_FileLoaded_DSR_Mod);
            tabPage1.Controls.Add(Text_FileLoaded_PTDE_Vanilla);
            tabPage1.Controls.Add(Text_FileLoaded_PTDE_Mod);
            tabPage1.Controls.Add(Button_SOTE_set_paths);
            tabPage1.Controls.Add(Button_Browse_PTDE_Vanilla);
            tabPage1.Controls.Add(label4);
            tabPage1.Controls.Add(ProgramProgressBar);
            tabPage1.Controls.Add(Button_Info);
            tabPage1.Controls.Add(Button_Browse_PTDE_Mod);
            tabPage1.Controls.Add(Button_Activate);
            tabPage1.Controls.Add(Button_Browse_DSR);
            tabPage1.Controls.Add(label2);
            tabPage1.Controls.Add(label3);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(365, 325);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Main";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // Setting_IsSOTE
            // 
            Setting_IsSOTE.AutoSize = true;
            Setting_IsSOTE.Checked = true;
            Setting_IsSOTE.CheckState = CheckState.Checked;
            Setting_IsSOTE.Enabled = false;
            Setting_IsSOTE.Location = new Point(113, 26);
            Setting_IsSOTE.Name = "Setting_IsSOTE";
            Setting_IsSOTE.Size = new Size(86, 19);
            Setting_IsSOTE.TabIndex = 89;
            Setting_IsSOTE.Text = "SOTE Mode";
            Setting_IsSOTE.UseVisualStyleBackColor = true;
            Setting_IsSOTE.CheckedChanged += Setting_IsSOTE_CheckedChanged;
            // 
            // Text_FileLoaded_DSR_Mod
            // 
            Text_FileLoaded_DSR_Mod.Location = new Point(104, 224);
            Text_FileLoaded_DSR_Mod.Name = "Text_FileLoaded_DSR_Mod";
            Text_FileLoaded_DSR_Mod.ReadOnly = true;
            Text_FileLoaded_DSR_Mod.Size = new Size(253, 23);
            Text_FileLoaded_DSR_Mod.TabIndex = 88;
            Text_FileLoaded_DSR_Mod.Text = "Unloaded";
            Text_FileLoaded_DSR_Mod.TextAlign = HorizontalAlignment.Center;
            // 
            // Text_FileLoaded_PTDE_Vanilla
            // 
            Text_FileLoaded_PTDE_Vanilla.Location = new Point(104, 169);
            Text_FileLoaded_PTDE_Vanilla.Name = "Text_FileLoaded_PTDE_Vanilla";
            Text_FileLoaded_PTDE_Vanilla.ReadOnly = true;
            Text_FileLoaded_PTDE_Vanilla.Size = new Size(253, 23);
            Text_FileLoaded_PTDE_Vanilla.TabIndex = 87;
            Text_FileLoaded_PTDE_Vanilla.Text = "Unloaded";
            Text_FileLoaded_PTDE_Vanilla.TextAlign = HorizontalAlignment.Center;
            // 
            // Text_FileLoaded_PTDE_Mod
            // 
            Text_FileLoaded_PTDE_Mod.Location = new Point(104, 111);
            Text_FileLoaded_PTDE_Mod.Name = "Text_FileLoaded_PTDE_Mod";
            Text_FileLoaded_PTDE_Mod.ReadOnly = true;
            Text_FileLoaded_PTDE_Mod.Size = new Size(253, 23);
            Text_FileLoaded_PTDE_Mod.TabIndex = 86;
            Text_FileLoaded_PTDE_Mod.Text = "Unloaded";
            Text_FileLoaded_PTDE_Mod.TextAlign = HorizontalAlignment.Center;
            // 
            // Button_SOTE_set_paths
            // 
            Button_SOTE_set_paths.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            Button_SOTE_set_paths.Location = new Point(105, 266);
            Button_SOTE_set_paths.Name = "Button_SOTE_set_paths";
            Button_SOTE_set_paths.Size = new Size(153, 24);
            Button_SOTE_set_paths.TabIndex = 84;
            Button_SOTE_set_paths.Text = "Debug SOTE: set paths";
            Button_SOTE_set_paths.UseVisualStyleBackColor = true;
            Button_SOTE_set_paths.Click += debug_button_Click;
            // 
            // Button_Browse_PTDE_Vanilla
            // 
            Button_Browse_PTDE_Vanilla.AllowDrop = true;
            Button_Browse_PTDE_Vanilla.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            Button_Browse_PTDE_Vanilla.Location = new Point(10, 169);
            Button_Browse_PTDE_Vanilla.Name = "Button_Browse_PTDE_Vanilla";
            Button_Browse_PTDE_Vanilla.Size = new Size(74, 24);
            Button_Browse_PTDE_Vanilla.TabIndex = 81;
            Button_Browse_PTDE_Vanilla.Text = "Browse";
            Button_Browse_PTDE_Vanilla.UseVisualStyleBackColor = true;
            Button_Browse_PTDE_Vanilla.Click += Button_Browse_PTDE_Vanilla_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(10, 151);
            label4.Name = "label4";
            label4.Size = new Size(97, 15);
            label4.TabIndex = 83;
            label4.Text = "PTDE Vanilla Files";
            // 
            // ProgramProgressBar
            // 
            ProgramProgressBar.Location = new Point(6, 296);
            ProgramProgressBar.Maximum = 1000;
            ProgramProgressBar.Name = "ProgramProgressBar";
            ProgramProgressBar.Size = new Size(353, 23);
            ProgramProgressBar.TabIndex = 80;
            // 
            // Button_Info
            // 
            Button_Info.Enabled = false;
            Button_Info.Location = new Point(314, 6);
            Button_Info.Name = "Button_Info";
            Button_Info.Size = new Size(45, 23);
            Button_Info.TabIndex = 79;
            Button_Info.Text = "Info";
            Button_Info.UseVisualStyleBackColor = true;
            Button_Info.Click += Button_Info_Click;
            // 
            // tabPage2
            // 
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(365, 325);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Options";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // FolderBrowser_PTDE_Mod
            // 
            FolderBrowser_PTDE_Mod.Description = "Select folder with PTDE mod files";
            FolderBrowser_PTDE_Mod.RootFolder = Environment.SpecialFolder.Recent;
            FolderBrowser_PTDE_Mod.ShowNewFolderButton = false;
            FolderBrowser_PTDE_Mod.UseDescriptionForTitle = true;
            // 
            // FolderBrowser_PTDE_Vanilla
            // 
            FolderBrowser_PTDE_Vanilla.Description = "Select folder with PTDE vanilla files";
            FolderBrowser_PTDE_Vanilla.RootFolder = Environment.SpecialFolder.Recent;
            FolderBrowser_PTDE_Vanilla.ShowNewFolderButton = false;
            FolderBrowser_PTDE_Vanilla.UseDescriptionForTitle = true;
            // 
            // FolderBrowser_DSR
            // 
            FolderBrowser_DSR.Description = "Select folder with PTDE mod files";
            FolderBrowser_DSR.RootFolder = Environment.SpecialFolder.Recent;
            FolderBrowser_DSR.ShowNewFolderButton = false;
            FolderBrowser_DSR.UseDescriptionForTitle = true;
            // 
            // Setting_CompileLua
            // 
            Setting_CompileLua.AutoSize = true;
            Setting_CompileLua.Checked = true;
            Setting_CompileLua.CheckState = CheckState.Checked;
            Setting_CompileLua.Location = new Point(11, 26);
            Setting_CompileLua.Name = "Setting_CompileLua";
            Setting_CompileLua.Size = new Size(96, 19);
            Setting_CompileLua.TabIndex = 90;
            Setting_CompileLua.Text = "Compile LUA";
            Setting_CompileLua.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(373, 353);
            Controls.Add(tabControl1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MainForm";
            Text = "Bore Param Comparison";
            Load += MainForm_Load;
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Button Button_Browse_PTDE_Mod;
        private Button Button_Activate;
        private Button Button_Browse_DSR;
        private Label label2;
        private Label label3;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Button Button_Info;
        public ProgressBar ProgramProgressBar;
        private Button Button_Browse_PTDE_Vanilla;
        private Label label4;
        private FolderBrowserDialog FolderBrowser_PTDE_Mod;
        private FolderBrowserDialog FolderBrowser_PTDE_Vanilla;
        private FolderBrowserDialog FolderBrowser_DSR;
        private Button Button_SOTE_set_paths;
        private TextBox Text_FileLoaded_DSR_Mod;
        private TextBox Text_FileLoaded_PTDE_Vanilla;
        private TextBox Text_FileLoaded_PTDE_Mod;
        private CheckBox Setting_IsSOTE;
        private CheckBox Setting_CompileLua;
    }
}