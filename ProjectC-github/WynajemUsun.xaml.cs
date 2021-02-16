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
    /// Interaction logic for WynajemUsun.xaml
    /// </summary>
    public partial class WynajemUsun : Page
    {
        RentalCarEntities _db = new RentalCarEntities();
        public WynajemUsun()
        {
            InitializeComponent();
            ShowRentalcar();
        }
        private void ShowRentalcar()
        {
            var rental = (from ep in _db.samochody
                          join e in _db.wynajem
                          on ep.nr_rejestracyjny
                          equals e.nr_rejestracyjny
                          select new
                          {
                              Id_wynaj = e.id_wynajmu,
                              Nr_Rej = e.nr_rejestracyjny,
                              Model = ep.model,
                              Marka = ep.marka,
                              Data_od = e.data_od,
                              Data_do = e.data_do,
                          }).OrderBy(x => x.Id_wynaj);
            tab_wynajem.ItemsSource = rental.ToList();
        }

        private void Usun_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(ID.Text))
            {
                MessageBox.Show("Wprowadź ID");
            }
            else
            {
                try
                {
                    var id = int.Parse(ID.Text);
                    wynajem deleteRental = _db.wynajem.FirstOrDefault(x => x.id_wynajmu.Equals(id));
                    _db.wynajem.Remove(deleteRental);
                    _db.SaveChanges();
                    MessageBox.Show("Usunięto pomyślnie");
                    ID.Text = String.Empty;
                    ShowRentalcar();
                }
                catch
                {
                    MessageBox.Show("Nie można wykonać operacji");
                }
            }
        }
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataOd.SelectedDate == null || DataDo.SelectedDate == null || ID.Text == null)
                    MessageBox.Show("Wprowadź datę");
                else
                {
                    var id = int.Parse(ID.Text);
                    var applyEdit = (from item in _db.wynajem where item.id_wynajmu.Equals(id) select item).First();
                    applyEdit.data_od = DataOd.SelectedDate;
                    applyEdit.data_do = DataDo.SelectedDate;
                    _db.SaveChanges();
                    MessageBox.Show("Operacja wykonna pomyślnie");
                    ShowRentalcar();
                    DataOd.Text = null;
                    DataDo.SelectedDate = null;
                }
            }
            catch
            {
                MessageBox.Show("Nie można wykonać operacji");
            }
        }
        private void tb_GotFocus(object sender, TextChangedEventArgs args)
        {
            TextBox tb = sender as TextBox;
            if (tb != null && ID.Text.Length != 0)
            {
                var id = int.Parse(ID.Text);
                var editQuery = from item in _db.wynajem
                                where item.id_wynajmu.Equals(id)
                                select new
                                {
                                    dataod = item.data_od,
                                    datado = item.data_do
                                };
                foreach (var item in editQuery)
                {
                    DataOd.Text = item.dataod.ToString();
                    DataDo.Text = item.datado.ToString();


                }
            }
        }

        
    }
}
