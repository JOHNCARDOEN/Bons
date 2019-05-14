using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Bestelbons
{
    public class BestelbonChange
    {
        public string Bestelbon { get; set; }

        public BestelbonChange(string bestelbon)
        {
            Bestelbon = bestelbon;
        }
    }
}
