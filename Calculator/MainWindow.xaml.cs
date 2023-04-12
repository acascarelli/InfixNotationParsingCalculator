using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Calculator.Tokens;
using Calculator.Parsers;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {

        private readonly char[] _operators = { '+', '-', '*', '/', '{', '}' };
        private readonly Tokenizer _tokenizer = new Tokenizer();
        private readonly ShuntingYardAlgorithm _algorithm = new ShuntingYardAlgorithm();
        private readonly PostfixNotationCalculator _calculator = new PostfixNotationCalculator();

        public MainWindow()
        {
            InitializeComponent();

            resultLabel.Text = "0";

            acButton.Click += AcButton_Click;
            negativeButton.Click += NegativeButton_Click;
            equalButton.Click += EqualButton_Click;
        }

        private void EqualButton_Click(object sender, RoutedEventArgs e)
        {
            string raw = resultLabel.Text.ToString();
            var infixNotationTokens = _tokenizer.Parse(raw);
            var postfixNotationTokens = _algorithm.Apply(infixNotationTokens);
            var result = _calculator.Calculate(postfixNotationTokens).Value;

            resultLabel.Text = $"{result}";
        }

        private void NegativeButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void AcButton_Click(object sender, RoutedEventArgs e)
        {
            resultLabel.Text = "0";
        }

        private void OperationButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedOperation = (sender as Button).Content.ToString();

            char lastToken = resultLabel.Text.ToString().Last();

            if (_operators.Any(c => lastToken.Equals(c)))
                return;
            else
                resultLabel.Text = $"{resultLabel.Text}{selectedOperation}";
        }

        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedValue = (sender as Button).Content.ToString();

            if (resultLabel.Text.ToString() == "0")
                resultLabel.Text = selectedValue.ToString();
            else
                resultLabel.Text = $"{resultLabel.Text}{selectedValue}";
        }

        private void decimalButton_Click(object sender, RoutedEventArgs e)
        {
            char lastToken = resultLabel.Text.ToString().Last();
            if (lastToken.Equals('.'))
            {
                return;
            }            
            else if (_operators.Any(c => lastToken.Equals(c)))
            {
                resultLabel.Text = $"{resultLabel.Text}0.";
            }
            else
                resultLabel.Text = $"{resultLabel.Text}.";
        }        

    }

}
