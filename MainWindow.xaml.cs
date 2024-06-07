using System;
using System.Linq;
using System.Windows;

namespace Praktika
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ClientButton_Click(object sender, RoutedEventArgs e)
        {
            ClientWindow clientWindow = new ClientWindow("LastNameValue", "FirstNameValue", "MiddleNameValue", "PhoneValue", "EmailValue");
            clientWindow.Show();
            this.Close();
        }

        private void AgentButton_Click(object sender, RoutedEventArgs e)
        {
            
            AgentWindow agentWindow = new AgentWindow("LastNameValue", "FirstNameValue", "MiddleNameValue", 0);
            agentWindow.Show();
            this.Close();
        }

        private void RealEstateButton_Click(object sender, RoutedEventArgs e)
        {
 
            RealEstateWindow realEstateWindow = new RealEstateWindow("CityValue", "StreetValue", "HouseValue", "NumberValue", 0, 0);
            realEstateWindow.Show();
            this.Close();
        }

        private void SupplyButton_Click(object sender, RoutedEventArgs e)
        {
            SupplyWindow supplyWindow = new SupplyWindow(0,0,0,0);
            supplyWindow.Show();
            this.Close();
        
        }

        private void DemandButton_Click(object sender, RoutedEventArgs e)
        {
            DemandWindow demandWindow = new DemandWindow();
            demandWindow.Show();
            this.Close();
        }
        private void DealButton_Click(object sender, RoutedEventArgs e)
        {
            DealWindow dealWindow = new DealWindow();
            dealWindow.Show();
            this.Close();
        }
    }
}
