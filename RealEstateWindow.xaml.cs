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
    /// Логика взаимодействия для RealEstateWindow.xaml
    /// </summary>
    public partial class RealEstateWindow : Window
    {
        private string adCity;
        private string adStreet;
        private string adHouse;
        private string adNumber;
        private int districtId;
        private int idType;
        public RealEstateWindow(string Adress_City, string Adress_Street, string Adress_House, string Adress_Number, int Distric_Id, int Id_type)
        {
            InitializeComponent();
            adCity = Adress_City;
            adStreet = Adress_Street;
            adHouse = Adress_House;
            adNumber = Adress_Number;
            districtId = Distric_Id;
            idType = Id_type;
            PopulateRealEstate();
        }
        public void PopulateRealEstate()
        {
            using (hikEntities db = new hikEntities())
            {
                var realeatate = db.RealEstate.ToList();
                RealEstateDG.ItemsSource = realeatate;
            }

        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddRealEstate addrealestateWindow = new AddRealEstate();
            addrealestateWindow.Show();
            PopulateRealEstate();
        }
        private void EstaeButton_Click(object sender, RoutedEventArgs e)
        {
            EstateWindow estateWindow = new EstateWindow(0, 0, 0, 0, 0,0,0);
            estateWindow.Show();
            PopulateRealEstate();
        }
        private void DistrictButton_Click(object sender, RoutedEventArgs e)
        {
            DistrictWindow districtWindow = new DistrictWindow( "NameDValue", "AreaValue");
            districtWindow.Show();
            PopulateRealEstate();
        }
        private void TypeButton_Click(object sender, RoutedEventArgs e)
        {
            TypeEstate typeEstate = new TypeEstate("NameTValue");
            typeEstate.Show();
            PopulateRealEstate();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedRealEstate = RealEstateDG.SelectedItem as RealEstate;

            if (selectedRealEstate != null)
            { 
                using (hikEntities db = new hikEntities())
                {
                    var hasSupplies = db.Supply.Any(s => s.RealEstateId == selectedRealEstate.Id);

                    if (hasSupplies)
                    {
                        MessageBox.Show("Невозможно удалить объект недвижимости, так как он связан с предложением.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return; 
                    }
                }
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить выбранный объект недвижимости?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Удаление объекта недвижимости
                    using (hikEntities db = new hikEntities())
                    {
                        db.RealEstate.Attach(selectedRealEstate);
                        db.RealEstate.Remove(selectedRealEstate);
                        db.SaveChanges();
                        PopulateRealEstate();

                        MessageBox.Show("Объект недвижимости успешно удален", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите объект недвижимости для удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedRealEstate = RealEstateDG.SelectedItem as RealEstate;
            if (selectedRealEstate != null)
            {
                ChangeRealEstate changeRealEstateWindow = new ChangeRealEstate(selectedRealEstate);
                changeRealEstateWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите объект недвижимости для изменения", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


    }
}