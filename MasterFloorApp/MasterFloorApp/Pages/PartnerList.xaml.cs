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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MasterFloorApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для PartnerList.xaml
    /// </summary>
    public partial class PartnerList : Page
    {
        public PartnerList()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            var OriginPartners = Model.MasterFloorDBEntities.GetContext().Partners.ToList();
            List<Utils.PartnerWithProcent> PartnersItems = new List<Utils.PartnerWithProcent>();
            foreach (var item in OriginPartners)
            {
                PartnersItems.Add(new Utils.PartnerWithProcent(item));
            }
            PartnerListView.ItemsSource = PartnersItems;
        }

        private void EditPartner_Click(object sender, RoutedEventArgs e)
        {
            Utils.Navigation.CurrentFrame.Navigate(new Pages.EditPage((sender as Button).DataContext as Utils.PartnerWithProcent));
        }

        private void HistoryPartner_Click(object sender, RoutedEventArgs e)
        {
            Utils.Navigation.CurrentFrame.Navigate(new Pages.HistoryPage((sender as Button).DataContext as Utils.PartnerWithProcent));
        }

        private void AddPartner_Click(object sender, RoutedEventArgs e)
        {
            Utils.Navigation.CurrentFrame.Navigate(new Pages.EditPage());
        }
    }
}
