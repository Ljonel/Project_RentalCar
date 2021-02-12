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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectC_github
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RentalCarEntities _db = new RentalCarEntities();

        public MainWindow()
        {
            InitializeComponent();
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






        private void Wynajem_Click(object sender, RoutedEventArgs e)
        {
            Wynajem wyn = new Wynajem();
            wyn.ShowDialog();
        }
        private void Pracownicy_Click(object sender, RoutedEventArgs e)
        {
            Pracownicy prac = new Pracownicy();
            prac.ShowDialog();
        }
        private void Klienci_Click(object sender, RoutedEventArgs e)
        {
            Klienci kl = new Klienci();
            kl.ShowDialog();
        }
    }
}
