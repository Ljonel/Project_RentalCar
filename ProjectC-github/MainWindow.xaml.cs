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
        /// <summary>
        /// Inicjalizacja połączenia z bazą danych "RentalCar", nazwa ADO.net w projekcie to "RentalCarEntities"
        /// </summary>
        RentalCarEntities _db = new RentalCarEntities();
        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Nawigacja w programie, każdy z przycisków otwiera odpowiednie okno
        /// </summary>
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
        private void Samochody_Click(object sender, RoutedEventArgs e)
        {
            Samochody s = new Samochody();
            s.ShowDialog();
        }
    }
}
