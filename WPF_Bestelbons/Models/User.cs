using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Bestelbons.Models
{
    public class User 
    {
        //private string _name;
        //private string _email;
        //private string _tel;

        //public string Name
        //{
        //    get { return _name; }
        //    set { _name = value;
        //         NotifyOfPropertyChange(() => Name);
        //    }
        //}

        //public string Email
        //{
        //    get { return _email; }
        //    set { _email = value;
        //        NotifyOfPropertyChange(() => Email);
        //    }
        //}


        //public string Tel
        //{
        //    get { return _tel; }
        //    set { _tel = value;
        //        NotifyOfPropertyChange(() => Tel);
        //    }
        //}

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Handtekening { get; set; }

    }
}
