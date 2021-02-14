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

        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
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
        pracownicy selectedEmployee = new pracownicy();
        private void EdytujPracownika_Click(object sender, RoutedEventArgs e)
        {
           
        }
        private void DeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            InputBox.Visibility = System.Windows.Visibility.Visible;
        }
        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            var id = int.Parse(InputTextBox.Text);
            pracownicy deleteEmployee = _db.pracownicy.FirstOrDefault(x => x.id_pracownika.Equals(id));
            _db.pracownicy.Remove(deleteEmployee);
            _db.SaveChanges();
            ShowEmployees();
            // After Yes hide this button
            InputBox.Visibility = System.Windows.Visibility.Collapsed;
            // Clear InputBox
            InputTextBox.Text = String.Empty;
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            // NoButton Clicked
            InputBox.Visibility = System.Windows.Visibility.Collapsed;
            // Clear InputBox
            InputTextBox.Text = String.Empty;
        }
        /// <summary>
        /// Walidacja formularza, funkcja sprawdza czy w elemencie w XAML zawierającym PrevierTextInput="Walidacja_numer" wprowadzane są tylko wartości liczbowe.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
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
