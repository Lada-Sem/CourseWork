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


namespace UIдизайн
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
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
        
        //проверка на гиперболу
       /* public static bool IsHyperbola(string function)
        {
            function = function.Replace(" ", "").ToLower();


            if (!function.Contains("x") || !function.Contains("y"))
            {
                return false;
            }


            // (y^2/a^2 - x^2/b^2 = 1) или (x^2/a^2 - y^2/b^2 = 1)
            string pattern = @"(y\^2\/[0-9]+ - x\^2\/[0-9]+ = 1)|(x\^2\/[0-9]+ - y\^2\/[0-9]+ = 1)|([0-9]+)\/x";


            Match match = Regex.Match(function, pattern);

            return match.Success;
        }*/

        public PlotModel GraphModel { get; set; }

        private PlotModel CreatePlotModel()
        {

            var plotModel = new PlotModel();


            // Горизонтальная ось
            var horizontalAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = -20,
                Maximum = 20,
                AbsoluteMaximum = 20,
                AbsoluteMinimum = -20,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Solid,
                Title = "X", // Подпись оси X
                TitleFontSize = 14,
                TitleFontWeight = FontWeights.Bold,
                AxislineColor = OxyColors.Red,
                MajorTickSize = 5, // Размер основных делений
                MinorTickSize = 3 // Размер мелких делений
            };

            // Вертикальная ось
            var verticalAxis = new LinearAxis
            {
                Position = AxisPosition.Right,
                Minimum = -20,
                Maximum = 20,
                AbsoluteMaximum = 20,
                AbsoluteMinimum = -20,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Solid,
                Title = "Y", // Подпись оси X
                TitleFontSize = 14,
                TitleFontWeight = FontWeights.Bold,
                AxislineColor = OxyColors.Black, // Цвет оси
                MajorTickSize = 5, // Размер основных делений
                MinorTickSize = 3, // Размер мелких делений

            };


            // Установка начального масштаба ( до 5)
            horizontalAxis.Minimum = -5;
            horizontalAxis.Maximum = 5;   

            verticalAxis.Minimum = -5;   
            verticalAxis.Maximum = 5;      

            // Добавление осей
            plotModel.Axes.Add(horizontalAxis);
            plotModel.Axes.Add(verticalAxis);

            var series1 = new LineSeries(); // Первая линия
            series1.Points.Clear();

            for (double i = -20; i <= 20; i += 0.1) // Диапазон по оси X от -20 до 20
            {
                double yValue = G(FunctionTextBox.Text, i); // Вычисляем значение функции

                if (yValue < -20 || yValue > 20) // Проверка на выход за пределы по оси Y
                {
                    continue;
                }

                series1.Points.Add(new OxyPlot.DataPoint(i, yValue));
            }

            plotModel.Series.Add(series1);
            //plotModel.PlotAreaBorderThickness = 0;


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

                if (arg[i] == 'c')
                {
                    i++;
                    if (arg[i] == 'o')
                    {
                        i++;
                        if (arg[i] == 's')
                        {
                            try
                            {
                                while (Pr("cos") <= Pr(s.Peek()))
                                {
                                    a[j] = s.Pop().ToString();
                                    j++;
                                }
                            }
                            catch { }
                            s.Push("cos");
                            continue;
                        }
                    }
                    if (arg[i] == 't')
                    {
                        i++;
                        if (arg[i] == 'g')
                        {
                            try
                            {
                                while (Pr("ctg") <= Pr(s.Peek()))
                                {
                                    a[j] = s.Pop().ToString();
                                    j++;
                                }
                            }
                            catch { }
                            s.Push("ctg");
                            continue;
                        }
                    }
                }
                if (arg[i] == 't')
                {
                    i++;
                    if (arg[i] == 'g')
                    {
                        try
                        {
                            while (Pr("tg") <= Pr(s.Peek()))
                            {
                                a[j] = s.Pop().ToString();
                                j++;
                            }
                        }
                        catch { }
                        s.Push("tg");
                        continue;
                    }
                }
                if (arg[i] == 's')
                {
                    i++;
                    if (arg[i] == 'i')
                    {
                        i++;
                        if (arg[i] == 'n')
                        {
                            try
                            {
                                while (Pr("sin") <= Pr(s.Peek()))
                                {
                                    a[j] = s.Pop().ToString();
                                    j++;
                                }
                            }
                            catch { }
                            s.Push("sin");
                            continue;
                        }
                    }
                    if (arg[i] == 'q')
                    {
                        i++;
                        if (arg[i] == 'r')
                        {
                            i++;
                            if (arg[i] == 't')
                            {
                                try
                                {
                                    while (Pr("sqrt") <= Pr(s.Peek()))
                                    {
                                        a[j] = s.Pop().ToString();
                                        j++;
                                    }
                                }
                                catch { }
                                s.Push("sqrt");
                                continue;
                            }
                        }
                    }
                }
                if (arg[i] == 'l')
                {
                    i++;
                    if (arg[i] == 'n')
                    {
                        try
                        {
                            while (Pr("ln") <= Pr(s.Peek()))
                            {
                                a[j] = s.Pop().ToString();
                                j++;
                            }
                        }
                        catch { }
                        s.Push("ln");
                        continue;
                    }
                    if (arg[i] == 'g')
                    {
                        try
                        {
                            while (Pr("lg") <= Pr(s.Peek()))
                            {
                                a[j] = s.Pop().ToString();
                                j++;
                            }
                        }
                        catch { }
                        s.Push("lg");
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
        static double G(string y, string x = "0")                 
        {
            string[] g1 = new string[y.Length];
            Opn(ref g1, y);
            double X;
            X = G(x, 0);
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
                case "sin": return 4;
                case "cos": return 4;
                case "tg": return 4;
                case "ctg": return 4;
                case "ln": return 4;
                case "lg": return 4;
                case "sqrt": return 5;
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
        //факториал
        static double Fact(double x)  
        {
            if (x < 0) return 0;
            if (x == 1) return 1;
            else return x * Fact(x - 1);
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
                        case "sin":
                            if (st.Peek() % Math.PI == 0)
                                st.Push(0);
                            else
                                st.Push(Math.Sin(st.Pop()));
                            break;
                        case "cos":
                            if ((st.Peek() + Math.PI / 2) % Math.PI == 0)
                                st.Push(0);
                            else
                                st.Push(Math.Cos(st.Pop()));
                            break;
                        case "tg":
                            if (st.Peek() % Math.PI == 0)
                                st.Push(0);
                            else
                                st.Push(Math.Tan(st.Pop()));
                            break;
                        case "ctg":
                            if ((st.Peek() + Math.PI / 2) % Math.PI == 0)
                                st.Push(0);
                            else
                                st.Push(1 / (Math.Tan(st.Pop())));
                            break;
                        case "ln":
                            if (st.Peek() < 0) { return 0; }
                            else st.Push(Math.Log(st.Pop()));
                            break;
                        case "lg":
                            if (st.Peek() < 0) { return 0; }
                            else st.Push(Math.Log10(st.Pop()));
                            break;
                        case "!":
                            st.Push(Fact(st.Pop()));
                            break;
                        case "sqrt":
                            if (st.Peek() < 0) { st.Pop(); st.Push(0); }
                            else
                                st.Push(Math.Pow(st.Pop(), 0.5));
                            break;
                    }
                }
            }
            return st.Pop();
        }
        //интеграл

        static double Integral(string a, string b, string[] f)        
        {
            double h = (G(b, 0) - G(a, 0)) / 30000;
            double s = 0;
            for (double i = G(a, 0) + h; i <= G(b, 0); i += h)
            {
                s += h * Opn_res(f, i);
            }
            return s;
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
        private void SinButton_Click(object sender, RoutedEventArgs e)
        {
            AppendText("sin");
        }

        private void CosButton_Click(object sender, RoutedEventArgs e)
        {
            AppendText("cos");
        }

        private void TgButton_Click(object sender, RoutedEventArgs e)
        {
            AppendText("tg");
        }

        private void CtgButton_Click(object sender, RoutedEventArgs e)
        {
            AppendText("ctg");
        }

        private void LnButton_Click(object sender, RoutedEventArgs e)
        {
            AppendText("ln");
        }

        private void LgButton_Click(object sender, RoutedEventArgs e)
        {
            AppendText("lg");
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

        private void FactorialButton_Click(object sender, RoutedEventArgs e)
        {
            AppendText("!");
        }

        private void PiButton_Click(object sender, RoutedEventArgs e)
        {
            AppendText("pi");
        }

        private void ExhibitorButton_Click(object sender, RoutedEventArgs e)
        {
            AppendText("e");
        }

        private void X_Button_Click(object sender, RoutedEventArgs e)
        {
            AppendText("x");
        }

        private void Y_Button_Click(object sender, RoutedEventArgs e)
        {
            AppendText("y");
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            string function = FunctionTextBox.Text;
            string[] a = new string[function.Length];
            Opn(ref a, function);
            try { label5.Content = G(FunctionTextBox.Text, textBox2.Text).ToString(); } catch { label5.Content = G(FunctionTextBox.Text, 0).ToString(); }

            
            string domain = FindDomain(function);
            string chislo = FunctiaChotn(function);
            chetnostLabel.Content = chislo;
            DomainOutputLabel.Content = domain;

            string result = FindIntersections(function);
            StepsOutput.Text = result;

        }

        private string FindIntersections(string function)
        {
            function = function.Replace(" ", "");

            // Проверка на линейную функцию вида kx + b или kx
            var linearMatch = Regex.Match(function, @"^(?<k>[-+]?\d*\.?\d*)([*])x(?<b>[-+]\d+)?$");
            if (linearMatch.Success)
            {
                double k = 1; // Значение по умолчанию
                double b = 0; // Значение по умолчанию

                // Проверка и попытка преобразования k
                if (linearMatch.Groups["k"].Success && !string.IsNullOrEmpty(linearMatch.Groups["k"].Value))
                {
                    if (!Double.TryParse(linearMatch.Groups["k"].Value, out k))
                    {
                        return "Некорректное значение для k.";
                    }
                }

                // Проверка и попытка преобразования b
                if (linearMatch.Groups["b"].Success && !string.IsNullOrEmpty(linearMatch.Groups["b"].Value))
                {
                    if (!Double.TryParse(linearMatch.Groups["b"].Value, out b))
                    {
                        return "Некорректное значение для b.";
                    }
                }

                double xIntercept = -b / k; // Пересечение с осью OX 
                double yIntercept = b;        // Пересечение с осью OY

                UpdateLabels(Math.Round(xIntercept), Math.Round(yIntercept)); // Обновляем метки

                return $"Линейная функция:\nС осью OX: ( {Math.Round(xIntercept)}, 0 )\nС осью OY: ( 0, {Math.Round(yIntercept)} )";
            }

            // Проверка на квадратичную функцию вида ax^2 + bx + c
            var quadraticMatch = Regex.Match(function, @"^(?<a>[-+]?\d*\.?\d*)x\^2(?<b>[-+]\d*\.?\d*)x(?<c>[-+]\d*\.?\d*)?$");
            if (quadraticMatch.Success)
            {
                double a = quadraticMatch.Groups["a"].Success ? Convert.ToDouble(quadraticMatch.Groups["a"].Value) : 1; // Если a не указан
                double b = quadraticMatch.Groups["b"].Success ? Convert.ToDouble(quadraticMatch.Groups["b"].Value) : 0;
                double c = quadraticMatch.Groups["c"].Success ? Convert.ToDouble(quadraticMatch.Groups["c"].Value) : 0;

                // Находим дискриминант
                double discriminant = b * b - 4 * a * c;

                string solutionSteps = $"Поэтапное решение:\nD = {b}^2 - 4 * {a} * {c} = {discriminant}\n";

                if (discriminant > 0)
                {
                    double x1 = (-b + Math.Sqrt(discriminant)) / (2 * a);
                    double x2 = (-b - Math.Sqrt(discriminant)) / (2 * a);
                    solutionSteps += $"Корни: x1 = ({-b} + √{discriminant}) / (2 * {a}) = {Math.Round(x1)}\n";
                    solutionSteps += $"Корни: x2 = ({-b} - √{discriminant}) / (2 * {a}) = {Math.Round(x2)}\n";
                    UpdateLabels(Math.Round(x1), Math.Round(c)); // Обновляем метки
                    return $"Квадратичная функция:\nС осью OX: ( {Math.Round(x1)}, 0 ) и ( {Math.Round(x2)}, 0 )\nС осью OY: ( 0, {Math.Round(c)} )\n\n{solutionSteps}";
                }
                else if (discriminant == 0)
                {
                    double x = -b / (2 * a);
                    solutionSteps += $"Корень: x = {-b} / (2 * {a}) = {Math.Round(x)}\n";
                    UpdateLabels(Math.Round(x), Math.Round(c)); // Обновляем метки
                    return $"Квадратичная функция:\nС осью OX: ( {Math.Round(x)}, 0 )\nС осью OY: ( 0, {Math.Round(c)} )\n\n{solutionSteps}";
                }
                else
                {
                    return "Квадратичная функция не пересекает ось OX.";
                }
            }

            Match powerMatch = Regex.Match(function, @"(?<k>-?\d*\.?\d+)?x\^(?<n>\d+)$");
            if (powerMatch.Success)
            {
                double k = powerMatch.Groups["k"].Success && !string.IsNullOrEmpty(powerMatch.Groups["k"].Value)
                    ? Convert.ToDouble(powerMatch.Groups["k"].Value)
                    : 1; // Если k не указан 
                int n;

                if (!Int32.TryParse(powerMatch.Groups["n"].Value, out n))
                {
                    return "Некорректное значение для n.";
                }

                if (n % 2 == 0) // Четная степень 
                {
                    UpdateLabels(0, Math.Round(k));
                    return $"Степенная функция:\nС осью OX: только в нуле\nС осью OY: ( 0, {Math.Round(k)} )";
                }
                else // Нечетная степень 
                {
                    UpdateLabels(0, Math.Round(k)); // Обновляем метки для нечетной степени 
                    return $"Степенная функция:\nС осью OX: все действительные числа\nС осью OY: ( 0, {Math.Round(k)} )";
                }
            }


            function = Regex.Replace(function, @"(\w+)\^(\d+)", "Pow($1, $2)");

            // Добавляем скобки вокруг степени для правильного порядка операций
            function = Regex.Replace(function, @"(\w+)(-?\d+)", "($1 - $2)");

            // Обработка общего уравнения через NCalc
            try
            {
                Expression expr = new Expression(function);

                // Находим y при x=0 для пересечения с осью Y
                expr.Parameters["x"] = 0;
                double yIntercept = Convert.ToDouble(expr.Evaluate());

                // Находим x при y=0 для пересечения с осью X
                double? xIntercept = null;
                string equationForX = function + " == 0"; // Добавляем условие равенства нулю

                for (double x = -100; x <= 100; x += 0.1)
                {
                    expr.Parameters["x"] = x;
                    if (Math.Abs(Convert.ToDouble(expr.Evaluate())) < 1e-6) // Проверка на близость к нулю
                    {
                        xIntercept = x;
                        break; // Выход из цикла при нахождении первого пересечения
                    }
                }

                if (xIntercept.HasValue)
                {
                    UpdateLabels(Math.Round(xIntercept.Value), Math.Round(yIntercept)); // Обновляем метки
                    return $"Общая функция:\nС осью OX: ( {Math.Round(xIntercept.Value)}, 0 )\nС осью OY: ( 0, {Math.Round(yIntercept)} )";
                }
                else
                {
                    UpdateLabels(0, Math.Round(yIntercept)); // Обновляем метки если нет пересечений с OX
                    return $"Общая функция:\nС осью OY: ( 0, {Math.Round(yIntercept)} )\nС осью OX: нет пересечений.";
                }
            }
            catch (Exception ex)
            {
                return "Некорректный ввод. Пожалуйста проверьте формат функции.";
            }
        }


        private void UpdateLabels(double xIntercept, double yIntercept)
        {
            LabelOx.Content = $"({Math.Round(xIntercept)}, 0)";
            LabelOy.Content = $"(0, {Math.Round(yIntercept)})";
        }

        private string FunctiaChotn(string function)
        {
            try
            {
                label5.Content = G(FunctionTextBox.Text, textBox2.Text).ToString();
            }
            catch
            {
                label5.Content = G(FunctionTextBox.Text, 0).ToString();
            }

            bool isEven = true;
            bool isOdd = true;

            // Проверяем значения для нескольких x
            for (double x = -10; x <= 10; x += 1) // Проверяем от -10 до 10 с шагом 1
            {
                double f_x = G(function, x);      // Вычисляем f(x)
                double f_neg_x = G(function, -x); // Вычисляем f(-x)

                if (f_neg_x != f_x) // Если f(-x) не равно f(x), функция не четная
                {
                    isEven = false;
                }

                if (f_neg_x != -f_x) // Если f(-x) не равно -f(x), функция не нечетная
                {
                    isOdd = false;
                }
            }

            // Определяем результат
            if (isEven)
            {
                chetnostLabel.Content = "Четная функция";
                return "Четная функция"; // Возвращаем строку
            }
            else if (isOdd)
            {
                chetnostLabel.Content = "Нечетная функция";
                return "Нечетная функция"; // Возвращаем строку
            }
            else
            {
                chetnostLabel.Content = "Функция общего вида";
                return "Функция общего вида"; // Возвращаем строку
            }
        }
        private string FindDomain(string function)
        {
            if (function.Contains("/"))
            {
                // Извлекаем знаменатель
                string denominator = ExtractDenominator(function);

                // Находим значения, которые делают знаменатель нулевым
                var problematicValues = FindValuesThatMakeZero(denominator);

                if (problematicValues.Any())
                {
                    return "x != " + string.Join(", x != ", problematicValues);
                }
            }

            // Проверка на корень (если применимо)
            if (function.Contains("sqrt"))
            {
                var sqrtParts = Regex.Matches(function, @"sqrt\((.*?)\)");
                foreach (Match match in sqrtParts)
                {
                    string innerExpression = match.Groups[1].Value;
                    var roots = FindValuesThatMakeNonNegative(innerExpression);
                    if (roots.Any())
                    {
                        return "x >= " + string.Join(", x >= ", roots);
                    }
                }
            }

            return "Все действительные числа";
        }

        private List<double> FindValuesThatMakeZero(string expression)
        {
            List<double> problematicValues = new List<double>();

            // Убираем пробелы и обрабатываем выражение
            expression = expression.Replace(" ", "");

            // Проверяем на квадратное уравнение
            Match quadraticMatch = Regex.Match(expression, @"(?<a>-?\d*\.?\d*)x\^2(?<b>[+-]\d*\.?\d*)x?(?<c>[+-]?\d*\.?\d*)?");

            if (quadraticMatch.Success)
            {
                double a = string.IsNullOrEmpty(quadraticMatch.Groups["a"].Value) ? 1 : double.Parse(quadraticMatch.Groups["a"].Value);
                double b = string.IsNullOrEmpty(quadraticMatch.Groups["b"].Value) ? 0 : double.Parse(quadraticMatch.Groups["b"].Value);
                double c = string.IsNullOrEmpty(quadraticMatch.Groups["c"].Value) ? 0 : double.Parse(quadraticMatch.Groups["c"].Value);

                // Вычисляем корни квадратного уравнения
                double discriminant = b * b - 4 * a * c;

                if (discriminant >= 0) // Только действительные корни
                {
                    problematicValues.Add((-b + Math.Sqrt(discriminant)) / (2 * a));
                    problematicValues.Add((-b - Math.Sqrt(discriminant)) / (2 * a));
                }
            }

            // Проверяем на линейное уравнение
            Match linearMatch = Regex.Match(expression, @"(?<a>-?\d*\.?\d*)\*?x(?<b>[+-]\d*\.?\d*)?");

            if (linearMatch.Success)
            {
                double a = string.IsNullOrEmpty(linearMatch.Groups["a"].Value) ? 1 : double.Parse(linearMatch.Groups["a"].Value);
                double b = string.IsNullOrEmpty(linearMatch.Groups["b"].Value) ? 0 : double.Parse(linearMatch.Groups["b"].Value);

                // Решаем уравнение ax + b = 0
                if (a != 0)
                {
                    problematicValues.Add(-b / a);
                }
            }

            return problematicValues.Distinct().ToList(); // Возвращаем уникальные значения, которые делают ноль
        }

        private string ExtractDenominator(string function)
        {
            // Разделяем по '/' и берем вторую часть как знаменатель
            var parts = function.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            return parts.Length > 1 ? parts[1].Trim() : "";
        }

        private List<double> FindValuesThatMakeNonNegative(string expression)
        {
            List<double> nonNegativeRoots = new List<double>();

            // Простой случай: ax^2 + bx + c >= 0
            // Можно добавить логику для нахождения значений, которые делают выражение неотрицательным

            return nonNegativeRoots;
        }

        private void CalculateIntegralButton_Click(object sender, RoutedEventArgs e)
        {
            /*  string arg = FunctionTextBox.Text;
              string[] a = new string[arg.Length];
              Opn(ref a, arg);
              label7.Content = Math.Round(Integral(textBox4.Text, textBox5.Text, a), 4).ToString();*/
        }
        private void UpdateGraph()
        {
            GraphModel = CreatePlotModel(); // Создаем новый PlotModel
            PlotView.Model = GraphModel; // Обновляем модель в PlotView
        }

        private void PlotGraphButton_Click(object sender, RoutedEventArgs e)
        {

            CreateDataTable(); // Обновляем данные в DataGrid
            UpdateGraph();
        }

        private void Proiz_Click(object sender, RoutedEventArgs e)
        {
            string functionInput = FunctionTextBox.Text;
            //  var variable = SymbolicExpression.Variable("x");
            //  var function = SymbolicExpression.Parse(functionInput);
            // var derivative = function.Differentiate(variable);
            //  Proiz1.Content = $"Производная: {derivative}";
        }

        private void ClearResearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {

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
            // Заменяем "x" на фактическое значение x в строке функции
            string expression = function.Replace("x", x.ToString());

            // Вычисляем значение выражения
            try
            {
                return EvaluateExpression(expression);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка в выражении: {ex.Message}");
                return 0;
            }
        }

        private double EvaluateExpression(string expression)
        {
            // Удаляем пробелы
            expression = expression.Replace(" ", "");

            // Если выражение является числом, возвращаем его
            if (double.TryParse(expression, out double result))
            {
                return result;
            }

            // Проверяем наличие операций
            int operatorIndex = FindLastOperator(expression);

            if (operatorIndex == -1)
                throw new ArgumentException("Неверное выражение");

            // Разделяем на левую и правую части
            char operation = expression[operatorIndex];
            double leftValue = EvaluateExpression(expression.Substring(0, operatorIndex));
            double rightValue = EvaluateExpression(expression.Substring(operatorIndex + 1));

            // Выполняем операцию
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

        // Находим последний оператор с наивысшим приоритетом
        private int FindLastOperator(string expression)
        {
            int operatorIndex = -1;
            int highestPriority = -1;

            // Определяем приоритет операций
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
            if (PlotView is OxyPlot.Wpf.PlotView plotView)
            {
                plotView.Model = new OxyPlot.PlotModel();
            }

            // Clear FunctionTextBox
            FunctionTextBox.Text = "";

            // Clear PointsDataGrid
            PointsDataGrid.ItemsSource = null; // Assuming it's a DataGrid

            // Clear DomainOutputLabel
            DomainOutputLabel.Content = "";

            // Clear LabelOx and LabelOy
            LabelOx.Content = "";
            LabelOy.Content = "";

            // Clear StepsOutput
            StepsOutput.Text = "";
        }
    }
}
