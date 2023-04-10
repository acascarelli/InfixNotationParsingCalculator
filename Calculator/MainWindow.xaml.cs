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

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private char[] operators = { '+', '-', '*', '/', '{', '}' };

        public MainWindow()
        {
            InitializeComponent();

            resultLabel.Content = "0";

            acButton.Click += AcButton_Click;
            negativeButton.Click += NegativeButton_Click;
            percentageButton.Click += PercentageButton_Click;
            equalButton.Click += EqualButton_Click;
        }

        private void EqualButton_Click(object sender, RoutedEventArgs e)
        {
            

        }

        private void PercentageButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NegativeButton_Click(object sender, RoutedEventArgs e)
        {
            char firstToken = resultLabel.Content.ToString().First();

            if (resultLabel.Content.ToString() == "0")
                return;

            if (operators.Any(c => firstToken.Equals(c)))
                return;

            resultLabel.Content = $"-{resultLabel.Content}";

        }

        private void AcButton_Click(object sender, RoutedEventArgs e)
        {
            resultLabel.Content = "0";
        }

        private void OperationButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedOperation = (sender as Button).Content.ToString();

            char lastToken = resultLabel.Content.ToString().Last();

            if (operators.Any(c => lastToken.Equals(c)))
                return;
            else
                resultLabel.Content = $"{resultLabel.Content}{selectedOperation}";
        }

        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {
            int selectedValue = int.Parse((sender as Button).Content.ToString());

            if (resultLabel.Content.ToString() == "0")
                resultLabel.Content = selectedValue;
            else
                resultLabel.Content = $"{resultLabel.Content}{selectedValue}";
        }

        private void decimalButton_Click(object sender, RoutedEventArgs e)
        {
            if (resultLabel.Content.ToString().Contains("."))
                return;
            resultLabel.Content = $"{resultLabel.Content}.";
        }

    }
    
}
