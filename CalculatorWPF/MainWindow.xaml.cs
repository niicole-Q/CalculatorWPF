using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace CalculatorWPF
{
    public partial class MainWindow : Window
    {
        private State _state = State.Number;
        private CultureInfo _cultureInfo = CultureInfo.GetCultureInfo("en-En");
        private double _firstOperand = 0;
        private string _currentOperation = "";
        private bool _shouldClearDisplay = true; 

        public MainWindow()
        {
            InitializeComponent();
            ResultTextBox.Content = "0";
        }

        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;
            string digit = button.Content.ToString();

            if (_shouldClearDisplay)
            {
                ResultTextBox.Content = digit;
                _shouldClearDisplay = false;
            }
            else
            {
                if ((string)ResultTextBox.Content == "0")
                {
                    if (digit == "0") return; 
                    ResultTextBox.Content = digit;
                }
                else
                {
                    ResultTextBox.Content += digit;
                }
            }
        }

        private void OperatorButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;
            string newOperation = button.Content.ToString();

            if (!double.TryParse((string)ResultTextBox.Content, NumberStyles.Any, _cultureInfo, out double currentNumber))
            {
                if (_shouldClearDisplay && !string.IsNullOrEmpty(_currentOperation))
                {
                    _currentOperation = newOperation;
                    return;
                }
                if (!double.TryParse("0", NumberStyles.Any, _cultureInfo, out currentNumber))
                    currentNumber = 0;
            }


            if (!string.IsNullOrEmpty(_currentOperation) && !_shouldClearDisplay)
            {
                _firstOperand = PerformOperation(_firstOperand, currentNumber, _currentOperation);
                ResultTextBox.Content = _firstOperand.ToString(_cultureInfo);
            }
            else 
            {
                _firstOperand = currentNumber;
            }

            _currentOperation = newOperation; 
            _shouldClearDisplay = true;     
        }

        private void EqualsButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_currentOperation))
            {
                return;
            }

            if (!double.TryParse((string)ResultTextBox.Content, NumberStyles.Any, _cultureInfo, out double secondNumber))
            {
                return;
            }

            if (_shouldClearDisplay)
            {
                secondNumber = _firstOperand;
            }

            double result = PerformOperation(_firstOperand, secondNumber, _currentOperation);
            ResultTextBox.Content = result.ToString(_cultureInfo);

            _firstOperand = result;
            _currentOperation = "";      
            _shouldClearDisplay = true;  
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            _firstOperand = 0;
            _currentOperation = "";
            ResultTextBox.Content = "0";
            _shouldClearDisplay = true;
        }

        private double PerformOperation(double num1, double num2, string op)
        {
            double result = 0;
            switch (op)
            {
                case "+": result = num1 + num2; break;
                case "-": result = num1 - num2; break;
                case "*": result = num1 * num2; break;
                case "/":
                    if (num2 == 0)
                    {
                        return double.NaN;
                    }
                    result = num1 / num2;
                    break;
            }
            return result;
        }

        private void DotButton_Click(object sender, RoutedEventArgs e)
        {

            if (_shouldClearDisplay)
            {
                ResultTextBox.Content = "0.";
                _shouldClearDisplay = false;
            }
            else
            {
                if (!((string)ResultTextBox.Content).Contains("."))
                {
                    ResultTextBox.Content += ".";
                }
            }
        }

        private void ToggleSignButton_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse((string)ResultTextBox.Content, NumberStyles.Any, _cultureInfo, out double number))
            {
                number = -number;
                ResultTextBox.Content = number.ToString(_cultureInfo);
            }
        }
    }
}