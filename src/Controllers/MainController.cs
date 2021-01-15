using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TYYongAutoPatcher.src.Models;
using TYYongAutoPatcher.src.UI;

namespace TYYongAutoPatcher.src.Controllers
{
    public enum StateCode
    {
        Cancelled,
        Configured,
        Connecting,
        Downloaded,
        Downloading,
        Error,
        ErrorConnectingFail,
        ErrorExtractingFail,
        ErrorWritingFail,
        Extracted,
        Extracting,
        GameReady,
        Initializing,
        Loading,
        Normal,
        ReadyToInstall,
        Retrying,
        Success,
    }

    class MainController
    {
        public SettingModel Setting;
        public MainUI ui;
        private FileDownloadController downloader;
        private ZipController zip;
        public CancellationTokenSource cts;

        public StateCode State { get; set; }

        private string settingFile;
        private string settingDir;
        private readonly string pwd;
        private readonly string tempDir;

        public MainController(MainUI ui)
        {
            this.ui = ui;
            Setting = new SettingModel();
            downloader = new FileDownloadController(this);
            zip = new ZipController(this);
            cts = new CancellationTokenSource();
            pwd = Directory.GetCurrentDirectory();
            settingDir = $"{pwd}\\data\\";
            settingFile = "setting.json";
            tempDir = $"{pwd}\\data\\temp\\";

        }

        public void UpdateState(StateCode s, string msg = "")
        {
            State = s;
            ui.UpdateLblState(msg);
        }

        public async Task Run()
        {

            State = StateCode.Initializing;
            ReadLocalSetting();
            await DownloadAndConfigSetting();
            if (State == StateCode.ErrorConnectingFail)
            {
                await RetryConnecting();
            }

            if (State == StateCode.Configured)
            {
                if (Setting.PatchList.Count > 0)
                {
                    string targetDir = $"{pwd}\\";
                    await ReadyTempFolder();
                    // Download patch and unzip
                    if (State == StateCode.ReadyToInstall)
                    {
                        foreach (var patch in Setting.PatchList)
                        {
                            var url = new Uri($"{Setting.Server.PatchDataDir}{patch.FileName}");
                            var fileName = $"{tempDir}{patch.FileName}";
                            await downloader.DownloadFilesAysnc(url, fileName, patch);
                            if (State != StateCode.ErrorConnectingFail)
                            {
                                await zip.Unzip(fileName, targetDir, patch);

                            }
                        }
                    }
                }
                if (cts.IsCancellationRequested) cts.Token.ThrowIfCancellationRequested();
                switch (State)
                {
                    case StateCode.Error:
                    case StateCode.Extracting:
                    case StateCode.ErrorConnectingFail:
                    case StateCode.ErrorExtractingFail:
                    case StateCode.ErrorWritingFail:
                    case StateCode.GameReady:
                        return;
                    default:
                        UpdateState(StateCode.GameReady);
                        break;
                }
            }
        }

        private async Task ReadyTempFolder()
        {
            if (Directory.Exists(tempDir)) await Task.Run(() => Directory.Delete(tempDir, true));
            // ui.AddMsg("準備文件中...");
            try
            {
                await Task.Run(() => Directory.CreateDirectory(tempDir));
                State = StateCode.ReadyToInstall;
            }
            catch (Exception ex)
            {
                UpdateState(StateCode.Error);
                ui.AddMsg(ex.Message, StateCode.Error);
            }
        }

        private void ReadLocalSetting()
        {
            var file = $"{settingDir}{settingFile}";
            if (File.Exists(file)) using (StreamReader reader = new StreamReader(file))
                {
                    // Read the setting json from local.
                    string json = reader.ReadToEnd();
                    Setting.LocalConfig = JsonConvert.DeserializeObject<LocalConfigModel>(json);
                }
            else WriteLocalSetting();

        }

        private void WriteLocalSetting()
        {
            try
            {
                // ui.AddMsg(e.Message);
                // Run below when file not found.
                // write the defualt setting json to local.
                var file = $"{settingDir}{settingFile}";
                Directory.CreateDirectory(settingDir);
                File.WriteAllText(file, JsonConvert.SerializeObject(Setting.LocalConfig));
            }
            catch (Exception ex)
            {
                // If the file can't be wrotten , stop the program.
                UpdateState(State = StateCode.ErrorWritingFail);
                ui.AddMsg(ex.Message);
            }
        }

        private async Task RetryConnecting()
        {
            var noOfRetry = 0;
            var maxNoOfRetry = 5;
            while (++noOfRetry <= maxNoOfRetry)
            {
                UpdateState(StateCode.Retrying);
                ui.AddMsg($"嘗試連接到伺服器中... {noOfRetry} / {maxNoOfRetry} ", StateCode.Retrying);
                await DownloadAndConfigSetting();
            }
            if (State == StateCode.ErrorConnectingFail)
            {
                UpdateState(StateCode.ErrorConnectingFail);
                ui.AddMsg("與伺服器失去連線.", StateCode.ErrorConnectingFail);
            }
        }

        private async Task DownloadAndConfigSetting()
        {
            try
            {
                await downloader.DownloadConfigAysnc(Setting.LocalConfig.Launcher.Server);
                //var configJson = await downloader.DownloadStringAysnc(Setting.LocalConfig.Launcher.Server);
                //var result = JsonConvert.DeserializeObject<SettingModel>(configJson);
                //Setting.Server = result.Server;
                //Setting.Game = result.Game;
                //Setting.PatchList = result.PatchList.FindAll(x => x.Version > Setting.LocalConfig.Launcher.PatchVersion);
                //Setting.PatchList.Sort();
                ui.SetLeftAndRightWeb();
                UpdateState(StateCode.Configured);
            }
            catch (Exception e)
            {
                State = StateCode.ErrorConnectingFail;
            }

        }

        public double GetTotalPercentage()
        {
            double total = 0;

            foreach (var patch in Setting.PatchList)
            {
                var patchExtracted = patch.NoOfZippedFiles == 0 ? 0 : 100.0 * patch.NoOfUnZippedFiles / patch.NoOfZippedFiles;
                var p = (1.0 * patch.DownloadedPercentage + patchExtracted) / 2;
                total += p;
            }

            total = total / Setting.PatchList.Count;
            //Console.WriteLine(total);

            return total;
        }

        public string SizeToString(double s)
        {
            double size = s;
            string[] sizes = { "Bytes", "KB", "MB", "GB", "TB" };
            int order = 0;
            while (size >= 1024 && order < sizes.Length - 1)
            {
                order++;
                size /= 1024;
            }
            return String.Format("{0:0.##} {1}", size, sizes[order]);
        }

        public async void CloseLauncher()
        {
            await DeleteAllTemp();
            ui.Close();

        }



        public void Launch()
        {
            var gameExe = $"{pwd}\\{Setting.Game.Exe}";
            if (File.Exists(gameExe))
            {
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                //startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.FileName = Setting.Game.Exe;
                startInfo.Arguments = Setting.Game.Arguments;
                process.StartInfo = startInfo;
                process.Start();
            }
            else
            {
                ui.ShowErrorMsg($"缺失遊戲文件，請嘗試重新安裝泰月勇Online!", $"找不到{Setting.Game.Exe}");
            }
            CloseLauncher();

        }

        public bool IsBusy()
        {
            switch (State)
            {
                case StateCode.Downloading:
                case StateCode.Extracting:
                    return true;
                default:
                    return false;
            }
        }

        public async Task DeleteTempFile(string fileName)
        {
            var file = $"{tempDir}{fileName}";
            Console.WriteLine(file);
            try
            {
                if (File.Exists(file)) await Task.Run(() => File.Delete(file));
            }
            catch (Exception ex)
            {
                //ui.AddMsg($"無法刪除 {fileName}", StateCode.Error);
            }
        }

        public async Task DeleteAllTemp()
        {
            try
            {
                if (Directory.Exists(tempDir)) await Task.Run(() => Directory.Delete(tempDir, true));

            }
            catch (Exception ex)
            {
                //ui.AddMsg($"無法刪除 {tempDir}", StateCode.Error);
            }
        }

        public async void Cancel()
        {
            cts.Cancel();
            await DeleteAllTemp();
        }

        public void UpdateLocalPatchVersion(PatchModel patch)
        {
            try
            {
                Setting.LocalConfig.Launcher.PatchVersion = patch.Version;
                var file = $"{settingDir}{settingFile}";
                File.WriteAllText(file, JsonConvert.SerializeObject(Setting.LocalConfig));
            }
            catch (Exception ex)
            {
                // If the file can't be wrotten , stop the program.
                UpdateState(State = StateCode.ErrorWritingFail);
                ui.AddMsg(ex.Message);
            }
        }
    }
}
