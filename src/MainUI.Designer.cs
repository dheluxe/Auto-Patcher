
namespace TYYongAutoPatcher
{
    partial class MainUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainUI));
            this.btn_launch = new System.Windows.Forms.Button();
            this.pnl_dragger = new System.Windows.Forms.Panel();
            this.lbl_goToWeb = new System.Windows.Forms.Label();
            this.lbl_copyright = new System.Windows.Forms.Label();
            this.btn_exit = new System.Windows.Forms.Button();
            this.lbx_messages = new System.Windows.Forms.ListBox();
            this.web_left = new System.Windows.Forms.WebBrowser();
            this.web_right = new System.Windows.Forms.WebBrowser();
            this.btn_shop = new System.Windows.Forms.Button();
            this.btn_event = new System.Windows.Forms.Button();
            this.lbl_downloadProgress = new System.Windows.Forms.Label();
            this.lbl_totalDownloadProgress = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.cbx_startWhenReady = new System.Windows.Forms.CheckBox();
            this.btn_register = new System.Windows.Forms.Button();
            this.pnl_dragger.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_launch
            // 
            this.btn_launch.BackgroundImage = global::TYYongAutoPatcher.Properties.Resources.start4;
            this.btn_launch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_launch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_launch.Enabled = false;
            this.btn_launch.FlatAppearance.BorderSize = 0;
            this.btn_launch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_launch.Location = new System.Drawing.Point(578, 431);
            this.btn_launch.Name = "btn_launch";
            this.btn_launch.Size = new System.Drawing.Size(150, 60);
            this.btn_launch.TabIndex = 0;
            this.btn_launch.UseVisualStyleBackColor = true;
            this.btn_launch.Click += new System.EventHandler(this.btn_launch_Click);
            this.btn_launch.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_launch_MouseDown);
            this.btn_launch.MouseEnter += new System.EventHandler(this.btn_launch_MouseEnter);
            this.btn_launch.MouseLeave += new System.EventHandler(this.btn_launch_MouseLeave);
            this.btn_launch.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_launch_MouseUp);
            // 
            // pnl_dragger
            // 
            this.pnl_dragger.BackColor = System.Drawing.Color.Transparent;
            this.pnl_dragger.Controls.Add(this.lbl_goToWeb);
            this.pnl_dragger.Controls.Add(this.lbl_copyright);
            this.pnl_dragger.Controls.Add(this.btn_exit);
            this.pnl_dragger.Controls.Add(this.btn_launch);
            this.pnl_dragger.Controls.Add(this.lbx_messages);
            this.pnl_dragger.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_dragger.Location = new System.Drawing.Point(0, 0);
            this.pnl_dragger.Name = "pnl_dragger";
            this.pnl_dragger.Size = new System.Drawing.Size(740, 525);
            this.pnl_dragger.TabIndex = 1;
            this.pnl_dragger.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DragDownHandler);
            this.pnl_dragger.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DragMoveHandler);
            this.pnl_dragger.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DragUpHandler);
            // 
            // lbl_goToWeb
            // 
            this.lbl_goToWeb.AutoSize = true;
            this.lbl_goToWeb.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbl_goToWeb.Font = new System.Drawing.Font("PMingLiU", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_goToWeb.ForeColor = System.Drawing.Color.Chocolate;
            this.lbl_goToWeb.Location = new System.Drawing.Point(88, 10);
            this.lbl_goToWeb.Name = "lbl_goToWeb";
            this.lbl_goToWeb.Size = new System.Drawing.Size(53, 12);
            this.lbl_goToWeb.TabIndex = 14;
            this.lbl_goToWeb.Text = "前往官網";
            this.lbl_goToWeb.Click += new System.EventHandler(this.lbl_goToWeb_Click);
            // 
            // lbl_copyright
            // 
            this.lbl_copyright.AutoSize = true;
            this.lbl_copyright.BackColor = System.Drawing.Color.Transparent;
            this.lbl_copyright.ForeColor = System.Drawing.SystemColors.Menu;
            this.lbl_copyright.Location = new System.Drawing.Point(10, 501);
            this.lbl_copyright.Name = "lbl_copyright";
            this.lbl_copyright.Size = new System.Drawing.Size(0, 12);
            this.lbl_copyright.TabIndex = 13;
            // 
            // btn_exit
            // 
            this.btn_exit.BackColor = System.Drawing.Color.Transparent;
            this.btn_exit.BackgroundImage = global::TYYongAutoPatcher.Properties.Resources.exit3;
            this.btn_exit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btn_exit.Cursor = System.Windows.Forms.Cursors.Hand;
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
            // lbx_messages
            // 
            this.lbx_messages.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lbx_messages.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbx_messages.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lbx_messages.Font = new System.Drawing.Font("PMingLiU", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbx_messages.ForeColor = System.Drawing.Color.White;
            this.lbx_messages.FormattingEnabled = true;
            this.lbx_messages.ItemHeight = 20;
            this.lbx_messages.Location = new System.Drawing.Point(13, 431);
            this.lbx_messages.Name = "lbx_messages";
            this.lbx_messages.Size = new System.Drawing.Size(543, 60);
            this.lbx_messages.TabIndex = 4;
            this.lbx_messages.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbx_messages_DrawItem);
            // 
            // web_left
            // 
            this.web_left.Location = new System.Drawing.Point(12, 47);
            this.web_left.MinimumSize = new System.Drawing.Size(20, 20);
            this.web_left.Name = "web_left";
            this.web_left.Size = new System.Drawing.Size(378, 304);
            this.web_left.TabIndex = 2;
            this.web_left.Url = new System.Uri("", System.UriKind.Relative);
            this.web_left.Visible = false;
            // 
            // web_right
            // 
            this.web_right.Location = new System.Drawing.Point(405, 47);
            this.web_right.MinimumSize = new System.Drawing.Size(20, 20);
            this.web_right.Name = "web_right";
            this.web_right.Size = new System.Drawing.Size(320, 228);
            this.web_right.TabIndex = 3;
            this.web_right.Visible = false;
            // 
            // btn_shop
            // 
            this.btn_shop.BackgroundImage = global::TYYongAutoPatcher.Properties.Resources.itemshop3;
            this.btn_shop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_shop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_shop.FlatAppearance.BorderSize = 0;
            this.btn_shop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_shop.Location = new System.Drawing.Point(405, 291);
            this.btn_shop.Name = "btn_shop";
            this.btn_shop.Size = new System.Drawing.Size(150, 60);
            this.btn_shop.TabIndex = 5;
            this.btn_shop.UseVisualStyleBackColor = true;
            this.btn_shop.Click += new System.EventHandler(this.btn_shop_Click);
            this.btn_shop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_shop_MouseDown);
            this.btn_shop.MouseEnter += new System.EventHandler(this.btn_shop_MouseEnter);
            this.btn_shop.MouseLeave += new System.EventHandler(this.btn_shop_MouseLeave);
            this.btn_shop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_shop_MouseUp);
            // 
            // btn_event
            // 
            this.btn_event.BackgroundImage = global::TYYongAutoPatcher.Properties.Resources.event3;
            this.btn_event.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_event.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_event.FlatAppearance.BorderSize = 0;
            this.btn_event.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_event.Location = new System.Drawing.Point(575, 291);
            this.btn_event.Name = "btn_event";
            this.btn_event.Size = new System.Drawing.Size(150, 60);
            this.btn_event.TabIndex = 6;
            this.btn_event.UseVisualStyleBackColor = true;
            this.btn_event.Click += new System.EventHandler(this.btn_event_Click);
            this.btn_event.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_event_MouseDown);
            this.btn_event.MouseEnter += new System.EventHandler(this.btn_event_MouseEnter);
            this.btn_event.MouseLeave += new System.EventHandler(this.btn_event_MouseLeave);
            this.btn_event.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_event_MouseUp);
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
            this.cbx_startWhenReady.Cursor = System.Windows.Forms.Cursors.Hand;
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
            this.btn_register.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_register.FlatAppearance.BorderSize = 0;
            this.btn_register.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_register.Location = new System.Drawing.Point(575, 395);
            this.btn_register.Name = "btn_register";
            this.btn_register.Size = new System.Drawing.Size(150, 17);
            this.btn_register.TabIndex = 12;
            this.btn_register.UseVisualStyleBackColor = true;
            this.btn_register.Click += new System.EventHandler(this.btn_register_Click);
            this.btn_register.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_register_MouseDown);
            this.btn_register.MouseEnter += new System.EventHandler(this.btn_register_MouseEnter);
            this.btn_register.MouseLeave += new System.EventHandler(this.btn_register_MouseLeave);
            this.btn_register.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_register_MouseUp);
            // 
            // MainUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackgroundImage = global::TYYongAutoPatcher.Properties.Resources.bg;
            this.ClientSize = new System.Drawing.Size(740, 525);
            this.Controls.Add(this.btn_register);
            this.Controls.Add(this.cbx_startWhenReady);
            this.Controls.Add(this.progressBar2);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.lbl_totalDownloadProgress);
            this.Controls.Add(this.lbl_downloadProgress);
            this.Controls.Add(this.btn_event);
            this.Controls.Add(this.btn_shop);
            this.Controls.Add(this.web_right);
            this.Controls.Add(this.web_left);
            this.Controls.Add(this.pnl_dragger);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(740, 525);
            this.Name = "MainUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "泰月勇Online 登錄器";
            this.pnl_dragger.ResumeLayout(false);
            this.pnl_dragger.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_launch;
        private System.Windows.Forms.Panel pnl_dragger;
        private System.Windows.Forms.WebBrowser web_left;
        private System.Windows.Forms.WebBrowser web_right;
        private System.Windows.Forms.Button btn_exit;
        private System.Windows.Forms.ListBox lbx_messages;
        private System.Windows.Forms.Button btn_shop;
        private System.Windows.Forms.Button btn_event;
        private System.Windows.Forms.Label lbl_downloadProgress;
        private System.Windows.Forms.Label lbl_totalDownloadProgress;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.CheckBox cbx_startWhenReady;
        private System.Windows.Forms.Button btn_register;
        private System.Windows.Forms.Label lbl_copyright;
        private System.Windows.Forms.Label lbl_goToWeb;
    }
}

