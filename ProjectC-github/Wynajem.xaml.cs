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
    /// Interaction logic for Wynajem.xaml
    /// </summary>
    public partial class Wynajem : Window
    {
        RentalCarEntities _db = new RentalCarEntities();
        IList<string> nrRej = new List<string>();
        IList<int> employeeList= new List<int>();
        IList<int> clientList = new List<int>();
       
        public Wynajem()
        {

            var nrQuery = from item in _db.rej_samochody select item;
            var employeeQuery = from item in _db.pracownicy select item;
            var clientsQuery = from item in _db.klienci select item;
           
           
            InitializeComponent();
            foreach (var item in nrQuery)
            {
               nrRej.Add(item.nr_rejestracyjny);
            }
            foreach (var item in employeeQuery)
            {
                employeeList.Add(item.id_pracownika);
            }
            foreach (var item in clientsQuery)
            {
                clientList.Add(item.id_klienta);
            }

            Nr_rej.ItemsSource = nrRej;
            Pracownicy.ItemsSource = employeeList;
            Klienci.ItemsSource = clientList;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
