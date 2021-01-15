﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        public FileDownloadController downloader;
        public ZipController zip;
        public StateCode State { get; set; }

        public string settingFile = "config.json";
        private readonly string tempDir = $"{Directory.GetCurrentDirectory()}\\data\\temp\\";

        public MainController(MainUI ui)
        {
            this.ui = ui;
            Setting = new SettingModel();
            downloader = new FileDownloadController(this);
            zip = new ZipController(this);
        }

        public void UpdateState(StateCode s, string msg = "")
        {
            State = s;
            ui.UpdateLblState(msg);
        }

        public async Task init()
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
                    string targetDir = $"{Directory.GetCurrentDirectory()}\\";
                    await ReadyTempFolder();
                    // Download patch and unzip
                    if (State == StateCode.ReadyToInstall)
                    {
                        foreach (var patch in Setting.PatchList)
                        {
                            var url = new Uri($"{Setting.Server.PatchDataDir}{patch.FileName}");
                            var fileName = $"{tempDir}{patch.FileName}";
                            await downloader.DownloadFilesAysnc(url, fileName, patch);
                            await zip.Unzip(fileName, targetDir, patch);
                        }
                    }
                }
            }
            UpdateState(StateCode.GameReady);
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
            try
            {
                // Load the local setting file
                using (StreamReader reader = new StreamReader(settingFile))
                {
                    // Read the setting json from local.
                    string json = reader.ReadToEnd();
                    Setting.LocalConfig = JsonConvert.DeserializeObject<LocalConfigModel>(json);
                }
            }
            catch (Exception e)
            {
                WriteLocalSetting();
            }
        }

        private void WriteLocalSetting()
        {
            try
            {
                // ui.AddMsg(e.Message);
                // Run below when file not found.
                // write the defualt setting json to local.
                File.WriteAllText(settingFile, JsonConvert.SerializeObject(Setting.LocalConfig));
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
                var configJson = await downloader.DownloadStringAysnc(Setting.LocalConfig.Launcher.Server);
                var result = JsonConvert.DeserializeObject<SettingModel>(configJson);
                Setting.Server = result.Server;
                Setting.Game = result.Game;
                Setting.PatchList = result.PatchList.FindAll(x => x.Verison > Setting.LocalConfig.Launcher.PatchVersion);
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
            Console.WriteLine(total);

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

        public void CloseLauncher()
        {
            //DeleteTemp();
            ui.Close();
        }

        public void Launch()
        {

        }
    }
}
