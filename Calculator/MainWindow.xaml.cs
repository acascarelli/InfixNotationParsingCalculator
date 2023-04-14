using Calculator.Parsers;
using Calculator.Tokens;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {

        private readonly char[] _operators = { '+', '-', '*', '/'};
        private readonly char[] _brackets = { '(', ')'};
        private readonly char negativeSign = '\u02C9';
        private readonly Tokenizer _tokenizer = new Tokenizer();
        private readonly ShuntingYardAlgorithm _algorithm = new ShuntingYardAlgorithm();
        private readonly PostfixNotationCalculator _calculator = new PostfixNotationCalculator();

        public MainWindow()
        {
            InitializeComponent();

            resultLabel.Text = "0";

            acButton.Click += AcButton_Click;
            negativeButton.Click += negativeButton_Click;
            equalButton.Click += EqualButton_Click;
            openingBracketButton.Click += OpeningBracketButton_Click;
            closingBracketButton.Click += ClosingBracketButton_Click;
        }

        private void EqualButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string raw = resultLabel.Text.ToString();
                var infixNotationTokens = _tokenizer.Parse(raw);
                var postfixNotationTokens = _algorithm.Apply(infixNotationTokens);
                var result = _calculator.Calculate(postfixNotationTokens).Value;

                resultLabel.Text = $"{result}";
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Invalid expression entered.", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                resultLabel.Text = "0";
            }
        }

        private void negativeButton_Click(object sender, RoutedEventArgs e)
        {
            int lastNegativeSign = resultLabel.Text.LastIndexOf(negativeSign);
            int LastOperator = resultLabel.Text.LastIndexOfAny(_operators);
            int LastBracket = resultLabel.Text.LastIndexOfAny(_brackets);

            if (LastOperator < 0 && LastBracket < 0)
            {
                if (lastNegativeSign >= 0)
                    resultLabel.Text = resultLabel.Text.Trim(negativeSign);
                else
                    resultLabel.Text = $"{negativeSign}{resultLabel.Text}";
            }
            else
            {
                string unmodifiedTokens = resultLabel.Text.Substring(0, LastOperator + 1);
                string lastOperand = resultLabel.Text.Substring(LastOperator + 1, resultLabel.Text.Length - LastOperator - 1);

                if (lastOperand.Contains(negativeSign))
                    resultLabel.Text = $"{unmodifiedTokens}{lastOperand.Trim(negativeSign)}";
                else
                    resultLabel.Text = $"{unmodifiedTokens}{negativeSign}{lastOperand}";
            }
        }

        private void AcButton_Click(object sender, RoutedEventArgs e)
        {
            resultLabel.Text = "0";
        }

        private void ClosingBracketButton_Click(object sender, RoutedEventArgs e)
        {
            if (resultLabel.Text.ToString() == "0")
                resultLabel.Text = ")";
            else
                resultLabel.Text = $"{resultLabel.Text})";
        }

        private void OpeningBracketButton_Click(object sender, RoutedEventArgs e)
        {
            if (resultLabel.Text.ToString() == "0")
                resultLabel.Text = "(";
            else
                resultLabel.Text = $"{resultLabel.Text}(";
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
