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
            this.Button_Browse_PTDE_Mod = new System.Windows.Forms.Button();
            this.Button_Activate = new System.Windows.Forms.Button();
            this.Button_Browse_DSR = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.debug_button = new System.Windows.Forms.Button();
            this.Button_Browse_PTDE_Vanilla = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.ProgramProgressBar = new System.Windows.Forms.ProgressBar();
            this.button1 = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.cb_log_field_specifics = new System.Windows.Forms.CheckBox();
            this.FolderBrowser_PTDE_Mod = new System.Windows.Forms.FolderBrowserDialog();
            this.FolderBrowser_PTDE_Vanilla = new System.Windows.Forms.FolderBrowserDialog();
            this.FolderBrowser_DSR = new System.Windows.Forms.FolderBrowserDialog();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // Button_Browse_PTDE_Mod
            // 
            this.Button_Browse_PTDE_Mod.AllowDrop = true;
            this.Button_Browse_PTDE_Mod.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Button_Browse_PTDE_Mod.Location = new System.Drawing.Point(10, 111);
            this.Button_Browse_PTDE_Mod.Name = "Button_Browse_PTDE_Mod";
            this.Button_Browse_PTDE_Mod.Size = new System.Drawing.Size(74, 24);
            this.Button_Browse_PTDE_Mod.TabIndex = 60;
            this.Button_Browse_PTDE_Mod.Text = "Browse";
            this.Button_Browse_PTDE_Mod.UseVisualStyleBackColor = true;
            this.Button_Browse_PTDE_Mod.Click += new System.EventHandler(this.Button_Browse_PTDE_Mod_Click);
            // 
            // Button_Activate
            // 
            this.Button_Activate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Button_Activate.Location = new System.Drawing.Point(283, 266);
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
            this.Button_Browse_DSR.Location = new System.Drawing.Point(10, 230);
            this.Button_Browse_DSR.Name = "Button_Browse_DSR";
            this.Button_Browse_DSR.Size = new System.Drawing.Size(74, 24);
            this.Button_Browse_DSR.TabIndex = 63;
            this.Button_Browse_DSR.Text = "Browse";
            this.Button_Browse_DSR.UseVisualStyleBackColor = true;
            this.Button_Browse_DSR.Click += new System.EventHandler(this.Button_Browse_DSR_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 15);
            this.label2.TabIndex = 74;
            this.label2.Text = "PTDE Mod Files";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 212);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 15);
            this.label3.TabIndex = 75;
            this.label3.Text = "DSR Mod Files";
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
            this.tabPage1.Controls.Add(this.debug_button);
            this.tabPage1.Controls.Add(this.Button_Browse_PTDE_Vanilla);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.ProgramProgressBar);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.Button_Browse_PTDE_Mod);
            this.tabPage1.Controls.Add(this.Button_Activate);
            this.tabPage1.Controls.Add(this.Button_Browse_DSR);
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
            // debug_button
            // 
            this.debug_button.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.debug_button.Location = new System.Drawing.Point(129, 170);
            this.debug_button.Name = "debug_button";
            this.debug_button.Size = new System.Drawing.Size(101, 24);
            this.debug_button.TabIndex = 84;
            this.debug_button.Text = "debug set paths";
            this.debug_button.UseVisualStyleBackColor = true;
            this.debug_button.Click += new System.EventHandler(this.debug_button_Click);
            // 
            // Button_Browse_PTDE_Vanilla
            // 
            this.Button_Browse_PTDE_Vanilla.AllowDrop = true;
            this.Button_Browse_PTDE_Vanilla.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Button_Browse_PTDE_Vanilla.Location = new System.Drawing.Point(10, 170);
            this.Button_Browse_PTDE_Vanilla.Name = "Button_Browse_PTDE_Vanilla";
            this.Button_Browse_PTDE_Vanilla.Size = new System.Drawing.Size(74, 24);
            this.Button_Browse_PTDE_Vanilla.TabIndex = 81;
            this.Button_Browse_PTDE_Vanilla.Text = "Browse";
            this.Button_Browse_PTDE_Vanilla.UseVisualStyleBackColor = true;
            this.Button_Browse_PTDE_Vanilla.Click += new System.EventHandler(this.Button_Browse_PTDE_Vanilla_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 152);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 15);
            this.label4.TabIndex = 83;
            this.label4.Text = "PTDE Vanilla Files";
            // 
            // ProgramProgressBar
            // 
            this.ProgramProgressBar.Location = new System.Drawing.Point(6, 296);
            this.ProgramProgressBar.Maximum = 1000;
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
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.cb_log_field_specifics);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(365, 325);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Options";
            this.tabPage2.UseVisualStyleBackColor = true;
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
            // FolderBrowser_PTDE_Mod
            // 
            this.FolderBrowser_PTDE_Mod.Description = "Select folder with PTDE mod files";
            this.FolderBrowser_PTDE_Mod.RootFolder = System.Environment.SpecialFolder.Recent;
            this.FolderBrowser_PTDE_Mod.ShowNewFolderButton = false;
            this.FolderBrowser_PTDE_Mod.UseDescriptionForTitle = true;
            // 
            // FolderBrowser_PTDE_Vanilla
            // 
            this.FolderBrowser_PTDE_Vanilla.Description = "Select folder with PTDE vanilla files";
            this.FolderBrowser_PTDE_Vanilla.RootFolder = System.Environment.SpecialFolder.Recent;
            this.FolderBrowser_PTDE_Vanilla.ShowNewFolderButton = false;
            this.FolderBrowser_PTDE_Vanilla.UseDescriptionForTitle = true;
            // 
            // FolderBrowser_DSR
            // 
            this.FolderBrowser_DSR.Description = "Select folder with PTDE mod files";
            this.FolderBrowser_DSR.RootFolder = System.Environment.SpecialFolder.Recent;
            this.FolderBrowser_DSR.ShowNewFolderButton = false;
            this.FolderBrowser_DSR.UseDescriptionForTitle = true;
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
        private ProgressBar ProgramProgressBar;
        private Button Button_Browse_PTDE_Vanilla;
        private Label label4;
        private FolderBrowserDialog FolderBrowser_PTDE_Mod;
        private FolderBrowserDialog FolderBrowser_PTDE_Vanilla;
        private FolderBrowserDialog FolderBrowser_DSR;
        private Button debug_button;
    }
}