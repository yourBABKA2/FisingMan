using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
using ООО_Улов.Entities;
using ООО_Улов.Infrastructure;

namespace ООО_Улов
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class Catalog : Window
    {
        int filterDis = 0;
        double[,] arrayDiscount = new double[,] { { 0, 100 }, { 0, 9.99}, { 10, 14.99}, { 15, 100 } }; 

        public Catalog()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            ComboBoxSort.SelectedIndex = 0;
            ComboBoxSort.ItemsSource = new List<string> { "По возрастанию", "По убыванию"};

            ComboBoxSale.SelectedIndex = 0;
            ComboBoxSale.ItemsSource = new List<string> { "Все диапазоны", "Скидка 0 - 9.99%", "Скидка 10 - 14.99%", "Скидка 15% и более" };
        }

        public void Update()
        {
            List<Entities.Catalog> product = null;
            product = Helper.DbContext.Catalog.ToList();

            var allCount = product.Count;
            SorByCost(ref product);
            product = product.Where(x => (x.CurrentSale >= arrayDiscount[filterDis, 0] && x.CurrentSale <= arrayDiscount[filterDis, 1])).ToList();

            var currentCount = product.Count;
            LabelCount.Content= "Товаров отображается: " +  currentCount + " " + "из " + allCount;
            ListViewCatalog.ItemsSource = product;
        }

        public void SorByCost(ref List<Entities.Catalog> product)
        {
            if (ComboBoxSort.SelectedIndex == 1) product = product.OrderByDescending(x => x.Cost).ToList();
            else product = product.OrderBy(x => x.Cost).ToList();
        }

        public void SortByDiscount() 
        {

        }

        private void OnBrnExitClick(object sender, RoutedEventArgs e)
        {
            if (MessageBoxResult.Yes == MessageBox.Show("Вы точно хотите выйти из приложения?", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question)) this.Close();
        }

        private void OnComboBoxSortSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }

        private void OnComboBoxSaleSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            filterDis = ComboBoxSale.SelectedIndex;
            Update();
        }
    }
}
