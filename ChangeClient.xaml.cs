using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Praktika
{
    /// <summary>
    /// Логика взаимодействия для ChangeClient.xaml
    /// </summary>
    public partial class ChangeClient : Window
    {
        private string clientLastName;
        private string clientFirstName;
        private string clientMiddleName;
        private string clientPhone;
        private string clientEmail;
        private Client selectedEmployee;

        public ChangeClient(Client selectedEmployee, string LastName, string FirstName, string MiddleName, string clientPhone, string Email)
        {
            InitializeComponent();
            this.selectedEmployee = selectedEmployee;
            this.clientLastName = LastName;
            this.clientFirstName = FirstName;
            this.clientMiddleName = MiddleName;
            this.clientPhone = clientPhone;
            this.clientEmail = Email;
            FillFields();
            
        }

        private void FillFields()
        {
            if (selectedEmployee != null)
            {
                LastNameTextBox.Text = selectedEmployee.LastName;
                FirstNameTextBox.Text = selectedEmployee.FirstName;
                MiddleNameTextBox.Text = selectedEmployee.MiddleName;
                PhoneTextBox.Text = selectedEmployee.Phone;
                EmailTextBox.Text = selectedEmployee.Email;
            }
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            string newLastName = LastNameTextBox.Text;
            string newFirstName = FirstNameTextBox.Text;
            string newMiddleName = MiddleNameTextBox.Text;
            string newPhone = PhoneTextBox.Text;
            string newEmail = EmailTextBox.Text;


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

            using (hikEntities db = new hikEntities())
            {
                var existingEmployee = db.Client.FirstOrDefault(u => u.Id == selectedEmployee.Id);
                if (existingEmployee != null)
                {
                    if (existingEmployee.LastName != newLastName || existingEmployee.FirstName != newFirstName || existingEmployee.MiddleName != newMiddleName || existingEmployee.Phone != newPhone || existingEmployee.Email != newEmail)
                    {
                        existingEmployee.LastName = newLastName;
                        existingEmployee.FirstName = newFirstName;
                        existingEmployee.MiddleName = newMiddleName;
                        existingEmployee.Phone = newPhone;
                        existingEmployee.Email = newEmail;
                        db.SaveChanges();
                        MessageBox.Show("Данные клиента успешно изменены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                        ClientWindow clientWindow = Application.Current.Windows.OfType<ClientWindow>().FirstOrDefault();
                        clientWindow?.PopulateClients();
                    }
                    else
                    {
                        MessageBox.Show("Нет изменений для сохранения", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Не удалось найти выбранного клиента", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
