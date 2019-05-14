using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_Bestelbons.Views
{
    /// <summary>
    /// Interaction logic for BestelbonOpmaakView.xaml
    /// </summary>
    public partial class BestelbonOpmaakView : UserControl
    {
        public BestelbonOpmaakView()
        {
            InitializeComponent();
        }

        private void Aantal_MouseEnter(object sender, MouseEventArgs e)
        {
            var tb = (TextBox)sender;
            if (tb != null)
            {
                tb.SelectAll();
                tb.Focus();

            }
        }

        private void Prijs_MouseEnter(object sender, MouseEventArgs e)
        {
            var tb = (TextBox)sender;
            if (tb != null)
            {
                tb.SelectAll();
                tb.Focus();

            }
        }
    }
}
