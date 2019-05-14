using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Bestelbons.Models;

namespace WPF_Bestelbons
{
    public class UserChangedMessage
    {
        public User user { get; set; }

        public UserChangedMessage(User message)
        {
            user = message;
        }
    }
}
