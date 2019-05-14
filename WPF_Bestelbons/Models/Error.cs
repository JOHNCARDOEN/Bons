using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Bestelbons.Models
{
    public class Error
    {
        public ErrorLevel Level { get; set; }
        public string ErrorMessage { get; set; }
        public bool Active { get; set; }
    }

    public enum ErrorLevel
    {
        Error,
        Warning,
        Info
    }
}
