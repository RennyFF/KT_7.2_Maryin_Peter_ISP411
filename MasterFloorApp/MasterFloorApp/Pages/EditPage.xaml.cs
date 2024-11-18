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
    /// Логика взаимодействия для EditPage.xaml
    /// </summary>
    public partial class EditPage : Page
    {
        private bool IsAdding { get; set; } = true;
        private Model.Partners CurrentPartner { get; set; }
        public EditPage(Model.Partners Partner)
        {
            InitializeComponent();
            Init(Partner);
        }

        private void Init(Model.Partners Partner)
        {
            try
            {
                if (Partner == null)
                {
                    CurrentPartner = new Model.Partners();
                }
                else
                {
                    CurrentPartner = Partner;
                    IsAdding = false;
                }

                var Types = Model.MasterFloorDBEntities.GetContext().PartnerType.ToList();
                Types.Insert(0, new Model.PartnerType() { Name = "Выберите партнера" });
                TypeCB.ItemsSource = Types;
                TypeCB.SelectedIndex = 0;
                if (!IsAdding)
                {
                    NameTB.Text = CurrentPartner.Name;
                    TypeCB.SelectedIndex = CurrentPartner.PartnerTypeId;
                    RatingTB.Text = CurrentPartner.Rating.ToString();
                    var Adress = Model.MasterFloorDBEntities.GetContext().Adresses
                        .Where(i => i.Id == CurrentPartner.AdressId).FirstOrDefault();
                    AdressTB.Text = $"{Adress.PostCodes.Name}, " +
                        $"{Adress.Regions.Name}, " +
                        $"{Adress.Cities.Name}, " +
                        $"{Adress.Streets.Name}, " +
                        $"{Adress.HouseNumber}";
                    FIOTB.Text = $"{CurrentPartner.Directors.SecondName} " +
                        $"{CurrentPartner.Directors.FirstName}" +
                        $"{CurrentPartner.Directors.LastName}";
                    PhoneTB.Text = CurrentPartner.Phone;
                    EmailTB.Text = CurrentPartner.Mail;
                }
            }
            catch (Exception)
            {
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Utils.Navigation.CurrentFrame.CanGoBack)
            {
                Utils.Navigation.CurrentFrame.GoBack();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var Context = Model.MasterFloorDBEntities.GetContext();
                StringBuilder Errors = new StringBuilder();
                if (string.IsNullOrEmpty(NameTB.Text)) { Errors.AppendLine("Заполните наименование!"); }
                if (TypeCB.SelectedIndex == 0) { Errors.AppendLine("Выберите тип!"); }
                if (string.IsNullOrEmpty(RatingTB.Text)) { Errors.AppendLine("Заполните рейтинг!"); }
                else
                {
                    if (!int.TryParse(RatingTB.Text, out int result)) { Errors.AppendLine("Рейтинг - не число!"); }
                    else
                    {
                        if (result < 0) { Errors.AppendLine("Рейтинг должет быть положительным!"); }
                    }
                }
                if (string.IsNullOrEmpty(AdressTB.Text)) { Errors.AppendLine("Заполните адрес!"); }
                else
                {
                    if (AdressTB.Text.Split(',').Length < 5 || AdressTB.Text.Split(',').Length > 5)
                    {
                        Errors.AppendLine("Данные адреса заполнены неверно! (Индекс, Регион, Город, Улица, Номер улицы)");
                    }
                }
                if (string.IsNullOrEmpty(FIOTB.Text)) { Errors.AppendLine("Заполните ФИО директора!"); }
                else
                {
                    if (AdressTB.Text.Split(' ').Length < 3 || AdressTB.Text.Split(' ').Length > 3)
                    {
                        Errors.AppendLine("Данные ФИО заполнены неверно! (Фамилия Имя Отчество)");
                    }
                }
                if (string.IsNullOrEmpty(PhoneTB.Text)) { Errors.AppendLine("Заполните номер телефона!"); }
                if (string.IsNullOrEmpty(EmailTB.Text)) { Errors.AppendLine("Заполните почту!"); }
                if (Errors.Length > 0)
                {
                    MessageBox.Show(Errors.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                CurrentPartner.Name = NameTB.Text;
                CurrentPartner.PartnerTypeId = TypeCB.SelectedIndex;
                CurrentPartner.Rating = int.Parse(RatingTB.Text);
                CurrentPartner.Phone = PhoneTB.Text;
                CurrentPartner.Mail = EmailTB.Text;
                var DirectorSplitted = FIOTB.Text.Split(' ');
                var Directors = Context.Directors.Where(i => i.SecondName == DirectorSplitted[0] 
                && i.FirstName == DirectorSplitted[1] 
                && i.LastName == DirectorSplitted[2]);
                if (Directors.FirstOrDefault() == null)
                {
                    var NewDirector = new Model.Directors() { SecondName = DirectorSplitted[0], 
                        FirstName = DirectorSplitted[1], 
                        LastName = DirectorSplitted[2] };
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
