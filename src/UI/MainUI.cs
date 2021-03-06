using Ionic.Zip;
using Microsoft.WindowsAPICodePack.Taskbar;
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
using TYYongAutoPatcher.src.Exceptions;
using TYYongAutoPatcher.src.Models;

namespace TYYongAutoPatcher.src.UI
{
    public partial class MainUI : Form
    {

        private bool isDraggin = false;
        private Point offset;
        private List<StateCode> sMsgList = new List<StateCode>();
        private MainController app;
        private Stopwatch watch;
        private Stopwatch downloadTimer;
        private FileVersionInfo fileVersionInfo;

        public List<List<MessagesModel>> Messages;

        public MainUI()
        {
            InitializeComponent();
            fileVersionInfo = FileVersionInfo.GetVersionInfo(Path.GetFileName(System.Windows.Forms.Application.ExecutablePath));
            lbl_copyright.Text = fileVersionInfo.LegalCopyright;
            app = new MainController(this);
            downloadTimer = Stopwatch.StartNew();

            Messages = new List<List<MessagesModel>>();
            Messages.Add(new List<MessagesModel>()); // zh-HK
            Messages.Add(new List<MessagesModel>()); // zh-CN
            Messages.Add(new List<MessagesModel>()); // en-US
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
            watch = Stopwatch.StartNew();
            app.init();
            try
            {
                await app.Run();
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine($"*************MainUI.timer_wait_Tick(object sender, EventArgs e): {ex.Message}");
                if (app.State == StateCode.Cancelled)
                {
                    for (var i = 0; i < app.ui.Messages.Count; i++)
                        Messages[i].Add(new MessagesModel($"{app.Language.Get(i).State.Cancelled}", StateCode.Error));
                    UpdateMsg();
                }

                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Error);
            }
            catch (InvalidTokenException ex)
            {
                ShowErrorMsg(app.Language.Text.UIComponent.Edited, app.Language.Text.UIComponent.AppName);
                this.Close();
            }
            finally
            {
                watch.Stop();
                ShowReport();
            }

        }

        public void ShowReport(bool isFinished = false)
        {
            if (app.Setting.PatchList.Count > 0)
            {
                var report = app.GetReport();
                var installedPercentage = isFinished ? "100.00%" : report.TotalPercentageOfExtractedToString;
                var downloadedPercentage = isFinished ? "100.00%" : report.TotalPercentageOfDownloadedToString;
                var text = app.Language.Text.UIComponent;
                lbl_report.ForeColor = lbl_state.ForeColor;
                lbl_report.Text = $" {text.Lbl_report1}{app.MSToString(watch.ElapsedMilliseconds)}  |  " +
                                  $" {text.Lbl_report2}{report.DownloadSpeedPerSecondToString}  |  " +
                                  $" {text.Lbl_report3}{report.NoOfDownloadedPatches}{text.Lbl_report_unit1} - {text.Lbl_report_unit2} {report.SizeOfDownloadedPatchesToString} ({downloadedPercentage})  |  " +
                                  $" {text.Lbl_report4}{report.NoOfUnzipped}{text.Lbl_report_unit1} - {text.Lbl_report_unit2} {report.SizeOfExtractedUnzippedFilesToString} ({installedPercentage})";
            }
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            if (app.IsBusy())
            {
                var text = app.Language.Text.UIComponent;
                DialogResult dResult = MessageBox.Show(text.ConfirmCancel, text.AppName, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
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

        // Allow start the game.
        public async void ReadyToStartGame()
        {
            await app.DeleteTempFolderAndFiles();
            btn_launch.Enabled = true;
            btn_launch.BackgroundImage = TYYongAutoPatcher.Properties.Resources.btn_basc_2;
            UpdateProgres(100, 100);
            var text = app.Language.Text.UIComponent;
            for (var i = 0; i < app.ui.Messages.Count; i++)
                Messages[i].Add(new MessagesModel($"{app.Language.Get(i).UIComponent.ReadyMsg1}", StateCode.Success));
            UpdateMsg();
            for (var i = 0; i < app.ui.Messages.Count; i++)
                Messages[i].Add(new MessagesModel($"{app.Language.Get(i).UIComponent.ReadyMsg2}", StateCode.Success));
            UpdateMsg();
            if (cbx_startWhenReady.Checked) app.Launch();
            if (app.Setting.PatchList.Count == 0) TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
        }

        #region click handler
        // Open browser click...
        private void lbl_goToWeb_Click(object sender, EventArgs e)
        {
            if (app.IsUri(app.Setting.Server.URL.OfficialWeb)) Process.Start(app.Setting.Server.URL.OfficialWeb);
        }
        private void btn_shop_Click(object sender, EventArgs e)
        {
            if (app.IsUri(app.Setting.Server.URL.Shop)) Process.Start(app.Setting.Server.URL.Shop);
        }
        private void btn_register_Click(object sender, EventArgs e)
        {
            if (app.IsUri(app.Setting.Server.URL.Register)) Process.Start(app.Setting.Server.URL.Register);
        }
        private void btn_event_Click(object sender, EventArgs e)
        {
            if (app.IsUri(app.Setting.Server.URL.Event)) Process.Start(app.Setting.Server.URL.Event);
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
            lbl_officialWeb.ForeColor = Color.SaddleBrown;
        }
        private void lbl_goToWeb_MouseUp(object sender, MouseEventArgs e)
        {
            lbl_officialWeb.ForeColor = Color.Chocolate;
        }
        //
        private void btn_exit_MouseDown(object sender, MouseEventArgs e)
        {
            btn_exit.BackgroundImage = TYYongAutoPatcher.Properties.Resources.btn_exit_1;
        }
        private void btn_exit_MouseEnter(object sender, EventArgs e)
        {
            btn_exit.BackgroundImage = TYYongAutoPatcher.Properties.Resources.btn_exit_2;
        }
        private void btn_exit_MouseLeave(object sender, EventArgs e)
        {
            btn_exit.BackgroundImage = TYYongAutoPatcher.Properties.Resources.btn_exit_3;
        }
        private void btn_exit_MouseUp(object sender, MouseEventArgs e)
        {
            btn_exit.BackgroundImage = TYYongAutoPatcher.Properties.Resources.btn_exit_3;
        }
        private void btn_launch_MouseDown(object sender, MouseEventArgs e)
        {

            btn_launch.BackgroundImage = TYYongAutoPatcher.Properties.Resources.btn_basc_1;
        }
        //
        private void btn_launch_MouseLeave(object sender, EventArgs e)
        {
            btn_launch.BackgroundImage = TYYongAutoPatcher.Properties.Resources.btn_basc_2;
        }
        private void btn_launch_MouseUp(object sender, MouseEventArgs e)
        {

            btn_launch.BackgroundImage = TYYongAutoPatcher.Properties.Resources.btn_basc_3;
        }
        private void btn_launch_MouseEnter(object sender, EventArgs e)
        {

            btn_launch.BackgroundImage = TYYongAutoPatcher.Properties.Resources.btn_basc_3;
        }
        //
        private void btn_register_MouseDown(object sender, MouseEventArgs e)
        {
            btn_reg.BackgroundImage = TYYongAutoPatcher.Properties.Resources.btn_basc_1_s;
        }
        private void btn_register_MouseLeave(object sender, EventArgs e)
        {
            btn_reg.BackgroundImage = TYYongAutoPatcher.Properties.Resources.btn_basc_2_s;
        }
        private void btn_register_MouseUp(object sender, MouseEventArgs e)
        {
            btn_reg.BackgroundImage = TYYongAutoPatcher.Properties.Resources.btn_basc_3_s;
        }
        private void btn_register_MouseEnter(object sender, EventArgs e)
        {
            btn_reg.BackgroundImage = TYYongAutoPatcher.Properties.Resources.btn_basc_3_s;
        }
        //
        private void btn_shop_MouseDown(object sender, MouseEventArgs e)
        {
            btn_shop.BackgroundImage = TYYongAutoPatcher.Properties.Resources.btn_basc_1;
        }
        private void btn_shop_MouseLeave(object sender, EventArgs e)
        {
            btn_shop.BackgroundImage = TYYongAutoPatcher.Properties.Resources.btn_basc_2;
        }
        private void btn_shop_MouseUp(object sender, MouseEventArgs e)
        {
            btn_shop.BackgroundImage = TYYongAutoPatcher.Properties.Resources.btn_basc_3;
        }
        private void btn_shop_MouseEnter(object sender, EventArgs e)
        {
            btn_shop.BackgroundImage = TYYongAutoPatcher.Properties.Resources.btn_basc_3;
        }
        //
        private void btn_event_MouseDown(object sender, MouseEventArgs e)
        {
            btn_event.BackgroundImage = TYYongAutoPatcher.Properties.Resources.btn_basc_1;
        }
        private void btn_event_MouseLeave(object sender, EventArgs e)
        {
            btn_event.BackgroundImage = TYYongAutoPatcher.Properties.Resources.btn_basc_2;
        }
        private void btn_event_MouseUp(object sender, MouseEventArgs e)
        {
            btn_event.BackgroundImage = TYYongAutoPatcher.Properties.Resources.btn_basc_3;
        }
        private void btn_event_MouseEnter(object sender, EventArgs e)
        {
            btn_event.BackgroundImage = TYYongAutoPatcher.Properties.Resources.btn_basc_3;
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
        public void Ready()
        {
            web_left.Url = new Uri(app.Setting.Server.URL.LeftWeb);
            web_right.Url = new Uri(app.Setting.Server.URL.RightWeb);
            web_left.Visible = true;
            web_right.Visible = true;
            btn_event.Enabled = true;
            btn_shop.Enabled = true;
            btn_reg.Enabled = true;
            lbl_officialWeb.Enabled = true;
            lbl_officialWeb.ForeColor = Color.Chocolate;
            btn_event.BackgroundImage = TYYongAutoPatcher.Properties.Resources.btn_basc_2;
            btn_shop.BackgroundImage = TYYongAutoPatcher.Properties.Resources.btn_basc_2;
            btn_reg.BackgroundImage = TYYongAutoPatcher.Properties.Resources.btn_basc_2_s;
        }

        public void UpdateVersion()
        {
            var size = app.Setting.PatchList.Count;
            var cur = app.Setting.LocalConfig.Launcher.PatchVersion;
            if (size > 0 && app.Setting.PatchList[size - 1].Version != cur)
            {
                lbl_value_currentVer.Text = $"{cur.ToString("N1")}";
                lbl_value_latestVer.Text = $"{app.Setting.PatchList[size - 1].Version.ToString("N1")}";
                lbl_title_latestVer.ForeColor = Color.OrangeRed;
                lbl_value_latestVer.ForeColor = Color.OrangeRed;
            }
            else
            {
                lbl_value_currentVer.Text = $"{cur.ToString("N1")}";
                lbl_value_latestVer.Text = $"{cur.ToString("N1")}";
                lbl_title_latestVer.ForeColor = Color.LightGreen;
                lbl_value_latestVer.ForeColor = Color.LightGreen;
            }
        }

        public void SetLatestVer()
        {

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
                ShowReport();
            }

        }

        public void UpdateProgres(double current, double total)
        {
            pgb_progress.Value = (int)current;
            lbl_value_progress.Text = $"{current.ToString("N1")}%";
            lbl_value_totalProgress.Text = $"{total.ToString("N1")}%";
            pgb_total.Value = (int)total;
            TaskbarManager.Instance.SetProgressValue((int)total, 100);
        }
        public EventHandler<ExtractProgressEventArgs> ExtractProgress(PatchModel patch)
        {
            return new EventHandler<ExtractProgressEventArgs>((sender, e) =>
            {
                if (e.EventType == ZipProgressEventType.Extracting_EntryBytesWritten)
                {
                    Invoke(new MethodInvoker(delegate ()
                    {
                        var text = app.Language.Text.UIComponent;
                        var name = e.CurrentEntry.FileName;
                        lbl_state.Text = $"{text.InstallFailed} {name} - {app.SizeToString(e.BytesTransferred)} / {app.SizeToString(e.TotalBytesToTransfer)}";
                        var percentage = 100.0 * e.BytesTransferred / e.TotalBytesToTransfer;
                        UpdateProgres(percentage, app.GetTotalPercentage());
                        ShowReport();
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
                    var elapsedSecond = downloadTimer.ElapsedMilliseconds / 1000.0;
                    patch.DownloadSpeedPerSecond = patch.DownloadedSize / elapsedSecond;
                    patch.TotalDownloadTime = elapsedSecond;
                    patch.Size = e.TotalBytesToReceive;
                    patch.DownloadedPercentage = 100.0 * patch.DownloadedSize / patch.Size;
                    //lbl_state.Text = $"正在下載 {patch.FileName} - {app.SizeToString(patch.DownloadedSize)} 共 {app.SizeToString(patch.Size)} ({patch.DownloadedPercentage}%)";
                    UpdateProgres(patch.DownloadedPercentage, app.GetTotalPercentage());
                    ShowReport();
                }));
            });
        }
        public AsyncCompletedEventHandler AsyncCompletedEventHandler(PatchModel patch)
        {
            return new AsyncCompletedEventHandler((sender, e) =>
            {
                Invoke(new MethodInvoker(delegate ()
                {
                    downloadTimer.Stop();
                    if (e.Error == null)
                    {
                        if (e.Cancelled)
                        {

                        }
                        else
                        {
                            var text = app.Language.Text.UIComponent;
                            for (var i = 0; i < app.ui.Messages.Count; i++)
                                Messages[i].Add(new MessagesModel($"{app.Language.Get(i).UIComponent.Downloaded} {patch.FileName} - {app.Language.Get(i).UIComponent.Lbl_report_unit2} {app.SizeToString(patch.Size)}", StateCode.Downloaded));
                            UpdateMsg();
                            for (var i = 0; i < app.ui.Messages.Count; i++)
                                Messages[i].Add(new MessagesModel($"{app.Language.Get(i).UIComponent.Installing} {patch.FileName}...", StateCode.Extracting));
                            UpdateMsg();
                            UpdateProgres(100, app.GetTotalPercentage());
                            ShowReport();
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
                case StateCode.DeniedToDownload:
                    myBrush = new SolidBrush(Color.FromArgb(248, 63, 94));
                    break;
                case StateCode.Success:
                case StateCode.Extracted:
                case StateCode.Downloaded:
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
                    case StateCode.Extracted:
                    case StateCode.Downloaded:
                        m = $"✔ {msg}";
                        break;
                    case StateCode.Downloading:
                        m = $"⭳ {msg}";
                        break;
                    case StateCode.Extracting:
                        m = $"↷ {msg}";
                        break;
                    case StateCode.Retrying:
                        m = $"⭮ {msg}";
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
        public void UpdateLblState(string msg, bool autoReady = true)
        {
            State text = app.Language.Text.State;
            switch (app.State)
            {
                case StateCode.DeniedToDownload:
                    lbl_state.ForeColor = Color.FromArgb(248, 63, 94);
                    break;
                case StateCode.Error:
                    lbl_state.ForeColor = Color.FromArgb(248, 63, 94);
                    app.Cancel(text.Error);
                    break;
                case StateCode.FailedToDownload:
                    lbl_state.ForeColor = Color.FromArgb(248, 63, 94);
                    msg = text.FailedToDownload;
                    break;
                case StateCode.ErrorExtractingFail:
                    lbl_state.ForeColor = Color.FromArgb(248, 63, 94);
                    app.Cancel(text.ErrorExtractingFail, false);
                    msg = text.ErrorExtractingFail;
                    break;
                case StateCode.ErrorConnectingFail:
                    lbl_state.ForeColor = Color.FromArgb(248, 63, 94);
                    app.Cancel(text.ErrorConnectingFail, false);
                    msg = text.ErrorConnectingFail;
                    break;
                case StateCode.ErrorWritingFail:
                    lbl_state.ForeColor = Color.FromArgb(248, 63, 94);
                    app.Cancel(text.ErrorWritingFail);
                    break;
                case StateCode.GameReady:
                case StateCode.UpdatingCompleted:
                    lbl_state.ForeColor = Color.FromArgb(177, 241, 167);
                    btn_launch.Enabled = true;
                    lbl_state.Text = $"✔ {text.GameReady}";
                    if (autoReady) ReadyToStartGame();
                    break;
                case StateCode.Downloading:
                    lbl_state.ForeColor = Color.FromArgb(130, 192, 231);
                    lbl_state.Text = text.Downloading;
                    break;
                case StateCode.Extracting:
                    lbl_state.ForeColor = Color.FromArgb(255, 138, 0);
                    lbl_state.Text = text.Extracting;
                    break;
                case StateCode.Initializing:
                    lbl_state.ForeColor = Color.FromArgb(44, 214, 168);
                    lbl_state.Text = text.Initializing;
                    break;
                case StateCode.Retrying:
                    lbl_state.ForeColor = Color.FromArgb(239, 39, 105);
                    lbl_state.Text = $"⭮ {text.Retrying} ";
                    break;
                case StateCode.Cancelled:
                    lbl_state.ForeColor = Color.FromArgb(248, 63, 94);
                    if (msg.Length > 0) lbl_state.Text = msg;
                    break;
                default:
                    lbl_state.ForeColor = Color.FromArgb(255, 255, 255);
                    break;

            }
            if (app.State == StateCode.UpdatingCompleted)
                lbl_state.Text = $"✔ {text.UpdatingCompleted}";

            if (msg.Length > 0) lbl_state.Text = msg;
        }

        public void CompeteUpdating()
        {
            app.UpdateState(StateCode.UpdatingCompleted);
            timer_delay.Enabled = true;
            timer_delay.Start();
        }

        public void ShowErrorMsg(string msg, string title = "")
        {
            var t = title.Length > 0 ? title : fileVersionInfo.ProductName;
            MessageBox.Show(msg, t, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void timer_delay_Tick(object sender, EventArgs e)
        {
            timer_delay.Enabled = false;
            ShowReport(true);
        }

        public Stopwatch GetDownloadTimer() { return downloadTimer; }

        public void UpdateUILanguage()
        {
            UIComponent text = app.Language.Text.UIComponent;

            var font = new Font("Microsoft YaHei", btn_event.Font.Size);
            var sFont = new Font("Microsoft YaHei", btn_reg.Font.Size);
            var mFont = new Font("Microsoft YaHei", cbx_startWhenReady.Font.Size);
            if (app.Setting.LocalConfig.Language.IndexOf("zh") == -1)
            {
                font = new Font("Algerian", btn_event.Font.Size);
                sFont = new Font("Algerian", btn_reg.Font.Size);
                mFont = new Font("Algerian", cbx_startWhenReady.Font.Size);
            }

            btn_shop.Font = font;
            btn_launch.Font = font;
            btn_event.Font = font;
            btn_reg.Font = sFont;
            cbx_startWhenReady.Font = mFont;

            btn_shop.Text = text.Btn_shop;
            btn_launch.Text = text.Btn_launch;
            btn_event.Text = text.Btn_event;
            btn_reg.Text = text.Btn_reg;
            lbl_title_currentVer.Text = text.Lbl_title_currentVer;
            lbl_title_latestVer.Text = text.Lbl_title_latestVer;
            lbl_title_progress.Text = text.Lbl_title_progress;
            lbl_title_totalProgress.Text = text.Lbl_title_totalProgress;
            lbl_officialWeb.Text = text.Lbl_officialWeb;
            cbx_startWhenReady.Text = text.Cbx_startWhenReady;
            UpdateLblState("", false);
            ShowReport();
            UpdateMsg(true);
        }

        private void lbl_en_Click(object sender, EventArgs e)
        {
            app.Language.SetLanguage("en-US");
        }

        private void lbl_tw_Click(object sender, EventArgs e)
        {
            app.Language.SetLanguage("zh-HK");
        }

        private void lbl_cn_Click(object sender, EventArgs e)
        {
            app.Language.SetLanguage("zh-CN");
        }

        public void UpdateMsg(bool willReRender = false)
        {
            var locale = app.Language.GetLocaleCode(app.Language.Locale);
            if (willReRender)
            {
                lbx_messages.Items.Clear();
                for (var i = 0; i < Messages[locale].Count; i++)
                {
                    AddMsg(Messages[locale][i].Text, Messages[locale][i].Color);
                }
            }
            else
            {
                var lastIdx = Messages[locale].Count - 1;
                var lastMsg = Messages[locale][lastIdx];
                AddMsg(lastMsg.Text, lastMsg.Color);
            }

        }
    }

}
