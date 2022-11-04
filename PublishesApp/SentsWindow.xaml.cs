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
using Word = Microsoft.Office.Interop.Word;

namespace PublishesApp
{
    /// <summary>
    /// Логика взаимодействия для SentsWindow.xaml
    /// </summary>
    public partial class SentsWindow : Window
    {
        ИзданияEntities _context = new ИзданияEntities();
        public SentsWindow()
        {
            InitializeComponent();
            sentsGrid.ItemsSource = _context.Отправления.OrderBy(p => p.Подписки.Дата_окончания).ToList();

            podpiskiCmbx.ItemsSource = _context.Подписки.ToList();
            podpiskiCmbx.DisplayMemberPath = "Формат1";
        }

        private void sentsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Отправления sent = sentsGrid.SelectedItem as Отправления;
            sentProperties.Content = "Индекс издания: " + sent.Подписки.Индекс_издания.Trim() + ". Окончание подписки: " + sent.Подписки.Конец + ". Сумма: " + sent.Подписки.Сумма;
        }

        private void goBtn_Click(object sender, RoutedEventArgs e)
        {
            if (podpiskiCmbx.Text != "" && firstName.Text != "" && sureName.Text != "" && role.Text != "" && date.Text != "")
            {
                if (DateTime.Parse(date.Text) > DateTime.Now)
                {
                    try
                    {
                        Подписки podpiska = podpiskiCmbx.SelectedItem as Подписки;

                        Отправления sent = new Отправления();
                        sent.Номер_подписки = podpiska.ИД;
                        sent.Имя_получателя = firstName.Text;
                        sent.Фамилия_получателя = sureName.Text;

                        if (patronymic.Text != "")
                            sent.Отчество_получателя = patronymic.Text;

                        sent.Должность = role.Text;
                        sent.Предполагаемая_дата = DateTime.Parse(date.Text);

                        _context.Отправления.Add(sent);
                        _context.SaveChanges();
                        sentsGrid.ItemsSource = _context.Отправления.OrderBy(p => p.Подписки.Дата_окончания).ToList();

                        sentProperties.Content = "";

                        podpiskiCmbx.SelectedIndex = -1;
                        firstName.Text = "";
                        sureName.Text = "";
                        patronymic.Text = "";
                        role.Text = "";
                        date.Text = "";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                    MessageBox.Show("Дата получения меньше сегодняшней даты!");
            }
            else
                MessageBox.Show("Заполните все поля");
        }

        private void savepdf_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var app = new Word.Application();
                Word.Document document = app.Documents.Add();

                Word.Paragraph paragraph = document.Paragraphs.Add();
                Word.Range range = paragraph.Range;

                range.Text = "Издания - история отправлений";
                paragraph.set_Style("Заголовок 1");
                range.InsertParagraphAfter();

                Word.Paragraph tableParagraph = document.Paragraphs.Add();
                Word.Range tableRange = tableParagraph.Range;
                Word.Table sentsTable = document.Tables.Add(tableRange, _context.Отправления.Count() + 1, 7);

                sentsTable.Borders.InsideLineStyle =
                sentsTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //studentsTable.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                //Word.Range cellRange;
                //cellRange.Text = "Организация";

                sentsTable.Cell(1, 1).Range.Text = "Организация";
                sentsTable.Cell(1, 2).Range.Text = "Издание";
                sentsTable.Cell(1, 3).Range.Text = "Получатель";
                sentsTable.Cell(1, 4).Range.Text = "Должность";
                sentsTable.Cell(1, 5).Range.Text = "Предполагаемая дата";
                sentsTable.Cell(1, 6).Range.Text = "Дата получения";
                sentsTable.Cell(1, 7).Range.Text = "Доставка";

                sentsTable.Rows[1].Range.Bold = 1;
                sentsTable.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

                int i = 1;
                foreach (Отправления sent in _context.Отправления)
                {
                    sentsTable.Cell(i + 1, 1).Range.Text = sent.Организация;
                    sentsTable.Cell(i + 1, 2).Range.Text = sent.Издание;
                    sentsTable.Cell(i + 1, 3).Range.Text = sent.Получатель;
                    sentsTable.Cell(i + 1, 4).Range.Text = sent.Должность;
                    sentsTable.Cell(i + 1, 5).Range.Text = sent.Примерная_дата;
                    sentsTable.Cell(i + 1, 6).Range.Text = sent.Дата;
                    sentsTable.Cell(i + 1, 7).Range.Text = sent.Доставка;
                    i++;
                }

                Word.Paragraph sentsSumParagraph = document.Paragraphs.Add();
                Word.Range sentsSumRange = sentsSumParagraph.Range;
                sentsSumRange.Text = $"Всего отправлений: {_context.Отправления.Count()}, не получены: {_context.Отправления.Where(s => s.Дата_получения == null).Count()}.";
                sentsSumRange.Font.Color = Word.WdColor.wdColorDarkRed;
                sentsSumRange.InsertParagraphAfter();

                document.Words.Last.InsertBreak(Word.WdBreakType.wdPageBreak);
                app.Visible = true;
                document.SaveAs2(@"D:\sentsPdf.pdf", Word.WdExportFormat.wdExportFormatPDF);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
