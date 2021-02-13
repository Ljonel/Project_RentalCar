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
        IList<string> sections = new List<string> { "Wypożyczenia", "Marketing"};
        public Pracownicy()
        {
            InitializeComponent();
            Combobox_dzial.ItemsSource = sections;
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
                MessageBox.Show("Wprowadzono dane");

              
            }
        }

        private void Pensja_walidacja(System.Object sender, System.Windows.Input.TextCompositionEventArgs e)
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
