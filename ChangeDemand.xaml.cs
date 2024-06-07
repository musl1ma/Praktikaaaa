using System;
using System.Linq;
using System.Windows;

namespace Praktika
{
    public partial class ChangeDemand : Window
    {
        private Demand selectedDemand;

        public ChangeDemand(Demand selectedDemand)
        {
            InitializeComponent();
            this.selectedDemand = selectedDemand;
            LoadComboBoxData();
            FillFields(); // Заполнить поля значениями из selectedDemand
        }

        private void LoadComboBoxData()
        {
            using (hikEntities db = new hikEntities())
            {
                var clients = db.Client.ToList();
                Client_IdComboBox.ItemsSource = clients;
                Client_IdComboBox.DisplayMemberPath = "LastName";
                Client_IdComboBox.SelectedValuePath = "Id";

                var agents = db.Agent.ToList();
                Agent_IdComboBox.ItemsSource = agents;
                Agent_IdComboBox.DisplayMemberPath = "LastName";
                Agent_IdComboBox.SelectedValuePath = "Id";

                var types = db.TypeRealEstate.ToList();
                Id_typeComboBox.ItemsSource = types;
                Id_typeComboBox.DisplayMemberPath = "Id_type";
                Id_typeComboBox.SelectedValuePath = "Id_type";

                var houseDemands = db.HouseDemand.ToList();
                HouseDemand_IdComboBox.ItemsSource = houseDemands;
                HouseDemand_IdComboBox.DisplayMemberPath = "Id";
                HouseDemand_IdComboBox.SelectedValuePath = "Id";

                var landDemands = db.LandDemand.ToList();
                LandDemand_IdComboBox.ItemsSource = landDemands;
                LandDemand_IdComboBox.DisplayMemberPath = "Id";
                LandDemand_IdComboBox.SelectedValuePath = "Id";

                var apartmentDemands = db.ApartmentDemand.ToList();
                ApartmentDemand_IdComboBox.ItemsSource = apartmentDemands;
                ApartmentDemand_IdComboBox.DisplayMemberPath = "Id";
                ApartmentDemand_IdComboBox.SelectedValuePath = "Id";
            }
        }

        private void FillFields()
        {
            if (selectedDemand != null)
            {
                Id_typeComboBox.SelectedValue = selectedDemand.Id_type;
                Client_IdComboBox.SelectedValue = selectedDemand.ClientId;
                Agent_IdComboBox.SelectedValue = selectedDemand.AgentId;
                Address_CityTextBox.Text = selectedDemand.Address_City;
                Address_StreetTextBox.Text = selectedDemand.Address_Street;
                Address_HouseTextBox.Text = selectedDemand.Address_House;
                Address_NumberTextBox.Text = selectedDemand.Address_Number;
                Min_PriceTextBox.Text = selectedDemand.MinPrice.HasValue ? selectedDemand.MinPrice.Value.ToString() : "";
                Max_Price_HouseTextBox.Text = selectedDemand.MaxPrice.HasValue ? selectedDemand.MaxPrice.Value.ToString() : "";
                HouseDemand_IdComboBox.SelectedValue = selectedDemand.Id_HouseDemand;
                LandDemand_IdComboBox.SelectedValue = selectedDemand.Id_LandDemand;
                ApartmentDemand_IdComboBox.SelectedValue = selectedDemand.Id_ApartmentDemand;
            }
        }
        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на null для selectedDemand перед использованием его Id
            if (selectedDemand == null)
            {
                MessageBox.Show("Выберите потребность для изменения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int newTypeId = (int)Id_typeComboBox.SelectedValue;
            int newClientId = (int)Client_IdComboBox.SelectedValue;
            int newAgentId = (int)Agent_IdComboBox.SelectedValue;
            string newCity = Address_CityTextBox.Text;
            string newStreet = Address_StreetTextBox.Text;
            string newHouse = Address_HouseTextBox.Text;
            string newNumber = Address_NumberTextBox.Text;
            decimal newMinPrice = 0;
            decimal newMaxPrice = 0;
            if (string.IsNullOrWhiteSpace(Min_PriceTextBox.Text) || string.IsNullOrWhiteSpace(Max_Price_HouseTextBox.Text))
            {
                // Если поле пустое, сохраняем значение по умолчанию 0
                newMinPrice = 0;
                newMaxPrice = 0;
            }
            else
            {
                // Если поле не пустое, пытаемся преобразовать в decimal
                decimal.TryParse(Min_PriceTextBox.Text, out newMinPrice);
                decimal.TryParse(Max_Price_HouseTextBox.Text, out newMaxPrice);
            }

            long? newHouseDemandId = HouseDemand_IdComboBox.SelectedItem != null ? (int ?)HouseDemand_IdComboBox.SelectedValue : null;
            long? newLandDemandId = LandDemand_IdComboBox.SelectedItem != null ? (int ?)LandDemand_IdComboBox.SelectedValue : null;
            long? newApartmentDemandId = ApartmentDemand_IdComboBox.SelectedItem != null ? (long?)(int?)ApartmentDemand_IdComboBox.SelectedValue : null;

            using (hikEntities db = new hikEntities())
            {
                var existingDemand = db.Demand.FirstOrDefault(d => d.Id == selectedDemand.Id);
                if (existingDemand != null)
                {
                    // Изменение данных в existingDemand
                    existingDemand.Id_type = newTypeId;
                    existingDemand.ClientId = newClientId;
                    existingDemand.AgentId = newAgentId;
                    existingDemand.Address_City = newCity;
                    existingDemand.Address_Street = newStreet;
                    existingDemand.Address_House = newHouse;
                    existingDemand.Address_Number = newNumber;
                    existingDemand.MinPrice = newMinPrice == 0 ? null : (long?)newMinPrice;
                    existingDemand.MaxPrice = newMaxPrice == 0 ? null : (long?)newMaxPrice;

                    // Проверка на null для nullable полей
                    if (newHouseDemandId.HasValue)
                    {
                        existingDemand.Id_HouseDemand = (int)newHouseDemandId;
                    }
                    else
                    {
                        existingDemand.Id_HouseDemand = null;
                    }
                    if (newLandDemandId.HasValue)
                    {
                        existingDemand.Id_LandDemand = (int)newLandDemandId;
                    }
                    else
                    {
                        existingDemand.Id_LandDemand = null;
                    }
                    if (newApartmentDemandId.HasValue)
                    {
                        existingDemand.Id_ApartmentDemand = (int)newApartmentDemandId;
                    }
                    else
                    {
                        existingDemand.Id_ApartmentDemand = null;
                    }

                    db.SaveChanges();
                    MessageBox.Show("Данные потребности успешно изменены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Не удалось найти выбранную потребность.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}