using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace Praktika
{
    /// <summary>
    /// Логика взаимодействия для AddRealEstate.xaml
    /// </summary>
    public partial class AddRealEstate : Window
    {
        public AddRealEstate()
        {
            InitializeComponent();
            LoadComboBoxData();
        }

        private void LoadComboBoxData()
        {
            using (hikEntities db = new hikEntities())
            {
                var Id = db.District.ToList();
                DistrictComboBox.ItemsSource = Id;

                var Id_type = db.TypeRealEstate.ToList();
                TypeComboBox.ItemsSource = Id_type;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string addressCity = Address_CityTextBox.Text;
            string addressStreet = Address_StreetTextBox.Text;
            string addressHouse = Address_HouseTextBox.Text;
            string addressNumber = Address_NumberTextBox.Text;
            var selectedDistrict = DistrictComboBox.SelectedItem as District;
            var selectedType = TypeComboBox.SelectedItem as TypeRealEstate;

            if (!Regex.IsMatch(addressCity, @"^[А-ЯЁ][а-яё\s]{0,49}$"))
            {
                MessageBox.Show("Проверьте корректность города", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!Regex.IsMatch(addressStreet, @"^[А-ЯЁ][а-яё\s]{0,49}$"))
            {
                MessageBox.Show("Проверьте корректность улицы", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            

            try
            {
                using (hikEntities db = new hikEntities())
                {
                    RealEstate newRealEstate = new RealEstate
                    {
                        Address_City = addressCity,
                        Address_Street = addressStreet,
                        Address_House = addressHouse,
                        Address_Number = addressNumber,
                        District_Id = selectedDistrict.Id,
                        Id_type = selectedType.Id_type
                    };

                    db.RealEstate.Add(newRealEstate);
                    db.SaveChanges();
                    MessageBox.Show("Объект недвижимости успешно добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (DbUpdateException ex)
            {
                var innerExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : "Inner exception is null";
                MessageBox.Show("Произошла ошибка при сохранении изменений: " + innerExceptionMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            this.Close();
            RealEstateWindow realEstateWindow = Application.Current.Windows.OfType<RealEstateWindow>().FirstOrDefault();
            realEstateWindow?.PopulateRealEstate();
        }
    }
}
