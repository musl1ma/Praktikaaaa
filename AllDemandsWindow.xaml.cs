using System.Linq;
using System.Windows;

namespace Praktika
{
    public partial class AllDemandsWindow : Window
    {
        public AllDemandsWindow()
        {
            InitializeComponent();
            PopulateData();
        }

        private void PopulateData()
        {
            using (hikEntities db = new hikEntities())
            {
                // Получаем данные о потребностях в домах и заполняем таблицу
                HouseDG.ItemsSource = db.HouseDemand.ToList();

                // Получаем данные о потребностях в земле и заполняем таблицу
                LandDG.ItemsSource = db.LandDemand.ToList();

                // Получаем данные о потребностях в квартирах и заполняем таблицу
                ApartmentDG.ItemsSource = db.ApartmentDemand.ToList();
            }
        }
    }
}
