using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Bestelbons.Models;

namespace WPF_Bestelbons
{
    public class InitialUserMessage
    {
        public User user { get; set; }

        public InitialUserMessage(User message)
        {
            user = message;
        }
    }
}
