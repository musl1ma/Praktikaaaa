using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Windows;

namespace Praktika
{
    public partial class AddDemand : Window
    {
        public AddDemand()
        {
            InitializeComponent();
            LoadComboBoxData();
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

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

            var selectedClientId = (int)Client_IdComboBox.SelectedValue;
            var selectedAgentId = (int)Agent_IdComboBox.SelectedValue;
            var selectedTypeId = (int)Id_typeComboBox.SelectedValue;
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

            try
            {
                using (hikEntities db = new hikEntities())
                {
                    Demand newDemand = new Demand
                    {
                        ClientId = selectedClientId,
                        AgentId = selectedAgentId,
                        Id_type = selectedTypeId,
                        Address_City = Address_CityTextBox.Text,
                        Address_Street = Address_StreetTextBox.Text,
                        Address_House = Address_HouseTextBox.Text,
                        Address_Number = Address_NumberTextBox.Text,
                        MinPrice = (long?)newMinPrice,
                        MaxPrice = (long?)newMaxPrice,
                        Id_HouseDemand = (int?)HouseDemand_IdComboBox.SelectedValue,
                        Id_LandDemand = (int?)LandDemand_IdComboBox.SelectedValue,
                        Id_ApartmentDemand = (int?)ApartmentDemand_IdComboBox.SelectedValue
                    };

                    db.Demand.Add(newDemand);
                    db.SaveChanges();
                    MessageBox.Show("Потребность успешно добавлена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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
            DemandWindow demandWindow = Application.Current.Windows.OfType<DemandWindow>().FirstOrDefault();
            demandWindow?.PopulateDemand();
        }
    }
}
