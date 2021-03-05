using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TYYongAutoPatcher.src.Models;

namespace TYYongAutoPatcher.src.Controllers
{
    class LanguageController
    {
        private MainController app;
        private LanguageModel Languages;

        public Language Text;

        public LanguageController(MainController app)
        {
            this.app = app;
        }

        public void ReadLanguage()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resource = assembly.GetManifestResourceNames().Where(s => s.EndsWith("languages.json")).FirstOrDefault();
            using (Stream stream = assembly.GetManifestResourceStream(resource))
            using (StreamReader reader = new StreamReader(stream))
            {
                string json = reader.ReadToEnd(); //Make string equal to full file
                Languages = JsonConvert.DeserializeObject<LanguageModel>(json);
            }
        }

        public void SetLanguage(string lan)
        {
            switch (lan)
            {
                case "zh-HK":
                case "zh-TW":
                    Text = Languages.ZhHK;
                    break;
                case "zh-CN":
                    Text = Languages.ZhCN;
                    break;
                case "en-US":
                default:
                    Text = Languages.EnUS;
                    break;
            }
            app.Setting.LocalConfig.Language = lan;
            app.ui.UpdateUILanguage();
            app.UpdateLocalLanguage(lan);
        }
    }
}
