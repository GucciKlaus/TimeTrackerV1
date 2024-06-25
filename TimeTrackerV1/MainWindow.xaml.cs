using DataLayerLib;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TimeTrackerV1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer? _timer;
        private DateTime _startTime;
        private TimeSpan _elapsedTime;
        private bool _running;
        private DataContext DB;
        private readonly List<string> PrintSelection = new List<string> { "Tag", "Woche", "Monat", "Jahr", "Gesamt" };
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeTimer();
            try
            {
                DB = new DataContext();
                DB.Database.EnsureDeleted();
                DB.Database.EnsureCreated();
                cb1.ItemsSource = DB.Users.Select(x => x.UserName).ToList();
                cb2.ItemsSource = PrintSelection;
                cb2.SelectedIndex = 4;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void InitializeTimer()
        {
            _running = false;
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(1);
            _timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _elapsedTime = DateTime.Now - _startTime;
            lbltime.Content = _elapsedTime.ToString(@"hh\:mm\:ss\:ff");
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            if (_running)
            {
                _timer.Stop();
                _running = false;
            }
            else
            {
                _startTime = DateTime.Now - _elapsedTime;
                _timer.Start();
                _running = true;
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (_running)
            {
                _startTime = DateTime.Now;
                _timer.Start();
                _running = true;
            }
            else
            {
                _startTime = DateTime.Now - _elapsedTime;
                _timer.Start();
                _running = true;
            }
        }

        private void btnEnd_Click(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
            _elapsedTime = TimeSpan.Zero;
            lbltime.Content = "00:00:00:00";
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            string today = DateTime.Now.ToString("dd.MM.yyyy");
            if (cb1.SelectedItem != null)
            {
                int selection = cb2.SelectedIndex;
                List<string> targetrows = new List<string>();
                switch (selection)
                {
                    case 0:
                        targetrows = DB.DataObjects.Where(x => x.Date == today).Select(x => x.ToString()).ToList();
                        Printobject poDay = new Printobject {Name = cb1.SelectedItem.ToString(), data = targetrows};
                        CreatePDF(poDay);
                        break;
                    case 1:

                        break;
                    case 2:

                        break;
                    case 3:

                        break;
                    case 4:

                        break;
                    default:
                        ShowWhatMessage();
                        break;

                }
            }
            else
            {
                ShowUserSelectionMessage();
            }
        }

        private void CreatePDF(Printobject po)
        {
            throw new NotImplementedException();
        }

        private void ShowWhatMessage()
        {
            MessageBox.Show("What The Hell?!", "Question", MessageBoxButton.OK, MessageBoxImage.Question);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cb1.SelectedItem != null)
            {
                string title = tbtitel.Text;
                string description = tbinfo.Text;
                string date = DateTime.Now.ToString("dd.MM.yyyy");
                string time = lbltime.Content.ToString();
                DateTime parsedTime;
                if (DateTime.TryParseExact(time, "HH:mm:ss:ff", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedTime))
                {
                    DateTime roundedTime = RoundUpToNearestFiveMinutes(parsedTime);
                    string formattedTime = roundedTime.ToString("HH:mm:ss");
                    time = formattedTime;
                    string selectedUsername = cb1.SelectedItem.ToString();
                    int userid = DB.Users.Where(x => x.UserName == selectedUsername).Select(x => x.UserID).FirstOrDefault();
                    DataObjectV2 dummy = new DataObjectV2 { Date = date, Timespan = time, Title = Title, Description = description, UserUserID = userid };
                    DB.DataObjects.Add(dummy);
                    DB.SaveChanges();
                    ReloadGrid();
                }
                else
                {
                    Console.WriteLine("Ungültiges Zeitformat.");
                }


                static DateTime RoundUpToNearestFiveMinutes(DateTime dt)
                {
                    int minutes = dt.Minute;
                    int remainder = minutes % 5;
                    int minutesToAdd = remainder == 0 ? 0 : 5 - remainder;
                    return dt.AddMinutes(minutesToAdd - dt.Second / 60.0 - dt.Millisecond / 60000.0);
                }
            }
            else
            {
                ShowUserSelectionMessage();
            }
        }

        private void cb1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb1.SelectedItem != null)
            {
                ReloadGrid();
            }

        }
        private void ShowUserSelectionMessage()
        {
            MessageBox.Show("Bitte zuerst Benutzer auswählen.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private void ReloadGrid()
        {
            string selectedUsername = cb1.SelectedItem.ToString();
            int userid = DB.Users.Where(x => x.UserName == selectedUsername).Select(x => x.UserID).FirstOrDefault();
            List<DataObjectV2> list = DB.DataObjects.Where(x => x.UserUserID == userid).ToList();
            dt1.ItemsSource = DataViewModel.ConvertDataList(list);
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            InputBox.Visibility = Visibility.Visible;
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            InputBox.Visibility = Visibility.Collapsed;
            string input = InputTextBox.Text;
            User u1 = new User { UserName = input };
            DB.Users.Add(u1);
            DB.SaveChanges();
            cb1.ItemsSource = DB.Users.Select(x => x.UserName).ToList();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            InputBox.Visibility = Visibility.Collapsed;
        }

        private void tbsearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string text = tbsearch.Text;
                ShowSearchResults(text);
            }
        }

        private void ShowSearchResults(string text)
        {
            List<string> rows = new List<string>();

            foreach (object item in dt1.Items)
            {
                DataGridRow row = dt1.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (row != null)
                {
                    string rowText = "";
                    foreach (DataGridColumn column in dt1.Columns)
                    {
                        FrameworkElement cellContent = column.GetCellContent(row);
                        if (cellContent != null)
                        {
                            string cellValue = "";

                            if (cellContent is TextBlock textBlock)
                            {
                                cellValue = textBlock.Text;
                            }
                            else if (cellContent is CheckBox checkBox)
                            {
                                cellValue = checkBox.IsChecked.HasValue ? checkBox.IsChecked.Value.ToString() : "";
                            }
                            else if (cellContent is ComboBox comboBox)
                            {
                                cellValue = comboBox.Text;
                            }
                            rowText += cellValue + ";";
                        }
                    }
                    rows.Add(rowText);
                }
            }

            //Rows durchlaufen um Übereinstimmungen zu finden
            SearchRows(rows, text);

        }

        private void SearchRows(List<string> rows, string goaltext)
        {
            List<string> targetrows = new List<string>();
            foreach (string row in rows)
            {
                if (row.Contains(goaltext))
                {
                    string rownew = row.Replace(";", " ");
                    targetrows.Add(rownew);
                }
            }

            ShowTargetRows(targetrows);
        }

        private void ShowTargetRows(List<string> targetrows)
        {
            string message = "Gefundene Ergebnisse:\n\n";
            foreach (string row in targetrows)
            {
                message += row + "\n";
            }
            MessageBox.Show(message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }


    }
}