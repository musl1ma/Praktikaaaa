using Microsoft.Extensions.Logging;
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Windows;

namespace Praktika
{
    public partial class CommissionWindow : Window
    {
        private dynamic selected;
        private int demandId;
        private int supplyId;

        public CommissionWindow(dynamic selected, int demandId, int supplyId)
        {
            InitializeComponent();
            this.selected = selected;
            this.demandId = demandId;
            this.supplyId = supplyId;
            FillFields();
        }

        private void FillFields()
        {
            if (selected != null)
            {
                var ServiceSeller = 0;
                var Price = 0;
                var Type = 0;
                using (hikEntities db = new hikEntities())
                {
                    var supply = db.Supply.FirstOrDefault(d => d.Id == supplyId);
                    if (supply != null)
                    {
                        Price = Convert.ToInt32(supply.Price);
                    }
                    else
                    {
                        MessageBox.Show("Не удалось найти выбранную потребность.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    var demand = db.Demand.FirstOrDefault(d => d.Id == demandId);

                    if (demand != null)
                    {
                        Type = demand.Id_type;
                    }
                    else
                    {
                        MessageBox.Show("Не удалось найти выбранную потребность.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    if (Type == 1)
                    {
                        var proc = Price / 100;
                        ServiceSeller = 36000 + proc;
                    }
                    if (Type == 2)
                    {
                        var proc = Price / 100;
                        ServiceSeller = 30000 + proc;
                    }
                    if (Type == 3)
                    {
                        var proc = (Price * 2) / 100;
                        ServiceSeller = 30000 + proc;
                    }
                    ServiceSellerTextBox.Text = Convert.ToString(ServiceSeller);
                }

                var ServiceBuyer = (Price * 3) / 100;
                ServiceBuyerTextBox.Text = Convert.ToString(ServiceBuyer);

                var DeductionsRealtorSeller = 0;
                var agentId = 0;
                var dealshare = 0;
                using (hikEntities db = new hikEntities())
                {
                    var supply = db.Supply.FirstOrDefault(d => d.Id == supplyId);
                    if (supply != null)
                    {
                        agentId = Convert.ToInt32(supply.AgentId);
                    }
                    else
                    {
                        MessageBox.Show("Не удалось найти выбранную потребность.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    var agent = db.Agent.FirstOrDefault(d => d.Id == agentId);
                    if (agent != null)
                    {
                        dealshare = Convert.ToInt32(agent.DealShare);
                    }
                    else
                    {
                        MessageBox.Show("Не удалось найти выбранную потребность.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                if (dealshare != null)
                {
                    DeductionsRealtorSeller = (ServiceSeller * dealshare) / 100;
                }
                if (dealshare == null)
                {
                    DeductionsRealtorSeller = (ServiceSeller * 45) / 100;
                }
                DeductionsRealtorSellerTextBox.Text = Convert.ToString(DeductionsRealtorSeller);

                var DeductionsRealtorBuyer = 0;
                var procAgentSeller = 0;
                var AgentId = 0;
                var Dealshare = 0;
                using (hikEntities db = new hikEntities())
                {
                    var demand = db.Demand.FirstOrDefault(d => d.Id == demandId);
                    if (demand != null)
                    {
                        AgentId = Convert.ToInt32(demand.AgentId);
                    }
                    else
                    {
                        MessageBox.Show("Не удалось найти выбранную потребность.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    var agent = db.Agent.FirstOrDefault(d => d.Id == AgentId);
                    if (agent != null)
                    {
                        Dealshare = Convert.ToInt32(agent.DealShare);
                    }
                    else
                    {
                        MessageBox.Show("Не удалось найти выбранную потребность.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                }
                if (Dealshare != null)
                {
                    DeductionsRealtorBuyer = (ServiceBuyer * Dealshare) / 100;
                }
                if (Dealshare == null)
                {
                    DeductionsRealtorBuyer = (ServiceBuyer * 45) / 100;
                }
                DeductionsRealtorBuyerTextBox.Text = Convert.ToString(DeductionsRealtorBuyer);

                var DeductionsRealtorCompany = ServiceBuyer - DeductionsRealtorBuyer + ServiceSeller - DeductionsRealtorSeller;
                DeductionsRealtorCompanyTextBox.Text = Convert.ToString(DeductionsRealtorCompany);

            }

        }



    }

}

