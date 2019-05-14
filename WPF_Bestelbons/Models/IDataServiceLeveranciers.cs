using Caliburn.Micro;

namespace WPF_Bestelbons.Models
{
    public interface IDataServiceLeveranciers
    {
        BindableCollection<Leverancier> GetAllLeveranciers(BindableCollection<Leverancier> Leverancierlist);
    }
}