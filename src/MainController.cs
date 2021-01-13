using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Ionic.Zip;
using System.Threading;

using TYYongAutoPatcher.src;

namespace TYYongAutoPatcher
{

    public class MainController
    {
        private readonly MainUI ui;
        private readonly string settingFilePath = "setting.json";
        private readonly string tempDir = $"{Directory.GetCurrentDirectory()}\\data\\temp\\";
        private WebClient client;
        private readonly ZipFile zipper;
        private int CurrentPatchVer { set; get; } = 0;
        public string ErrorMsg { get; set; } = "";

        // Number of re trying connecting
        public int NoTried { get; set; } = 0;

        // Maximum of re trying connecting
        public int MaxNoTried { get; set; } = 5;

        public Setting Setting { get; set; }

        public StateCode State { get; set; }

        public MainController(MainUI ui)
        {
            this.ui = ui;
            Setting = new Setting();
            client = new WebClient();
            zipper = new ZipFile();
        }

        public void Init()
        {
            try
            {
                State = StateCode.Initializing;
                using (StreamReader reader = new StreamReader(settingFilePath))
                {
                    // Read the setting json from local.
                    string json = reader.ReadToEnd();
                    Setting.LocalSetting = JsonConvert.DeserializeObject<LocalSetting>(json);
                    // Download config.json from the web server.
                    DonwloadConfigFileAsync();
                }
            }
            catch (Exception e)
            {
                //data = new IniData();
                //ini.WriteFile(fileName, data);
                ui.AddMsg(e.Message);

                try
                {
                    // write the defualt setting json.
                    File.WriteAllText(@"setting.json", JsonConvert.SerializeObject(Setting.LocalSetting));
                    DonwloadConfigFileAsync();

                }
                catch (Exception ex)
                {
                    ui.AddMsg(ex.Message);
                }
            }
        }


        private void ExtractProgress(Object sender, ExtractProgressEventArgs e)
        {
            if (e.EventType == ZipProgressEventType.Extracting_EntryBytesWritten)
            {
                var name = e.CurrentEntry.FileName;
                var eSize = e.BytesTransferred;
                var tSize = e.TotalBytesToTransfer;
                tSize = tSize == 0 ? 1 : tSize;
                ui.UpdateMsg($"正在解壓 {name} {eSize}/{tSize} ({eSize / tSize * 100}%)", StateCode.Extracting);

            }
            else if (e.EventType == ZipProgressEventType.Extracting_BeforeExtractEntry)
            {

            }
        }
        private void Unzip(string fileName)
        {
            string targetDir = $"{Directory.GetCurrentDirectory()}\\";
            using (var zip = ZipFile.Read(fileName))
            {
                zip.ExtractProgress += ExtractProgress;
                ui.AddMsg($"正在解壓 ", StateCode.Extracting);
                foreach (var entry in zip)
                {
                   entry.Extract (targetDir, ExtractExistingFileAction.OverwriteSilently);
                }
            }

        }


        private AsyncCompletedEventHandler DownloadPatchCompleted(string fileName)
        {
            return new AsyncCompletedEventHandler((sender, e) =>
            {

                if (e.Cancelled)
                {
                    // Delete files
                    return;
                }
                if (e.Error == null)
                {
                    WebClient internode = (WebClient)sender;
                    Unzip(fileName);
                    if (Setting.PatchList.Count > 0) Patch(internode);

                }
                else
                {

                }
            });
        }


        private DownloadProgressChangedEventHandler DownloadPatchProgressChangedHandler(string fileName)
        {
            return new DownloadProgressChangedEventHandler((sender, e) =>
            {
                State = StateCode.Downloading;
                var downloaded = Math.Round(e.BytesReceived * 1.0 / 1024 / 1024, 2);
                var fileSize = Math.Round(e.TotalBytesToReceive * 1.0 / 1024 / 1024, 2);
                if (e.ProgressPercentage == 100) ui.UpdateMsg($"完成下載 {fileName} - {downloaded}MB 共 {fileSize}M ({e.ProgressPercentage}%)", StateCode.Downloading);
                else ui.UpdateMsg($"正在下戴 {fileName} - {downloaded}MB 共 {fileSize}MB ({e.ProgressPercentage}%)", StateCode.Downloading);
                ui.UpdateDownloadProgress(e.ProgressPercentage);
            });
        }

        public void Patch(WebClient sender = null)
        {
            var wc = sender == null ? client : sender;

            if (Setting.PatchList.Count > 0)
            {
                var patch = Setting.PatchList[0];
                if (!Directory.Exists(tempDir)) Directory.CreateDirectory(tempDir);

                var fileName = $"{ tempDir }{ patch.FileName}";
                var url = new Uri($"{Setting.ServerConnection.PatchDataDir}{patch.FileName}");

                wc.DownloadFileCompleted += DownloadPatchCompleted(fileName);
                wc.DownloadProgressChanged += DownloadPatchProgressChangedHandler(patch.FileName);
                ui.AddMsg($"正在準準 {fileName}", StateCode.Downloading);
                wc.DownloadFileAsync(url, fileName);

                // Update patch version
                Setting.LocalSetting.Launcher.PatchVersion = patch.Verison;
                // Delete old patch version
                Setting.PatchList.Remove(patch);
            }

            else
            {
                ReadyToStartGame();
            }

        }

        #region Download Config File Async
        private void DonwloadConfigCompleted(Object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                // Delete files
                return;
            }
            if (e.Error == null)
            {
                var result = JsonConvert.DeserializeObject<Setting>(e.Result);
                Setting.ServerConnection = result.ServerConnection;
                Setting.Game = result.Game;
                Setting.PatchList = result.PatchList.FindAll(x => x.Verison > Setting.LocalSetting.Launcher.PatchVersion);
                ui.SetLeftAndRightWeb();
                //Ready dir for download patch files.
                if (!Directory.Exists(tempDir)) Directory.CreateDirectory(tempDir);
                Patch();
                //ui.Wait(1000);
            }
            else
            {
                State = StateCode.Retrying;
                if (++NoTried <= MaxNoTried)
                {
                    ui.AddMsg($"嘗試連接到伺服器中... {NoTried} / {MaxNoTried} ", StateCode.Retrying);
                    WebClient internode = (WebClient)sender;
                    internode.DownloadStringAsync(new Uri(Setting.LocalSetting.Launcher.Server));
                }
                else
                {
                    State = StateCode.Error;
                    ui.AddMsg("與伺服器失去連線.", StateCode.Error);
                }
            };
        }

        private void DownloadConfigProgressChangedHandler(object sender, DownloadProgressChangedEventArgs e)
        {

        }

        private void DonwloadConfigFileAsync()
        {
            var url = new Uri(Setting.LocalSetting.Launcher.Server);
            client.DownloadProgressChanged += DownloadConfigProgressChangedHandler;
            client.DownloadStringCompleted += DonwloadConfigCompleted;
            client.DownloadStringAsync(url);


        }
        #endregion

        private void DeleteTemp()
        {
            Directory.Delete(tempDir, true);

            // FOR DEV SO DEL
            //if (State == StateCode.GameReady) UpdateLocalSettingFile();
        }

        private void UpdateLocalSettingFile()
        {
            File.WriteAllText(@"setting.json", JsonConvert.SerializeObject(Setting.LocalSetting));
        }

        public bool IsBusy()
        {
            return client.IsBusy || State == StateCode.Extracting;
        }
        public void Launch()
        {

        }

        public void CloseLauncher()
        {
            DeleteTemp();
            ui.Close();
        }

        private void ReadyToStartGame()
        {
            State = StateCode.GameReady;
            ui.ReadyToStartGame();
        }

        private void FileDownloadComplete(object sender, AsyncCompletedEventArgs e)
        {

        }

        public string HashString(string s)
        {
            var sb = new StringBuilder();

            using (HashAlgorithm algorithm = SHA256.Create())
            {
                foreach (byte b in algorithm.ComputeHash(Encoding.UTF8.GetBytes(s)))
                    sb.Append(b.ToString("X2"));
            }
            return sb.ToString();
        }



    }
}
