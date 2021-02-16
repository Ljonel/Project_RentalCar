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
        /// <summary>
        ///Inicjalizacja połączenia z bazą danych "RentalCar", nazwa ADO.net w projekcie to "RentalCarEntities"
        /// </summary>
        RentalCarEntities _db = new RentalCarEntities();
        //Lista numerów ID dla klientów i pracowników dostępnych w wypożyczalni
        IList<int> employeeList= new List<int>();
        IList<int> clientList = new List<int>();
        
        public Wynajem()
        {
            InitializeComponent();
            ShowCombobox();
            ShowRentalcar();
        }

        ///summary
        ///Funkcja wpisujaca do ComboBoxów odpowiednie wartości 
        ///summary 
        private void ShowCombobox()
        {
            //Polecenie wyciągające z bazy tylko te numery rejestracyjne samochodów, które są aktualnie możliwe do wypożyczenia
            var nrQuery =
                    (from item in _db.samochody select item.nr_rejestracyjny)
                    .Except(from emp in _db.wynajem select emp.nr_rejestracyjny);
            //Polecenie wyciągające z bazy numery ID pracownikow 
            var employeeQuery = from item in _db.pracownicy select item;
            //Polecenie wyciągające z bazy numery ID klientów 
            var clientsQuery = from item in _db.klienci select item;
            //Wpisanie do list wyników poleceń LINQ
            foreach (var item in employeeQuery)
            {
                employeeList.Add(item.id_pracownika);
            }
            foreach (var item in clientsQuery)
            {
                clientList.Add(item.id_klienta);
            }
            //Wypisanie wartości w XAML
            Nr_rej.ItemsSource = nrQuery.ToList();
            Pracownicy.ItemsSource = employeeList;
            Klienci.ItemsSource = clientList;
        }

        /// <summary>
        /// SELECT wynajem.nr_rejestracyjny, wynajem.id_wynajmu, samochody.marka 
        /// FROM samochody 
        /// JOIN wynajem ON wynajem.nr_rejestracyjny=samochody.nr_rejestracyjny;
        /// Funkcja wyciąga z bazy dane samochodów które są aktualnie wypożyczone i wpisuje je do DataGrid
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

        /// <summary>
        /// Po kliknięciu w przycisk "Dodaj", dodany do bazy zostaje nowy rekord
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Warunek sprawdza czy TextBoxy są wypełnione. Jeśli nie wyświetlony zostaje odpowiedni komunikat.
           if(Pracownicy.SelectedItem == null || Klienci.SelectedItem == null || Nr_rej.SelectedItem == null || DataOd.SelectedDate == null || DataDo.SelectedDate == null)
            {
                MessageBox.Show("Wprowadź dane");
            }
            else
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
           
        }
        /// <summary>
        /// Po kliknięciu w przycisk "Usuń" nprogram nawiguje użytkownika do kolejnej strony
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UsunWynajecie_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new WynajemUsun();
        }
    }
}
