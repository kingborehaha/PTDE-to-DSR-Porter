namespace DSRPorter
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
            debug_button = new Button();
            Button_Browse_PTDE_Vanilla = new Button();
            label4 = new Label();
            ProgramProgressBar = new ProgressBar();
            button1 = new Button();
            tabPage2 = new TabPage();
            label6 = new Label();
            cb_log_field_specifics = new CheckBox();
            FolderBrowser_PTDE_Mod = new FolderBrowserDialog();
            FolderBrowser_PTDE_Vanilla = new FolderBrowserDialog();
            FolderBrowser_DSR = new FolderBrowserDialog();
            setting_CompileLua = new CheckBox();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
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
            Button_Browse_DSR.Location = new Point(10, 230);
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
            label3.Location = new Point(10, 212);
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
            tabPage1.Controls.Add(setting_CompileLua);
            tabPage1.Controls.Add(debug_button);
            tabPage1.Controls.Add(Button_Browse_PTDE_Vanilla);
            tabPage1.Controls.Add(label4);
            tabPage1.Controls.Add(ProgramProgressBar);
            tabPage1.Controls.Add(button1);
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
            // debug_button
            // 
            debug_button.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            debug_button.Location = new Point(105, 266);
            debug_button.Name = "debug_button";
            debug_button.Size = new Size(153, 24);
            debug_button.TabIndex = 84;
            debug_button.Text = "Debug SOTE: set paths";
            debug_button.UseVisualStyleBackColor = true;
            debug_button.Click += debug_button_Click;
            // 
            // Button_Browse_PTDE_Vanilla
            // 
            Button_Browse_PTDE_Vanilla.AllowDrop = true;
            Button_Browse_PTDE_Vanilla.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            Button_Browse_PTDE_Vanilla.Location = new Point(10, 170);
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
            label4.Location = new Point(10, 152);
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
            // button1
            // 
            button1.Location = new Point(314, 6);
            button1.Name = "button1";
            button1.Size = new Size(45, 23);
            button1.TabIndex = 79;
            button1.Text = "Info";
            button1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(label6);
            tabPage2.Controls.Add(cb_log_field_specifics);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(365, 325);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Options";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label6.Location = new Point(6, 3);
            label6.Name = "label6";
            label6.Size = new Size(91, 21);
            label6.TabIndex = 82;
            label6.Text = "Formatting";
            // 
            // cb_log_field_specifics
            // 
            cb_log_field_specifics.AutoSize = true;
            cb_log_field_specifics.Checked = true;
            cb_log_field_specifics.CheckState = CheckState.Checked;
            cb_log_field_specifics.Location = new Point(6, 27);
            cb_log_field_specifics.Name = "cb_log_field_specifics";
            cb_log_field_specifics.Size = new Size(162, 19);
            cb_log_field_specifics.TabIndex = 69;
            cb_log_field_specifics.Text = "Log specific field changes";
            cb_log_field_specifics.UseVisualStyleBackColor = true;
            cb_log_field_specifics.CheckedChanged += cb_log_field_specifics_CheckedChanged;
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
            // setting_CompileLua
            // 
            setting_CompileLua.AutoSize = true;
            setting_CompileLua.Checked = true;
            setting_CompileLua.CheckState = CheckState.Checked;
            setting_CompileLua.Location = new Point(118, 111);
            setting_CompileLua.Name = "setting_CompileLua";
            setting_CompileLua.Size = new Size(96, 19);
            setting_CompileLua.TabIndex = 85;
            setting_CompileLua.Text = "Compile LUA";
            setting_CompileLua.UseVisualStyleBackColor = true;
            setting_CompileLua.CheckedChanged += setting_CompileLua_CheckedChanged;
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
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
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
        private CheckBox cb_log_field_specifics;
        private Label label6;
        private Button button1;
        public ProgressBar ProgramProgressBar;
        private Button Button_Browse_PTDE_Vanilla;
        private Label label4;
        private FolderBrowserDialog FolderBrowser_PTDE_Mod;
        private FolderBrowserDialog FolderBrowser_PTDE_Vanilla;
        private FolderBrowserDialog FolderBrowser_DSR;
        private Button debug_button;
        private CheckBox setting_CompileLua;
    }
}