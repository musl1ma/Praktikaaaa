using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Windows;

using System.Windows.Controls;
namespace Praktika
{

    public partial class DealWindow : Window
    {
        public static System.Windows.Controls.DataGrid dealGrid;
        public DealWindow()
        {
            InitializeComponent();
            PopulateDeal();
            dealGrid = DealDG;
        }

        public void PopulateDeal()
        {
            using (var context = new hikEntities())
            {
                var query = from demand in context.Demand
                            from supply in demand.Supply
                            select new
                            {
                                demand_Id = demand.Id,
                                supply_Id = supply.Id
                            };

                DealDG.ItemsSource = query.ToList();
            }
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void CommissionButton_Click(object sender, RoutedEventArgs e)
        {
            // Запрос для получения идентификаторов спроса и предложения
            using (var context = new hikEntities())
            {
                var query = from demand in context.Demand
                            from supply in demand.Supply
                            select new
                            {
                                demand_Id = demand.Id,
                                supply_Id = supply.Id
                            };

                var queryResult = query.ToList(); // Выполнение запроса для получения результатов
            }

            // Предполагается, что DealDG.SelectedItem имеет тип, соответствующий результату запроса
            var selected = DealDG.SelectedItem as dynamic;

            if (selected != null)
            {
                int demand_Id = selected.demand_Id;
                int supply_Id = selected.supply_Id;

                CommissionWindow districtWindow = new CommissionWindow(selected, demand_Id, supply_Id);
                districtWindow.ShowDialog();
                PopulateDeal();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите сделку для того, чтобы рассчитать отчисления и комиссии", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);


            }
        }
        private void DeleteDeal(int demandId, int supplyId)
        {
            using (var context = new hikEntities())
            {
                var demand = context.Demand.Include("Supply").FirstOrDefault(d => d.Id == demandId);
                if (demand != null)
                {
                    var supply = demand.Supply.FirstOrDefault(s => s.Id == supplyId);
                    if (supply != null)
                    {
                        demand.Supply.Remove(supply);
                        context.SaveChanges();
                        PopulateDeal();
                    }
                }
            }
        }

        private void DeleteRow_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var dataContext = button?.DataContext;

            if (dataContext != null)
            {
                dynamic item = dataContext;
                int demandId = item.demand_Id;
                int supplyId = item.supply_Id;

                DeleteDeal(demandId, supplyId);
            }
        }




        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddDeal addDealWindow = new AddDeal();
            addDealWindow.Show();
            PopulateDeal();
        }
    }
}
