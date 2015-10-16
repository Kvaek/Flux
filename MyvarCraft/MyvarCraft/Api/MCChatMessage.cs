using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Api
{
    public class MCChatMessage
    {
        public MCChatMessage(string message)
        {
            text = message;
        }

        public string text = string.Empty;
        public bool bold = false;
        public bool italic = false;
        public bool underlined = false;
        public bool strikethrough = false;
        public bool obfuscated = false;
    }
}
