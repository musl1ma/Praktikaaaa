using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Windows;

namespace Praktika
{
    public partial class AddDeal : Window
    {
        public AddDeal()
        {
            InitializeComponent();
            LoadComboBoxData();
        }

        private void LoadComboBoxData()
        {
            using (hikEntities db = new hikEntities())
            {
                var demands = db.Demand.ToList();
                IdDemandComboBox.ItemsSource = demands;
                IdDemandComboBox.DisplayMemberPath = "Id";
                IdDemandComboBox.SelectedValuePath = "Id";

                var supplies = db.Supply.ToList();
                IdSupplyComboBox.ItemsSource = supplies;
                IdSupplyComboBox.DisplayMemberPath = "Id";
                IdSupplyComboBox.SelectedValuePath = "Id";
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (IdDemandComboBox.SelectedItem == null || IdSupplyComboBox.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите потребность и предложение.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int selectedDemandId = (int)IdDemandComboBox.SelectedValue;
            int selectedSupplyId = (int)IdSupplyComboBox.SelectedValue;

            try
            {
                using (hikEntities db = new hikEntities())
                {
                    var demand = db.Demand.FirstOrDefault(d => d.Id == selectedDemandId);
                    var supply = db.Supply.FirstOrDefault(s => s.Id == selectedSupplyId);

                    if (demand != null && supply != null)
                    {
                        // Проверка, что потребность и предложение уже не участвуют в сделке
                        bool demandInDeal = db.Demand.Any(d => d.Id == selectedDemandId && d.Supply.Any());
                        bool supplyInDeal = db.Supply.Any(s => s.Id == selectedSupplyId && s.Demand.Any());

                        if (demandInDeal || supplyInDeal)
                        {
                            MessageBox.Show("Выбранные потребность или предложение уже участвуют в другой сделке.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        // Добавляем предложение к потребности
                        demand.Supply.Add(supply);
                        db.SaveChanges();
                        MessageBox.Show("Сделка успешно добавлена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Не удалось найти выбранные потребность или предложение.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
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
            DealWindow dealWindow = Application.Current.Windows.OfType<DealWindow>().FirstOrDefault();
            dealWindow?.PopulateDeal();
        }

    }
}


