using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Praktika
{
    /// <summary>
    /// Логика взаимодействия для AgentWindow.xaml
    /// </summary>
    public partial class AgentWindow : Window
    {
        private string agentLastName;
        private string agentFirstName;
        private string agentMiddleName;
        private int agentDealShare;
        public AgentWindow()
        {
            InitializeComponent();
            PopulateAgent();
        }
        public AgentWindow(string LastName, string FirstName, string MiddleName, int DealShare)
        {
            InitializeComponent();
            agentLastName = LastName;
            agentFirstName = FirstName;
            agentMiddleName = MiddleName;
            agentDealShare = DealShare;
            PopulateAgent();
        }

        public void PopulateAgent()
        {
            using (hikEntities db = new hikEntities())
            {
                var agent = db.Agent.ToList();
                AgentDG.ItemsSource = agent;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddAgent addAgent = new AddAgent();
            addAgent.Show();
            PopulateAgent();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Agent selectedEmployee = AgentDG.SelectedItem as Agent;
            if (selectedEmployee == null)
            {
                MessageBox.Show("Выберите риелтора для редактирования", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ChangeAgent changeagent = new ChangeAgent(selectedEmployee, agentLastName, agentFirstName, agentMiddleName, agentDealShare);
            changeagent.ShowDialog();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedAgent = AgentDG.SelectedItem as Agent;

            if (selectedAgent != null)
            {
                // Проверка наличия связей с потребностями или предложениями
                using (hikEntities db = new hikEntities())
                {
                    // Проверка наличия связей с потребностями
                    var hasDemands = db.Demand.Any(d => d.AgentId == selectedAgent.Id);

                    // Проверка наличия связей с предложениями
                    var hasSupplies = db.Supply.Any(s => s.AgentId == selectedAgent.Id);

                    if (hasDemands || hasSupplies)
                    {
                        MessageBox.Show("Невозможно удалить риелтора, так как он связан с потребностями или предложениями.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return; // Прерываем выполнение метода
                    }
                }

                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить выбранного риелтора?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Удаление риелтора
                    using (hikEntities db = new hikEntities())
                    {
                        db.Agent.Attach(selectedAgent);
                        db.Agent.Remove(selectedAgent);
                        db.SaveChanges();
                        PopulateAgent();

                        MessageBox.Show("Риелтор успешно удален", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите риелтора для удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
