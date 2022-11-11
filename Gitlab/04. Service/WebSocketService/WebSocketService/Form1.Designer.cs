namespace WebSocketService
{
    partial class Form1
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
            this.btnStart = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnListFile = new System.Windows.Forms.Button();
            this.btnDownloadFile = new System.Windows.Forms.Button();
            this.btnDownloadFolder = new System.Windows.Forms.Button();
            this.btnChooseFile = new System.Windows.Forms.Button();
            this.btnCreateFolder = new System.Windows.Forms.Button();
            this.btnUploadFolder = new System.Windows.Forms.Button();
            this.txtCheckFileExist = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(208, 12);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(208, 62);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 1;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.BtnSend_Click);
            // 
            // btnListFile
            // 
            this.btnListFile.Location = new System.Drawing.Point(208, 105);
            this.btnListFile.Name = "btnListFile";
            this.btnListFile.Size = new System.Drawing.Size(75, 23);
            this.btnListFile.TabIndex = 2;
            this.btnListFile.Text = "List File";
            this.btnListFile.UseVisualStyleBackColor = true;
            this.btnListFile.Click += new System.EventHandler(this.BtnListFile_Click);
            // 
            // btnDownloadFile
            // 
            this.btnDownloadFile.Location = new System.Drawing.Point(195, 160);
            this.btnDownloadFile.Name = "btnDownloadFile";
            this.btnDownloadFile.Size = new System.Drawing.Size(100, 23);
            this.btnDownloadFile.TabIndex = 3;
            this.btnDownloadFile.Text = "Download File";
            this.btnDownloadFile.UseVisualStyleBackColor = true;
            this.btnDownloadFile.Click += new System.EventHandler(this.BtnDownloadFile_Click);
            // 
            // btnDownloadFolder
            // 
            this.btnDownloadFolder.Location = new System.Drawing.Point(195, 204);
            this.btnDownloadFolder.Name = "btnDownloadFolder";
            this.btnDownloadFolder.Size = new System.Drawing.Size(100, 23);
            this.btnDownloadFolder.TabIndex = 4;
            this.btnDownloadFolder.Text = "Download Folder";
            this.btnDownloadFolder.UseVisualStyleBackColor = true;
            this.btnDownloadFolder.Click += new System.EventHandler(this.BtnDownloadFolder_Click);
            // 
            // btnChooseFile
            // 
            this.btnChooseFile.Location = new System.Drawing.Point(208, 248);
            this.btnChooseFile.Name = "btnChooseFile";
            this.btnChooseFile.Size = new System.Drawing.Size(75, 23);
            this.btnChooseFile.TabIndex = 5;
            this.btnChooseFile.Text = "Choose file";
            this.btnChooseFile.UseVisualStyleBackColor = true;
            this.btnChooseFile.Click += new System.EventHandler(this.BtnChooseFile_Click);
            // 
            // btnCreateFolder
            // 
            this.btnCreateFolder.Location = new System.Drawing.Point(195, 301);
            this.btnCreateFolder.Name = "btnCreateFolder";
            this.btnCreateFolder.Size = new System.Drawing.Size(100, 23);
            this.btnCreateFolder.TabIndex = 6;
            this.btnCreateFolder.Text = "Tạo cấu trúc thư mục";
            this.btnCreateFolder.UseVisualStyleBackColor = true;
            this.btnCreateFolder.Click += new System.EventHandler(this.btnCreateFolder_Click);
            // 
            // btnUploadFolder
            // 
            this.btnUploadFolder.Location = new System.Drawing.Point(195, 348);
            this.btnUploadFolder.Name = "btnUploadFolder";
            this.btnUploadFolder.Size = new System.Drawing.Size(100, 23);
            this.btnUploadFolder.TabIndex = 7;
            this.btnUploadFolder.Text = "Upload Folder";
            this.btnUploadFolder.UseVisualStyleBackColor = true;
            this.btnUploadFolder.Click += new System.EventHandler(this.BtnUploadFolder_Click);
            // 
            // txtCheckFileExist
            // 
            this.txtCheckFileExist.Location = new System.Drawing.Point(195, 388);
            this.txtCheckFileExist.Name = "txtCheckFileExist";
            this.txtCheckFileExist.Size = new System.Drawing.Size(100, 23);
            this.txtCheckFileExist.TabIndex = 8;
            this.txtCheckFileExist.Text = "Check File Exist";
            this.txtCheckFileExist.UseVisualStyleBackColor = true;
            this.txtCheckFileExist.Click += new System.EventHandler(this.TxtCheckFileExist_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(394, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "File 3D";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtCheckFileExist);
            this.Controls.Add(this.btnUploadFolder);
            this.Controls.Add(this.btnCreateFolder);
            this.Controls.Add(this.btnChooseFile);
            this.Controls.Add(this.btnDownloadFolder);
            this.Controls.Add(this.btnDownloadFile);
            this.Controls.Add(this.btnListFile);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnStart);
            this.Name = "Form1";
            this.Text = "Web Socket Service";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnListFile;
        private System.Windows.Forms.Button btnDownloadFile;
        private System.Windows.Forms.Button btnDownloadFolder;
        private System.Windows.Forms.Button btnChooseFile;
        private System.Windows.Forms.Button btnCreateFolder;
        private System.Windows.Forms.Button btnUploadFolder;
        private System.Windows.Forms.Button txtCheckFileExist;
        private System.Windows.Forms.Button button1;
    }
}

