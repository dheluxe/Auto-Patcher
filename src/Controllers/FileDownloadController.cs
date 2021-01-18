using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TYYongAutoPatcher.src.Models;
using TYYongAutoPatcher.src.UI;

namespace TYYongAutoPatcher.src.Controllers
{
    class FileDownloadController
    {

        private MainController app;
        public WebClient client;

        public FileDownloadController(MainController app)
        {
            this.app = app;
        }

        public async Task DownloadFilesAysnc(Uri url, string savePath, PatchModel patch)
        {
            app.UpdateState(StateCode.Downloading);
            try
            {
                client = new WebClient();
                client.DownloadProgressChanged += app.ui.DownloadPatchProgressChangedHandler(patch);
                client.DownloadFileCompleted += app.ui.AsyncCompletedEventHandler(patch);
                app.ui.AddMsg($"正在下載 {patch.FileName}...", StateCode.Downloading);
                Stopwatch timer = app.ui.GetDownloadTimer();
                timer.Restart();
                timer.Start();
                await client.DownloadFileTaskAsync(url, savePath);
                client.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"*************FileDownloadController.DownloadFilesAysnc(Uri url, string savePath, PatchModel patch): {ex.Message}");
                app.UpdateState(StateCode.ErrorConnectingFail);
                app.ui.AddMsg($"無法取得更新包 {patch.FileName}", StateCode.ErrorConnectingFail);
            }
        }

        public async Task<string> DownloadStringAysnc(string url)
        {
            var client = new WebClient();
            var result = await client.DownloadStringTaskAsync(url);
            return result;
        }

        public async Task DownloadConfigAysnc(string url)
        {
            var client = new WebClient();
            client.DownloadStringCompleted += app.ui.DownloadStrinCompletedgAysncHandler;
            await client.DownloadStringTaskAsync(url);

        }


    }
}
