using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Windows;

namespace Praktika
{
    public partial class AddSypply : Window
    {
        public AddSypply()
        {
            InitializeComponent();
            LoadComboBoxData();
        }

        private void LoadComboBoxData()
        {
            using (hikEntities db = new hikEntities())
            {
                var clients = db.Client.ToList();
                ClientIdComboBox.ItemsSource = clients;
                ClientIdComboBox.DisplayMemberPath = "LastName"; 
                ClientIdComboBox.SelectedValuePath = "Id";

                var agents = db.Agent.ToList();
                AgentIdComboBox.ItemsSource = agents;
                AgentIdComboBox.DisplayMemberPath = "LastName"; 
                AgentIdComboBox.SelectedValuePath = "Id";

                var realEstates = db.RealEstate.ToList();
                RealEstateIdComboBox.ItemsSource = realEstates;
                RealEstateIdComboBox.DisplayMemberPath = "Id"; 
                RealEstateIdComboBox.SelectedValuePath = "Id";
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var price = PriceTextBox.Text;

            if (!long.TryParse(price, out long priceValue) ||
                ClientIdComboBox.SelectedItem == null ||
                AgentIdComboBox.SelectedItem == null ||
                RealEstateIdComboBox.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, заполните все поля", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedClientId = (int)ClientIdComboBox.SelectedValue;
            var selectedAgentId = (int)AgentIdComboBox.SelectedValue;
            var selectedRealEstateId = (int)RealEstateIdComboBox.SelectedValue;

            try
            {
                using (hikEntities db = new hikEntities())
                {
                    Supply newSupply = new Supply
                    {
                        ClientId = selectedClientId,
                        AgentId = selectedAgentId,
                        RealEstateId = selectedRealEstateId,
                        Price = priceValue
                    };
                    db.Supply.Add(newSupply);
                    db.SaveChanges();
                    MessageBox.Show("Предложение успешно добавлено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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
            SupplyWindow supplyWindow = Application.Current.Windows.OfType<SupplyWindow>().FirstOrDefault();
            supplyWindow?.PopulateSupply();
        }
    }
}



