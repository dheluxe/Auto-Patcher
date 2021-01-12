using System;
using System.ComponentModel;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using IniParser;
using IniParser.Model;

namespace TYYongAutoPatcher
{

    public class RequestList
    {
        public string OFFICIAL_WEB { get; set; } = "http://127.0.0.1/";
        public string LEFT_WEB { get; set; } = "http://www.yong-online.com.tw/WebImages/launch/launcher.html";
        public string RIGHT_WEB { get; set; } = "http://www.yong-online.com.tw/boards.aspx";
        public string SHOP { get; set; } = "http://127.0.0.1/shop";
        public string EVENT { get; set; } = "http://127.0.0.1/event";
        public string REGISTER { get; set; } = "http://127.0.0.1/register";
        public string PATCH_LIST { get; set; } = "http://127.0.0.1/patch/list.txt";
        public string PATCH_DATA_DIR { get; set; } = "http://127.0.0.1/patch/data/";
    }

    class MainController
    {
        private readonly MainUI UI;
        private readonly WebClient client;
        private readonly FileIniDataParser ini;
        private string fileName = "config.ini";
        public RequestList url;

        public MainController(MainUI UI)
        {
            this.UI = UI;
            client = new WebClient();
            ini = new FileIniDataParser();
            url = new RequestList();
        }

        public void Init()
        {
            IniData data;
            try
            {
                data = ini.ReadFile(fileName);
                var type = typeof(RequestList);
                foreach (var item in url.GetType().GetProperties())
                {
                    var value = data["LAUNCHER_REQUEST_URL"][item.Name];
                    if (value != null)
                    {
                        PropertyInfo pInfo = type.GetProperty(item.Name);
                        pInfo.SetValue(url, Convert.ChangeType(value, pInfo.PropertyType), null);
                    }
                    else
                    {
                        data["LAUNCHER_REQUEST_URL"][item.Name] = item.GetValue(url, null).ToString();
                        ini.WriteFile(fileName, data);
                    }
                }
            }
            catch (Exception e)
            {
                data = new IniData();
                // Add request list to ini
                foreach (var item in url.GetType().GetProperties())
                {
                    data["LAUNCHER_REQUEST_URL"][item.Name] = item.GetValue(url, null).ToString();
                }

                data["PATCH"]["VERSION"] = "0";
                ini.WriteFile(fileName, data);
            }

            // Show left and right sides web.
            finally
            {
                UI.SetLeftAndRightWeb();
            }

        }

        public string ErrorMsg { get; set; } = "";
        public bool IsBusy { get; set; } = false;
        public void Launch()
        {

        }

        public void CloseLauncher()
        {
            UI.Close();
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
