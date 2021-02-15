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
    /// Interaction logic for Klienci.xaml
    /// </summary>
    public partial class Klienci : Window
    {
        RentalCarEntities _db = new RentalCarEntities();
        public Klienci()
        {
            InitializeComponent();
            ShowClients();
        }

        private void ShowClients()
        {
            var q = from item in _db.klienci
                    select new
                    {
                        Id_klient = item.id_klienta,
                        Imie = item.imie,
                        Nazwisko = item.nazwisko,
                        Miasto = item.miasto,
                        Ulica = item.ulica,
                        Kod = item.kod,
                        Pesel = item.pesel
                    };
            tab_klienci.ItemsSource = q.ToList();
        }

        private void AddClient_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(Imie.Text) || String.IsNullOrEmpty(Nazwisko.Text) || String.IsNullOrEmpty(Miasto.Text) || String.IsNullOrEmpty(Ulica.Text) || String.IsNullOrEmpty(Kod.Text) || String.IsNullOrEmpty(Pesel.Text))
            {
                MessageBox.Show("Wprowadź dane");
            }
            else
            {
                var addClient = new klienci()
                {
                    imie = Imie.Text,
                    nazwisko = Nazwisko.Text,
                    miasto = Miasto.Text,
                    ulica = Ulica.Text,
                    kod = Kod.Text,
                    pesel = Pesel.Text
                };
                _db.klienci.Add(addClient);
                _db.SaveChanges();
                this.Hide();
            }
        }
        private void EditClient_Click(object sender, RoutedEventArgs e)
        {

        }
        private void DeleteClient_Click(object sender, RoutedEventArgs e)
        {
            InputBox.Visibility = System.Windows.Visibility.Visible;
        }
        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
           
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