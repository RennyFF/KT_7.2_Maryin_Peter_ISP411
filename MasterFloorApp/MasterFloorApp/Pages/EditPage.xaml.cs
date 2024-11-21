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
        public EditPage(Utils.PartnerWithProcent Partner)
        {
            InitializeComponent();
            Init(Model.MasterFloorDBEntities.GetContext().Partners.Where(i => i.Id == Partner.Id).First(), false);
        }
        public EditPage()
        {
            InitializeComponent();
            Init(new Model.Partners(), true);
        }

        private void Init(Model.Partners Partner, bool isAdding)
        {
            try
            {
                IsAdding = isAdding;
                CurrentPartner = Partner;

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
                        $"{CurrentPartner.Directors.FirstName} " +
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
                    if (AdressTB.Text.Split(',').Length != 5)
                    {
                        Errors.AppendLine("Данные адреса заполнены неверно! (Индекс, Регион, Город, Улица, Номер улицы)");
                    }
                }
                if (string.IsNullOrEmpty(FIOTB.Text)) { Errors.AppendLine("Заполните ФИО директора!"); }
                else
                {
                    if (FIOTB.Text.Split(' ').Length != 3)
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
                var TempSecondName = DirectorSplitted[0];
                var TempFirstName = DirectorSplitted[1];
                var TempLastName = DirectorSplitted[2];
                var Directors = Context.Directors.Where(i => i.SecondName == TempSecondName
                && i.FirstName == TempFirstName
                && i.LastName == TempLastName);
                if (Directors.FirstOrDefault() == null)
                {
                    var NewDirector = new Model.Directors()
                    {
                        SecondName = DirectorSplitted[0],
                        FirstName = DirectorSplitted[1],
                        LastName = DirectorSplitted[2]
                    };
                    Context.Directors.Add(NewDirector);
                    Context.SaveChanges();
                    CurrentPartner.DirectorId = NewDirector.Id;
                }
                else
                {
                    CurrentPartner.DirectorId = Directors.First().Id;
                }
                var AdressSplitted = AdressTB.Text.Split(',');
                string TempPostCode = AdressSplitted[0].Trim();
                string TempRegion = AdressSplitted[1].Trim();
                string TempCity = AdressSplitted[2].Trim();
                string TempStreet = AdressSplitted[3].Trim();
                string TempHouseNumber = AdressSplitted[4].Trim();
                var PostCodes = Context.PostCodes.Where(i => i.Name.Trim() == TempPostCode);
                var Regions = Context.Regions.Where(i => i.Name.Trim() == TempRegion);
                var Cities = Context.Cities.Where(i => i.Name.Trim() == TempCity);
                var Streets = Context.Streets.Where(i => i.Name.Trim() == TempStreet);
                if (PostCodes.FirstOrDefault() != null
                    && Regions.FirstOrDefault() != null
                    && Cities.FirstOrDefault() != null
                    && Streets.FirstOrDefault() != null)
                {
                    var Adresses = Context.Adresses.Where(i => i.PostCodeId == PostCodes.FirstOrDefault().Id
                    && i.RegionId == Regions.FirstOrDefault().Id
                    && i.CityId == Cities.FirstOrDefault().Id
                    && i.StreetId == Streets.FirstOrDefault().Id
                    && i.HouseNumber == TempHouseNumber).FirstOrDefault();
                    if (Adresses != null)
                    {
                        CurrentPartner.AdressId = Adresses.Id;
                    };
                }
                else
                {
                    var NewAdress = new Model.Adresses();
                    if (PostCodes.FirstOrDefault() == null)
                    {
                        var NewPostCode = new Model.PostCodes() { Name = TempPostCode };
                        Context.PostCodes.Add(NewPostCode);
                        Context.SaveChanges();
                        NewAdress.PostCodeId = NewPostCode.Id;
                    }
                    else
                    {
                        NewAdress.PostCodeId = PostCodes.First().Id;
                    }
                    if (Regions.FirstOrDefault() == null)
                    {
                        var NewRegion = new Model.Regions() { Name = TempRegion };
                        Context.Regions.Add(NewRegion);
                        Context.SaveChanges();
                        NewAdress.RegionId = NewRegion.Id;
                    }
                    else
                    {
                        NewAdress.RegionId = Regions.First().Id;
                    }
                    if (Cities.FirstOrDefault() == null)
                    {
                        var NewCity = new Model.Cities() { Name = TempCity };
                        Context.Cities.Add(NewCity);
                        Context.SaveChanges();
                        NewAdress.CityId = NewCity.Id;
                    }
                    else
                    {
                        NewAdress.CityId = Cities.First().Id;
                    }
                    if (Streets.FirstOrDefault() == null)
                    {
                        var NewStreet = new Model.Streets() { Name = TempStreet };
                        Context.Streets.Add(NewStreet);
                        Context.SaveChanges();
                        NewAdress.StreetId = NewStreet.Id;
                    }
                    else
                    {
                        NewAdress.StreetId = Streets.First().Id;
                    }
                    NewAdress.HouseNumber = TempHouseNumber;
                    Context.Adresses.Add(NewAdress);
                    Context.SaveChanges();
                    CurrentPartner.AdressId = NewAdress.Id;
                }
                if (IsAdding)
                {
                    Context.Partners.Add(CurrentPartner);
                    Context.SaveChanges();
                    MessageBox.Show("Пользователь успешно добавлен!", "Уведомление!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    Context.SaveChanges();
                    MessageBox.Show("Пользователь успешно обновлен!", "Уведомление!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                Utils.Navigation.CurrentFrame.Navigate(new Pages.PartnerList());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
