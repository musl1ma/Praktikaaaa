using System;
using System.Linq;
using System.Windows;

namespace Praktika
{
    public partial class ChangeSupply : Window
    {
        private Supply selectedSupply;

        public ChangeSupply(Supply selectedSupply)
        {
            InitializeComponent();
            this.selectedSupply = selectedSupply;
            LoadClientsAgentsAndRealEstates();
            FillFields();
        }

        private void LoadClientsAgentsAndRealEstates()
        {
            using (hikEntities db = new hikEntities())
            {
                var clients = db.Client.ToList();
                ClientIdComboBox.ItemsSource = clients;
                ClientIdComboBox.DisplayMemberPath = "LastName"; // измените на нужное поле для отображения
                ClientIdComboBox.SelectedValuePath = "Id";

                var agents = db.Agent.ToList();
                AgentIdComboBox.ItemsSource = agents;
                AgentIdComboBox.DisplayMemberPath = "LastName"; // измените на нужное поле для отображения
                AgentIdComboBox.SelectedValuePath = "Id";

                var realEstates = db.RealEstate
                                    .Select(re => new { re.Id, Address = re.Address_City + ", " + re.Address_Street + ", " + re.Address_Number })
                                    .ToList();
                RealEstateIdComboBox.ItemsSource = realEstates;
                RealEstateIdComboBox.DisplayMemberPath = "Address";
                RealEstateIdComboBox.SelectedValuePath = "Id";
            }
        }

        private void FillFields()
        {
            if (selectedSupply != null)
            {
                ClientIdComboBox.SelectedValue = selectedSupply.ClientId;
                AgentIdComboBox.SelectedValue = selectedSupply.AgentId;
                RealEstateIdComboBox.SelectedValue = selectedSupply.RealEstateId;
                PriceTextBox.Text = selectedSupply.Price.ToString();
            }
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            if (ClientIdComboBox.SelectedItem == null || AgentIdComboBox.SelectedItem == null || RealEstateIdComboBox.SelectedItem == null || string.IsNullOrWhiteSpace(PriceTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var newClientId = (int)ClientIdComboBox.SelectedValue;
            var newAgentId = (int)AgentIdComboBox.SelectedValue;
            var newRealEstateId = (int)RealEstateIdComboBox.SelectedValue;
            decimal newPrice;

            if (!decimal.TryParse(PriceTextBox.Text, out newPrice))
            {
                MessageBox.Show("Проверьте корректность цены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (hikEntities db = new hikEntities())
            {
                var existingSupply = db.Supply.FirstOrDefault(u => u.Id == selectedSupply.Id);
                if (existingSupply != null)
                {
                    if (existingSupply.ClientId != newClientId ||
                        existingSupply.AgentId != newAgentId ||
                        existingSupply.RealEstateId != newRealEstateId ||
                        existingSupply.Price != (long)newPrice)
                    {
                        existingSupply.ClientId = newClientId;
                        existingSupply.AgentId = newAgentId;
                        existingSupply.RealEstateId = newRealEstateId;
                        existingSupply.Price = (long)newPrice;
                        db.SaveChanges();
                        MessageBox.Show("Данные предложения успешно изменены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Нет изменений для сохранения.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Не удалось найти выбранное предложение.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
