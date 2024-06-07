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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Praktika
{
    /// <summary>
    /// Логика взаимодействия для ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : Window

    {
        private string clientLastName;
        private string clientFirstName;
        private string clientMiddleName;
        private string clientPhone;
        private string clientEmail;
        public ClientWindow(string LastName, string FirstName, string MiddleName, string Phone, string Email)
        {

            InitializeComponent();
            clientLastName = LastName;
            clientFirstName = FirstName;
            clientMiddleName = MiddleName;
            clientPhone = Phone;
            clientEmail = Email;
            PopulateClients();
        }
        public void PopulateClients()
        {
            using (hikEntities db = new hikEntities())
            {
                var clients = db.Client.ToList();
                ClientDG.ItemsSource = clients;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddEditClient addEditClientWindow = new AddEditClient();
            addEditClientWindow.Show();
            PopulateClients();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Client selectedEmployee = ClientDG.SelectedItem as Client;
            if (selectedEmployee == null)
            {
                MessageBox.Show("Выберите клиента для редактирования", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ChangeClient changeclient = new ChangeClient(selectedEmployee, clientLastName, clientFirstName, clientMiddleName, clientPhone, clientEmail);
            changeclient.ShowDialog();
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedClient = ClientDG.SelectedItem as Client;

            if (selectedClient != null)
            {
                using (hikEntities db = new hikEntities())
                {
                    var hasDemands = db.Demand.Any(d => d.ClientId == selectedClient.Id);
                    var hasSupplies = db.Supply.Any(s => s.ClientId == selectedClient.Id);

                    if (hasDemands || hasSupplies)
                    {
                        MessageBox.Show("Невозможно удалить клиента, так как он связан с потребностями или предложениями.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return; 
                    }
                }

                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить выбранного клиента?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                  
                    using (hikEntities db = new hikEntities())
                    {
                        db.Client.Attach(selectedClient);
                        db.Client.Remove(selectedClient);
                        db.SaveChanges();
                        PopulateClients();

                        MessageBox.Show("Клиент успешно удален", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите клиента для удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
       
    }
}