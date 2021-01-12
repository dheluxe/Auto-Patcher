﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace TYYongAutoPatcher
{
    public enum StateCode
    {
        Success,
        Error,
        Downloading,
        Connecting,
        Extracting,
        Completed,
        Retrying,
        Normal,
    }
    public partial class MainUI : Form
    {

        private bool isDraggin = false;
        private Point offset;
        private MainController app;
        private List<StateCode> sMsgList = new List<StateCode>();

        public MainUI()
        {
            InitializeComponent();
            app = new MainController(this);
            app.Init();
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(Path.GetFileName(System.Windows.Forms.Application.ExecutablePath));
            lbl_copyright.Text = fileVersionInfo.LegalCopyright;

        }
        #region click handler
        private void btn_launch_Click(object sender, EventArgs e)
        {
            if (app.ErrorMsg.Length == 0)
            {
                app.Launch();
            }
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            if (app.IsBusy)
            {
                DialogResult dResult = MessageBox.Show("正在更新中，是否結束?", "泰月勇Online 登錄器", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dResult.Equals(DialogResult.OK))
                {
                    app.CloseLauncher();
                }
            }
            else
            {
                app.CloseLauncher();
            }

        }

        // Open browser click...

        private void btn_register_Click(object sender, EventArgs e)
        {
            Process.Start(app.url.REGISTER);
        }
        private void btn_event_Click(object sender, EventArgs e)
        {
            Process.Start(app.url.EVENT);
        }
        private void btn_shop_Click(object sender, EventArgs e)
        {
            Process.Start(app.url.SHOP);
        }
        private void lbl_goToWeb_Click(object sender, EventArgs e)
        {
            Process.Start(app.url.OFFICIAL_WEB);
        }
        #endregion
        #region Buttons UI control
        private void btn_exit_MouseEnter(object sender, EventArgs e)
        {
            btn_exit.BackgroundImage = TYYongAutoPatcher.Properties.Resources.exit1;
        }

        private void btn_exit_MouseLeave(object sender, EventArgs e)
        {
            btn_exit.BackgroundImage = TYYongAutoPatcher.Properties.Resources.exit3;
        }

        private void btn_exit_MouseDown(object sender, MouseEventArgs e)
        {
            btn_exit.BackgroundImage = TYYongAutoPatcher.Properties.Resources.exit2;
        }

        private void btn_exit_MouseUp(object sender, MouseEventArgs e)
        {
            btn_exit.BackgroundImage = TYYongAutoPatcher.Properties.Resources.exit3;
        }

        private void btn_launch_MouseEnter(object sender, EventArgs e)
        {
            if (btn_launch.Enabled)
            {
                btn_launch.BackgroundImage = TYYongAutoPatcher.Properties.Resources.start4;

            }
            else
            {
                btn_launch.BackgroundImage = TYYongAutoPatcher.Properties.Resources.start1;
            }
        }

        private void btn_launch_MouseLeave(object sender, EventArgs e)
        {
            if (btn_launch.Enabled)
            {
                btn_launch.BackgroundImage = TYYongAutoPatcher.Properties.Resources.start4;

            }
            else
            {
                btn_launch.BackgroundImage = TYYongAutoPatcher.Properties.Resources.start3;
            }
        }

        private void btn_launch_MouseDown(object sender, MouseEventArgs e)
        {
            if (btn_launch.Enabled)
            {
                btn_launch.BackgroundImage = TYYongAutoPatcher.Properties.Resources.start4;
            }
            else
            {
                btn_launch.BackgroundImage = TYYongAutoPatcher.Properties.Resources.start2;
            }
        }


        private void btn_launch_MouseUp(object sender, MouseEventArgs e)
        {
            if (btn_launch.Enabled)
            {
                btn_launch.BackgroundImage = TYYongAutoPatcher.Properties.Resources.start4;
            }
            else
            {
                btn_launch.BackgroundImage = TYYongAutoPatcher.Properties.Resources.start1;
            }
        }

        private void btn_register_MouseDown(object sender, MouseEventArgs e)
        {
            btn_register.BackgroundImage = TYYongAutoPatcher.Properties.Resources.reg2;
        }


        private void btn_register_MouseEnter(object sender, EventArgs e)
        {
            btn_register.BackgroundImage = TYYongAutoPatcher.Properties.Resources.reg1;
        }

        private void btn_register_MouseLeave(object sender, EventArgs e)
        {
            btn_register.BackgroundImage = TYYongAutoPatcher.Properties.Resources.reg3;
        }

        private void btn_register_MouseUp(object sender, MouseEventArgs e)
        {
            btn_register.BackgroundImage = TYYongAutoPatcher.Properties.Resources.reg3;
        }


        private void btn_shop_MouseDown(object sender, MouseEventArgs e)
        {
            btn_shop.BackgroundImage = TYYongAutoPatcher.Properties.Resources.itemshop2;
        }

        private void btn_shop_MouseEnter(object sender, EventArgs e)
        {
            btn_shop.BackgroundImage = TYYongAutoPatcher.Properties.Resources.itemshop1;
        }

        private void btn_shop_MouseLeave(object sender, EventArgs e)
        {
            btn_shop.BackgroundImage = TYYongAutoPatcher.Properties.Resources.itemshop3;
        }

        private void btn_shop_MouseUp(object sender, MouseEventArgs e)
        {
            btn_shop.BackgroundImage = TYYongAutoPatcher.Properties.Resources.itemshop3;
        }

        private void btn_event_MouseDown(object sender, MouseEventArgs e)
        {
            btn_event.BackgroundImage = TYYongAutoPatcher.Properties.Resources.event2;
        }

        private void btn_event_MouseEnter(object sender, EventArgs e)
        {
            btn_event.BackgroundImage = TYYongAutoPatcher.Properties.Resources.event1;
        }

        private void btn_event_MouseLeave(object sender, EventArgs e)
        {
            btn_event.BackgroundImage = TYYongAutoPatcher.Properties.Resources.event3;
        }

        private void btn_event_MouseUp(object sender, MouseEventArgs e)
        {
            btn_event.BackgroundImage = TYYongAutoPatcher.Properties.Resources.event3;
        }
        #endregion
        #region Dragger
        private void DragMoveHandler(object sender, MouseEventArgs e)
        {
            if (isDraggin)
            {
                Point currentScreenPos = PointToScreen(e.Location);
                Location = new Point(currentScreenPos.X - offset.X, currentScreenPos.Y - offset.Y);
            }
        }

        private void DragDownHandler(object sender, MouseEventArgs e)
        {
            offset.X = e.X;
            offset.Y = e.Y;
            isDraggin = true;
        }

        private void DragUpHandler(object sender, MouseEventArgs e)
        {
            isDraggin = false;
        }
        #endregion

        public void SetLeftAndRightWeb()
        {
            web_left.Url = new Uri(app.url.LEFT_WEB);
            web_right.Url = new Uri(app.url.RIGHT_WEB);
            web_left.Visible = true;
            web_right.Visible = true;
        }


        //Add message to listbox
        public void AddMsg(string msg, StateCode state = StateCode.Normal)
        {
            lbx_messages.Items.Add(msg);
            lbx_messages.SelectedIndex = lbx_messages.Items.Count - 1;
            sMsgList.Add(state);
        }

        private void lbx_messages_DrawItem(object sender, DrawItemEventArgs e)
        {

            if (e.Index < 0) return;

            // Draw the background of the ListBox control for each item.
            e.DrawBackground();
            // Define the default color of the brush as black.

            Brush myBrush = Brushes.White;

            // Determine the color of the brush to draw each item based 
            // on the index of the item to draw.

            switch (sMsgList[e.Index])
            {
                case StateCode.Error:
                    //myBrush = Brushes.PaleVioletRed; pINK
                    myBrush = new SolidBrush(Color.FromArgb(248, 63, 94));
                    break;
                case StateCode.Success:
                    myBrush = new SolidBrush(Color.FromArgb(177, 241, 167));
                    break;
                case StateCode.Downloading:
                    myBrush = Brushes.LightSkyBlue;
                    break;
                case StateCode.Extracting:
                    myBrush = Brushes.IndianRed;
                    break;
                case StateCode.Normal:
                default:
                    myBrush = Brushes.White;
                    break;
            }

            // Draw the current item text based on the current Font 
            // and the custom brush settings.
            e.Graphics.DrawString(lbx_messages.Items[e.Index].ToString(),
                e.Font, myBrush, e.Bounds.X + 2, e.Bounds.Y + 6);
            // If the ListBox has focus, draw a focus rectangle around the selected item.
            e.DrawFocusRectangle();
        }
    }

}
