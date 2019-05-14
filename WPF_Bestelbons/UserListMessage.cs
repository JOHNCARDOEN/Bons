using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using WPF_Bestelbons.Models;

namespace WPF_Bestelbons
{
    public class UserListMessage
    {
        public BindableCollection<User> userlist { get; set; }

        public UserListMessage(BindableCollection<User> userlistmessage)
        {
            userlist = userlistmessage;
        }

    }
}
