
namespace TYYongAutoPatcher
{
    partial class frm_MainUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_MainUI));
            this.btn_launch = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_exit = new System.Windows.Forms.Button();
            this.wbs_left = new System.Windows.Forms.WebBrowser();
            this.wbs_right = new System.Windows.Forms.WebBrowser();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.btn_shop = new System.Windows.Forms.Button();
            this.btn_event = new System.Windows.Forms.Button();
            this.lbl_downloadProgress = new System.Windows.Forms.Label();
            this.lbl_totalDownloadProgress = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.cbx_startWhenReady = new System.Windows.Forms.CheckBox();
            this.btn_register = new System.Windows.Forms.Button();
            this.lbl_copyright = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_launch
            // 
            this.btn_launch.BackgroundImage = global::TYYongAutoPatcher.Properties.Resources.start4;
            this.btn_launch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_launch.FlatAppearance.BorderSize = 0;
            this.btn_launch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_launch.Location = new System.Drawing.Point(575, 431);
            this.btn_launch.Name = "btn_launch";
            this.btn_launch.Size = new System.Drawing.Size(150, 60);
            this.btn_launch.TabIndex = 0;
            this.btn_launch.UseVisualStyleBackColor = true;
            this.btn_launch.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btn_launch_MouseClick);
            this.btn_launch.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_launch_MouseDown);
            this.btn_launch.MouseEnter += new System.EventHandler(this.btn_launch_MouseEnter);
            this.btn_launch.MouseLeave += new System.EventHandler(this.btn_launch_MouseLeave);
            this.btn_launch.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_launch_MouseUp);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.btn_exit);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(740, 30);
            this.panel1.TabIndex = 1;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DragDownHandler);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DragMoveHandler);
            this.panel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DragUpHandler);
            // 
            // btn_exit
            // 
            this.btn_exit.BackColor = System.Drawing.Color.Transparent;
            this.btn_exit.BackgroundImage = global::TYYongAutoPatcher.Properties.Resources.exit3;
            this.btn_exit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btn_exit.FlatAppearance.BorderSize = 0;
            this.btn_exit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_exit.Location = new System.Drawing.Point(710, 6);
            this.btn_exit.Margin = new System.Windows.Forms.Padding(0);
            this.btn_exit.Name = "btn_exit";
            this.btn_exit.Size = new System.Drawing.Size(18, 16);
            this.btn_exit.TabIndex = 5;
            this.btn_exit.UseVisualStyleBackColor = false;
            this.btn_exit.Click += new System.EventHandler(this.btn_exit_Click);
            this.btn_exit.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_exit_MouseDown);
            this.btn_exit.MouseEnter += new System.EventHandler(this.btn_exit_MouseEnter);
            this.btn_exit.MouseLeave += new System.EventHandler(this.btn_exit_MouseLeave);
            this.btn_exit.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_exit_MouseUp);
            // 
            // wbs_left
            // 
            this.wbs_left.Location = new System.Drawing.Point(12, 47);
            this.wbs_left.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbs_left.Name = "wbs_left";
            this.wbs_left.Size = new System.Drawing.Size(378, 304);
            this.wbs_left.TabIndex = 2;
            // 
            // wbs_right
            // 
            this.wbs_right.Location = new System.Drawing.Point(405, 47);
            this.wbs_right.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbs_right.Name = "wbs_right";
            this.wbs_right.Size = new System.Drawing.Size(320, 228);
            this.wbs_right.TabIndex = 3;
            // 
            // listBox1
            // 
            this.listBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.listBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBox1.ForeColor = System.Drawing.Color.White;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(12, 431);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(543, 60);
            this.listBox1.TabIndex = 4;
            // 
            // btn_shop
            // 
            this.btn_shop.BackgroundImage = global::TYYongAutoPatcher.Properties.Resources.itemshop3;
            this.btn_shop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_shop.FlatAppearance.BorderSize = 0;
            this.btn_shop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_shop.Location = new System.Drawing.Point(405, 291);
            this.btn_shop.Name = "btn_shop";
            this.btn_shop.Size = new System.Drawing.Size(150, 60);
            this.btn_shop.TabIndex = 5;
            this.btn_shop.UseVisualStyleBackColor = true;
            // 
            // btn_event
            // 
            this.btn_event.BackgroundImage = global::TYYongAutoPatcher.Properties.Resources.event3;
            this.btn_event.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_event.FlatAppearance.BorderSize = 0;
            this.btn_event.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_event.Location = new System.Drawing.Point(575, 291);
            this.btn_event.Name = "btn_event";
            this.btn_event.Size = new System.Drawing.Size(150, 60);
            this.btn_event.TabIndex = 6;
            this.btn_event.UseVisualStyleBackColor = true;
            // 
            // lbl_downloadProgress
            // 
            this.lbl_downloadProgress.AutoSize = true;
            this.lbl_downloadProgress.BackColor = System.Drawing.Color.Transparent;
            this.lbl_downloadProgress.ForeColor = System.Drawing.Color.White;
            this.lbl_downloadProgress.Location = new System.Drawing.Point(11, 372);
            this.lbl_downloadProgress.Name = "lbl_downloadProgress";
            this.lbl_downloadProgress.Size = new System.Drawing.Size(29, 12);
            this.lbl_downloadProgress.TabIndex = 7;
            this.lbl_downloadProgress.Text = "下載";
            // 
            // lbl_totalDownloadProgress
            // 
            this.lbl_totalDownloadProgress.AutoSize = true;
            this.lbl_totalDownloadProgress.BackColor = System.Drawing.Color.Transparent;
            this.lbl_totalDownloadProgress.ForeColor = System.Drawing.Color.White;
            this.lbl_totalDownloadProgress.Location = new System.Drawing.Point(10, 399);
            this.lbl_totalDownloadProgress.Name = "lbl_totalDownloadProgress";
            this.lbl_totalDownloadProgress.Size = new System.Drawing.Size(53, 12);
            this.lbl_totalDownloadProgress.TabIndex = 8;
            this.lbl_totalDownloadProgress.Text = "整體進度";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(68, 369);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(487, 16);
            this.progressBar1.TabIndex = 9;
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(68, 398);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(487, 16);
            this.progressBar2.TabIndex = 10;
            // 
            // cbx_startWhenReady
            // 
            this.cbx_startWhenReady.AutoSize = true;
            this.cbx_startWhenReady.BackColor = System.Drawing.Color.Transparent;
            this.cbx_startWhenReady.Font = new System.Drawing.Font("PMingLiU", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cbx_startWhenReady.ForeColor = System.Drawing.Color.Silver;
            this.cbx_startWhenReady.Location = new System.Drawing.Point(575, 372);
            this.cbx_startWhenReady.Name = "cbx_startWhenReady";
            this.cbx_startWhenReady.Size = new System.Drawing.Size(143, 17);
            this.cbx_startWhenReady.TabIndex = 11;
            this.cbx_startWhenReady.Text = "準備好遊戲就馬上始";
            this.cbx_startWhenReady.UseVisualStyleBackColor = false;
            // 
            // btn_register
            // 
            this.btn_register.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_register.BackgroundImage")));
            this.btn_register.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_register.FlatAppearance.BorderSize = 0;
            this.btn_register.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_register.Location = new System.Drawing.Point(575, 395);
            this.btn_register.Name = "btn_register";
            this.btn_register.Size = new System.Drawing.Size(150, 17);
            this.btn_register.TabIndex = 12;
            this.btn_register.UseVisualStyleBackColor = true;
            // 
            // lbl_copyright
            // 
            this.lbl_copyright.AutoSize = true;
            this.lbl_copyright.BackColor = System.Drawing.Color.Transparent;
            this.lbl_copyright.ForeColor = System.Drawing.SystemColors.Menu;
            this.lbl_copyright.Location = new System.Drawing.Point(12, 498);
            this.lbl_copyright.Name = "lbl_copyright";
            this.lbl_copyright.Size = new System.Drawing.Size(233, 12);
            this.lbl_copyright.TabIndex = 13;
            this.lbl_copyright.Text = "Copyright © [year] [owner]. All rights reserved. ";
            // 
            // frm_MainUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackgroundImage = global::TYYongAutoPatcher.Properties.Resources.bg;
            this.ClientSize = new System.Drawing.Size(740, 525);
            this.Controls.Add(this.lbl_copyright);
            this.Controls.Add(this.btn_register);
            this.Controls.Add(this.cbx_startWhenReady);
            this.Controls.Add(this.progressBar2);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.lbl_totalDownloadProgress);
            this.Controls.Add(this.lbl_downloadProgress);
            this.Controls.Add(this.btn_event);
            this.Controls.Add(this.btn_shop);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.wbs_right);
            this.Controls.Add(this.wbs_left);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btn_launch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(740, 525);
            this.Name = "frm_MainUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "泰月勇Online 登錄器";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_launch;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.WebBrowser wbs_left;
        private System.Windows.Forms.WebBrowser wbs_right;
        private System.Windows.Forms.Button btn_exit;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button btn_shop;
        private System.Windows.Forms.Button btn_event;
        private System.Windows.Forms.Label lbl_downloadProgress;
        private System.Windows.Forms.Label lbl_totalDownloadProgress;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.CheckBox cbx_startWhenReady;
        private System.Windows.Forms.Button btn_register;
        private System.Windows.Forms.Label lbl_copyright;
    }
}

