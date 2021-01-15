using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using TYYongAutoPatcher.src.Controllers;
using TYYongAutoPatcher.src.Models;

namespace TYYongAutoPatcher.src.UI
{
    public partial class MainUI : Form
    {

        private bool isDraggin = false;
        private Point offset;
        private List<StateCode> sMsgList = new List<StateCode>();
        private MainController app;

        public MainUI()
        {
            InitializeComponent();
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(Path.GetFileName(System.Windows.Forms.Application.ExecutablePath));
            lbl_copyright.Text = fileVersionInfo.LegalCopyright;

            app = new MainController(this);

        }
        #region click handler
        // Open browser click...
        private void lbl_goToWeb_Click(object sender, EventArgs e)
        {
        }
        private void btn_exit_Click(object sender, EventArgs e)
        {
            app.CloseLauncher();
        }
        private void btn_shop_Click(object sender, EventArgs e)
        {
        }
        private void btn_register_Click(object sender, EventArgs e)
        {
        }
        private void btn_event_Click(object sender, EventArgs e)
        {
        }
        private void btn_launch_Click(object sender, EventArgs e)
        {

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
            if (!btn_launch.Enabled)
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
            if (!btn_launch.Enabled)
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
            if (!btn_launch.Enabled)
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
            if (!btn_launch.Enabled)
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
            web_left.Url = new Uri(app.Setting.Server.LeftWeb);
            web_right.Url = new Uri(app.Setting.Server.RightWeb);
            web_left.Visible = true;
            web_right.Visible = true;
        }

        public bool GetCbxStartWhenReady()
        {
            return cbx_startWhenReady.Checked;
        }

        public void ReadyToStartGame()
        {
            btn_launch.Enabled = true;
            btn_launch.BackgroundImage = TYYongAutoPatcher.Properties.Resources.start3;
            pgb_download.Value = 100;
            pgb_total.Value = 100;
            lbl_value_download.Text = "100%";
            lbl_value_total.Text = "100%";
            AddMsg("已成功連線並傳送遊戲資料.", StateCode.Success);
            AddMsg("已完成進入遊戲的各種設定.", StateCode.Success);
            if (cbx_startWhenReady.Checked) app.Launch();
        }


        //Add message to the listbox
        public void AddMsg(string msg, StateCode state = StateCode.Normal)
        {
            Invoke(new MethodInvoker(delegate ()
            {
                sMsgList.Add(state);
                lbx_messages.Items.Add(msg);
                lbx_messages.SelectedIndex = lbx_messages.Items.Count - 1;

            }));
        }

        //Update last message in the listbox
        public void UpdateMsg(string msg, StateCode state = StateCode.Normal)
        {
            Invoke(new MethodInvoker(delegate ()
            {
                sMsgList.RemoveAt(sMsgList.Count - 1);
                sMsgList.Add(state);
                lbx_messages.Items.RemoveAt(lbx_messages.Items.Count - 1);
                AddMsg(msg, state);

            }));
        }

        public EventHandler<ExtractProgressEventArgs> ExtractProgress(PatchModel patch)
        {
            return new EventHandler<ExtractProgressEventArgs>((sender, e) =>
            {
                if (e.EventType == ZipProgressEventType.Extracting_EntryBytesWritten)
                {
                    Invoke(new MethodInvoker(delegate ()
                    {
                        var name = e.CurrentEntry.FileName;
                        var eSize = Math.Round(e.BytesTransferred * 1.0 / 1024, 2);
                        var tSize = Math.Round(e.TotalBytesToTransfer * 1.0 / 1024, 2);
                        tSize = tSize == 0 ? 1 : tSize;
                        lbl_state.ForeColor = Color.IndianRed;
                        lbl_state.Text = $"正在解壓 {name} {app.SizeToString(e.BytesTransferred)} / {app.SizeToString(e.TotalBytesToTransfer)} ({e.BytesTransferred / e.TotalBytesToTransfer * 100}%)";

                        var total = app.GetTotalPercentage();
                        lbl_value_total.Text = $"{total.ToString(".#")}%";
                        pgb_total.Value = (int)total;

                    }));


                }
                else if (e.EventType == ZipProgressEventType.Extracting_BeforeExtractEntry)
                {

                }
            });
        }


        public DownloadProgressChangedEventHandler DownloadPatchProgressChangedHandler(PatchModel patch)
        {
            return new DownloadProgressChangedEventHandler((sender, e) =>
            {
                app.State = StateCode.Downloading;

                Invoke(new MethodInvoker(delegate ()
                {
                    patch.DownloadedSize = e.BytesReceived;
                    patch.Size = e.TotalBytesToReceive;
                    patch.DownloadedPercentage = e.ProgressPercentage;
                    pgb_download.Value = patch.DownloadedPercentage;
                    lbl_value_download.Text = $"{patch.DownloadedPercentage}%";
                    lbl_state.Text = $"正在下載 {patch.FileName} - {app.SizeToString(patch.DownloadedSize)} 共 {app.SizeToString(patch.Size)} ({patch.DownloadedPercentage}%)";

                    var total = app.GetTotalPercentage();
                    lbl_value_total.Text = $"{total.ToString(".#")}%";
                    pgb_total.Value = (int)total;
                }));
            });
        }

        public AsyncCompletedEventHandler AsyncCompletedEventHandler(PatchModel patch)
        {
            return new AsyncCompletedEventHandler((sender, e) =>
            {
                Invoke(new MethodInvoker(delegate ()
                {
                    //lbl_state.Text = $"己下載 {fileName} - {fileSzie}MB)";
                    AddMsg($"己下載 {patch.FileName} - 共 {app.SizeToString(patch.Size)}", StateCode.Success);
                    AddMsg($"正在解壓 {patch.FileName}...", StateCode.Extracting);
                    var total = app.GetTotalPercentage();
                    lbl_value_total.Text = $"{total.ToString(".#")}%";
                    pgb_total.Value = (int)total;
                }));
            });
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
                case StateCode.ErrorConnectingFail:
                case StateCode.ErrorWritingFail:
                case StateCode.ErrorExtractingFail:
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
            //e.DrawFocusRectangle();
        }

        public void Wait(int ms)
        {
            timer_wait.Enabled = true;
            timer_wait.Interval = ms;
            timer_wait.Start();
        }

        private async void timer_wait_Tick(object sender, EventArgs e)
        {
            timer_wait.Stop();
            timer_wait.Enabled = false;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            await app.init();
            watch.Stop();
            var elaspedMs = watch.ElapsedMilliseconds;
            AddMsg($"耗時: {elaspedMs / 1000}杪");
        }

        private  void MainUI_Load(object sender, EventArgs e)
        {
            timer_wait.Enabled = true;
            timer_wait.Start();
        }

        public void UpdateLblState(string msg)
        {

            switch (app.State)
            {
                case StateCode.ErrorConnectingFail:
                    lbl_state.ForeColor = Color.FromArgb(248, 63, 94);
                    lbl_state.Text = "無法連接到伺服器";
                    break;
                case StateCode.GameReady:
                    lbl_state.ForeColor = Color.FromArgb(177, 241, 167);
                    btn_launch.Enabled = true;
                    lbl_state.Text = "準備就緒";
                    ReadyToStartGame();
                    break;
                case StateCode.Downloading:
                    lbl_state.ForeColor = Color.LightSkyBlue;
                    lbl_state.Text = "下載中...";
                    break;
                case StateCode.Extracting:
                    lbl_state.ForeColor = Color.IndianRed;
                    lbl_state.Text = "解壓中...";
                    break;
                case StateCode.Initializing:
                    lbl_state.Text = "準備中...";
                    break;
                case StateCode.Retrying:
                    lbl_state.Text = "重新連接中...";
                    break;
                case StateCode.Error:
                    lbl_state.ForeColor = Color.FromArgb(248, 63, 94);
                    lbl_state.Text = "發生未知的錯誤";
                    break;
                default:
                    lbl_state.ForeColor = Color.FromArgb(255, 255, 255);
                    break;

            }
            if (msg.Length > 0)
            {
                lbl_state.Text = msg;
            }

        }
    }

}
