using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TYYongAutoPatcher
{
    public partial class frm_MainUI : Form
    {

        private bool isDraggin = false;
        private Point offset;
        private MainController app;

        public frm_MainUI()
        {
            InitializeComponent();
            app = new MainController();
        }

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

        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_launch_MouseEnter(object sender, EventArgs e)
        {
            if (app.ErrorMsg.Length > 0)
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
            if (app.ErrorMsg.Length > 0)
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
            if (app.ErrorMsg.Length > 0)
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
            if (app.ErrorMsg.Length > 0)
            {
                btn_launch.BackgroundImage = TYYongAutoPatcher.Properties.Resources.start4;
            }
            else
            {
                btn_launch.BackgroundImage = TYYongAutoPatcher.Properties.Resources.start1;
            }
        }

        private void btn_launch_MouseClick(object sender, MouseEventArgs e)
        {
            if (app.ErrorMsg.Length == 0)
            {
                app.launch();
            }
        }
    }
}
