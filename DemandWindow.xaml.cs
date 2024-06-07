using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Praktika
{
    /// <summary>
    /// Логика взаимодействия для DemandWindow.xaml
    /// </summary>
    public partial class DemandWindow : Window
    {
        public DemandWindow()
        {
            InitializeComponent();
            PopulateDemand();
        }

        public void PopulateDemand()
        {
            using (hikEntities db = new hikEntities())
            {
                var demands = db.Demand.ToList();
                DemandDG.ItemsSource = demands;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddDemand addDemandWindow = new AddDemand();
            addDemandWindow.Show();
            PopulateDemand();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedDemand = DemandDG.SelectedItem as Demand;
            if (selectedDemand != null)
            {
                ChangeDemand changeDemandWindow = new ChangeDemand(selectedDemand);
                changeDemandWindow.ShowDialog();
                PopulateDemand();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите потребность для изменения", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedDemand = DemandDG.SelectedItem as Demand;

            if (selectedDemand != null)
            {
                using (hikEntities db = new hikEntities())
                {
                    // Загрузка данных о сделках
                    var dealsQuery = from demand in db.Demand
                                     from supply in demand.Supply
                                     select new
                                     {
                                         demand_Id = demand.Id,
                                         supply_Id = supply.Id
                                     };

                    var deals = dealsQuery.ToList();

                    // Проверка, связана ли выбранная потребность с какой-либо сделкой
                    var hasDeals = deals.Any(deal => deal.demand_Id == selectedDemand.Id);

                    if (hasDeals)
                    {
                        MessageBox.Show("Невозможно удалить потребность, так как она участвует в сделке.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                // Запрос подтверждения удаления
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить выбранную потребность?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    using (hikEntities db = new hikEntities())
                    {
                        // Удаление потребности
                        db.Demand.Attach(selectedDemand);
                        db.Demand.Remove(selectedDemand);
                        db.SaveChanges();
                        PopulateDemand();

                        MessageBox.Show("Потребность успешно удалена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите потребность для удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void TypeButton_Click(object sender, RoutedEventArgs e)
        {
            TypeEstate typeEstate = new TypeEstate("NameTValue");
            typeEstate.Show();
        }

        private void DemandsButton_Click(object sender, RoutedEventArgs e)
        {
            AllDemandsWindow allDemandsWindow = new AllDemandsWindow();
            allDemandsWindow.Show();
        }
    }
}
