

namespace TYYongAutoPatcher.src.Models
{
    public class LauncherModel
    {
        // get config.json url
        public string Server { get; set; } = "http://192.168.0.139/patch/config.json";

        // local patch version
        public int PatchVersion { get; set; } = 0;
    }
}
