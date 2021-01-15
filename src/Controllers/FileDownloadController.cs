using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TYYongAutoPatcher.src.Models;
using TYYongAutoPatcher.src.UI;

namespace TYYongAutoPatcher.src.Controllers
{
    class FileDownloadController
    {

        private MainController app;

        public FileDownloadController(MainController app)
        {
            this.app = app;
        }

        public async Task DownloadFilesAysnc(Uri url, string savePath, PatchModel patch)
        {
            app.UpdateState(StateCode.Downloading);
            try
            {
                WebClient client = new WebClient();
                client.DownloadProgressChanged += app.ui.DownloadPatchProgressChangedHandler(patch);
                client.DownloadFileCompleted += app.ui.AsyncCompletedEventHandler(patch);
                app.ui.AddMsg($"正在下載 {patch.FileName}...");
                await client.DownloadFileTaskAsync(url, savePath);
            }
            catch (Exception e)
            {
                app.UpdateState(StateCode.ErrorConnectingFail);
                app.ui.AddMsg($"無法取得文件 {url}");
            }
        }

        public async Task<string> DownloadStringAysnc(string url)
        {
            WebClient client = new WebClient();
            var result = await client.DownloadStringTaskAsync(url);
            return result;
        }
    }
}
