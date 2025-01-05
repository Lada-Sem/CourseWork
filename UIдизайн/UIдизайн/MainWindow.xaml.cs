using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Reflection.Emit;
using NCalc;
using Expression = NCalc.Expression;
using System.Data;
using System.Diagnostics;
using FontWeights = OxyPlot.FontWeights;
using OxyPlot.Annotations;
using MathNet.Symbolics;
using MathNet.Numerics;
using MathNet.Numerics.RootFinding;

namespace UIдизайн
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        private Stack<ApplicationState> undoStack = new Stack<ApplicationState>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShowCalculator_Click(object sender, RoutedEventArgs e)
        {
            CalculatorPanel.Visibility = CalculatorPanel.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public PlotModel GraphModel { get; set; }

        private PlotModel CreatePlotModel()
        {
            string function = FunctionTextBox.Text.Trim();
            var plotModel = new PlotModel();

            var horizontalAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = -20,
                Maximum = 20,
                AbsoluteMaximum = 20,
                AbsoluteMinimum = -20,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Solid,
                Title = "X",
                TitleFontSize = 14,
                TitleFontWeight = FontWeights.Bold,
            };

            var verticalAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Minimum = -20,
                Maximum = 20,
                AbsoluteMaximum = 20,
                AbsoluteMinimum = -20,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Solid,
                Title = "Y",
                TitleFontSize = 14,
                TitleFontWeight = FontWeights.Bold,
            };

            plotModel.Axes.Add(horizontalAxis);
            plotModel.Axes.Add(verticalAxis);

            var xAxisLine = new LineSeries
            {
                Color = OxyColors.Red,
                StrokeThickness = 2
            };

            xAxisLine.Points.Add(new OxyPlot.DataPoint(-20, 0));
            xAxisLine.Points.Add(new OxyPlot.DataPoint(20, 0));

            var yAxisLine = new LineSeries
            {
                Color = OxyColors.Red,
                StrokeThickness = 2
            };

            yAxisLine.Points.Add(new OxyPlot.DataPoint(0, -20));
            yAxisLine.Points.Add(new OxyPlot.DataPoint(0, 20));

            plotModel.Series.Add(xAxisLine);
            plotModel.Series.Add(yAxisLine);


            

            var series1 = new LineSeries();
            series1.Points.Clear();

            for (double i = -20; i <= 20; i += 0.1)
            { double yValue = G(FunctionTextBox.Text, i); 
                if (yValue < -20 || yValue > 20) 
                { continue; }
                series1.Points.Add(new OxyPlot.DataPoint(i, yValue)); 
            }
            

        plotModel.Series.Add(series1);

            return plotModel;
        }

        static void Opn(ref string[] a, string arg)               
        {
            Stack<string> s = new Stack<string>();
            int j = 0;
            for (int i = 0; i < arg.Length; i++) 
            {
                double num;
                bool isNum = double.TryParse(arg[i].ToString(), out num); 
                if (isNum)
                {
                    a[j] = X10(ref i, arg).ToString();
                    j++;
                    continue;
                }
                if (arg[i] == '!')
                {
                    a[j] = "!";
                    j++;
                    continue;
                }
                if (arg[i] == 'e')
                {
                    a[j] = "e";
                    j++;
                    continue;
                }
                if (arg[i] == 'x')
                {
                    a[j] = "x";
                    j++;
                    continue;
                }
                if (arg[i] == 'p')
                {
                    i++;
                    if (arg[i] == 'i')
                    {
                        a[j] = "pi";
                        j++;
                        continue;
                    }
                }                
                if (arg[i] == '(') s.Push("(");
                if (arg[i] == ')')
                {
                    while (s.Peek() != "(")
                    {
                        a[j] = s.Pop().ToString();
                        j++;
                    }
                    s.Pop();
                    continue;
                }
                if (arg[i] == '+' || arg[i] == '-' || arg[i] == '*' || arg[i] == '/' || arg[i] == '^')
                {
                    try
                    {
                        while (Pr(arg[i]) <= Pr(s.Peek()))
                        {
                            a[j] = s.Pop().ToString();
                            j++;
                        }
                    }
                    catch { }
                    s.Push(arg[i].ToString());
                }
            }
            while (s.Count > 0) { a[j] = s.Pop().ToString(); j++; }
        }
        static double G(string y, double x = 0)                   
        {
            string[] g1 = new string[y.Length];
            Opn(ref g1, y);
            try { return Opn_res(g1, x); } catch { return 0; }
        }

        static double Pr(char x)                                    
        {
            if (x == '+') return 1;
            if (x == '-') return 1;
            if (x == '*') return 2;
            if (x == '/') return 2;
            if (x == '^') return 3;
            return 0;
        }
        static double Pr(string x)                                  
        {
            switch (x)
            {
                case "+": return 1;
                case "-": return 1;
                case "*": return 2;
                case "/": return 2;
                case "^": return 3;
                
                default: return 0;
            }
        }
        static double X10(ref int i, string arg)                   
        {
            double o;
            double k = Convert.ToDouble(arg[i].ToString());
            if (i + 1 < arg.Length && double.TryParse(arg[i + 1].ToString(), out o))
                while (double.TryParse(arg[i + 1].ToString(), out o))
                {
                    k = k * 10 + Convert.ToDouble(arg[i + 1].ToString());
                    i++;
                    if (i + 1 == arg.Length) break;
                }
            if (i + 1 < arg.Length && arg[i + 1] == '.')
            {
                i++;
                if (i + 1 < arg.Length && double.TryParse(arg[i + 1].ToString(), out o))
                {
                    int b = 1;
                    while (double.TryParse(arg[i + 1].ToString(), out o))
                    {
                        k = k + Convert.ToDouble(arg[i + 1].ToString()) / Math.Pow(10, b);
                        i++;
                        if (i + 1 == arg.Length) break;
                        b++;
                    }
                }
            }
            return k;
        }
       
        static double Opn_res(string[] a, string x = "0")          
        {
            return Opn_res(a, G(x, 0));
        }

        static double Opn_res(string[] a, double x = 0)           
        {
            Stack<double> st = new Stack<double>();
            for (int i = 0; i < a.Length; i++)
            {
                double num;
                bool isNum = double.TryParse(a[i], out num);
                if (isNum)
                    st.Push(num);
                else
                {
                    double op2;
                    switch (a[i])
                    {
                        case "e":
                            st.Push(Math.E);
                            break;
                        case "x":
                            st.Push(x);
                            break;
                        case "pi":
                            st.Push(Math.PI);
                            break;
                        case "+":
                            st.Push(st.Pop() + st.Pop());
                            break;
                        case "*":
                            st.Push(st.Pop() * st.Pop());
                            break;
                        case "-":
                            op2 = st.Pop();
                            if (st.Count != 0) st.Push(st.Pop() - op2);
                            else st.Push(0 - op2);
                            break;
                        case "/":
                            op2 = st.Pop();
                            if (op2 != 0.0)
                                st.Push(st.Pop() / op2);
                            else
                                MessageBox.Show("Ошибка. Деление на ноль");
                            break;
                        case "^":
                            op2 = st.Pop();
                            st.Push(Math.Pow(st.Pop(), op2));
                            break;
                        
                    }
                }
            }
            return st.Pop();
        }
       
        //ввод функции через созданную клавиатуру
        private void AppendText(string text)
        {
            FunctionTextBox.Text += text;
        }

        private void OneButton_Click(object sender, RoutedEventArgs e)
        {
            AppendText("1");
        }

        private void TwoButton_Click(object sender, RoutedEventArgs e)
        {
            AppendText("2");
        }

        private void ThreeButton_Click(object sender, RoutedEventArgs e)
        {
            AppendText("3");
        }

        private void FourButton_Click(object sender, RoutedEventArgs e)
        {
            AppendText("4");
        }

        private void FiveButton_Click(object sender, RoutedEventArgs e)
        {
            AppendText("5");
        }

        private void SixButton_Click(object sender, RoutedEventArgs e)
        {
            AppendText("6");
        }

        private void SevenButton_Click(object sender, RoutedEventArgs e)
        {
            AppendText("7");
        }

        private void EightButton_Click(object sender, RoutedEventArgs e)
        {
            AppendText("8");
        }

        private void NineButton_Click(object sender, RoutedEventArgs e)
        {
            AppendText("9");
        }

        private void ZeroButton_Click(object sender, RoutedEventArgs e)
        {
            AppendText("0");
        }

        private void OperatorButton_Click(object sender, RoutedEventArgs e)
        {
            AppendText(".");
        }

        private void EqualsButton_Click(object sender, RoutedEventArgs e)
        {
            AppendText("=");
        }

        private void PlusButton_Click(object sender, RoutedEventArgs e)
        {
            AppendText("+");
        }

        private void MinusButton_Click(object sender, RoutedEventArgs e)
        {
            AppendText("-");
        }

        private void MultiplyButton_Click(object sender, RoutedEventArgs e)
        {
            AppendText("*");
        }

        private void DivideButton_Click(object sender, RoutedEventArgs e)
        {
            AppendText("/");
        }

        private void FirstBracketButton_Click(object sender, RoutedEventArgs e)
        {
            AppendText("(");
        }

        private void SecondBracketButton_Click(object sender, RoutedEventArgs e)
        {
            AppendText(")");
        }

        private void DegreeButton_Click(object sender, RoutedEventArgs e)
        {
            AppendText("^");
        }

        private void X_Button_Click(object sender, RoutedEventArgs e)
        {
            AppendText("x");
        }

        private void Y_Button_Click(object sender, RoutedEventArgs e)
        {
            AppendText("y");
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(FunctionTextBox.Text))
            {
                FunctionTextBox.Text = FunctionTextBox.Text.Substring(0, FunctionTextBox.Text.Length - 1);
            }
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            string function = FunctionTextBox.Text.Trim();

            if (!IsValidFunction(function))
            {
                ClearAllButton_Click(null,null);
                return;
            }
            string[] a = new string[function.Length];
            Opn(ref a, function);
            
            string domain = FindDomain(function);
            string chislo = FunctiaChotn(function);
            chetnostLabel.Content = chislo;
            DomainOutputLabel.Content = domain;

            string result = FindIntersections(function);
            StepsOutput.Text = result;


            var variable = SymbolicExpression.Variable("x");
            var proiz = SymbolicExpression.Parse(function);

            var derivative = proiz.Differentiate(variable);
            Proizvodnay.Content = $"({function})' = {derivative}";

            SolveDerivativeEquation(derivative, function);
        }
        
        private bool IsValidFunction(string function)
        {
            function = function.Replace(" ", "");

            if (!Regex.IsMatch(function, @"^[\d\+\-\*\/\.\^x()]+$"))
            {
                MessageBox.Show("Ошибка: Ввод содержит недопустимые символы."); return false;
            }

            if (string.IsNullOrEmpty(function))
            {
                MessageBox.Show("Ошибка: Ввод пуст.");
                return false;
            }

            if (function.Contains("/0"))
            {
                MessageBox.Show("Ошибка: Деление на ноль.");
                return false;
            }
            if (!function.Contains('x'))
            {
                MessageBox.Show("Ошибка: Функция должна содержать переменную 'x'.");
                return false;
            }

            var linearMatch = Regex.IsMatch(function, @"^[-+]?(\d*\.?\d*)?[*]?x([-+]\d+)?$");
            var quadraticMatch = Regex.IsMatch(function, @"^([-+]?\d*\.?\d*\*?x\^2)?([-+]?\d*\.?\d*\*?x)?([-+]?\d*\.?\d*)?$");
            var powerMatch = Regex.IsMatch(function, @"^[-+]?\d*\.?\d*[\+\-\*\/]?x?[\^]?[\d]*$");
            var hyperbolaMatch = Regex.IsMatch(function, @"^[-+]?(\d*\.?\d*)?\/x$");

            if (!(linearMatch || quadraticMatch || powerMatch || hyperbolaMatch))
            {
                MessageBox.Show("Функция должна быть линейной, квадратичной или степенной.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void SolveDerivativeEquation(SymbolicExpression derivative, string function)
        {
            Func<double, double> derivativeFunction = x => derivative.Evaluate(new Dictionary<string, FloatingPoint> { { "x", x } }).RealValue;
            Func<double, double> originalFunctionDelegate = x => SymbolicExpression.Parse(function).Evaluate(new Dictionary<string, FloatingPoint> { { "x", x } }).RealValue;

            double lowerBound = -20;
            double upperBound = 20;

            var roots = FindRoots(derivativeFunction, lowerBound, upperBound);

            if (roots.Length > 0)
            {
                SolutionsOutputLabel.Content = "Найденные корни: ";
                foreach (var root in roots)
                {
                    SolutionsOutputLabel.Content += $"x ≈ {root:F2}, ";
                }
                SolutionsOutputLabel.Content = ((string)SolutionsOutputLabel.Content).TrimEnd(',', ' ');
            }
            else
            {
                SolutionsOutputLabel.Content = "Корней нет.";
            }

            List<FunctionInfo> functionInfos = new List<FunctionInfo>();

            if (roots.Length > 0)
            {
                double previousRoot = lowerBound;

                functionInfos.Add(new FunctionInfo { Interval = $"(-∞, {roots[0]:F2})", Behavior = GetBehavior(derivativeFunction, previousRoot, roots[0]) });

                for (int i = 0; i < roots.Length; i++)
                {
                    if (i > 0)
                    {
                        functionInfos.Add(new FunctionInfo { Interval = $"({roots[i - 1]:F2}, {roots[i]:F2})", Behavior = GetBehavior(derivativeFunction, roots[i - 1], roots[i]) });
                    }

                    previousRoot = roots[i];
                }

                functionInfos.Add(new FunctionInfo { Interval = $"({roots[roots.Length - 1]:F2}, +∞)", Behavior = GetBehavior(derivativeFunction, previousRoot, upperBound) });
            }
            else
            {
                functionInfos.Add(new FunctionInfo { Interval = "Нет корней", Behavior = "Не определено" });
            }

            MonoDataGrid.ItemsSource = functionInfos;

            CalculateExtrema(originalFunctionDelegate, roots);
        }

        private void CalculateExtrema(Func<double, double> originalFunctionDelegate, double[] roots)
        {
            double? minValue = null;
            double? maxValue = null;
            double lowerBound = -20;
            double upperBound = 20;

            double valueAtLowerBound = originalFunctionDelegate(lowerBound);
            double valueAtUpperBound = originalFunctionDelegate(upperBound);

            minValue = Math.Min(valueAtLowerBound, valueAtUpperBound);
            maxValue = Math.Max(valueAtLowerBound, valueAtUpperBound);

            foreach (var root in roots)
            {
                double valueAtRoot = originalFunctionDelegate(root);

                if (valueAtRoot < minValue)
                {
                    minValue = valueAtRoot;
                    MinValueLabel.Content = $"{minValue:F2}";
                }

                if (valueAtRoot > maxValue)
                {
                    maxValue = valueAtRoot;
                    MaxValueLabel.Content = $"{maxValue:F2}";
                }
            }

            if (roots.Length == 0)
            {
                MinValueLabel.Content = "отсутствует";
                MaxValueLabel.Content = "отсутствует";
            }
        }

        private string GetBehavior(Func<double, double> function, double start, double end)
        {
            double midPoint = (start + end) / 2;

            if (function(midPoint) < 0)
                return "↓"; 
            else if (function(midPoint) > 0)
                return "↑"; 

            return "";
        }

        private double[] FindRoots(Func<double, double> function, double lowerBound, double upperBound)
        {
            List<double> roots = new List<double>();

            double stepSize = 0.01; 
            for (double x = lowerBound; x < upperBound; x += stepSize)
            {
                if (function(x) * function(x + stepSize) < 0)
                {
                    try
                    {
                        double root = Brent.FindRoot(function, x, x + stepSize);
                        roots.Add(root);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка при нахождении корня: {ex.Message}");
                    }
                }
            }

            return roots.ToArray();
        }

        private string FindIntersections(string function)
        {
            function = function.Replace(" ", "");

            // Квадратичная функция ax^2 + bx + c
            var quadraticMatch = Regex.Match(function, @"^(?<a>[-+]?\d*\.?\d*)\*?x\^2(?<b>[-+]?\d*\.?\d*)\*?x(?<c>[-+]?\d*\.?\d*)?$");
            if (quadraticMatch.Success)
            {
                double a = quadraticMatch.Groups["a"].Success && !string.IsNullOrEmpty(quadraticMatch.Groups["a"].Value)
                    ? Convert.ToDouble(quadraticMatch.Groups["a"].Value)
                    : 1;
                double b = quadraticMatch.Groups["b"].Success && !string.IsNullOrEmpty(quadraticMatch.Groups["b"].Value)
                    ? Convert.ToDouble(quadraticMatch.Groups["b"].Value)
                    : 0;
                double c = quadraticMatch.Groups["c"].Success && !string.IsNullOrEmpty(quadraticMatch.Groups["c"].Value)
                    ? Convert.ToDouble(quadraticMatch.Groups["c"].Value)
                    : 0;

                double discriminant = b * b - 4 * a * c;

                string solutionSteps = $"Поэтапное решение:\nD = {b}^2 - 4 * {a} * {c} = {discriminant}\n";

                double yIntercept = c;

                if (discriminant > 0)
                {
                    double x1 = (-b + Math.Sqrt(discriminant)) / (2 * a);
                    double x2 = (-b - Math.Sqrt(discriminant)) / (2 * a);
                    solutionSteps += $"Корни: x1 = {Math.Round(x1, 2)}, x2 = {Math.Round(x2, 2)}\n";

                    UpdateLabels(x1, yIntercept);

                    return $"Квадратичная функция:\nС осью OX: ( {Math.Round(x1, 2)}, 0 ) и ( {Math.Round(x2, 2)}, 0 )\nС осью OY: ( 0, {Math.Round(yIntercept, 2)} )\n\n{solutionSteps}";
                }
                else if (discriminant == 0)
                {
                    double x = -b / (2 * a);
                    solutionSteps += $"Корень: x = {Math.Round(x, 2)}\n";

                    UpdateLabels(x, yIntercept);

                    return $"Квадратичная функция:\nС осью OX: ( {Math.Round(x, 2)}, 0 )\nС осью OY: ( 0, {Math.Round(yIntercept, 2)} )\n\n{solutionSteps}";
                }
                else
                {
                    UpdateLabels(0, Math.Round(yIntercept));
                    return $"Квадратичная функция не пересекает ось OX.\nС осью OY: (0, {Math.Round(yIntercept, 2)})";
                }
            }


            // Линейная функция kx + b или kx
            Match linearMatch = Regex.Match(function, @"^(?<k>[-+]?\d*\.?\d*)\*?x\s*\+?\s*(?<b>[-+]?\d*\.?\d*)?$");

            if (linearMatch.Success)
            {
                double k = double.TryParse(linearMatch.Groups["k"].Value, out double kVal) ? kVal : 1;
                double b = double.TryParse(linearMatch.Groups["b"].Value, out double bVal) ? bVal : 0;

                double xIntercept = -b / k;
                double yIntercept = b;

                UpdateLabels(xIntercept, yIntercept);

                return $"Линейная функция:\nС осью OX: ({Math.Round(xIntercept, 2)}, 0)\nС осью OY: (0, {Math.Round(yIntercept, 2)})";
            }
            // Степенная функция
            Match powerMatch = Regex.Match(function, @"^(?<sign>[-+])?(?<k>\d*\.?\d+)?\*?x\^(?<n>\d+)$");

            if (powerMatch.Success)
            {
                double k = 1; 
                if (powerMatch.Groups["k"].Success && !string.IsNullOrEmpty(powerMatch.Groups["k"].Value))
                {
                    k = Convert.ToDouble(powerMatch.Groups["k"].Value);
                }

                int n;
                if (!int.TryParse(powerMatch.Groups["n"].Value, out n))
                {
                    return "Некорректное значение для n.";
                }

                if (powerMatch.Groups["sign"].Success && powerMatch.Groups["sign"].Value == "-")
                {
                    k *= -1;
                }

                double yIntercept = k * Math.Pow(0, n); 
                if (n == 0)
                {
                    UpdateLabels(0, Math.Round(yIntercept, 2));
                    return $"Степенная функция:\nС осью OX: только в нуле\nС осью OY: (0, {Math.Round(yIntercept, 2)})";
                }
                else if (n % 2 == 0)
                {
                    UpdateLabels(0, Math.Round(yIntercept, 2));
                    return $"Степенная функция:\nС осью OX: только в нуле\nС осью OY: (0, {Math.Round(yIntercept, 2)})";
                }
                else
                {
                    UpdateLabels(0, Math.Round(yIntercept, 2));
                    return $"Степенная функция:\nС осью OX: все действительные числа\nС осью OY: (0, {Math.Round(yIntercept, 2)})";
                }
            }
            return "Некорректный ввод. Пожалуйста проверьте формат функции.";
        }

        private void UpdateLabels(double xIntercept, double yIntercept)
        {
            LabelOx.Content = $"({Math.Round(xIntercept, 2)}, 0)";
            LabelOy.Content = $"(0, {Math.Round(yIntercept, 2)})";

            StepsOutput.Text = $"Пересечение с осью OX: ({Math.Round(xIntercept, 2)}, 0)\nПересечение с осью OY: (0, {Math.Round(yIntercept, 2)})";
        }

        private string FunctiaChotn(string function)
        {
            bool isEven = true;
            bool isOdd = true;

            for (double x = -10; x <= 10; x += 1) 
            {
                double f_x = G(function, x);      
                double f_neg_x = G(function, -x); 

                if (f_neg_x != f_x) 
                {
                    isEven = false;
                }

                if (f_neg_x != -f_x)
                {
                    isOdd = false;
                }
            }

            if (isEven)
            {
                chetnostLabel.Content = "Четная функция";
                return "Четная функция"; 
            }
            else if (isOdd)
            {
                chetnostLabel.Content = "Нечетная функция";
                return "Нечетная функция"; 
            }
            else
            {
                chetnostLabel.Content = "Функция общего вида";
                return "Функция общего вида"; 
            }
        }

        private string FindDomain(string function)
        {
           function = function.Replace(" ", "");

            if (function.Contains("/"))
            {
                string denominator = ExtractDenominator(function);
                var roots = FindValuesThatMakeZero(denominator);
                if (roots.Any())
                {
                    return "x ≠ " + string.Join(", x ≠ ", roots);
                }
            }

            if (Regex.IsMatch(function, @"\^\(1\/2\)|^\(0\.5\)")) 
            {
                MatchCollection matches = Regex.Matches(function, @"(.*?)\^(\(1\/2\)|0\.5)");
                foreach (Match match in matches)
                {
                    string innerExpression = match.Groups[1].Value;
                    List<double> roots = FindValuesThatMakeNonNegative(innerExpression);
                    if (roots.Count > 0)
                    {
                        return "x ≥ " + string.Join(", x ≥ ", roots);
                    }
                }
            }

            if (Regex.IsMatch(function, @"x\^-")) return "x ≠ 0";


            return "Все действительные числа";
        }

        private List<double> FindValuesThatMakeNonNegative(string expression)
        {
            List<double> nonNegativeRoots = new List<double>();
            return nonNegativeRoots;
        }

        private List<double> FindValuesThatMakeZero(string expression)
        {
            List<double> problematicValues = new List<double>();

            expression = expression.Replace(" ", "");

            // квадратное уравнение
            Match quadraticMatch = Regex.Match(expression, @"(?<a>-?\d*\.?\d*)x\^2(?<b>[+-]\d*\.?\d*)x?(?<c>[+-]?\d*\.?\d*)?");

            if (quadraticMatch.Success)
            {
                double a = string.IsNullOrEmpty(quadraticMatch.Groups["a"].Value) ? 1 : double.Parse(quadraticMatch.Groups["a"].Value);
                double b = string.IsNullOrEmpty(quadraticMatch.Groups["b"].Value) ? 0 : double.Parse(quadraticMatch.Groups["b"].Value);
                double c = string.IsNullOrEmpty(quadraticMatch.Groups["c"].Value) ? 0 : double.Parse(quadraticMatch.Groups["c"].Value);

                double discriminant = b * b - 4 * a * c;

                if (discriminant >= 0) 
                {
                    problematicValues.Add((-b + Math.Sqrt(discriminant)) / (2 * a));
                    problematicValues.Add((-b - Math.Sqrt(discriminant)) / (2 * a));
                }
            }

            // линейное уравнение
            Match linearMatch = Regex.Match(expression, @"(?<a>-?\d*\.?\d*)\*?x(?<b>[+-]\d*\.?\d*)?");

            if (linearMatch.Success)
            {
                double a = string.IsNullOrEmpty(linearMatch.Groups["a"].Value) ? 1 : double.Parse(linearMatch.Groups["a"].Value);
                double b = string.IsNullOrEmpty(linearMatch.Groups["b"].Value) ? 0 : double.Parse(linearMatch.Groups["b"].Value);

                if (a != 0)
                {
                    problematicValues.Add(-b / a);
                }
            }

            return problematicValues.Distinct().ToList(); 
        }

        private string ExtractDenominator(string function)
        {
            var parts = function.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            return parts.Length > 1 ? parts[1].Trim() : "";
        }

        private void UpdateGraph()
        {
            GraphModel = CreatePlotModel(); 
            PlotView.Model = GraphModel;
        }

        private void PlotGraphButton_Click(object sender, RoutedEventArgs e)
        {
            string function = FunctionTextBox.Text.Trim();
            if (!IsValidFunction(function))
            {
                ClearAllButton_Click(null, null);
                return;
            }

            CreateDataTable();
            this.DataContext = this;
            UpdateGraph();
        }

        private void CreateDataTable()
        {
            List<DataPoint> points = new List<DataPoint>();
            double[] xValues = { -1, 0, 1 }; 
            foreach (double x in xValues)
            {
                double y = CalculateY(FunctionTextBox.Text, x);
                points.Add(new DataPoint(x, y));
            }
            PointsDataGrid.ItemsSource = points;
        }

        private double CalculateY(string function, double x)
        {
            string expression = ConvertToNCalcExpression(function);
            expression = expression.Replace("x", x.ToString()); 
            var e = new Expression(expression);
            try
            {
                return Convert.ToDouble(e.Evaluate());
            }
            catch (Exception ex)
            {
                return 0; 
            }
        }

        private string ConvertToNCalcExpression(string expression)
        {
            expression = Regex.Replace(expression, @"x\^(\d+)", "Pow(x, $1)");

            return expression;
        }

        private double EvaluateExpression(string expression)
        {
            expression = expression.Replace(" ", "");

            if (double.TryParse(expression, out double result))
            {
                return result;
            }

            int operatorIndex = FindLastOperator(expression);

            if (operatorIndex == -1)
                throw new ArgumentException("Неверное выражение");

            char operation = expression[operatorIndex];
            double leftValue = EvaluateExpression(expression.Substring(0, operatorIndex));
            double rightValue = EvaluateExpression(expression.Substring(operatorIndex + 1));

            switch (operation)
            {
                case '+':
                    return leftValue + rightValue;
                case '-':
                    return leftValue - rightValue;
                case '*':
                    return leftValue * rightValue;
                case '/':
                    return leftValue / rightValue;
                case '^':
                    return Math.Pow(leftValue, rightValue);
                default:
                    throw new ArgumentException("Неверный оператор");
            }
        }


        private int FindLastOperator(string expression)
        {
            int operatorIndex = -1;
            int highestPriority = -1;

            //  приоритет операций
            Dictionary<char, int> priorities = new Dictionary<char, int>()
            {
                {'^', 3},
                {'*', 2},
                {'/', 2},
                {'+', 1},
                {'-', 1}
            };

            for (int i = 0; i < expression.Length; i++)
            {
                if (priorities.ContainsKey(expression[i]) && priorities[expression[i]] >= highestPriority)
                {
                    operatorIndex = i;
                    highestPriority = priorities[expression[i]];
                }
            }

            return operatorIndex;
        }

        public class DataPoint
        {
            public double X { get; set; }
            public double Y { get; set; }

            public DataPoint(double x, double y)
            {
                X = x;
                Y = y;
            }
        }

        private void ClearAllButton_Click(object sender, RoutedEventArgs e)
        {
            SaveCurrentState();
            if (PlotView is OxyPlot.Wpf.PlotView plotView)
            {
                plotView.Model = new OxyPlot.PlotModel();
            }

            FunctionTextBox.Text = "";
            chetnostLabel.Content = "";

            PointsDataGrid.ItemsSource = null;

            DomainOutputLabel.Content = "";

            LabelOx.Content = "";
            LabelOy.Content = "";

            StepsOutput.Text = "";
            Proizvodnay.Content = "";
            SolutionsOutputLabel.Content = "";
            MinValueLabel.Content = "";
            MaxValueLabel.Content = "";
            MonoDataGrid.ItemsSource = null;
            PointsDataGrid.ItemsSource = null;
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            if (undoStack.Count > 0)
            {
                ApplicationState previousState = undoStack.Pop();
                RestoreState(previousState);
                MessageBox.Show("Отменено последнее действие.");
            }
            else
            {
                MessageBox.Show("Нет действий для отмены.");
            }
        }
        private void SaveCurrentState()
        {
            ApplicationState currentState = new ApplicationState
            {
                FunctionText = FunctionTextBox.Text,
                DomainOutput = DomainOutputLabel.Content.ToString(),
                ChetnostOutput = chetnostLabel.Content.ToString(),
                LabelOxContent = LabelOx.Content.ToString(),
                LabelOyContent = LabelOy.Content.ToString(),
                StepsOutputText = StepsOutput.Text,
                ProizvodnayContent = Proizvodnay.Content.ToString(),
                SolutionsOutputContent = SolutionsOutputLabel.Content.ToString(),
                MinValueContent = MinValueLabel.Content.ToString(),
                MaxValueContent = MaxValueLabel.Content.ToString(),
                MonoDataGridItemsSource = MonoDataGrid.ItemsSource,
                PointsDataGridItemSource = PointsDataGrid.ItemsSource,
                GraphModel = (PlotView as OxyPlot.Wpf.PlotView)?.Model
            };
            undoStack.Push(currentState);
        }

        private void RestoreState(ApplicationState state)
        {
            FunctionTextBox.Text = state.FunctionText;
            DomainOutputLabel.Content = state.DomainOutput;
            chetnostLabel.Content = state.ChetnostOutput;
            LabelOx.Content = state.LabelOxContent;
            LabelOy.Content = state.LabelOyContent;
            StepsOutput.Text = state.StepsOutputText;
            Proizvodnay.Content = state.ProizvodnayContent;
            SolutionsOutputLabel.Content = state.SolutionsOutputContent;
            MinValueLabel.Content = state.MinValueContent;
            MaxValueLabel.Content = state.MaxValueContent;

            MonoDataGrid.ItemsSource = state.MonoDataGridItemsSource;
            PointsDataGrid.ItemsSource = state.PointsDataGridItemSource;

            if (PlotView is OxyPlot.Wpf.PlotView plotView)
            {
                plotView.Model = state.GraphModel; 
            }
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.Z)
            {
                Undo_Click(this, null); 
                e.Handled = true; 
            }
        }

        private void ShowAbout_Click(object sender, RoutedEventArgs e)
        {
            string aboutMessage = "Название программы: Исследование функции\n" +
                                  "Версия: 1.0.0\n\n" +
                                  "Это приложение предназначено для выполнения математических расчетов и визуализации графиков.\n" +
                                  "Вы можете использовать его для анализа функций и получения различных решений.\n\n" +
                                  "Разработано студенткой группы 21 П-1, Семакиной Ладой Владиславовной. Все права защищены.\n\n"+
                                  "2024 г.";

            MessageBox.Show(aboutMessage, "О программе", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Instruct_Click(object sender, RoutedEventArgs e)
        {
            InstructionWindow instructionWindow = new InstructionWindow();
            instructionWindow.ShowDialog();
        }
    }

    public class FunctionInfo
    {
        public string Interval { get; set; }
        public string Behavior { get; set; }
    }

    public class ApplicationState
    {
        public string FunctionText { get; set; }
        public string DomainOutput { get; set; }
        public string ChetnostOutput { get; set; }
        public string LabelOxContent { get; set; }
        public string LabelOyContent { get; set; }
        public string StepsOutputText { get; set; }
        public string ProizvodnayContent { get; set; }

        public string SolutionsOutputContent { get; set; }

        public string MinValueContent { get; set; }

        public string MaxValueContent { get; set; }

        public System.Collections.IEnumerable MonoDataGridItemsSource { get; set; }
        public System.Collections.IEnumerable PointsDataGridItemSource { get; set; }
        public OxyPlot.PlotModel GraphModel { get; set; }

    }
}
