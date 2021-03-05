using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TYYongAutoPatcher.src.Models
{
    public enum LanguageEnum
    {
        TW,
        EN,
        CN,
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class UIComponent
    {
        public string Btn_launch { get; set; }
        public string Btn_reg { get; set; }
        public string Btn_shop { get; set; }
        public string Btn_event { get; set; }
        public string Lbl_title_progress { get; set; }
        public string Lbl_title_totalProgress { get; set; }
        public string Lbl_officialWeb { get; set; }
        public string Lbl_title_currentVer { get; set; }
        public string Lbl_title_latestVer { get; set; }
        public string Cbx_startWhenReady { get; set; }
        public string ErrorConnectingFail { get; set; }
        public string Retrying { get; set; }
        public string Edited { get; set; }
        public string Lbl_report1 { get; set; }
        public string Lbl_report2 { get; set; }
        public string Lbl_report3 { get; set; }
        public string Lbl_report4 { get; set; }
        public string Lbl_report_unit1 { get; set; }
        public string Lbl_report_unit2 { get; set; }
        public string ConfirmCancel { get; set; }
        public string AppName { get; set; }
        public string ReadyMsg1 { get; set; }
        public string ReadyMsg2 { get; set; }
        public string UnitSecond { get; set; }
        public string Downloading { get; set; }
        public string DownloadFailed { get; set; }
        public string InstallFailed { get; set; }
        public string IntalledPatch { get; set; }
    }

    public class State
    {
        public string DeniedToDownload { get; set; }
        public string Error { get; set; }
        public string ErrorConnectingFail { get; set; }
        public string ErrorWritingFail { get; set; }
        public string GameReady { get; set; }
        public string Downloading { get; set; }
        public string Extracting { get; set; }
        public string Initializing { get; set; }
        public string Retrying { get; set; }
        public string UpdatingCompleted { get; set; }
        public string Cancelled { get; set; }
        public string Failed { get; set; }
    }

    public class Language
    {
        public UIComponent UIComponent { get; set; }
        public State State { get; set; }
    }

    public class LanguageModel
    {
        [JsonProperty("zh-HK")]
        public Language ZhHK { get; set; }
        [JsonProperty("zh-CN")]
        public Language ZhCN { get; set; }
        [JsonProperty("en-US")]
        public Language EnUS { get; set; }

    }
}
