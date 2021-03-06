
using System.Collections.Generic;
using System.Drawing;
using TYYongAutoPatcher.src.Controllers;

namespace TYYongAutoPatcher.src.Models
{
    public class MessagesModel
    {

        public string Text { get; set; }
        public StateCode Color { get; set; }

        public MessagesModel(string Text, StateCode Color)
        {
            this.Text = Text;
            this.Color = Color;
        }
    }

}
