using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Bestelbons.Models;

namespace WPF_Bestelbons
{
   public class ErrorMessage
    {
        public Error Error { get; set; }

        public ErrorMessage (Error error)
        {
            Error = error;
        }
    }
}
