

namespace TYYongAutoPatcher.src.Models
{
    public class ServerModel
    {
        public string OfficialWeb { get; set; }
        public string LeftWeb { get; set; }
        public string RightWeb { get; set; }
        public string Shop { get; set; }
        public string Event { get; set; }
        public string Register { get; set; }
        public string PatchDataDir { get; set; }

        public string Schema = @"
{
  'Server': {
    'OfficialWeb': 'string',
    'LeftWeb': 'string',
    'RightWeb': 'string',
    'Shop': 'string',
    'Event': 'string',
    'Register': 'string',
    'PatchDataDir': 'string'
  },
  'Game': {
    'Exe': 'string',
    'Arguments': 'string'
  },
  'PatchList': [
    {
      'Version': 0,
      'FileName': 'string'
    }
  ]
}
";
    }
}
