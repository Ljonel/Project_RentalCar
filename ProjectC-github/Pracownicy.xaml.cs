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
            combobox_dzial.ItemsSource = sections;
        }
    }
}
