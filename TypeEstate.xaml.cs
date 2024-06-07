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
    /// Логика взаимодействия для TypeEstate.xaml
    /// </summary>
    public partial class TypeEstate : Window
    {
        private string namet;
        public TypeEstate(string Namet)
        {
            InitializeComponent();
            namet = Namet;
            PopulateType();
        }
        public void PopulateType()
        {
            using (hikEntities db = new hikEntities())
            {
                var typeRealEstates = db.TypeRealEstate.ToList();
                TypeDG.ItemsSource = typeRealEstates;
            }
        }
    }
}
