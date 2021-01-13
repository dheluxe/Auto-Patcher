using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using IniParser;
using IniParser.Model;
using Newtonsoft.Json;
using TYYongAutoPatcher.src;

namespace TYYongAutoPatcher
{

    public class MainController
    {
        private readonly MainUI ui;
        private readonly string settingFilePath = "setting.json";
        private WebClient client;
        public string ErrorMsg { get; set; } = "";

        // Number of re trying connecting
        public int NoTried { get; set; } = 0;

        // Maximum of re trying connecting
        public int MaxNoTried { get; set; } = 5;

        public Setting Setting { get; set; }

        private StateCode state;
        public StateCode State
        {
            get { return state; }
            set
            {
                if (value == StateCode.GameReady)
                {
                    if (ui.GetCbxStartWhenReady()) Launch();
                    ui.ReadyToStartGame();
                }
                state = value;
            }
        }

        public MainController(MainUI ui)
        {
            this.ui = ui;
            Setting = new Setting();
            client = new WebClient();
        }

        public void Init()
        {
            try
            {
                state = StateCode.Initializing;
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

        public void Patch()
        {

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
                ui.SetLeftAndRightWeb();
                Patch();
            }
            else
            {
                state = StateCode.Retrying;
                if (++NoTried <= MaxNoTried)
                {
                    ui.AddMsg($"嘗試連接到伺服器中... {NoTried} / {MaxNoTried} ", StateCode.Retrying);
                    WebClient internode = (WebClient)sender;
                    internode.DownloadStringAsync(new Uri(Setting.LocalSetting.Launcher.Server));

                }
                else
                {
                    state = StateCode.Error;
                    ui.AddMsg("與伺服器失去連線.", StateCode.Error);
                }
            };
        }

        private void DonwloadConfigFileAsync()
        {
            client.DownloadProgressChanged += (sender, e) =>
            {
                var loading = e.ProgressPercentage;
            };
            client.DownloadStringCompleted += DonwloadConfigCompleted;

            client.DownloadStringAsync(new Uri(Setting.LocalSetting.Launcher.Server));
        }
        #endregion

        public bool IsBusy()
        {
            return client.IsBusy || state == StateCode.Extracting;
        }
        public void Launch()
        {

        }

        public void DeleteTempFiles(bool willRestorePatchVer)
        {

        }

        public void CloseLauncher()
        {
            DeleteTempFiles(true);
            ui.Close();
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
