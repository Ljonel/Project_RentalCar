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
using System.Windows.Shapes;

namespace ProjectC_github
{
    /// <summary>
    /// Interaction logic for Wynajem.xaml
    /// </summary>
    public partial class Wynajem : Window
    {
        RentalCarEntities _db = new RentalCarEntities();
        IList<int> employeeList= new List<int>();
        IList<int> clientList = new List<int>();

        
        public Wynajem()
        {
            InitializeComponent();
            ShowCombobox();
            ShowRentalcar();
        }

        private void ShowCombobox()
        {
            ///summary
            ///Polecenie wyciągające z bazy tylko te samochody które aktualnie nie są wypożyczone
            ///summary 
            var nrQuery =
                    (from item in _db.samochody select item.nr_rejestracyjny)
                    .Except(from emp in _db.wynajem select emp.nr_rejestracyjny);
            
            var employeeQuery = from item in _db.pracownicy select item;
            var clientsQuery = from item in _db.klienci select item;
           
            foreach (var item in employeeQuery)
            {
                employeeList.Add(item.id_pracownika);
            }
            foreach (var item in clientsQuery)
            {
                clientList.Add(item.id_klienta);
            }

            Nr_rej.ItemsSource = nrQuery.ToList();
            Pracownicy.ItemsSource = employeeList;
            Klienci.ItemsSource = clientList;
        }

        /// <summary>
        /// SELECT wynajem.nr_rejestracyjny, wynajem.id_wynajmu, samochody.marka 
        /// FROM samochody 
        /// JOIN wynajem ON wynajem.nr_rejestracyjny=samochody.nr_rejestracyjny;
        /// </summary>
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
                           }).OrderBy(x=>x.Id_wynaj);
                tab_wynajem.ItemsSource = rental.ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
            var addCar = new wynajem()
            {
                data_od = Convert.ToDateTime(DataOd.Text),
                data_do = Convert.ToDateTime(DataDo.Text),
                nr_rejestracyjny = Nr_rej.SelectedItem.ToString(),
                id_pracownika = Convert.ToInt32(Pracownicy.SelectedItem),
                id_klienta = Convert.ToInt32(Klienci.SelectedItem)
            };
            _db.wynajem.Add(addCar);
            _db.SaveChanges();
            this.Hide();
        }
        private void UsunWynajecie_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new WynajemUsun();
        }
    }
}
