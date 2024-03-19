using System.Net.Http;
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

namespace SZGYA_WPF_Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int last;
        private int current;
        private string currentStr = "";
        private string secondaryStr = "";
        private string op = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void numberBtnClick(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            updateDisplay(b.Content.ToString());
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            updateDisplay(string.Empty);
            updateSecondaryDisplay(string.Empty);
        }

        private void opHandler(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            if (op == string.Empty)
            {
                op = b.Content.ToString();
                last = current;
                updateDisplay(string.Empty);
                updateSecondaryDisplay($"{last} {op}");
            }
            else
            {
                op = b.Content.ToString();
                updateSecondaryDisplay($"{last} {op}");
            }
        }

        private void calculateResult(object sender, RoutedEventArgs e)
        {
            updateSecondaryDisplay($"{last} {op} {current}");
            switch (op)
            {
                case "+":
                    last = last + current;
                    updateDisplay($"{last}", true);
                    op = string.Empty;
                    break;
                case "-":
                    last = last - current;
                    updateDisplay($"{last}", true);
                    op = string.Empty;
                    break;
                case "X":
                    last = last * current;
                    updateDisplay($"{last}", true);
                    op = string.Empty;
                    break;
                case "/":
                    last = last / current;
                    updateDisplay($"{last}", true);
                    op = string.Empty;
                    break;
                default:
                    break;
            }
        }

        private void updateDisplay(string content, bool clear = false)
        {
            if (clear || content == string.Empty) currentStr = string.Empty;
            currentStr += content;
            current = currentStr != string.Empty ? int.Parse(currentStr) : 0;
            lblCurrent.Content = currentStr;
        }

        private void updateSecondaryDisplay(string content)
        {
            secondaryStr = content;
            lblSecondary.Content = secondaryStr;
        }
    }
}