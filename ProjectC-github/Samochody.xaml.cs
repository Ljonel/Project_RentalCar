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
    /// Interaction logic for Samochody.xaml
    /// </summary>
    public partial class Samochody : Window
    {
        RentalCarEntities _db = new RentalCarEntities();
        IList<string> categories = new List<string> { "Samochody luksusowe", "Samochody rodzinne", "Samochody miejskie", "Samochody dostawcze" };
        public Samochody()
        {
            InitializeComponent();
            Kategoria.ItemsSource = categories;
            ShowCars();
        }
        private void ShowCars()
        {
            var query = from item in _db.samochody
                        select new CarShape
                        {
                            id_samochodu = item.id_samochodu,
                            nr_rejestracyjny = item.nr_rejestracyjny,
                            marka = item.marka,
                            model = item.model,
                            wersja = item.wersja,
                            rocznik = item.rocznik,
                            poj_silnika = item.poj_silnika,
                            rodzaj_paliwa = item.rodzaj_paliwa,
                            przebieg = item.przebieg,
                            cena = item.cennik.cena_za_dobe,
                        };
            tab_samochody.ItemsSource = query.ToList();
        }

        private void AddCar_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(Nr_rej.Text) || String.IsNullOrEmpty(Marka.Text) || String.IsNullOrEmpty(Model.Text) || String.IsNullOrEmpty(Wersja.Text) || String.IsNullOrEmpty(Rocznik.Text) || String.IsNullOrEmpty(Poj_silnika.Text) || String.IsNullOrEmpty(Paliwo.Text) || String.IsNullOrEmpty(Przebieg.Text) || String.IsNullOrEmpty(Cena.Text))
            {
                MessageBox.Show("Wprowadź dane");
            }
            else
            {
                try
                {
                    var addCar = new samochody()
                    {
                        nr_rejestracyjny = Nr_rej.Text,
                        marka = Marka.Text,
                        model = Model.Text,
                        wersja = Wersja.Text,
                        rocznik = Rocznik.Text,
                        poj_silnika = Poj_silnika.Text,
                        rodzaj_paliwa = Paliwo.Text,
                        przebieg = int.Parse(Przebieg.Text)
                    };
                    _db.samochody.Add(addCar);
                    _db.SaveChanges();
                    ShowCars();
                }
                catch
                {
                    MessageBox.Show("Nie mozna wykonać operacji");

                }

            }
        }
        private void EditCar_Click(object sender, RoutedEventArgs e)
        {
           
        }
        private void DeleteCar_Click(object sender, RoutedEventArgs e)
        {
            InputBox.Visibility = System.Windows.Visibility.Visible;
        }
        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
           
        }
        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            // NoButton Clicked
            InputBox.Visibility = System.Windows.Visibility.Collapsed;
            // Clear InputBox
            InputTextBox.Text = String.Empty;
        }
        private void tb_GotFocus(object sender, TextChangedEventArgs args)
        {
            
        }

        private void Walidacja_numer(System.Object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = IsTextNumeric(e.Text);

        }
        private static bool IsTextNumeric(string str)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^0-9]");
            return reg.IsMatch(str);
        }
    }
}
