using Ionic.Zip;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
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


        // Wait for loading UI before running the app.
        private void MainUI_Load(object sender, EventArgs e)
        {
            timer_wait.Enabled = true;
            timer_wait.Start();
        }
        // Run the app.
        private async void timer_wait_Tick(object sender, EventArgs e)
        {
            timer_wait.Stop();
            timer_wait.Enabled = false;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                await app.Run();
            }
            catch (OperationCanceledException ex)
            {
                //lbl_state.ForeColor = Color.FromArgb(248, 63, 94);
                //lbl_state.Text = "✘更新失敗";
            }
            finally
            {
                watch.Stop();
                if (app.Setting.PatchList.Count > 0)
                {
                    lbl_report.ForeColor = lbl_state.ForeColor;
                    lbl_report.Text = $"耗時: {app.MSToString(watch.ElapsedMilliseconds)} |  " +
                                      $"已下載更新包: {app.GetNoOfDownloadedPatches()}個 - 共 {app.GetSizeOfDownloadedPatches()}  |  " +
                                      $"已安裝更新包: {app.GetNoOfUnzipped()}個 - 共 {app.GetSizeOfUnzipped()}";
                }

            }

        }

        #region click handler
        // Open browser click...
        private void lbl_goToWeb_Click(object sender, EventArgs e)
        {
        }
        private void btn_exit_Click(object sender, EventArgs e)
        {
            if (app.IsBusy())
            {
                DialogResult dResult = MessageBox.Show("正在更新中，是否取消?", "泰月勇Online 登錄器", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dResult.Equals(DialogResult.OK))
                {
                    app.Cancel();
                }
            }
            else
            {
                app.CloseLauncher();
            }
        }
        private void btn_shop_Click(object sender, EventArgs e)
        {
            if (app.IsUri(app.Setting.Server.Shop)) Process.Start(app.Setting.Server.Shop);
        }
        private void btn_register_Click(object sender, EventArgs e)
        {
            if (app.IsUri(app.Setting.Server.Register)) Process.Start(app.Setting.Server.Register);
        }
        private void btn_event_Click(object sender, EventArgs e)
        {
            if (app.IsUri(app.Setting.Server.Event)) Process.Start(app.Setting.Server.Event);
        }
        private void btn_launch_Click(object sender, EventArgs e)
        {
            app.Launch();
        }
        #endregion
        #region Mouse event UI control
        private void cbx_startWhenReady_MouseDown(object sender, MouseEventArgs e)
        {
            cbx_startWhenReady.ForeColor = Color.Gray;
        }
        private void cbx_startWhenReady_MouseUp(object sender, MouseEventArgs e)
        {
            cbx_startWhenReady.ForeColor = Color.LightGray;
        }
        //
        private void lbl_goToWeb_MouseDown(object sender, MouseEventArgs e)
        {
            lbl_goToWeb.ForeColor = Color.SaddleBrown;
        }
        private void lbl_goToWeb_MouseUp(object sender, MouseEventArgs e)
        {
            lbl_goToWeb.ForeColor = Color.Chocolate;
        }
        //
        private void btn_exit_MouseDown(object sender, MouseEventArgs e)
        {
            btn_exit.BackgroundImage = TYYongAutoPatcher.Properties.Resources.exit1;
        }
        private void btn_exit_MouseEnter(object sender, EventArgs e)
        {
            btn_exit.BackgroundImage = TYYongAutoPatcher.Properties.Resources.exit2;
        }
        private void btn_exit_MouseLeave(object sender, EventArgs e)
        {
            btn_exit.BackgroundImage = TYYongAutoPatcher.Properties.Resources.exit3;
        }
        private void btn_exit_MouseUp(object sender, MouseEventArgs e)
        {
            btn_exit.BackgroundImage = TYYongAutoPatcher.Properties.Resources.exit2;
        }
        //
        private void btn_launch_MouseLeave(object sender, EventArgs e)
        {
            btn_launch.BackgroundImage = TYYongAutoPatcher.Properties.Resources.start1;
        }
        private void btn_launch_MouseEnter(object sender, EventArgs e)
        {

            btn_launch.BackgroundImage = TYYongAutoPatcher.Properties.Resources.start2;
        }
        private void btn_launch_MouseUp(object sender, MouseEventArgs e)
        {

            btn_launch.BackgroundImage = TYYongAutoPatcher.Properties.Resources.start2;
        }
        private void btn_launch_MouseDown(object sender, MouseEventArgs e)
        {

            btn_launch.BackgroundImage = TYYongAutoPatcher.Properties.Resources.start3;
        }
        //
        private void btn_register_MouseLeave(object sender, EventArgs e)
        {
            btn_register.BackgroundImage = TYYongAutoPatcher.Properties.Resources.reg1;
        }
        private void btn_register_MouseEnter(object sender, EventArgs e)
        {
            btn_register.BackgroundImage = TYYongAutoPatcher.Properties.Resources.reg2;
        }
        private void btn_register_MouseUp(object sender, MouseEventArgs e)
        {
            btn_register.BackgroundImage = TYYongAutoPatcher.Properties.Resources.reg2;
        }
        private void btn_register_MouseDown(object sender, MouseEventArgs e)
        {
            btn_register.BackgroundImage = TYYongAutoPatcher.Properties.Resources.reg3;
        }
        //
        private void btn_shop_MouseLeave(object sender, EventArgs e)
        {
            btn_shop.BackgroundImage = TYYongAutoPatcher.Properties.Resources.itemshop1;
        }
        private void btn_shop_MouseEnter(object sender, EventArgs e)
        {
            btn_shop.BackgroundImage = TYYongAutoPatcher.Properties.Resources.itemshop2;
        }
        private void btn_shop_MouseUp(object sender, MouseEventArgs e)
        {
            btn_shop.BackgroundImage = TYYongAutoPatcher.Properties.Resources.itemshop2;
        }
        private void btn_shop_MouseDown(object sender, MouseEventArgs e)
        {
            btn_shop.BackgroundImage = TYYongAutoPatcher.Properties.Resources.itemshop3;
        }
        //
        private void btn_event_MouseLeave(object sender, EventArgs e)
        {
            btn_event.BackgroundImage = TYYongAutoPatcher.Properties.Resources.event1;
        }
        private void btn_event_MouseUp(object sender, MouseEventArgs e)
        {
            btn_event.BackgroundImage = TYYongAutoPatcher.Properties.Resources.event2;
        }
        private void btn_event_MouseEnter(object sender, EventArgs e)
        {
            btn_event.BackgroundImage = TYYongAutoPatcher.Properties.Resources.event2;
        }
        private void btn_event_MouseDown(object sender, MouseEventArgs e)
        {
            btn_event.BackgroundImage = TYYongAutoPatcher.Properties.Resources.event3;
        }
        #endregion
        #region Dragger, Normally, you dont need to edit it.
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
        // Set left and right web
        public void SetLeftAndRightWeb()
        {
            web_left.Url = new Uri(app.Setting.Server.LeftWeb);
            web_right.Url = new Uri(app.Setting.Server.RightWeb);
            web_left.Visible = true;
            web_right.Visible = true;
        }
        // Allow start the game.
        public async void ReadyToStartGame()
        {
            await app.DeleteAllTemp();
            btn_launch.Enabled = true;
            btn_launch.BackgroundImage = TYYongAutoPatcher.Properties.Resources.start3;
            pgb_progress.Value = 100;
            pgb_total.Value = 100;
            lbl_value_progress.Text = "100.0%";
            lbl_value_totalProgress.Text = "100.0%";
            AddMsg("已成功連線並傳送遊戲資料.", StateCode.Success);
            AddMsg("已完成進入遊戲的各種設定.", StateCode.Success);
            if (cbx_startWhenReady.Checked) app.Launch();
        }
 
        #region Update last message in the listbox
        //public void UpdateMsg(string msg, StateCode state = StateCode.Normal)
        //{
        //    Invoke(new MethodInvoker(delegate ()
        //    {
        //        sMsgList.RemoveAt(sMsgList.Count - 1);
        //        sMsgList.Add(state);
        //        lbx_messages.Items.RemoveAt(lbx_messages.Items.Count - 1);
        //        AddMsg(msg, state);

        //    }));
        //}
        #endregion
        #region UI Update for the async events.
        public void DownloadStrinCompletedgAysncHandler(Object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                var configJson = e.Result;
                var result = JsonConvert.DeserializeObject<SettingModel>(configJson);
                app.Setting.Server = result.Server;
                app.Setting.Game = result.Game;
                app.Setting.PatchList = result.PatchList.FindAll(x => x.Version > app.Setting.LocalConfig.Launcher.PatchVersion);
                app.Setting.PatchList.Sort();
            }

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
                        var percentage = 100.0 * e.BytesTransferred / e.TotalBytesToTransfer;
                        lbl_state.Text = $"↷正在安裝 {name} - {app.SizeToString(e.BytesTransferred)} / {app.SizeToString(e.TotalBytesToTransfer)}";

                        pgb_progress.Value = (int)percentage;
                        lbl_value_progress.Text = $"{percentage.ToString("N1")}%";

                        var total = app.GetTotalPercentage();
                        lbl_value_totalProgress.Text = $"{total.ToString("N1")}%";
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
                if (app.cts.Token.IsCancellationRequested)
                {
                    ((WebClient)sender).CancelAsync();
                    return;
                }
                app.State = StateCode.Downloading;

                Invoke(new MethodInvoker(delegate ()
                {
                    patch.DownloadedSize = e.BytesReceived;
                    patch.Size = e.TotalBytesToReceive;
                    patch.DownloadedPercentage = 100.0 * patch.DownloadedSize / patch.Size;
                    pgb_progress.Value = (int)patch.DownloadedPercentage;
                    lbl_value_progress.Text = $"{patch.DownloadedPercentage.ToString("N1")}%";
                    //lbl_state.Text = $"正在下載 {patch.FileName} - {app.SizeToString(patch.DownloadedSize)} 共 {app.SizeToString(patch.Size)} ({patch.DownloadedPercentage}%)";

                    var total = app.GetTotalPercentage();
                    lbl_value_totalProgress.Text = $"{total.ToString("N1")}%";
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
                    if (e.Error == null)
                    {
                        if (e.Cancelled)
                        {

                        }
                        else
                        {
                            //lbl_state.Text = $"己下載 {fileName} - {fileSzie}MB)";
                            AddMsg($"己下載更新包 {patch.FileName} - 共 {app.SizeToString(patch.Size)}", StateCode.Success);
                            AddMsg($"正在安裝更新包 {patch.FileName}...", StateCode.Extracting);
                            var total = app.GetTotalPercentage();
                            lbl_value_totalProgress.Text = $"{total.ToString("N1")}%";
                            pgb_total.Value = (int)total;
                        }
                    }

                }));
            });
        }
        #endregion

        // Customized listbox draw item handler.
        private void lbx_messages_DrawItem(object sender, DrawItemEventArgs e)
        {

            if (e.Index < 0) return;

            // Draw the background of the ListBox control for each item.
            e.DrawBackground();
            // Define the default color of the brush as black.

            Brush myBrush = Brushes.White;

            // Determine the color of the brush to draw each item based 
            // on the index of the item to draw.

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected) e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(99, 99, 99)), e.Bounds);

            switch (sMsgList[e.Index])
            {
                case StateCode.Error:
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
                    myBrush = new SolidBrush(Color.FromArgb(130, 192, 231));
                    break;
                case StateCode.Extracting:
                    myBrush = new SolidBrush(Color.FromArgb(255, 138, 0));
                    break;
                case StateCode.Retrying:
                    myBrush = new SolidBrush(Color.FromArgb(239, 39, 105));
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

        //Add message to the listbox
        public void AddMsg(string msg, StateCode state = StateCode.Normal)
        {
            Invoke(new MethodInvoker(delegate ()
            {
                var m = msg;
                sMsgList.Add(state);
                switch (state)
                {
                    case StateCode.Success:
                        m = $"✔{msg}";
                        break;
                    case StateCode.Downloading:
                        m = $"⭳{msg}";
                        break;
                    case StateCode.Extracting:
                        m = $"↷{msg}";
                        break;
                    case StateCode.Retrying:
                        m = $"⭮{msg}";
                        break;
                    case StateCode.Error:
                    case StateCode.ErrorConnectingFail:
                    case StateCode.ErrorExtractingFail:
                    case StateCode.ErrorWritingFail:
                        m = $"✘{msg}";
                        break;

                }
                lbx_messages.Items.Add(m);
                lbx_messages.SelectedIndex = lbx_messages.Items.Count - 1;

            }));
        }

        // update state label.
        public void UpdateLblState(string msg)
        {

            switch (app.State)
            {
                case StateCode.Error:
                case StateCode.ErrorExtractingFail:
                    lbl_state.ForeColor = Color.FromArgb(248, 63, 94);
                    lbl_state.Text = "✘發生未知的錯誤";
                    app.Cancel();
                    break;
                case StateCode.ErrorConnectingFail:
                    lbl_state.ForeColor = Color.FromArgb(248, 63, 94);
                    lbl_state.Text = "✘無法連接到伺服器";
                    app.Cancel();
                    break;
                case StateCode.ErrorWritingFail:
                    lbl_state.ForeColor = Color.FromArgb(248, 63, 94);
                    lbl_state.Text = "✘無法寫入本地檔案";
                    app.Cancel();
                    break;
                case StateCode.GameReady:
                case StateCode.UpdatedSuccess:
                    lbl_state.ForeColor = Color.FromArgb(177, 241, 167);
                    btn_launch.Enabled = true;
                    lbl_state.Text = "✔準備就緒";
                    ReadyToStartGame();
                    break;
                case StateCode.Downloading:
                    lbl_state.ForeColor = Color.FromArgb(130, 192, 231);
                    lbl_state.Text = "下載中更新包...";
                    break;
                case StateCode.Extracting:
                    lbl_state.ForeColor = Color.FromArgb(255, 138, 0);
                    lbl_state.Text = "安裝中...";
                    break;
                case StateCode.Initializing:
                    lbl_state.ForeColor = Color.FromArgb(44, 214, 168);
                    lbl_state.Text = "連接中...";
                    break;
                case StateCode.Retrying:
                    lbl_state.ForeColor = Color.FromArgb(239, 39, 105);
                    lbl_state.Text = "⭮重新連接中...";
                    break;
                case StateCode.Cancelled:
                    lbl_state.ForeColor = Color.FromArgb(248, 63, 94);
                    lbl_state.Text = "✘更新失敗";
                    break;
                default:
                    lbl_state.ForeColor = Color.FromArgb(255, 255, 255);
                    break;

            }
            if (app.State == StateCode.UpdatedSuccess)
                lbl_state.Text = "✔更新完畢";

            if (msg.Length > 0) lbl_state.Text = msg;
        }

        public void UpdateProgress()
        {
            app.UpdateState(StateCode.UpdatedSuccess);
        }

        public void ShowErrorMsg(string msg, string title = "泰月勇Online")
        {
            MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

}
