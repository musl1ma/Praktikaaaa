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

namespace Praktika
{
    /// <summary>
    /// Логика взаимодействия для DistrictWindow.xaml
    /// </summary>
    public partial class DistrictWindow : Window
     
    {
        private string named;
        private string area;

        public DistrictWindow(string NameD, string Area)
        {
            InitializeComponent();
            named = NameD;
            area = Area;
            PopulateDistrict();
        }
        public void PopulateDistrict()
        {
            using (hikEntities db = new hikEntities())
            {
                var district = db.District.ToList();
                DistrictDG.ItemsSource = district;
            }
        }
    }
}
