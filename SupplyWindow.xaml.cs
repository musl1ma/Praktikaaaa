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

    public partial class SupplyWindow : Window
    {
        private int clientId;
        private int agentId;
        private int realEstateId;
        private int price;
        public SupplyWindow(int ClientId, int AgentId, int RealEstateId, int Price)
        {
            InitializeComponent();
            clientId = ClientId;
            agentId = AgentId;
            realEstateId = RealEstateId;
            price = Price;
            PopulateSupply();
        }
        public void PopulateSupply()
        {
            using (hikEntities db = new hikEntities())
            {
                var realeatate = db.Supply.ToList();
                SupplyDG.ItemsSource = realeatate;
            }

        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddSypply addSypply = new AddSypply();
            addSypply.Show();
            PopulateSupply();
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Предполагается, что SupplyDG.SelectedItem возвращает объект типа Supply
            Supply selectedSupply = SupplyDG.SelectedItem as Supply;
            if (selectedSupply == null)
            {
                MessageBox.Show("Выберите предложение для редактирования", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Теперь передаем selectedSupply в конструктор ChangeSupply
            ChangeSupply changeSupply = new ChangeSupply(selectedSupply);
            changeSupply.ShowDialog();
            PopulateSupply();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)

        {
            var selectedEmployee = SupplyDG.SelectedItem as Supply;

            if (selectedEmployee != null)
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить выбранное предложение?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    using (hikEntities db = new hikEntities())
                    {
                        db.Supply.Attach(selectedEmployee);
                        db.Supply.Remove(selectedEmployee);
                        db.SaveChanges();
                        PopulateSupply();

                        MessageBox.Show("Предложение успешно удален", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите предложение для удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
