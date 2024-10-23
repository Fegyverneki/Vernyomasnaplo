using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Vernyomasnaplo
{
    public partial class MainWindow : Window
    {
        private List<BloodPressureEntry> entries = new List<BloodPressureEntry>();

        public MainWindow()
        {
            InitializeComponent();
            DataGrid.ItemsSource = entries;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(SystoleTextBox.Text, out int systole) &&
                int.TryParse(DiastoleTextBox.Text, out int diastole) &&
                int.TryParse(PulseTextBox.Text, out int pulse))
            {
                var entry = new BloodPressureEntry
                {
                    Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    Systole = systole,
                    Diastole = diastole,
                    Pulse = pulse,
                    Status = GetBloodPressureStatus(systole, diastole)
                };

                entries.Add(entry);
                DataGrid.Items.Refresh();
                ClearInputFields();
            }
            else
            {
                MessageBox.Show("Érvényes szám adatot adjon meg.");
            }
        }

        private string GetBloodPressureStatus(int systole, int diastole)
        {
            if (systole < 90 || diastole < 60) return "Alacsony";
            if (systole < 120 && diastole < 80) return "Normál";
            if (systole < 130 && diastole < 80) return "Prehypertension";
            if (systole < 140 || diastole < 90) return "1 Hypertension";
            return "2 Hypertension";
        }

        private void ClearInputFields()
        {
            SystoleTextBox.Clear();
            DiastoleTextBox.Clear();
            PulseTextBox.Clear();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                Title = "Adatok mentése szöveges fájlba",
                FileName = UserNameTextBox.Text
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                {
                    foreach (var entry in entries)
                    {
                        writer.WriteLine($"{entry.Date},{entry.Systole},{entry.Diastole},{entry.Pulse},{entry.Status}");
                    }
                }
                MessageBox.Show("Adatok sikeresen mentve!");
            }
        }
        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                Title = "Adatok betöltése szöveges fájlból"
            };

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                string filePath = openFileDialog.FileName;

                if (File.Exists(filePath))
                {
                    entries.Clear();
                    var lines = File.ReadAllLines(filePath);
                    foreach (var line in lines)
                    {
                        var parts = line.Split(',');
                        if (parts.Length == 5)
                        {
                            entries.Add(new BloodPressureEntry
                            {
                                Date = parts[0],
                                Systole = int.Parse(parts[1]),
                                Diastole = int.Parse(parts[2]),
                                Pulse = int.Parse(parts[3]),
                                Status = parts[4]
                            });
                        }
                    }
                    DataGrid.Items.Refresh();
                    MessageBox.Show("Adatok sikeresen betöltve!");
                }
                else
                {
                    MessageBox.Show("Nem található mentett adat.");
                }
            }
        }
        // TextBox Default strings displayed in gray
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Set default placeholder text
            UserNameTextBox.Tag = "Név";
            SystoleTextBox.Tag = "Szisztolé";
            DiastoleTextBox.Tag = "Diasztolé";
            PulseTextBox.Tag = "Impulzus";
            // Set initial text and color
            UserNameTextBox.Text = UserNameTextBox.Tag.ToString();
            SystoleTextBox.Text = SystoleTextBox.Tag.ToString();
            DiastoleTextBox.Text = DiastoleTextBox.Tag.ToString();
            PulseTextBox.Text = PulseTextBox.Tag.ToString();
            UserNameTextBox.Foreground = Brushes.Gray;
            SystoleTextBox.Foreground = Brushes.Gray;
            DiastoleTextBox.Foreground = Brushes.Gray;
            PulseTextBox.Foreground = Brushes.Gray;
        }
        private void UserNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (UserNameTextBox != null && string.IsNullOrWhiteSpace(UserNameTextBox.Text))
            {
                UserNameTextBox.Text = UserNameTextBox.Tag.ToString();
                UserNameTextBox.Foreground = Brushes.Gray; // Placeholder color
            }
        }

        private void UserNameTextBox_GotFocus_1(object sender, RoutedEventArgs e)
        {
            if (UserNameTextBox != null && UserNameTextBox.Text == UserNameTextBox.Tag.ToString())
            {
                UserNameTextBox.Text = "";
                UserNameTextBox.Foreground = Brushes.Black;
            }
        }

        private void SystoleTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SystoleTextBox != null && SystoleTextBox.Text == SystoleTextBox.Tag.ToString())
            {
                SystoleTextBox.Text = "";
                SystoleTextBox.Foreground = Brushes.Black;
            }
        }

        private void SystoleTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (SystoleTextBox != null && string.IsNullOrWhiteSpace(SystoleTextBox.Text))
            {
                SystoleTextBox.Text = SystoleTextBox.Tag.ToString();
                SystoleTextBox.Foreground = Brushes.Gray; // Placeholder color
            }
        }

        private void DiastoleTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (DiastoleTextBox != null && DiastoleTextBox.Text == DiastoleTextBox.Tag.ToString())
            {
                DiastoleTextBox.Text = "";
                DiastoleTextBox.Foreground = Brushes.Black;
            }
        }

        private void DiastoleTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (DiastoleTextBox != null && string.IsNullOrWhiteSpace(DiastoleTextBox.Text))
            {
                DiastoleTextBox.Text = DiastoleTextBox.Tag.ToString();
                DiastoleTextBox.Foreground = Brushes.Gray; // Placeholder color
            }
        }

        private void PulseTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (PulseTextBox != null && PulseTextBox.Text == PulseTextBox.Tag.ToString())
            {
                PulseTextBox.Text = "";
                PulseTextBox.Foreground = Brushes.Black;
            }
        }

        private void PulseTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PulseTextBox != null && string.IsNullOrWhiteSpace(PulseTextBox.Text))
            {
                PulseTextBox.Text = PulseTextBox.Tag.ToString();
                PulseTextBox.Foreground = Brushes.Gray; // Placeholder color
            }
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }

    public class BloodPressureEntry
    {
        public string Date { get; set; }
        public int Systole { get; set; }
        public int Diastole { get; set; }
        public int Pulse { get; set; }
        public string Status { get; set; }
    }


}
