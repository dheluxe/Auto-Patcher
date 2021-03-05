

using Newtonsoft.Json;

namespace TYYongAutoPatcher.src.Models
{
    public class GameModel
    {
        public string Exe { get; set; }
        public Arguments Arguments { get; set; }
    }

    public class Arguments
    {
        [JsonProperty("zh-HK")]
        public string ZhHK { get; set; }
        [JsonProperty("zh-CN")]
        public string ZhCN { get; set; }
        [JsonProperty("en-US")]
        public string EnUS { get; set; }

    }
}
