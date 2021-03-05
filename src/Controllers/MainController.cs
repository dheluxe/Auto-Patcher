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
using TYYongAutoPatcher.src.Exceptions;
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
        ReadyToDownload,
        Retrying,
        Success,
        UpdatingCompleted,
        DeniedToDownload,
    }

    class MainController
    {
        public StateCode State { get; set; }
        public SettingModel Setting;
        public MainUI ui;
        public CancellationTokenSource cts;
        public CryptionController cryption;

        private FileDownloadController downloader;
        private ZipController zip;
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
            cryption = new CryptionController();

        }

        public void UpdateState(StateCode s, string msg = "")
        {
            State = s;
            ui.UpdateLblState(msg);
        }

        public async Task Run()
        {
            ReadLocalSetting();
            CheckVersionIsVaild();
            UpdateState(StateCode.Initializing);
            await DeleteTempFolderAndFiles();
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
                    if (Setting.Server.IsDownloadAllowed)
                    {
                        if (State == StateCode.ReadyToDownload)
                        {
                            foreach (var patch in Setting.PatchList)
                            {
                                var urlStr = $"{Setting.Server.URL.PatchDataDir}{patch.FileName}";
                                if (patch.DownloadLinks != null && patch.DownloadLinks.Length > 0)
                                {
                                    //TODO: Allow try to download different downloads link.
                                    urlStr = patch.DownloadLinks[0];
                                }
                                var url = new Uri(urlStr);
                                var fileName = $"{tempDir}{patch.FileName}";
                                await downloader.DownloadFilesAysnc(url, fileName, patch);
                                if (State != StateCode.ErrorConnectingFail)
                                {
                                    await zip.Unzip(fileName, targetDir, patch);
                                }
                            }
                        }
                    }
                    else
                    {
                        UpdateState(StateCode.DeniedToDownload, "需要手動下載並安裝更新包");
                        ui.AddMsg("需要手動下載並安裝更新包", StateCode.DeniedToDownload);
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
                    case StateCode.UpdatingCompleted:
                    case StateCode.DeniedToDownload:
                        return;
                    default:
                        UpdateState(StateCode.GameReady);
                        break;
                }
            }
        }

        private void CheckVersionIsVaild()
        {
            try
            {
                var deVer = double.Parse(cryption.Decrypt(Setting.LocalConfig.Launcher.Token));
                if (deVer != Setting.LocalConfig.Launcher.PatchVersion) throw new InvalidTokenException();
            }
            catch (Exception ex)
            {
                throw new InvalidTokenException();
            }
        }

        private async Task ReadyTempFolder()
        {
            try
            {
                if (Directory.Exists(tempDir)) await Task.Run(() => Directory.Delete(tempDir, true));
                // ui.AddMsg("準備文件中...");
                await Task.Run(() => Directory.CreateDirectory(tempDir));
                State = StateCode.ReadyToDownload;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"*************MainController.ReadyTempFolder(): {ex.Message}");
                UpdateState(StateCode.Error);
                //ui.AddMsg(ex.Message, StateCode.Error);
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
                Setting.LocalConfig.Launcher.Token = cryption.Encrypt(Setting.LocalConfig.Launcher.PatchVersion.ToString());
                File.WriteAllText(file, JsonConvert.SerializeObject(Setting.LocalConfig));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"*************MainController.WriteLocalSetting(): {ex.Message}");
                // If the file can't be wrotten , stop the program.
                UpdateState(StateCode.ErrorWritingFail);
                //ui.AddMsg(ex.Message);
            }
        }

        private async Task RetryConnecting()
        {
            var noOfRetry = 0;
            var maxNoOfRetry = 5;
            while (++noOfRetry <= maxNoOfRetry && State == StateCode.ErrorConnectingFail)
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
                ui.Ready();
                ui.UpdateVersion();
                UpdateState(StateCode.Configured);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"*************MainController.DownloadAndConfigSetting(): {ex.Message}");
                State = StateCode.ErrorConnectingFail;
            }

        }

        public double GetTotalPercentage()
        {
            double total = 0;

            foreach (var patch in Setting.PatchList)
            {
                var patchExtracted = patch.NoOfZippedFiles == 0 ? 0 : 100.0 * patch.NoOfUnZippedFiles / patch.NoOfZippedFiles;
                var p = (1.0 * patch.DownloadedPercentage + patchExtracted) / 2; // Two tasks percentages will be 200, so dividing by 2
                total += p;
            }

            total = total / Setting.PatchList.Count; // finished patches percentages divided by total patches.
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
            var format = order == 0 ? "{0:n0}" : "{0:n2}";
            return $"{string.Format(format, size)} {sizes[order]}";
        }

        public async void CloseLauncher()
        {
            await DeleteTempFolderAndFiles();
            ui.Close();
        }

        public async void Launch()
        {
            var gameExe = $"{pwd}\\{Setting.Game.Exe}";
            if (File.Exists(gameExe))
            {
                await DeleteTempFolderAndFiles();
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                //startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.FileName = Setting.Game.Exe;
                //startInfo.Arguments = Setting.Game.Arguments;
                startInfo.Arguments = "tyyong" + "_game_" + "start-A4908E588";
                process.StartInfo = startInfo;
                process.Start();
            }
            else
            {
                ui.ShowErrorMsg($"缺失遊戲文件，請嘗試重新安裝泰月勇Online!", $"找不到檔案 {Setting.Game.Exe}");
            }
            ui.Close();
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
            try
            {
                if (File.Exists(file)) await Task.Run(() => File.Delete(file));
            }
            catch (IOException ex)
            {
                Console.WriteLine($"*************MainController.DeleteTempFile(string fileName): {ex.Message}");
                //ui.AddMsg($"無法刪除 {fileName}", StateCode.Error);
            }
        }

        public async Task DeleteTempFolderAndFiles()
        {
            try
            {
                if (Directory.Exists(tempDir)) await Task.Run(() => Directory.Delete(tempDir, true));

            }
            catch (IOException ex)
            {
                Console.WriteLine($"*************MainController.DeleteTempFolderAndFiles(): {ex.Message}");
                //ui.AddMsg($"無法刪除 {tempDir}", StateCode.Error);
            }
        }

        public async void Cancel(string msg = "✘更新失敗")
        {
            cts.Cancel();
            await DeleteTempFolderAndFiles();
            UpdateState(StateCode.Cancelled, msg);
        }

        public void UpdateLocalPatchVersion(PatchModel patch)
        {
            try
            {
                Setting.LocalConfig.Launcher.PatchVersion = patch.Version;
                Setting.LocalConfig.Launcher.Token = cryption.Encrypt(patch.Version.ToString());
                var file = $"{settingDir}{settingFile}";
                File.WriteAllText(file, JsonConvert.SerializeObject(Setting.LocalConfig));
                ui.UpdateVersion();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"*************MainController.UpdateLocalPatchVersion(): {ex.Message}");
                // If the file can't be wrotten , stop the program.
                UpdateState(State = StateCode.ErrorWritingFail);
                ui.AddMsg(ex.Message);
            }
        }

        public bool IsUri(string uri)
        {
            Uri uriResult;
            return Uri.TryCreate(uri, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }


        public PatchReportModel GetReport()
        {
            var report = new PatchReportModel();
            foreach (var patch in Setting.PatchList)
            {
                report.SizeOfExtractedZippedFiles += patch.SizeOfZippedFiles;
                report.SizeOfExtractedUnzippedFiles += patch.SizeOfUnzippedFiles;
                report.SizeOfDownloadedPatches += patch.DownloadedSize;
                report.SizeOfDownloadPatches += patch.Size;
                report.DownloadSpeedPerSecond = patch.DownloadSpeedPerSecond > 0 ? patch.DownloadSpeedPerSecond : report.DownloadSpeedPerSecond;
                if (patch.DownloadedPercentage >= 100) report.NoOfDownloadedPatches++;
                if (patch.IsUnzipSucceed) report.NoOfUnzipped++;
            }
            //report.AvegragedownloadSpeedPerSecond = report.SizeOfDownloadPatches / report.TotalDownloadTime;
            //report.AvegragedownloadSpeedPerSecondToString = $"{SizeToString(report.AvegragedownloadSpeedPerSecond)} / 秒";

            report.DownloadSpeedPerSecondToString = $"{SizeToString(report.DownloadSpeedPerSecond)} / 秒";

            var pDownloaded = report.SizeOfDownloadedPatches / report.SizeOfDownloadPatches;
            pDownloaded = pDownloaded >= 1 ? report.NoOfDownloadedPatches : pDownloaded + report.NoOfDownloadedPatches;
            report.TotalPercentageOfDownloaded = 100 * (pDownloaded) / (Setting.PatchList.Count);

            if (report.TotalPercentageOfDownloaded > 0) report.TotalPercentageOfDownloadedToString = $"{report.TotalPercentageOfDownloaded.ToString("n2")}%";
            else report.TotalPercentageOfDownloadedToString = $"0.00%";

            var pExtracted = report.SizeOfExtractedZippedFiles / report.SizeOfDownloadedPatches;
            pExtracted = pExtracted >= 1 ? report.NoOfUnzipped : pExtracted + report.NoOfUnzipped;
            report.TotalPercentageOfExtracted = 100 * pExtracted / Setting.PatchList.Count;

            if (report.TotalPercentageOfExtracted > 0) report.TotalPercentageOfExtractedToString = $"{report.TotalPercentageOfExtracted.ToString("n2")}%";
            else report.TotalPercentageOfExtractedToString = $"0.00%";

            report.SizeOfExtractedZippedFilesToString = SizeToString(report.SizeOfExtractedZippedFiles);
            report.SizeOfExtractedUnzippedFilesToString = SizeToString(report.SizeOfExtractedUnzippedFiles);
            report.SizeOfDownloadedPatchesToString = SizeToString(report.SizeOfDownloadedPatches);

            return report;

        }

        public string WillAddZero(int time)
        {
            return time < 10 ? $"0{time}" : $"{time}";
        }

        public string MSToString(double ms)
        {
            var t = TimeSpan.FromMilliseconds(ms);
            var result = "";

            if (t.Hours == 0 && t.Minutes == 0 && t.Seconds == 0)
            {
                result = $"{(ms / 1000).ToString("N2")}秒 ";
            }
            else
            {
                result += $"{WillAddZero(t.Hours)} : ";
                result += $"{WillAddZero(t.Minutes)} : ";
                result += $"{WillAddZero(t.Seconds)}";
            }
            return result;
        }

    }
}
