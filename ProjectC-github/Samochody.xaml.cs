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
        /// <summary>
        ///Inicjalizacja połączenia z bazą danych "RentalCar", nazwa ADO.net w projekcie to "RentalCarEntities"
        /// </summary>
        RentalCarEntities _db = new RentalCarEntities();
        /// Listy z których wybierane są odpowiednie dane dla narzuconych z góry kategorii samochodów i paliwa
        IList<string> categories = new List<string> { "Samochody luksusowe", "Samochody rodzinne", "Samochody miejskie", "Samochody dostawcze" };
        IList<string> fuel = new List<string> { "benzyna", "diesel", "gas"};
        /// <summary>
        /// Po załadowaniu okna do ComboBoxów wpisywane są dane z list
        /// Oraz wyświetlene zostają samochody dostępne w bazie danych
        /// </summary>
        public Samochody()
        {
            InitializeComponent();
            Kategoria.ItemsSource = categories;
            Paliwo.ItemsSource = fuel;
            ShowCars();
        }
        /// <summary>
        /// Funkcja wyciąga dane z bazy RantalCar i wpisuje je do tablicy samochodów DataGrid w XAML'u
        /// </summary>
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
        /// <summary>
        /// Funkcja pobiera dane z formularza i dodaje je do bazy danych
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    //Tutaj przez to jak skonstruowałem sobie baze mialem wielki problem zeby to wymyslic, na szczęscie wpadłem w koncu na to jak poprawnie dodawac id cennika za pomocą kategorii z formularza
                    /* W SQL
                       INSERT INTO samochody (nr_rejestracyjny, marka, model, wersja, rocznik, poj_silnika, rodzaj_paliwa,przebieg,id_cennik)
                       VALUES ('WB 013','Toyota','Yaris','3','2020','2000','benzyna','2000', (SELECT id_cennik FROM cennik WHERE kategoria LIKE 'Samochody luksusowe'));*/
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
        /// <summary>
        /// Funkcja pobiera dane z formularza i edytuje w bazie ten rekord którego ID podał uzytkownik
        /// Po wpisaniu przez użytkownika ID, formularz automatycznie pola danymi które odpowiadają ID tego rekordu w bazie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    
                    //Po udanej operacji formularz zostaje wyczyszczony
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
        /// <summary>
        /// Po kliknięciu w przycisk "Usuń" wyświetla się InputBox dodatkowo potwierdzający usunięcie
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteCar_Click(object sender, RoutedEventArgs e)
        {
            InputBox.Visibility = System.Windows.Visibility.Visible;
        }
        /// <summary>
        /// Potwierdzenie przyciskiem "Usuń" usuwa dany rekord z bazy danych
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(InputTextBox.Text))
            {
                MessageBox.Show("Wprowadź ID");
            }
            else
            {
                var id = int.Parse(InputTextBox.Text);
                samochody deleteCar = _db.samochody.FirstOrDefault(x => x.id_samochodu.Equals(id));
                _db.samochody.Remove(deleteCar);
                _db.SaveChanges();
                ShowCars();
                // Po kliknięciu "Usuń" InputBox zostaje ukryty
                InputBox.Visibility = System.Windows.Visibility.Collapsed;
                // Czyszczenie InputBoxa
                InputTextBox.Text = String.Empty;
            }
        }
        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            // Po kliknięciu "Anuluj" przycisk zostaje ukryty
            InputBox.Visibility = System.Windows.Visibility.Collapsed;
            // Czyszczenie InputBoxa
            InputTextBox.Text = String.Empty;
        }

        /// <summary>
        /// Funkcja automatycznie wypełnia formularz danymi.
        /// Po wpisaniu przez użytkownika ID rekordu, pola formularza zostają wypełnione tymi danymi, które odpowiadają temu numerowi ID
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void tb_GotFocus(object sender, TextChangedEventArgs args)
        {
            TextBox tb = sender as TextBox;
            if (tb != null && ID.Text.Length != 0)
            {
                var id = int.Parse(ID.Text);
                var editQuery = from item in _db.samochody
                                where item.id_samochodu.Equals(id)
                                select new
                                {
                                    id = item.id_samochodu,
                                    nr = item.nr_rejestracyjny,
                                    marka= item.marka,
                                    model = item.model,
                                    wersja = item.wersja,
                                    rocznik = item.rocznik,
                                    poj = item.poj_silnika,
                                    paliwo = item.rodzaj_paliwa,
                                    przebieg = item.przebieg,
                                    id_c = item.id_cennik
                                };
                foreach (var item in editQuery)
                {
                    //Wyświetlanie kategorii wyszło nieco bardziej skomplikowane przez konstrukcje bazy danych
                    //Po wpisaniu id, do zmiennej category przypisywana jest nazwa kategorii z tabeli cennik, która równa się id z tabeli samochody
                    var category = _db.cennik.Where(x => x.id_cennik.Equals(item.id_c)).First();
                    //
                    Nr_rej.Text = item.nr;
                    Marka.Text = item.marka;
                    Model.Text = item.model;
                    Wersja.Text = item.wersja;
                    Rocznik.Text = item.rocznik;
                    Poj_silnika.Text = item.poj;
                    Paliwo.SelectedItem = item.paliwo;
                    Przebieg.Text = item.przebieg.ToString();
                    Kategoria.SelectedItem = category.kategoria;
                }
            }
        }
        /// <summary>
        /// Funckcja pozwala na wpisanie do TextBoxa tylko cyfry, co zapobiega wprowadzaniu błędnych danych.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
