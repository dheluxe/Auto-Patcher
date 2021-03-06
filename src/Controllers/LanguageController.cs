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
        public string Locale;
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
            Locale = lan;
            Text = Get(lan);
            app.Setting.LocalConfig.Language = lan;
            app.ui.UpdateUILanguage();
            app.UpdateLocalLanguage(lan);
        }

        public Language Get(string lan)
        {
            switch (lan)
            {
                case "zh-HK":
                case "zh-TW":
                    return Languages.ZhHK;
                case "zh-CN":
                    return Languages.ZhCN;
                case "en-US":
                default:
                    return Languages.EnUS;
            }
        }

        public Language Get(int lan)
        {
            switch (lan)
            {
                case 0:
                    return Languages.ZhHK;
                case 1:
                    return Languages.ZhCN;
                case 2:
                default:
                    return Languages.EnUS;
            }
        }

        public int GetLocaleCode(string lan)
        {
            switch (lan)
            {
                case "zh-HK":
                case "zh-TW":
                    return 0;
                case "zh-CN":
                    return 1;
                case "en-US":
                default:
                    return 2;
            }
        }
    }
}
