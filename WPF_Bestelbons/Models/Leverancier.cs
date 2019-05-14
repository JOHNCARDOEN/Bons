using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace WPF_Bestelbons.Models
{
   public class Leverancier : PropertyChangedBase
    {
        private bool _emailValid;

        public bool EmailValid
        {
            get { return _emailValid; }
            set
            {
                _emailValid = value;
                NotifyOfPropertyChange(() => EmailValid);
            }
        }

        public string Name { get; set; }
        private string _email;

        public string Email
        {
            get { return _email; }
            set
            {
                string pattern = "^(?(\")(\".+?(?<!\\\\)\"@)|(([0-9a-z]((\\.(?!\\.))|[-!#\\$%&\'\\*\\+/=\\?\\^`\\{\\}\\|~\\w])*)(?<=[0-9a-z])@))(?(\\[)(\\[(\\d{1,3}\\.){3}\\d{1,3}\\])|(([0-9a-z][-\\w]*[0-9a-z]*\\.)+[a-z0-9][\\-a-z0-9]{0,22}[a-z0-9]))$";
                Match match = Regex.Match(value, pattern);
                if (match.Success)
                {
                    EmailValid = true;
                }
                else
                {
                    EmailValid = false;
                }

                _email = value;
            }
        }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Postcode { get; set; }
        public string City { get; set; }    
        public string Tel { get; set; }

    }
}
