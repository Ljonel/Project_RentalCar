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
using System.Text.RegularExpressions;

namespace ProjectC_github
{
    /// <summary>
    /// Interaction logic for Pracownicy.xaml
    /// </summary>
    public partial class Pracownicy : Window
    {
        RentalCarEntities _db = new RentalCarEntities();

        //Lista działów dla pracowników dostępnych w wypożyczalni
        IList<string> sections = new List<string> { "Wypożyczenia", "Marketing" };
        IList<string> positions = new List<string> { "Sprzedawca", "Menadżer", "Dyrektor" };

        public Pracownicy()
        {
            InitializeComponent();
            Combobox_dzial.ItemsSource = sections;
            Combobox_stanowisko.ItemsSource = positions;
            ShowEmployees();
        }
        /// <summary>
        /// Funkcja wyciąga dane z bazy RantalCar i wpisuje je do tablicy pracowników DataGrid w XAML'u
        /// </summary>
        private void ShowEmployees()
        {
            var q = from item in _db.pracownicy
                    select new
                    {
                        Id_prac = item.id_pracownika,
                        Imie = item.imie,
                        Nazwisko = item.nazwisko,
                        Dzial = item.dzial,
                        Stanowisko = item.stanowisko,
                        Pensja = item.pensja
                    };
            tab_pracownicy.ItemsSource = q.ToList();
        }
        /// <summary>
        /// Funkcja pobiera dane z formularza i dodaje je do bazy danych do tabeli pracownicy.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            //Warunek sprawdza czy TextBoxy na pewno są wypełnione, w przeciwnym razie program prosi uzytkownika o wpisanie danych.
            if (String.IsNullOrEmpty(Imie.Text) || String.IsNullOrEmpty(Nazwisko.Text) || String.IsNullOrEmpty(Pensja.Text) || String.IsNullOrEmpty(Combobox_dzial.Text))
            {
                MessageBox.Show("Wprowadź dane");
            }
            else
            {
                var addEmployee = new pracownicy()
                {
                    imie = Imie.Text,
                    nazwisko = Nazwisko.Text,
                    dzial = Combobox_dzial.SelectedItem.ToString(),
                    stanowisko = Combobox_stanowisko.SelectedItem.ToString(),
                    pensja = Convert.ToDecimal(Pensja.Text),
                };
                _db.pracownicy.Add(addEmployee);
                _db.SaveChanges();
                this.Hide();
            }
           
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
                    var editQuery = from item in _db.pracownicy
                                    where item.id_pracownika.Equals(id)
                                    select new
                                    {
                                        id = item.id_pracownika,
                                        im = item.imie,
                                        n = item.nazwisko,
                                        dz = item.dzial,
                                        stan = item.stanowisko,
                                        pen = item.pensja
                                    };
                    foreach (var item in editQuery)
                    {
                        Imie.Text = item.im;
                        Nazwisko.Text = item.n;
                        Combobox_dzial.SelectedItem = item.dz;
                        Combobox_stanowisko.SelectedItem = item.stan;
                        Pensja.Text = item.pen.ToString();
                    }
            }
        }
        /// <summary>
        /// Funkcja pobiera dane z formularza i edytuje w bazie ten rekord którego ID podał uzytkownik
        /// Po wpisaniu przez użytkownika ID, formularz automatycznie pola danymi które odpowiadają ID tego rekordu w bazie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditEmployee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(String.IsNullOrEmpty(Imie.Text) || String.IsNullOrEmpty(Nazwisko.Text) || String.IsNullOrEmpty(Pensja.Text) || String.IsNullOrEmpty(Combobox_stanowisko.Text) || String.IsNullOrEmpty(Combobox_dzial.Text))
                    MessageBox.Show("Nie można wykonać operacji");
                else 
                {
                    var id = int.Parse(ID.Text);
                    var applyEdit = (from item in _db.pracownicy where item.id_pracownika.Equals(id) select item).First();
                    applyEdit.imie = Imie.Text;
                    applyEdit.nazwisko = Nazwisko.Text;
                    applyEdit.dzial = Combobox_dzial.SelectedItem.ToString();
                    applyEdit.stanowisko = Combobox_stanowisko.SelectedItem.ToString();
                    applyEdit.pensja = Convert.ToDecimal(Pensja.Text);
                    _db.SaveChanges();
                    MessageBox.Show("Operacja wykonna pomyślnie");

                    //Po udanej operacji formularz zostaje wyczyszczony
                    Imie.Text = "";
                    Nazwisko.Text = "";
                    Combobox_dzial.SelectedItem = null;
                    Combobox_stanowisko.SelectedItem = null;
                    Pensja.Text = "";
                    ShowEmployees();
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
        private void DeleteEmployee_Click(object sender, RoutedEventArgs e)
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
                pracownicy deleteEmployee = _db.pracownicy.FirstOrDefault(x => x.id_pracownika.Equals(id));
                _db.pracownicy.Remove(deleteEmployee);
                _db.SaveChanges();
                ShowEmployees();
                // Po kliknięciu "Usuń" InputBox zostaje ukryty
                InputBox.Visibility = System.Windows.Visibility.Collapsed;
                // Czyszczenie InputBoxa
                InputTextBox.Text = String.Empty;
            }
        }
        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            //Po kliknięciu "Anuluj" InputBox zostaje ukryty
            InputBox.Visibility = System.Windows.Visibility.Collapsed;
            // Czyszczenie InputBoxa
            InputTextBox.Text = String.Empty;
        }
        
        /// <summary>
        /// Walidacja formularza, funkcja sprawdza czy w elemencie w XAML zawierającym PrevierTextInput="Walidacja_numer" wprowadzane są tylko wartości liczbowe.
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
