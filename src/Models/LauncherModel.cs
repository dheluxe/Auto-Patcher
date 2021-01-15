

namespace TYYongAutoPatcher.src.Models
{
    public class LauncherModel
    {
        // get config.json url
        public string Server { get; set; } = "http://127.0.0.1/patch/config.json";

        // local patch version
        public int PatchVersion { get; set; } = 0;
    }
}
