using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Bestelbons.Models;

namespace WPF_Bestelbons
{
    public class AddLeverancier
    {
        public Leverancier Leverancier { get; set; }
        public bool Edit { get; set; }

        public AddLeverancier(Leverancier leverancier, bool edit)
        {
            Leverancier = leverancier;
            Edit = edit;
        }
    }
}
