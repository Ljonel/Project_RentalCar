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
        IList<string> fuel = new List<string> { "benzyna", "diesel", "gas"};

        public Samochody()
        {
            InitializeComponent();
            Kategoria.ItemsSource = categories;
            Paliwo.ItemsSource = fuel;
            ShowCars();
     
        }
        private void ShowCars()
        {
            var query = from item in _db.samochody
                        select new
                        {
                            id_samochodu= item.id_samochodu,
                            Nr_rej = item.nr_rejestracyjny,
                            Marka = item.marka,
                            Model = item.model,
                            Wersja = item.wersja,
                            Rocznik = item.rocznik,
                            Poj_silnika = item.poj_silnika,
                            Rodzaj_paliwa = item.rodzaj_paliwa,
                            Przebieg = item.przebieg,
                            Cena = item.cennik.cena_za_dobe
                        };
            tab_samochody.ItemsSource = query.ToList();
        }

        private void AddCar_Click(object sender, RoutedEventArgs e)
        {
            
            
            if (String.IsNullOrEmpty(Nr_rej.Text) || String.IsNullOrEmpty(Marka.Text) || String.IsNullOrEmpty(Model.Text) || String.IsNullOrEmpty(Wersja.Text) || String.IsNullOrEmpty(Rocznik.Text) || String.IsNullOrEmpty(Poj_silnika.Text) || String.IsNullOrEmpty(Paliwo.Text) || String.IsNullOrEmpty(Przebieg.Text))
            {
                MessageBox.Show("Wprowadź dane");
            }
            else
            {
                try
                {
                    //Tutaj mialem wielki problem zeby to wymyslic, na szczęscie wpadłem w koncu na to jak poprawnie dodawac id cennika za pomocą kategorii z formularza
                    /*   Something like this in sql
                       INSERT INTO samochody (nr_rejestracyjny, marka, model, wersja, rocznik, poj_silnika, rodzaj_paliwa,przebieg,id_cennik)
                       VALUES ('WB 013','Toyota','Yaris','3','2020','2000','benzyna','2000', (SELECT id_cennik FROM cennik WHERE kategoria LIKE 'Samochody luksusowe'));
                    */
                    var k = Kategoria.SelectedItem.ToString();
                    var result = _db.cennik.Where(x => x.kategoria.Contains(k)).First();
                    //MessageBox.Show(result.id_cennik.ToString());

                    var addCar = new samochody()
                    {
                        nr_rejestracyjny = Nr_rej.Text,
                        marka = Marka.Text,
                        model = Model.Text,
                        wersja = Wersja.Text,
                        rocznik = Rocznik.Text,
                        poj_silnika = Poj_silnika.Text,
                        rodzaj_paliwa = Paliwo.SelectedItem.ToString(),
                        przebieg = int.Parse(Przebieg.Text),
                       id_cennik = result.id_cennik
                    };
                    _db.samochody.Add(addCar);
                    _db.SaveChanges();
                    MessageBox.Show("Dodano pomyślnie");
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
            try
            {
                if (String.IsNullOrEmpty(Nr_rej.Text) || String.IsNullOrEmpty(Marka.Text) || String.IsNullOrEmpty(Model.Text) || String.IsNullOrEmpty(Wersja.Text) || String.IsNullOrEmpty(Rocznik.Text) || String.IsNullOrEmpty(Poj_silnika.Text) || String.IsNullOrEmpty(Paliwo.Text) || String.IsNullOrEmpty(Przebieg.Text) || String.IsNullOrEmpty(Kategoria.Text))
                    MessageBox.Show("Nie można wykonać operacji");
                else
                {
                    var id = int.Parse(ID.Text);
                    var k = Kategoria.SelectedItem.ToString();
                    var result = _db.cennik.Where(x => x.kategoria.Contains(k)).First();

                    var applyEdit = (from item in _db.samochody where item.id_samochodu.Equals(id) select item).First();
                    applyEdit.nr_rejestracyjny = Nr_rej.Text;
                    applyEdit.marka = Marka.Text;
                    applyEdit.model = Model.Text;
                    applyEdit.wersja = Wersja.Text;
                    applyEdit.rocznik = Rocznik.Text;
                    applyEdit.poj_silnika = Poj_silnika.Text;
                    applyEdit.rodzaj_paliwa = Paliwo.SelectedItem.ToString();
                    applyEdit.przebieg = int.Parse(Przebieg.Text);
                    applyEdit.id_cennik = result.id_cennik;
                    _db.SaveChanges();
                    MessageBox.Show("Operacja wykonna pomyślnie");
                    Nr_rej.Text = "";
                    Marka.Text = "";
                    Model.Text = "";
                    Wersja.Text = "";
                    Rocznik.Text = "";
                    Poj_silnika.Text = "";
                    Paliwo.SelectedItem = null;
                    Przebieg.Text = "";
                    Kategoria.SelectedItem = null;
                    ShowCars();
                }
            }
            catch
            {
                MessageBox.Show("Nie można wykonać operacji");
            }
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
