namespace WebSocketService
{
    partial class QLTK
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QLTK));
            this.notifyQLTK = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmVersion = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmExit = new System.Windows.Forms.ToolStripMenuItem();
            this.chkStartAutomatic = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.tsmUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyQLTK
            // 
            this.notifyQLTK.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyQLTK.BalloonTipText = "QLTK Service";
            this.notifyQLTK.BalloonTipTitle = "QLTK Service";
            this.notifyQLTK.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyQLTK.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyQLTK.Icon")));
            this.notifyQLTK.Text = "QLTK Service";
            this.notifyQLTK.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.NotifyQLTK_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmVersion,
            this.tsmUpdate,
            this.tsmOpen,
            this.tsmExit});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(184, 114);
            // 
            // tsmVersion
            // 
            this.tsmVersion.Name = "tsmVersion";
            this.tsmVersion.Size = new System.Drawing.Size(183, 22);
            this.tsmVersion.Text = "Version: 1.0.0";
            // 
            // tsmOpen
            // 
            this.tsmOpen.Name = "tsmOpen";
            this.tsmOpen.Size = new System.Drawing.Size(183, 22);
            this.tsmOpen.Text = "Mở phần mềm";
            this.tsmOpen.Click += new System.EventHandler(this.tsmOpen_Click);
            // 
            // tsmExit
            // 
            this.tsmExit.Name = "tsmExit";
            this.tsmExit.Size = new System.Drawing.Size(183, 22);
            this.tsmExit.Text = "Tắt phần mềm";
            this.tsmExit.Click += new System.EventHandler(this.tsmExit_Click);
            // 
            // chkStartAutomatic
            // 
            this.chkStartAutomatic.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkStartAutomatic.AutoSize = true;
            this.chkStartAutomatic.Location = new System.Drawing.Point(56, 24);
            this.chkStartAutomatic.Name = "chkStartAutomatic";
            this.chkStartAutomatic.Size = new System.Drawing.Size(149, 17);
            this.chkStartAutomatic.TabIndex = 0;
            this.chkStartAutomatic.Text = "Khởi động cùng Windows";
            this.chkStartAutomatic.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(89, 57);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Lưu";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // tsmUpdate
            // 
            this.tsmUpdate.Name = "tsmUpdate";
            this.tsmUpdate.Size = new System.Drawing.Size(183, 22);
            this.tsmUpdate.Text = "Cập nhật phần mềm";
            this.tsmUpdate.Click += new System.EventHandler(this.tsmUpdate_Click);
            // 
            // QLTK
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(277, 115);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.chkStartAutomatic);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "QLTK";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "QLTK Service";
            this.Load += new System.EventHandler(this.QLTK_Load);
            this.Resize += new System.EventHandler(this.QLTK_Resize);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyQLTK;
        private System.Windows.Forms.CheckBox chkStartAutomatic;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmOpen;
        private System.Windows.Forms.ToolStripMenuItem tsmExit;
        private System.Windows.Forms.ToolStripMenuItem tsmVersion;
        private System.Windows.Forms.ToolStripMenuItem tsmUpdate;
    }
}