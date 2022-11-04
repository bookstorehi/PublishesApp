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
using System.Windows.Shapes;

namespace PublishesApp
{
    /// <summary>
    /// Логика взаимодействия для PublishChange.xaml
    /// </summary>
    public partial class PublishChange : Window
    {
        Издания _publish;
        MainWindow _window;
        public PublishChange(MainWindow window, Издания publish)
        {
            InitializeComponent();
            _publish = publish;
            _window = window;

            publishIndex.Content = publish.Индекс;

            nameBtn.Text = publish.Названия;

            publishType.Text = publish.Тип_издания;
            if (publish.Тип_издания == "Журнал")
                publishType.SelectedIndex = 0;
            else if (publish.Тип_издания == "Газета")
                publishType.SelectedIndex = 1;

            publishQuantity.Text = publish.Количество_издания.ToString();
        }

        private void goBtn_Click(object sender, RoutedEventArgs e)
        {
            if (nameBtn.Text != "" && publishType.Text != "" && publishQuantity.Text != "")
            {
                int quantity;
                bool x = int.TryParse(publishQuantity.Text, out quantity);
                if (x)
                {
                    try
                    {
                        _window._context.Издания.Attach(_publish);
                        _publish.Названия = nameBtn.Text;
                        _publish.Тип_издания = publishType.Text;
                        _publish.Количество_издания = quantity;
                        _window._context.SaveChanges();

                        _window.izdaniyaGrid.ItemsSource = _window._context.Издания.ToList();
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Произошла ошибка. Подробности: " + ex.Message);
                    }
                }
                else
                    MessageBox.Show("Неверное количество");
            }
            else
                MessageBox.Show("Пожалуйста, заполните все поля.");
        }
    }
}
