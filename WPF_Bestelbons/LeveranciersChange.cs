using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Bestelbons.Models;

namespace WPF_Bestelbons
{
    public class LeveranciersChange
    {
        public Leverancier Leverancier { get; set; }

        public LeveranciersChange(Leverancier leverancier)
        {
            Leverancier = leverancier;
        }
    }
}
