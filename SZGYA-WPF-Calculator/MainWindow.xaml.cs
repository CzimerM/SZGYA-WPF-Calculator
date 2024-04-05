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
using static System.Net.Mime.MediaTypeNames;

namespace SZGYA_WPF_Calculator
{
    /// <summary>
    /// A számológép képes 2 bármely double típusba beleférő számon a gombok által elérhető műveleteket végrehajtani.
    /// - Az alkalmazás reszponzív. 
    /// - Az eredmény és a számítás két külön mezőben jelennek meg.
    /// - A billentyűzet numpad gombjai is használhatóak.
    /// </summary>
    public partial class MainWindow : Window
    {
        private double last = 0;
        private double current = 0;
        private string currentStr = "";
        private string secondaryStr = "";
        private string op = "";

        public MainWindow()
        {
            InitializeComponent();
            updateDisplay("0");
            updateSecondaryDisplay(string.Empty);
            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);
        }

        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString().Last() >= '0' && e.Key.ToString().Last() <= '9')
            {
                string numString = $"{e.Key.ToString().Last()}";
                updateNum(numString);
            }
            else
            {
                switch (e.Key.ToString())
                {
                    case "Back":
                        backspace();
                        break;
                    case "Add":
                        opHandler("+");
                        break;
                    case "Subtract":
                        opHandler("-");
                        break;
                    case "Multiply":
                        opHandler("X");
                        break;
                    case "Divide":
                        opHandler("/");
                        break;
                    case "Decimal":
                        updateNum(",");
                        break;
                    case "Return":
                        calculateResult();
                        break;
                    default: break;
                }
            }
        }

        private void numberBtnClick(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            updateNum(b.Content.ToString());

        }

        void updateNum(string num)
        {
            if (num == "," && current == 0) updateDisplay(num);
            else updateDisplay(num, current == 0);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            updateDisplay("0", true);
            updateSecondaryDisplay(string.Empty);
        }

        private void btnDeletePartial_Click(object sender, RoutedEventArgs e)
        {
            updateDisplay("0", true);
        }

        private void opHandlerBtn(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            opHandler(b.Content.ToString());
        }

        private void opHandler(string newOp)
        {
            if (op == string.Empty)
            {
                op = newOp;
                last = current;
                if (op == "x²")
                {
                    updateSecondaryDisplay($"{last}²");
                    current = Math.Pow(current, 2);
                    updateDisplay($"{current}", true);
                    op = string.Empty;
                }
                else if (op == "√x")
                {
                    updateSecondaryDisplay($"√{current}");
                    current = Math.Sqrt(current);
                    updateDisplay($"{current}", true);
                    op = string.Empty;
                }
                else if (op == "±")
                {
                    updateSecondaryDisplay($"negate({current})");
                    current = -current;
                    updateDisplay($"{current}", true);
                    op = string.Empty;
                }
                else if (op == "⅟x")
                {
                    updateSecondaryDisplay($"⅟({current})");
                    if (current == 0)
                    {
                        lblCurrent.Content = "Nullával nem lehet osztani";
                        op = string.Empty;
                    }
                    else
                    {
                        current = 1 / current;
                        updateDisplay($"{current}", true);
                        op = string.Empty;
                    }
                }
                else
                {
                    updateDisplay(string.Empty);
                    updateSecondaryDisplay($"{last} {op}");
                }
            }
            else
            {
                op = newOp;
                updateSecondaryDisplay($"{last} {op}");
            }
        }

        private void btnEgyenlo_Click(object sender, RoutedEventArgs e)
        {
            calculateResult();
        }

        private void calculateResult()
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
                    if (current == 0)
                    {
                        lblCurrent.Content = "Nullával nem lehet osztani";
                        op = string.Empty;
                        break;
                    }
                    last = last / current;
                    updateDisplay($"{last}", true);
                    op = string.Empty;
                    break;
                case "^":
                    last = Math.Pow(last, current);
                    updateDisplay($"{last}", true);
                    op = string.Empty;
                    break;
                default:
                    updateSecondaryDisplay($"{current}");
                    break;
            }
        }

        private void updateDisplay(string content, bool clear = false)
        {
            if (clear || content == string.Empty) currentStr = string.Empty;
            currentStr += content;
            current = currentStr != string.Empty ? double.Parse(currentStr) : 0;
            lblCurrent.Content = currentStr;
        }

        private void updateSecondaryDisplay(string content)
        {
            secondaryStr = content;
            lblSecondary.Content = secondaryStr;
        }

        private void chkTudomanyos_Checked(object sender, RoutedEventArgs e)
        {
            gridCalc.RowDefinitions[0].Height = new GridLength(1, GridUnitType.Star);
        }

        private void chkTudomanyosUnchecked(object sender, RoutedEventArgs e)
        {
            gridCalc.RowDefinitions[0].Height = new GridLength(0, GridUnitType.Pixel);
        }

        private void btnBackspace_Click(object sender, RoutedEventArgs e)
        {
            backspace();
        }

        private void backspace()
        {
            currentStr = String.Concat(currentStr.Take(currentStr.Length - 1));
            if (currentStr == "") currentStr = "0";
            updateDisplay(currentStr, true);
        }
    }
}