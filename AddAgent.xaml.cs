using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// <summary>
    /// Логика взаимодействия для AddAgent.xaml
    /// </summary>
    public partial class AddAgent : Window
    {
        public AddAgent()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string lastName = LastNameTextBox.Text;
            string firstName = FirstNameTextBox.Text;
            string middleName = MiddleNameTextBox.Text;
            string dealshareText = DealShareTextBox.Text;

            if (string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(firstName))
            {
                MessageBox.Show("Пожалуйста, заполните все поля", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!Regex.IsMatch(lastName, @"^[А-ЯЁ][а-яё]{0,14}$"))
            {
                MessageBox.Show("Проверьте корректность фамилии", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!Regex.IsMatch(firstName, @"^[А-ЯЁ][а-яё]{0,14}$"))
            {
                MessageBox.Show("Проверьте корректность имени", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!Regex.IsMatch(middleName, @"^[А-ЯЁ][а-яё]{0,14}$"))
            {
                MessageBox.Show("Проверьте корректность отчества", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(dealshareText, out int dealshare) || dealshare < 0 || dealshare > 100)
            {
                MessageBox.Show("Проверьте корректность доли сделки (от 0 до 100)", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                using (hikEntities db = new hikEntities())
                {
                    Agent newEmployee = new Agent
                    {
                        LastName = lastName,
                        FirstName = firstName,
                        MiddleName = middleName,
                        DealShare = dealshare 
                    };

                    db.Agent.Add(newEmployee);
                    db.SaveChanges();
                    MessageBox.Show("Риелтор успешно добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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
            AgentWindow agentWindow = Application.Current.Windows.OfType<AgentWindow>().FirstOrDefault();
            agentWindow?.PopulateAgent();
        }
    }
}
