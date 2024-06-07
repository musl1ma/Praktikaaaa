using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace Praktika
{
    /// <summary>
    /// Логика взаимодействия для ChangeAgent.xaml
    /// </summary>
    public partial class ChangeAgent : Window
    {
        private string agentLastName;
        private string agentFirstName;
        private string agentMiddleName;
        private int agentDealShare;
        private Agent selectedEmployee;

        public ChangeAgent(Agent selectedEmployee, string LastName, string FirstName, string MiddleName, int DealShare)
        {
            InitializeComponent();
            this.selectedEmployee = selectedEmployee;
            this.agentLastName = LastName;
            this.agentFirstName = FirstName;
            this.agentMiddleName = MiddleName;
            this.agentDealShare = DealShare;
            FillFields();
        }

        private void FillFields()
        {
            if (selectedEmployee != null)
            {
                LastNameTextBox.Text = selectedEmployee.LastName;
                FirstNameTextBox.Text = selectedEmployee.FirstName;
                MiddleNameTextBox.Text = selectedEmployee.MiddleName;
                DealShareTextBox.Text = selectedEmployee.DealShare.ToString();
            }
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            string newLastName = LastNameTextBox.Text;
            string newFirstName = FirstNameTextBox.Text;
            string newMiddleName = MiddleNameTextBox.Text;
            string newDealShareText = DealShareTextBox.Text;

            if (string.IsNullOrEmpty(newLastName) || string.IsNullOrEmpty(newFirstName) || string.IsNullOrEmpty(newMiddleName))
            {
                MessageBox.Show("Пожалуйста, заполните все поля", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!Regex.IsMatch(newLastName, @"^[А-ЯЁ][а-яё]{0,14}$"))
            {
                MessageBox.Show("Проверьте корректность фамилии", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!Regex.IsMatch(newFirstName, @"^[А-ЯЁ][а-яё]{0,14}$"))
            {
                MessageBox.Show("Проверьте корректность имени", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!Regex.IsMatch(newMiddleName, @"^[А-ЯЁ][а-яё]{0,14}$"))
            {
                MessageBox.Show("Проверьте корректность отчества", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!int.TryParse(newDealShareText, out int newDealShare) || newDealShare < 0 || newDealShare > 100)
            {
                MessageBox.Show("Проверьте корректность доли сделки (от 0 до 100)", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (hikEntities db = new hikEntities())
            {
                var existingEmployee = db.Agent.FirstOrDefault(u => u.Id == selectedEmployee.Id);
                if (existingEmployee != null)
                {
                    if (existingEmployee.LastName != newLastName || existingEmployee.FirstName != newFirstName || existingEmployee.MiddleName != newMiddleName || existingEmployee.DealShare != newDealShare)
                    {
                        existingEmployee.LastName = newLastName;
                        existingEmployee.FirstName = newFirstName;
                        existingEmployee.MiddleName = newMiddleName;
                        existingEmployee.DealShare = newDealShare;
                        db.SaveChanges();
                        MessageBox.Show("Данные риелтора успешно изменены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                        AgentWindow agentWindow = Application.Current.Windows.OfType<AgentWindow>().FirstOrDefault();
                        agentWindow?.PopulateAgent();
                    }
                    else
                    {
                        MessageBox.Show("Нет изменений для сохранения", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Не удалось найти выбранного риелтора", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
