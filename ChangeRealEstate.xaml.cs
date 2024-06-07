using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Praktika
{
    public partial class ChangeRealEstate : Window
    {
        private RealEstate selectedRealEstate;

        public ChangeRealEstate(RealEstate selectedRealEstate)
        {
            InitializeComponent();
            this.selectedRealEstate = selectedRealEstate;
            LoadDistrictsAndTypes();
            FillFields();
        }

        private void LoadDistrictsAndTypes()
        {
            using (hikEntities db = new hikEntities())
            {
                var districts = db.District.ToList();
                DistrictComboBox.ItemsSource = districts;
                DistrictComboBox.DisplayMemberPath = "Name";
                DistrictComboBox.SelectedValuePath = "Id";

                var types = db.TypeRealEstate.ToList();
                TypeComboBox.ItemsSource = types;
                TypeComboBox.DisplayMemberPath = "Id_type";
                TypeComboBox.SelectedValuePath = "Id_type";
            }
        }

        private void FillFields()
        {
            if (selectedRealEstate != null)
            {
                Address_CityTextBox.Text = selectedRealEstate.Address_City;
                Address_StreetTextBox.Text = selectedRealEstate.Address_Street;
                Address_HouseTextBox.Text = selectedRealEstate.Address_House;
                Address_NumberTextBox.Text = selectedRealEstate.Address_Number;
                DistrictComboBox.SelectedValue = selectedRealEstate.District_Id;
                TypeComboBox.SelectedValue = selectedRealEstate.Id_type;
            }
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            string newAddressCity = Address_CityTextBox.Text;
            string newAddressStreet = Address_StreetTextBox.Text;
            string newAddressHouse = Address_HouseTextBox.Text;
            string newAddressNumber = Address_NumberTextBox.Text;
            var newDistrict = DistrictComboBox.SelectedItem as District;
            var newType = TypeComboBox.SelectedItem as TypeRealEstate;

          
            using (hikEntities db = new hikEntities())
            {
                var existingRealEstate = db.RealEstate.FirstOrDefault(u => u.Id == selectedRealEstate.Id);
                if (existingRealEstate != null)
                {
                    if (existingRealEstate.Address_City != newAddressCity ||
                        existingRealEstate.Address_Street != newAddressStreet ||
                        existingRealEstate.Address_House != newAddressHouse ||
                        existingRealEstate.Address_Number != newAddressNumber ||
                        existingRealEstate.District_Id != newDistrict.Id ||
                        existingRealEstate.Id_type != newType.Id_type)
                    {
                        existingRealEstate.Address_City = newAddressCity;
                        existingRealEstate.Address_Street = newAddressStreet;
                        existingRealEstate.Address_House = newAddressHouse;
                        existingRealEstate.Address_Number = newAddressNumber;
                        existingRealEstate.District_Id = newDistrict.Id;
                        existingRealEstate.Id_type = newType.Id_type;
                        db.SaveChanges();
                        MessageBox.Show("Данные объекта недвижимости успешно изменены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                        RealEstateWindow realEstateWindow = Application.Current.Windows.OfType<RealEstateWindow>().FirstOrDefault();
                        realEstateWindow?.PopulateRealEstate();
                    }
                    else
                    {
                        MessageBox.Show("Нет изменений для сохранения", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Не удалось найти выбранный объект недвижимости", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
