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

namespace ProjectC_github
{
    /// <summary>
    /// Interaction logic for WynajemUsun.xaml
    /// </summary>
    public partial class WynajemUsun : Page
    {
        RentalCarEntities _db = new RentalCarEntities();
        public WynajemUsun()
        {
            InitializeComponent();
            ShowRentalcar();
        }
        private void ShowRentalcar()
        {
            var rental = (from ep in _db.samochody
                          join e in _db.wynajem
                          on ep.nr_rejestracyjny
                          equals e.nr_rejestracyjny
                          select new
                          {
                              Id_wynaj = e.id_wynajmu,
                              Nr_Rej = e.nr_rejestracyjny,
                              Model = ep.model,
                              Marka = ep.marka,
                              Data_od = e.data_od,
                              Data_do = e.data_do,
                              Id_prac = e.id_pracownika,
                              Id_kli = e.id_klienta
                          }).OrderBy(x => x.Id_wynaj);
            tab_wynajem.ItemsSource = rental.ToList();
        }
        

    }
}
