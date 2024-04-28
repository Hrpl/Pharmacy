using Pharmacy.Models;
using Pharmacy.ViewModels;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MessageBox = System.Windows.Forms.MessageBox;
using Syncfusion.UI.Xaml.Grid.Converter;
using Syncfusion.XlsIO;

namespace Pharmacy
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Employee User = new Employee();
        public Medicine AddMedicine = new Medicine();
        public ObservableCollection<MedicInRequest> MedicineInRequest = new();
        public ObservableCollection<MedicInRequest> AcceptMedic = new();
        public ObservableCollection<MedicInOrder> MedicineInOrder = new();

        
        public MainWindow(Employee _user)
        {
            InitializeComponent();

            using (PharmacyDbContext db = new PharmacyDbContext())
            {
                sortUserComboBox.ItemsSource = new List<string>() { "По возрастанию цены", "По убыванию цены" };

                List<string> filtertList = db.Medicines.Select(u => u.Manufacture).Distinct().ToList();
                filtertList.Insert(0, "Все производители");
                filterUserComboBox.ItemsSource = filtertList;
                countProducts.Text = $"Количество: {db.Medicines.Count()}";
            }

            if (_user.RoleId == 3)
            {
                PanelMainEmployee.Visibility = Visibility.Collapsed;
                PanelEmployee.Visibility = Visibility.Visible;
            }

            if (_user.RoleId == 2)
            {
                PanelMainEmployee.Visibility = Visibility.Visible;
                PanelEmployee.Visibility = Visibility.Collapsed;
            }

            this.DataContext = new ListMedicine();
            _user = User;
        }

        private void sortUserComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }

        private void filterUserComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();
        }

        private void Update()
        {
            using (PharmacyDbContext db = new PharmacyDbContext())
            {

                var current = db.Medicines.ToList();
                medicineListView.ItemsSource = current;

                if (sortUserComboBox.SelectedIndex != -1)
                {
                    if (sortUserComboBox.SelectedValue == "По убыванию цены")
                    {
                        current = db.Medicines.OrderByDescending(u => u.SalePrice).ToList();
                    }

                    if (sortUserComboBox.SelectedValue == "По возрастанию цены")
                    {
                        current = db.Medicines.OrderBy(u => u.SalePrice).ToList();
                    }
                }

                if (filterUserComboBox.SelectedIndex != -1)
                {
                    if (db.Medicines.Select(u => u.Manufacture).Distinct().ToList().Contains(filterUserComboBox.SelectedValue))
                    {
                        current = current.Where(u => u.Manufacture == filterUserComboBox.SelectedValue.ToString()).ToList();
                    }
                    else
                    {
                        current = current.ToList();
                    }
                }

                if (searchBox.Text.Length > 0)
                {

                    current = current.Where(u => u.Name.Contains(searchBox.Text) || u.Manufacture.Contains(searchBox.Text)).ToList();

                }

                medicineListView.ItemsSource = current;
                countProducts.Text = $"Количество: {current.Count} из {db.Medicines.ToList().Count}";
            }
        }

        private void ViewListMedicine(object sender, RoutedEventArgs e) //Главный врач
        {
            addedMedInAccept.Visibility = Visibility.Collapsed;
            AcceptRequest.Visibility = Visibility.Collapsed;
            medicineListView.Visibility = Visibility.Visible;
            FilterPanel.Visibility = Visibility.Visible;
            stackPanelMedicine.Visibility = Visibility.Collapsed;
            stackPanelRequest.Visibility = Visibility.Collapsed;
            ViewIssueList.Visibility = Visibility.Collapsed;
            addedMedInOrder.Visibility = Visibility.Collapsed;
            AddOrder.Visibility = Visibility.Collapsed;

            using (PharmacyDbContext db = new PharmacyDbContext())
            {
                medicineListView.ItemsSource = db.Medicines.ToList();
            }
        }

        private void RouteToAddMedicine(object sender, RoutedEventArgs e)//Главный врач
        {
            addedMedInAccept.Visibility = Visibility.Collapsed;
            AcceptRequest.Visibility = Visibility.Collapsed;
            medicineListView.Visibility = Visibility.Collapsed;
            stackPanelRequest.Visibility = Visibility.Collapsed;

            stackPanelMedicine.Visibility = Visibility.Visible;

            ViewIssueList.Visibility = Visibility.Collapsed;
            FilterPanel.Visibility = Visibility.Collapsed;
            addedMedInOrder.Visibility = Visibility.Collapsed;
            AddOrder.Visibility = Visibility.Collapsed;
        }
        private void RouteToAddRequest(object sender, RoutedEventArgs e)//Главный врач
        {
            addedMedInOrder.Visibility = Visibility.Collapsed;
            AddOrder.Visibility = Visibility.Collapsed;
            addedMedInAccept.Visibility = Visibility.Collapsed;
            AcceptRequest.Visibility = Visibility.Collapsed;
            medicineListView.Visibility = Visibility.Collapsed;
            stackPanelMedicine.Visibility = Visibility.Collapsed;
            FilterPanel.Visibility = Visibility.Collapsed;

            stackPanelRequest.Visibility = Visibility.Visible;
            ViewIssueList.Visibility = Visibility.Visible;
        }
        private void RouteToAddOrder(object sender, RoutedEventArgs e) // сотрудник
        {
            addedMedInAccept.Visibility = Visibility.Collapsed;
            AcceptRequest.Visibility = Visibility.Collapsed;
            medicineListView.Visibility = Visibility.Collapsed;
            stackPanelMedicine.Visibility = Visibility.Collapsed;
            FilterPanel.Visibility = Visibility.Collapsed;
            stackPanelRequest.Visibility = Visibility.Collapsed;
            ViewIssueList.Visibility = Visibility.Collapsed;

            addedMedInOrder.Visibility = Visibility.Visible;
            AddOrder.Visibility = Visibility.Visible;
        }

        private void RouteToAcceptRequest(object sender, RoutedEventArgs e)// сотрудник
        {
            addedMedInAccept.Visibility = Visibility.Visible;
            AcceptRequest.Visibility = Visibility.Visible;

            

            addedMedInOrder.Visibility = Visibility.Collapsed;
            AddOrder.Visibility = Visibility.Collapsed;
            medicineListView.Visibility = Visibility.Collapsed;
            stackPanelMedicine.Visibility = Visibility.Collapsed;
            FilterPanel.Visibility = Visibility.Collapsed;
            stackPanelRequest.Visibility = Visibility.Collapsed;
            ViewIssueList.Visibility = Visibility.Collapsed;
        }
        private void ExitWindow(object sender, RoutedEventArgs e)
        {
            new Login().Show();
            this.Close();
        }

        private void NewMedicine(object sender, RoutedEventArgs e)
        {
            using (PharmacyDbContext db = new PharmacyDbContext())
            {
                bool isCurrent = true;
                if(NameMedicine.Text == "") isCurrent = false;
                if(ManufactureMedicine.Text == "") isCurrent = false ;
                if(ManufactureMedicine.Text == "" || Convert.ToInt32(SalePrice.Text) <= 0) isCurrent = false;

                if (isCurrent)
                {
                    AddMedicine.Name = NameMedicine.Text;
                    AddMedicine.Manufacture = ManufactureMedicine.Text;
                    AddMedicine.Quantity = 0;
                    AddMedicine.SalePrice = Convert.ToInt32(SalePrice.Text);
                    db.Medicines.Add(AddMedicine);
                    try
                    {
                        db.SaveChanges();
                        MessageBox.Show("Лекарство добавлено", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch
                    {
                        MessageBox.Show("Лекарство не добавлено", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else MessageBox.Show("Неверные значения полей", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddMedicineInRequest(object sender, RoutedEventArgs e)
        {
            
            using (PharmacyDbContext db = new PharmacyDbContext())
            {
                bool isCurrent = true;

                try
                {
                    var med = db.Medicines.Where(m => m.Name == MedicineComboBox.SelectedItem).First();

                    foreach (var item in MedicineInRequest)
                    {
                        if (item.MedicineId == med.Id) isCurrent = false;
                    }

                    if (Quantity.Text == "") isCurrent = false;

                    if (isCurrent)
                    {
                        MedicInRequest req = new MedicInRequest();
                        try
                        {
                            req.MedicineId = med.Id;
                            req.Quantity = Convert.ToInt32(Quantity.Text);
                            
                            MedicineInRequest.Add(req);
                        }
                        catch
                        {
                            MessageBox.Show("Лекарство не было добавлено", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                    else
                    {
                        MessageBox.Show("Лекарство не было добавлено", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch { MessageBox.Show("Лекарство не было добавлено", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
            Quantity.Text = "";
            issueList.ItemsSource = MedicineInRequest;
        }

        private void CreateRequest(object sender, RoutedEventArgs e)
        {
            using (PharmacyDbContext db = new PharmacyDbContext())
            {
                if (MedicineInRequest != null)
                {
                    bool isCurrent = true;

                    foreach (var item in db.Request.ToList()) // проверка на уникальность номера
                    {
                        if (item.Number == NumberRequest.Text) 
                        {
                            isCurrent = false;
                            MessageBox.Show("Заявка c таким номером уже существует", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        } 
                    }

                    if (isCurrent)
                    {
                        var req = new Request();
                        if (NumberRequest.Text != "" && Provider.Text != "")
                        {
                            req.DateTime = DateTime.Now;
                            req.ProviderName = Provider.Text;
                            req.Status = "Create";
                            req.Number = NumberRequest.Text;

                            foreach (var item in MedicineInRequest)
                            {
                                item.RequestNumber = NumberRequest.Text;
                            }

                            db.Request.Add(req);
                            db.MedicInRequest.AddRange(MedicineInRequest);

                            try
                            {
                                db.SaveChanges();
                                MessageBox.Show("Заявка создана", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                MedicineInRequest.Clear();
                                issueList.ItemsSource = MedicineInRequest;

                            }
                            catch { MessageBox.Show("Заявка не создана", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                        }
                    }
                }
                Provider.Text = "";
                NumberRequest.Text = "";
            }
        }

        private void AddAcceptMedicine(object sender, RoutedEventArgs e)
        {
            var med = new MedicInRequest();
            using (PharmacyDbContext db = new PharmacyDbContext())
            {
                try
                {
                    med.MedicineId = db.Medicines.Where(m => m.Name == AcceptMedicineComboBox.Text).First().Id;
                    med.Quantity = Convert.ToInt32(AcceptQuantity.Text);
                    med.InUnitPrice = Convert.ToInt32(AcceptPrice.Text);
                    med.AllPrice = med.Quantity * med.InUnitPrice;
                    AcceptMedic.Add(med);
                    addedMedInAcceptList.ItemsSource = AcceptMedic;
                    
                    MessageBox.Show("Лекарство добавлено", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch 
                {
                    MessageBox.Show("Лекарство не добавлено", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void AcceptMedicine(object sender, RoutedEventArgs e)
        {
            using (PharmacyDbContext db = new PharmacyDbContext())
            {
                bool isCurrent = true;
                int count = 0;

                if (AcceptProvider.Text == "") isCurrent = false;
                if (AcceptNumberRequest.Text == "") isCurrent = false;
                if (AcceptPrice.Text == "") isCurrent = false;

                if (isCurrent) // поля не были пусты
                {
                    var listMed = db.MedicInRequest.Where(m => m.RequestNumber == AcceptNumberRequest.Text).ToList();

                    if (listMed != null) // есть такие лекарста с номером заявки
                    {
                        foreach (var i in listMed)
                        {
                            foreach (var j in AcceptMedic)
                            {
                                if (i.MedicineId == j.MedicineId && i.Quantity == j.Quantity) count++; //проверка что все лекарства введены
                            }
                        }
                        if (count == listMed.Count) // все ли лекарства пришли
                        {
                            var req = db.Request.Where(r => r.ProviderName == AcceptProvider.Text && r.Number == AcceptNumberRequest.Text).FirstOrDefault(); //поиск заявки
                            if (req != null && req.Status == "Create") // была ли выполнена заявка ранее
                            {
                                req.Status = "Accept";
                                req.SummaryPrice = 0;

                                foreach (var item in AcceptMedic)
                                {

                                    req.SummaryPrice += item.Quantity * item.InUnitPrice;

                                    var med = db.Medicines.Find(item.MedicineId);
                                    med.Quantity += item.Quantity;

                                    db.Medicines.Update(med);
                                }

                                db.Request.Update(req);
                                db.MedicInRequest.UpdateRange(AcceptMedic);

                                try
                                {
                                    db.SaveChanges();
                                    MessageBox.Show("Накладная принята", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    AcceptMedic.Clear();
                                    addedMedInAcceptList.ItemsSource = AcceptMedic;

                                }
                                catch (Exception)
                                {
                                    MessageBox.Show("Ошибка при сохранении", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }

                            }
                            else MessageBox.Show("Заявка уже принята или её не существует", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else MessageBox.Show("Накладная не принята", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else MessageBox.Show("Не верный номер накладной", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else MessageBox.Show("Поля не заполнены", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddOrderMedicine(object sender, RoutedEventArgs e)
        {
            var med = new MedicInOrder();
            bool isRepeat = false;
            using (PharmacyDbContext db = new PharmacyDbContext())
            {
                try
                {
                    med.MedicineId = db.Medicines.Where(m => m.Name == OrderMedicineComboBox.SelectedItem).First().Id;
                    med.PriceForOne = db.Medicines.Where(m => m.Name == OrderMedicineComboBox.SelectedItem).First().SalePrice;
                    med.Quantity = Convert.ToInt32(OrderQuantity.Text);
                    foreach (var m in MedicineInOrder)
                    {
                        if(m.MedicineId == med.MedicineId) isRepeat = true;
                    }
                    if (!isRepeat)
                    {
                        MedicineInOrder.Add(med);
                        
                        MessageBox.Show("Лекарство добавлено в заказ", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else { MessageBox.Show("Лекарство уже добавлено в заказ", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                catch (Exception)
                {
                    MessageBox.Show("Лекарство не добавлено в заказ", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            addedMedInOrderList.ItemsSource = MedicineInOrder;
        }

        private void OrderMedicine(object sender, RoutedEventArgs e)
        {
            using (PharmacyDbContext db = new PharmacyDbContext())
            {
                var order = new Order();
                order.Date = DateTime.Now;
                order.SummaryPrice = 0;

                if(MedicineInOrder.Count > 0)
                {
                    foreach (var item in MedicineInOrder)
                    {
                        order.SummaryPrice += item.Quantity * item.PriceForOne;
                    }

                    db.Order.Add(order);

                    try
                    {
                        db.SaveChanges(); ;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Ошибка при сохранении заказа", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    order = db.Order.ToList().Last();

                    foreach (var item in MedicineInOrder)
                    {
                        item.OrderId = order.Id;
                    }

                    db.MedicInOrders.AddRange(MedicineInOrder);

                    try
                    {
                        db.SaveChanges();
                        MedicineInOrder.Clear();
                        addedMedInOrderList.ItemsSource = MedicineInOrder;
                        MessageBox.Show("Заказ создан", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Ошибка при сохранении позиций заказа", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else MessageBox.Show("В заказ не добавлены лекарства", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AcceptClearLabel(object sender, RoutedEventArgs e)
        {
            AcceptQuantity.Text = "";
            AcceptMedicineComboBox.SelectedItem = null;
        }

        
    }
}