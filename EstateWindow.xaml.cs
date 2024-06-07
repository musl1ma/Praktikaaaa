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
    /// Логика взаимодействия для EstateWindow.xaml
    /// </summary>
    public partial class EstateWindow : Window
    {
        private int floorAp;
        private int roomAp;
        private float totalareaAp;
        private int floorH;
        private int roomH;
        private float totalareaH;
        private float totalareaL;

        public EstateWindow(int FloorAp, int RoomAp, float TotalAreaAp, int FloorH, int RoomH, float TotalAreaH, float TotalAreaL)
        {
            InitializeComponent();
            floorAp = FloorAp;
            roomAp = RoomAp;
            totalareaAp = TotalAreaAp;
            floorH = FloorH;
            roomH = RoomH;
            totalareaAp = totalareaH;
            totalareaL = TotalAreaL;
            PopulateAp();
            PopulateH();
            PopulateL();
        }
        public void PopulateAp()
        {
            using (hikEntities db = new hikEntities())
            {
                var apartments = db.Apartment.ToList();
                ApartmentDG.ItemsSource = apartments;
            }
        }
        public void PopulateH()
        {
            using (hikEntities db = new hikEntities())
            {
                var house = db.House.ToList();
                HouseDG.ItemsSource = house;
            }
        }
        public void PopulateL()
        {
            using (hikEntities db = new hikEntities())
            {
                var land = db.Land.ToList();
                LandDG.ItemsSource = land;

            }
        }
    }
}