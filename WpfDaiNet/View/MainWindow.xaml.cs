using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using WpfDaiNet.ViewModel;

namespace WpfDaiNet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly string ImgFilter = "Image files|*.bmp;*.jpg;*.png;";
        private readonly  MainWindowViewModel _viewModel;
        private readonly Result _resultWindow;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainWindowViewModel();
            _resultWindow = new Result(this);
            DataContext = _viewModel;
            objectsTextbox.PreviewTextInput += new TextCompositionEventHandler(textBox_PreviewTextInput);
            clustersTextbox.PreviewTextInput += new TextCompositionEventHandler(textBox_PreviewTextInput);
            kNumbersTextbox.PreviewTextInput += new TextCompositionEventHandler(textBox_PreviewTextInput);
            limitTextbox.PreviewTextInput += new TextCompositionEventHandler(textBox_PreviewTextInput);
            clonesTextBox.PreviewTextInput += new TextCompositionEventHandler(textBox_PreviewTextInput);
            amplificationTextBox.PreviewTextInput += new TextCompositionEventHandler(textBox_PreviewTextInput);
            mutationCTextBox.PreviewTextInput += new TextCompositionEventHandler(textBox_DoublePreviewTextInput);
            delayTextBox.PreviewTextInput += new TextCompositionEventHandler(textBox_PreviewTextInput);
        }

        void textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0)) e.Handled = true;
        }

        void textBox_DoublePreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = true;
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SetSettings(clusteringRadioBtn.IsChecked == true,
                clustersTextbox.Text,
                kNumbersTextbox.Text, 
                limitTextbox.Text, 
                cloningComboBox.Text,
                mutationComboBox.Text, 
                clonesTextBox.Text,
                amplificationTextBox.Text,
                mutationCTextBox.Text,
                delayTextBox.Text);

            Hide();

            _resultWindow.Show();
        }

        private static string GetResultText(string text, bool isPlus, int maxValue = 100)
        {
            var summand = isPlus ? 1 : -1;

            if (!string.IsNullOrEmpty(text) && int.TryParse(text, out var number))
            {
                var result = number + summand;
                if (result >= 2 && result <= maxValue)
                    return result.ToString();
            }

            return "2";
        }

        private void clustersUpButton_Click(object sender, RoutedEventArgs e)
        {
            clustersTextbox.Text = GetResultText(clustersTextbox.Text, true, 20);
        }

        private void clustersDownButton_Click(object sender, RoutedEventArgs e)
        {
            clustersTextbox.Text = GetResultText(clustersTextbox.Text, false);
        }


        private void kNumbersUpButton_Click(object sender, RoutedEventArgs e)
        {
            kNumbersTextbox.Text = GetResultText(kNumbersTextbox.Text, true);
        }

        private void kNumbersDownButton_Click(object sender, RoutedEventArgs e)
        {
            kNumbersTextbox.Text = GetResultText(kNumbersTextbox.Text, false);
        }

        private void limitUpButton_Click(object sender, RoutedEventArgs e)
        {
            limitTextbox.Text = GetResultText(limitTextbox.Text, true, 1000);
        }

        private void limitDownButton_Click(object sender, RoutedEventArgs e)
        {
            limitTextbox.Text = GetResultText(limitTextbox.Text, false);
        }

        private void objectsUpButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.ObjectsCount = int.Parse(GetResultText(objectsTextbox.Text, true, 5000));
        }

        private void objectsDownButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.ObjectsCount = int.Parse(GetResultText(objectsTextbox.Text, false));
        }

        private void loadButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = ImgFilter;
            if (openFileDialog.ShowDialog() == true)
            {
                _viewModel.ImageSource = openFileDialog.FileName;
            }
        }

        private void clonesUpButton_Click(object sender, RoutedEventArgs e)
        {
            clonesTextBox.Text = GetResultText(clonesTextBox.Text, true);
        }

        private void clonesDownButton_Click(object sender, RoutedEventArgs e)
        {
            clonesTextBox.Text = GetResultText(clonesTextBox.Text, false);
        }

        private void amplificationUpButton_Click(object sender, RoutedEventArgs e)
        {
            amplificationTextBox.Text = GetResultText(amplificationTextBox.Text, true, 10);
        }

        private void amplificationDownButton_Click(object sender, RoutedEventArgs e)
        {
            amplificationTextBox.Text = GetResultText(amplificationTextBox.Text, false);
        }


        private static string GetDoubleResultText(string text, bool isPlus)
        {
            var summand = isPlus ? 0.1 : -0.1;

            if (!string.IsNullOrEmpty(text) && double.TryParse(text, out var number))
            {
                var result = Math.Round(number + summand, 1);
                if (result >= 0.1 && result < 1)
                    return result.ToString();
            }

            return "0,1";
        }

        private void mutationCUpButton_Click(object sender, RoutedEventArgs e)
        {
            mutationCTextBox.Text = GetDoubleResultText(mutationCTextBox.Text, true);
        }
        private void mutationCDownButton_Click(object sender, RoutedEventArgs e)
        {
            mutationCTextBox.Text = GetDoubleResultText(mutationCTextBox.Text, false);
        }

        private void cloningComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = (ComboBox)sender;
            var selectedText = ((ComboBoxItem)comboBox.SelectedItem).Content.ToString();

            if (clonesGrid != null)
            {
                if (selectedText == "Proportional")
                    amplificationGrid.IsEnabled = true;
               else amplificationGrid.IsEnabled = false;

                if (selectedText == "Static")
                    clonesGrid.IsEnabled = true;
                else clonesGrid.IsEnabled = false;
            }
        }

        private void mutationComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = (ComboBox)sender;
            var selectedText = ((ComboBoxItem)comboBox.SelectedItem).Content.ToString();

            if (mutationCGrid != null)
            {
                if (selectedText == "Static")
                    mutationCGrid.IsEnabled = true;
                else mutationCGrid.IsEnabled = false;
            }
        }

        private void delayUpButton_Click(object sender, RoutedEventArgs e)
        {
            delayTextBox.Text = GetResultText(delayTextBox.Text, true, 5000);
        }

        private void delaynDownButton_Click(object sender, RoutedEventArgs e)
        {
            delayTextBox.Text = GetResultText(delayTextBox.Text, false, 5000);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
