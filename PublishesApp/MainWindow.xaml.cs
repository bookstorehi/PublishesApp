using System;
using System.Collections.Generic;
using System.Data;
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

namespace PublishesApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ИзданияEntities _context = new ИзданияEntities();
        public MainWindow()
        {
            InitializeComponent();
            izdaniyaGrid.ItemsSource = _context.Издания.ToList();
            podpiskiGrid.ItemsSource = _context.Подписки.ToList();
        }

        private void izdaniyaBtn_Click(object sender, RoutedEventArgs e)
        {
            if (izdaniyaGrid.SelectedItem != null)
            {
                Издания izdanie = izdaniyaGrid.SelectedItems[0] as Издания;
                PublishChange window = new PublishChange(this, izdanie);
                window.ShowDialog();
            }
            else
            {
                izdaniyaBtn.Content = "Выбери издание";
                izdaniyaBtn.IsEnabled = false;
            }
        }

        private void otpravleniyaBtn_Click(object sender, RoutedEventArgs e)
        {
            SentsWindow window = new SentsWindow();
            window.ShowDialog();
        }

        private void podpiskiGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (podpiskiGrid.SelectedItem != null)
            {
                Подписки podpiska = podpiskiGrid.SelectedItems[0] as Подписки;

                podpiskaIndex.Content = podpiska.Индекс_издания;
            }
            else
            {
                podpiskaIndex.Content = "";
            }
        }

        private void izdaniyaGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (izdaniyaGrid.SelectedItem != null)
            {
                Издания izdanie = izdaniyaGrid.SelectedItems[0] as Издания;

                podpiskiGrid.ItemsSource = _context.Подписки.Where(x => x.Индекс_издания == izdanie.Индекс).ToList();

                izdaniyaBtn.Content = "Изменить данные";
                izdaniyaBtn.IsEnabled = true;
            }
            else
            {
                izdaniyaBtn.Content = "Выбери издание";
                izdaniyaBtn.IsEnabled = false;
            }
        }

        private void delpodpiskaBtn_Click(object sender, RoutedEventArgs e)
        {
            Подписки podpiska = (sender as Button).DataContext as Подписки;

            MessageBoxResult question = MessageBox.Show($"Отменить подписку?\nИздание: {podpiska.Издания.Названия}\nОрганизация: {podpiska.Организация}", "Отмена подписки", MessageBoxButton.OKCancel);
            if (question == MessageBoxResult.OK)
            {
                _context.Entry(podpiska).State = System.Data.Entity.EntityState.Deleted;
                _context.Подписки.Remove(podpiska);
                _context.SaveChanges();

                if (izdaniyaGrid.SelectedItem != null)
                {
                    Издания izdanie = izdaniyaGrid.SelectedItems[0] as Издания;
                    podpiskiGrid.ItemsSource = _context.Подписки.Where(x => x.Индекс_издания == izdanie.Индекс).ToList();
                }
                else
                    podpiskiGrid.ItemsSource = _context.Подписки.ToList();
            }
            
        }

        private void podpiskiBtn_Click(object sender, RoutedEventArgs e)
        {
            podpiskiGrid.ItemsSource = _context.Подписки.ToList();
            izdaniyaGrid.SelectedIndex = -1;
        }
    }
}
