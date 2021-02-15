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
                try
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
                    ShowClients();
                }
                catch
                {
                    MessageBox.Show("Nie mozna wykonać operacji");

                }

            }
        }
        private void EditClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(Imie.Text) || String.IsNullOrEmpty(Nazwisko.Text) || String.IsNullOrEmpty(Miasto.Text) || String.IsNullOrEmpty(Ulica.Text) || String.IsNullOrEmpty(Kod.Text) || String.IsNullOrEmpty(Pesel.Text))
                    MessageBox.Show("Nie można wykonać operacji");
                else
                {
                    var id = int.Parse(ID.Text);
                    var applyEdit = (from item in _db.klienci where item.id_klienta.Equals(id) select item).First();
                    applyEdit.imie = Imie.Text;
                    applyEdit.nazwisko = Nazwisko.Text;
                    applyEdit.miasto = Miasto.Text;
                    applyEdit.ulica = Ulica.Text;
                    applyEdit.kod = Kod.Text;
                    applyEdit.pesel = Pesel.Text;
                    _db.SaveChanges();
                    MessageBox.Show("Operacja wykonna pomyślnie");
                    Imie.Text = "";
                    Nazwisko.Text = "";
                    Miasto.Text = null;
                    Ulica.Text = null;
                    Kod.Text = "";
                    Pesel.Text = "";
                    ShowClients();
                }
            }
            catch
            {
                MessageBox.Show("Nie można wykonać operacji");
            }
        }
        private void DeleteClient_Click(object sender, RoutedEventArgs e)
        {
            InputBox.Visibility = System.Windows.Visibility.Visible;
        }
        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(InputTextBox.Text))
            {
                MessageBox.Show("Wprowadź ID");
            }
            else
            {
                var id = int.Parse(InputTextBox.Text);
                klienci deleteClient = _db.klienci.FirstOrDefault(x => x.id_klienta.Equals(id));
                _db.klienci.Remove(deleteClient);
                _db.SaveChanges();
                ShowClients();
                // After Yes hide this button
                InputBox.Visibility = System.Windows.Visibility.Collapsed;
                // Clear InputBox
                InputTextBox.Text = String.Empty;
            }
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
            TextBox tb = sender as TextBox;
            if (tb != null && ID.Text.Length != 0)
            {
                var id = int.Parse(ID.Text);
                var editQuery = from item in _db.klienci
                                where item.id_klienta.Equals(id)
                                select new
                                {
                                    id = item.id_klienta,
                                    im = item.imie,
                                    n = item.nazwisko,
                                    m = item.miasto,
                                    u= item.ulica,
                                    k = item.kod,
                                    p = item.pesel
                                };
                foreach (var item in editQuery)
                {
                    Imie.Text = item.im;
                    Nazwisko.Text = item.n;
                    Miasto.Text = item.m;
                    Ulica.Text = item.u;
                    Kod.Text = item.k;
                    Pesel.Text = item.p;
                }
            }
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