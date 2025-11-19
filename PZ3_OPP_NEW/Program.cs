using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IExpr
{
    IEnumerable<string> Variables { get; } //переменные
    bool IsConstant { get; } //константы
    bool IsPolynomial { get; } //полиномы
    int PolynomialDegree { get; } //степень полинома
    double Compute(IReadOnlyDictionary<string, double> variableValues);

    // Новый метод для получения коэффициентов полинома
    IReadOnlyDictionary<string, double> GetPolynomialCoefficients();

    //напомнить на паре!! про impli

    // Операции между двумя IExpr
}
public abstract class Expr : IExpr
{
    public static implicit operator Expr(int value) => new Constant(value);
    public static implicit operator Expr(double value) => new Constant(value);

    public abstract IEnumerable<string> Variables { get; }
    public abstract bool IsConstant { get; }
    public abstract bool IsPolynomial { get; }
    public abstract int PolynomialDegree { get; }

    public abstract double Compute(IReadOnlyDictionary<string, double> variableValues);
    public abstract IReadOnlyDictionary<string, double> GetPolynomialCoefficients();
    public static Expr operator -(Expr a, Expr b) => new Subtract(a, b);
    public static Expr operator +(Expr a, Expr b) => new Add(a, b);
    public static Expr operator *(Expr a, Expr b) => new Multiply(a, b);
    public static Expr operator /(Expr a, Expr b) => new Divide(a, b);
    public static Expr operator -(Expr a) => new UnaryMinus(a);
    public abstract override string ToString();
}
public abstract class BinaryOperation : Expr
{
    protected IExpr Left { get; }
    protected IExpr Right { get; }

    public BinaryOperation(IExpr x_Var, IExpr y_Var)
    {
        Left = x_Var ;
        Right = y_Var;
    }
}

public class Add : BinaryOperation
{
    private IExpr _Left;
    private IExpr _Right;

    public Add(IExpr x_var, IExpr y_var) : base(x_var, y_var)
    {
        _Left = x_var;
        _Right = y_var;
    }

    public override IEnumerable<string> Variables => _Left.Variables.Union(_Right.Variables);

    public override bool IsConstant => _Left.IsConstant && _Right.IsConstant;

    public override bool IsPolynomial => _Left.IsPolynomial && _Right.IsPolynomial;

    public override int PolynomialDegree
    {
        get
        {
            if (!IsPolynomial) return -1;
            return Math.Max(_Left.PolynomialDegree, _Right.PolynomialDegree);
        }
    }

    public override double Compute(IReadOnlyDictionary<string, double> variableValues)
        => _Left.Compute(variableValues) + _Right.Compute(variableValues);

    public override IReadOnlyDictionary<string, double> GetPolynomialCoefficients()
    {
        if (!IsPolynomial)
            throw new InvalidOperationException("Выражение не является полиномом");

        var leftCoeffs = _Left.GetPolynomialCoefficients();
        var rightCoeffs = _Right.GetPolynomialCoefficients();

        var result = new Dictionary<string, double>();

        foreach ((string key, double coefficient) in leftCoeffs)
        {
            if (!result.TryAdd(key, coefficient))
                result[key] += coefficient;
        }

        foreach ((string key, double coefficient) in rightCoeffs)
        {
            if (!result.TryAdd(key, coefficient))
                result[key] += coefficient;
        }

        return result;
    }

    public override string ToString() => $"({_Left} + ({_Right}))";
}

public class Subtract : BinaryOperation
{
    private IExpr _Left;
    private IExpr _Right;

    public Subtract(IExpr x_var, IExpr y_var) : base(x_var, y_var)
    {
        _Left = x_var;
        _Right = y_var;
    }

    public override IEnumerable<string> Variables => _Left.Variables.Union(_Right.Variables);

    public override bool IsConstant => _Left.IsConstant && _Right.IsConstant;

    public override bool IsPolynomial => _Left.IsPolynomial && _Right.IsPolynomial;

    public override int PolynomialDegree
    {
        get
        {
            if (!IsPolynomial) return -1;
            return Math.Max(_Left.PolynomialDegree, _Right.PolynomialDegree);
        }
    }

    public override double Compute(IReadOnlyDictionary<string, double> variableValues)
        => _Left.Compute(variableValues) - _Right.Compute(variableValues);

    public override IReadOnlyDictionary<string, double> GetPolynomialCoefficients()
    {
        if (!IsPolynomial)
            throw new InvalidOperationException("Выражение не является полиномом");

        var leftCoeffs = _Left.GetPolynomialCoefficients();
        var rightCoeffs = _Right.GetPolynomialCoefficients();

        var result = new Dictionary<string, double>();

        foreach ((string key, double coefficient) in leftCoeffs)
        {
            if (!result.TryAdd(key, coefficient))
                result[key] += coefficient;
        }

        foreach ((string key, double coefficient) in rightCoeffs)
        {
            if (!result.TryAdd(key, -coefficient))
                result[key] -= coefficient;
        }

        return result;
    }

    public override string ToString() => $"({_Left} - {_Right})";
}

public class Multiply : BinaryOperation
{
    private IExpr _Left;
    private IExpr _Right;

    public Multiply(IExpr x_var, IExpr y_var) : base(x_var, y_var)
    {
        _Left = x_var;
        _Right =    y_var;
    }

    public override IEnumerable<string> Variables => _Left.Variables.Union(_Right.Variables);

    public override bool IsConstant => _Left.IsConstant && _Right.IsConstant;

    public override bool IsPolynomial => _Left.IsPolynomial && _Right.IsPolynomial;

    public override int PolynomialDegree
    {
        get
        {
            if (!IsPolynomial) return -1;

            int leftDegree = _Left.PolynomialDegree;
            int rightDegree = _Right.PolynomialDegree;

            // Если обе части - константы
            if (leftDegree == 0 && rightDegree == 0) return 0;

            // Если одна из частей - константа (степень 0), возвращаем степень другой части
            if (leftDegree == 0) return rightDegree;
            if (rightDegree == 0) return leftDegree;

            return leftDegree + rightDegree;
        }
    }

    public override double Compute(IReadOnlyDictionary<string, double> variableValues)
        => _Left.Compute(variableValues) * _Right.Compute(variableValues);

    public override IReadOnlyDictionary<string, double> GetPolynomialCoefficients()
    {
        if (!IsPolynomial)
            throw new InvalidOperationException("Выражение не является полиномом");

        var leftCoeffs = _Left.GetPolynomialCoefficients();
        var rightCoeffs = _Right.GetPolynomialCoefficients();

        var result = new Dictionary<string, double>();

        // Перемножаем каждый моном из левой части с каждым мономом из правой части
        foreach ((string leftKey, double leftCoefficient) in leftCoeffs)
        {
            foreach ((string rightKey, double rightCoefficient) in rightCoeffs)
            {
                string newKey = MultiplyTerms(leftKey, rightKey);
                double newCoefficient = leftCoefficient * rightCoefficient;

                if (!result.TryAdd(newKey, newCoefficient))
                    result[newKey] += newCoefficient;
            }
        }

        return result;
    }

    private string MultiplyTerms(string term1, string term2)
    {
        // Пустая строка представляет константу
        if (string.IsNullOrEmpty(term1)) return term2;
        if (string.IsNullOrEmpty(term2)) return term1;

        // Разбиваем термины на переменные
        var vars1 = ParseTerm(term1);
        var vars2 = ParseTerm(term2);

        // Объединяем и суммируем степени
        var resultVars = new Dictionary<string, int>();

        foreach ((string key, int coefficient) in vars1)
        {
            if (!resultVars.TryAdd(key, coefficient))
                resultVars[key] += coefficient;
        }

        foreach ((string key, int coefficient) in vars2)
        {
            if (!resultVars.TryAdd(key, coefficient))
                resultVars[key] += coefficient;
        }

        // Сортируем переменные по имени и формируем строку термина
        return string.Join("*", resultVars.OrderBy(kvp => kvp.Key)
            .Select(kvp => kvp.Value == 1 ? kvp.Key : $"{kvp.Key}^{kvp.Value}"));
    }

    private Dictionary<string, int> ParseTerm(string term)
    {
        var result = new Dictionary<string, int>();

        // Разбиваем на части по '*'
        var parts = term.Split('*');

        foreach (var part in parts)
        {
            if (part.Contains('^'))
            {
                var subParts = part.Split('^');
                string varName = subParts[0];
                int power = int.Parse(subParts[1]);
                result[varName] = power;
            }
            else
            {
                result[part] = 1;
            }
        }

        return result;
    }

    public override string ToString() => $"({_Left} * {_Right})";
}

public class Divide : BinaryOperation
{
    private IExpr _Left;
    private IExpr _Right;

    public Divide(IExpr x_var, IExpr y_var) : base(x_var, y_var)
    {
        _Left = x_var;
        _Right = y_var;
    }

    public override IEnumerable<string> Variables => _Left.Variables.Union(_Right.Variables);

    public override bool IsConstant => _Left.IsConstant && _Right.IsConstant;

    public override bool IsPolynomial => // Деление является полиномом только если знаменатель - константа
            _Left.IsPolynomial && _Right.IsConstant && _Right.IsPolynomial;

    public override int PolynomialDegree
    {
        get
        {
            if (!IsPolynomial) return -1;
            return _Left.PolynomialDegree; // Деление на константу не меняет степень
        }
    }

    public override double Compute(IReadOnlyDictionary<string, double> variableValues)
    {
        double denominator = _Right.Compute(variableValues);
        if (Math.Abs(denominator) < 1e-10)
            throw new DivideByZeroException("Деление на ноль");
        return _Left.Compute(variableValues) / denominator;
    }

    public override IReadOnlyDictionary<string, double> GetPolynomialCoefficients()
    {
        if (!IsPolynomial)
            throw new InvalidOperationException("Выражение не является полиномом");

        var leftCoeffs = _Left.GetPolynomialCoefficients();
        double divisor = _Right.Compute(new Dictionary<string, double>());

        if (Math.Abs(divisor) < 1e-10)
            throw new DivideByZeroException("Деление на ноль");

        var result = new Dictionary<string, double>();

        foreach ((string key, double coefficient) in leftCoeffs)
        {
            result[key] = coefficient / divisor;
        }

        return result;
    }

    public override string ToString() => $"({_Left} / {_Right})";
}

public abstract class UnaryOperation : Expr
{
    protected IExpr Oper { get; }

    public UnaryOperation(IExpr oper)
    {
        Oper = oper;
    }

}

public class UnaryMinus : UnaryOperation
{
    public UnaryMinus(IExpr oper) : base(oper) { }

    public override IEnumerable<string> Variables => Oper.Variables;

    public override bool IsConstant => Oper.IsConstant;

    public override bool IsPolynomial => Oper.IsPolynomial;

    public override int PolynomialDegree => Oper.PolynomialDegree;

    public override double Compute(IReadOnlyDictionary<string, double> variableValues)
        => -Oper.Compute(variableValues);

    public override IReadOnlyDictionary<string, double> GetPolynomialCoefficients()
    {
        if (!IsPolynomial)
            throw new InvalidOperationException("Выражение не является полиномом");

        var coeffs = Oper.GetPolynomialCoefficients();
        var result = new Dictionary<string, double>();

        foreach ((string key, double coefficient) in coeffs)
        {
            if (!result.TryAdd(key, -coefficient))
                result[key] = -coefficient;
        }

        return result;
    }

    public override string ToString() => $"-({Oper})";
}

public abstract class Function : Expr
{
    protected IExpr Funk { get; }

    public Function(IExpr funk)
    {
        Funk = funk;
    }

}

public class Constant : Expr
{
    double _value;

    public Constant(double value) { _value = value; }

    // Неявные преобразования для Constant

    public override IEnumerable<string> Variables => Enumerable.Empty<string>();

    public override bool IsConstant => true;

    public override bool IsPolynomial => true;

    public override int PolynomialDegree => 0;

    public override double Compute(IReadOnlyDictionary<string, double> variableValues) => _value;


    public override IReadOnlyDictionary<string, double> GetPolynomialCoefficients()
    {
        return new Dictionary<string, double> { { "", _value } };
    }

    public override string ToString()
    {
        return _value.ToString();
    }
}

public class Variable : Expr
{
    string _name;

    public Variable(string name) { _name = name; }

    public static implicit operator Variable(string name) => new Variable(name);

    public override IEnumerable<string> Variables => [_name];

    public override bool IsConstant => false;

    public override bool IsPolynomial => true;

    public override int PolynomialDegree => 1;
    public override double Compute(IReadOnlyDictionary<string, double> variableValues)
    {
        if (!variableValues.ContainsKey(_name))
        {
            throw new ArgumentException($"Значение переменной {_name} не указано");
        }

        return variableValues[_name];
    }

    public override IReadOnlyDictionary<string, double> GetPolynomialCoefficients()
    {
        return new Dictionary<string, double> { { _name, 1.0 } };
    }

    public override string ToString() => _name;
}


// Степенные функции: корень, степень, показательная, логарифм

public class IDegree : BinaryOperation
{
    public IDegree(IExpr x_Var, IExpr y_Var) : base(x_Var, y_Var) { }

    public override IEnumerable<string> Variables
    {
        get
        {
            var baseVars = Left.Variables ?? Enumerable.Empty<string>();
            var exponentVars = Right.Variables ?? Enumerable.Empty<string>();
            return baseVars.Concat(exponentVars).Distinct();
        }
    }

    public override bool IsConstant => Left.IsConstant && Right.IsConstant;

    public override bool IsPolynomial
    {
        get
        {
            if (Left.IsConstant && Right.IsConstant) return true;

            if (!Right.IsConstant) return false; // Показатель должен быть константой

            if (Right is Constant constY)
            {
                try
                {
                    double exponentValue = constY.Compute(new Dictionary<string, double>());
                    bool isInteger = Math.Abs(exponentValue - Math.Round(exponentValue)) < 1e-10;
                    bool isNonNegative = exponentValue >= 0;

                    return isInteger && isNonNegative && Left.IsPolynomial;
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }
    }

    public override int PolynomialDegree
    {
        get
        {
            if (!IsPolynomial) return -1;

            if (Left.IsConstant && Right.IsConstant) return 0;

            try
            {
                double exponentValue = Right.Compute(new Dictionary<string, double>());
                int exponentInt = (int)Math.Round(exponentValue);
                int baseDegree = Left.PolynomialDegree;

                // Если основание - константа (степень 0), то результат тоже константа
                if (baseDegree == 0) return 0;

                return baseDegree * exponentInt;
            }
            catch
            {
                return 0;
            }
        }
    }

    public override double Compute(IReadOnlyDictionary<string, double> variableValues)
    {
        double base_X = Left.Compute(variableValues);
        double degr_Y = Right.Compute(variableValues);

        if (Math.Abs(base_X) < 1e-10 && degr_Y < 0)
        {
            throw new ArgumentException($"Ноль в отрицательной степени {degr_Y}.");
        }

        if (base_X < 0 && Math.Abs(degr_Y % 1) > 1e-10)
        {
            throw new ArgumentException($"Отрицательное число {base_X} в дробной степени {degr_Y}");
        }

        return Math.Pow(base_X, degr_Y);
    }

    public override IReadOnlyDictionary<string, double> GetPolynomialCoefficients()
    {
        if (!IsPolynomial)
            throw new InvalidOperationException("Выражение не является полиномом");

        // Для степеней с целым неотрицательным показателем
        if (Right is Constant constExponent)
        {
            double exponentValue = constExponent.Compute(new Dictionary<string, double>());
            int exponentInt = (int)Math.Round(exponentValue);

            if (exponentInt == 0)
            {
                return new Dictionary<string, double> { { "", 1.0 } };
            }

            // Возводим полином в степень путем многократного умножения
            var result = Left.GetPolynomialCoefficients();

            for (int i = 1; i < exponentInt; i++)
            {
                var temp = new Multiply(new Polynomial(result), new Polynomial(Left.GetPolynomialCoefficients()));
                result = temp.GetPolynomialCoefficients();
            }

            return result;
        }

        throw new InvalidOperationException("Невозможно вычислить коэффициенты для данной степени");
    }

    public override string ToString()
    {
        if (Right is Constant constX)
        {
            double Res = constX.Compute(new Dictionary<string, double>());

            if (Math.Abs(Res - 0.5) < 1e-10) { return $"Sqrt({Left})"; }
        }

        return $"{Left}^{Right}";
    }
}

public class ILog : BinaryOperation
{
    public ILog(IExpr base_x, IExpr degr_y) : base(base_x, degr_y) { }

    public override IEnumerable<string> Variables
    {
        get
        {
            var baseVars = Left.Variables ?? Enumerable.Empty<string>();
            var argumentVars = Right.Variables ?? Enumerable.Empty<string>();
            return baseVars.Concat(argumentVars).Distinct();
        }
    }

    public override bool IsConstant => Left.IsConstant && Right.IsConstant;

    public override bool IsPolynomial => false;

    public override int PolynomialDegree => -1;

    public override double Compute(IReadOnlyDictionary<string, double> variableValues)
    {
        double base_x = Left.Compute(variableValues);
        double degr_y = Right.Compute(variableValues);

        if (base_x <= 0) { throw new ArgumentException($"Основание должно быть больше 0"); }

        if (Math.Abs(base_x - 1) < 1e-10) { throw new ArgumentException($"Основание не может быть 1"); }

        if (degr_y <= 0) { throw new ArgumentException($"Аргумент должен быть больше 0"); }

        double LnBase = Math.Log(base_x);
        double LnDegr = Math.Log(degr_y);

        return LnDegr / LnBase;
    }

    public override IReadOnlyDictionary<string, double> GetPolynomialCoefficients()
    {
        throw new InvalidOperationException("Логарифм не является полиномом");
    }

    public override string ToString() { return $"log[{Left}]({Right})"; }
}

// Вспомогательный класс для представления полинома из коэффициентов
public class Polynomial : IExpr
{
    private IReadOnlyDictionary<string, double> _coefficients;

    public Polynomial(IReadOnlyDictionary<string, double> coefficients)
    {
        _coefficients = coefficients;
    }

    public IEnumerable<string> Variables
    {
        get
        {
            var allVars = new HashSet<string>();
            foreach (var term in _coefficients.Keys)
            {
                if (!string.IsNullOrEmpty(term))
                {
                    var vars = term.Split('*')
                        .Select(v => v.Contains('^') ? v.Split('^')[0] : v);
                    foreach (var varName in vars)
                    {
                        allVars.Add(varName);
                    }
                }
            }
            return allVars;
        }
    }

    public bool IsConstant => _coefficients.All(kvp => string.IsNullOrEmpty(kvp.Key)) && _coefficients.Count == 1;

    public bool IsPolynomial => true;

    public int PolynomialDegree
    {
        get
        {
            int maxDegree = 0;
            foreach (var term in _coefficients.Keys)
            {
                if (string.IsNullOrEmpty(term))
                    continue;

                int termDegree = 0;
                var parts = term.Split('*');
                foreach (var part in parts)
                {
                    if (part.Contains('^'))
                    {
                        termDegree += int.Parse(part.Split('^')[1]);
                    }
                    else
                    {
                        termDegree += 1;
                    }
                }
                maxDegree = Math.Max(maxDegree, termDegree);
            }
            return maxDegree;
        }
    }

    public double Compute(IReadOnlyDictionary<string, double> variableValues)
    {
        double result = 0;
        foreach (var kvp in _coefficients)
        {
            if (string.IsNullOrEmpty(kvp.Key))
            {
                result += kvp.Value;
            }
            else
            {
                double termValue = kvp.Value;
                var parts = kvp.Key.Split('*');
                foreach (var part in parts)
                {
                    if (part.Contains('^'))
                    {
                        var subParts = part.Split('^');
                        string varName = subParts[0];
                        int power = int.Parse(subParts[1]);
                        if (!variableValues.ContainsKey(varName))
                            throw new ArgumentException($"Значение переменной {varName} не указано");
                        termValue *= Math.Pow(variableValues[varName], power);
                    }
                    else
                    {
                        if (!variableValues.ContainsKey(part))
                            throw new ArgumentException($"Значение переменной {part} не указано");
                        termValue *= variableValues[part];
                    }
                }
                result += termValue;
            }
        }
        return result;
    }

    public IReadOnlyDictionary<string, double> GetPolynomialCoefficients() => _coefficients;

    public override string ToString()
    {
        var terms = new List<string>();
        foreach (var kvp in _coefficients.OrderByDescending(kvp =>
        {
            if (string.IsNullOrEmpty(kvp.Key)) return 0;
            int degree = 0;
            var parts = kvp.Key.Split('*');
            foreach (var part in parts)
            {
                if (part.Contains('^'))
                    degree += int.Parse(part.Split('^')[1]);
                else
                    degree += 1;
            }
            return degree;
        }))
        {
            if (Math.Abs(kvp.Value) < 1e-10)
                continue;

            string termStr;
            if (string.IsNullOrEmpty(kvp.Key))
            {
                termStr = kvp.Value.ToString();
            }
            else
            {
                string coeffStr = Math.Abs(kvp.Value - 1) < 1e-10 ? "" :
                                 Math.Abs(kvp.Value + 1) < 1e-10 ? "-" :
                                 $"{kvp.Value}*";
                termStr = $"{coeffStr}{kvp.Key}";
            }

            terms.Add(termStr);
        }

        if (terms.Count == 0)
            return "0";

        return string.Join(" + ", terms).Replace("+ -", "- ");
    }
}

class Program
{
    public static IDegree Sqrt(IExpr expr) => new IDegree(expr, new Constant(0.5));

    // Метод для красивого вывода коэффициентов
    public static void PrintCoefficients(IReadOnlyDictionary<string, double> coefficients, string expressionName)
    {
        Console.WriteLine($"\n{expressionName}:");
        if (coefficients.Count == 0)
        {
            Console.WriteLine("  Нет коэффициентов (выражение не является полиномом)");
            return;
        }

        foreach (var kvp in coefficients.OrderByDescending(kvp =>
        {
            if (string.IsNullOrEmpty(kvp.Key)) return 0;
            int degree = 0;
            var parts = kvp.Key.Split('*');
            foreach (var part in parts)
            {
                if (part.Contains('^'))
                    degree += int.Parse(part.Split('^')[1]);
                else
                    degree += 1;
            }
            return degree;
        }))
        {
            string termName = string.IsNullOrEmpty(kvp.Key) ? "константа" : kvp.Key;
            Console.WriteLine($"  {termName}: {kvp.Value}");
        }
    }

    static void Main()
    {
        Expr x = new Variable("x");
        Expr y = new Variable("y");
        Expr c = new Constant(3);

        Console.WriteLine("Тестирование вычисления коэффициентов полинома:");
        Console.WriteLine("==============================================");

        // Тест 1: Простой полином
        var expr1 = x * x + 2 * x + 1;
        Console.WriteLine($"\nТест 1 - Квадратный полином: {expr1}");
        var coeffs1 = expr1.GetPolynomialCoefficients();
        PrintCoefficients(coeffs1, "Коэффициенты");

        // Тест 2: Полином с несколькими переменными
        var expr2 = x * x + 2 * x * y + y * y;
        Console.WriteLine($"\nТест 2 - Полином двух переменных: {expr2}");
        var coeffs2 = expr2.GetPolynomialCoefficients();
        PrintCoefficients(coeffs2, "Коэффициенты");

        // Тест 3: Умножение полиномов
        var expr3 = (x + 1) * (x + 2);
        Console.WriteLine($"\nТест 3 - Умножение полиномов: {expr3}");
        var coeffs3 = expr3.GetPolynomialCoefficients();
        PrintCoefficients(coeffs3, "Коэффициенты");

        // Тест 4: Деление на константу
        var expr4 = (x * x + 2 * x + 1) / 2;
        Console.WriteLine($"\nТест 4 - Деление на константу: {expr4}");
        var coeffs4 = expr4.GetPolynomialCoefficients();
        PrintCoefficients(coeffs4, "Коэффициенты");

        // Тест 5: Степень полинома
        var expr5 = new IDegree(x + 1, new Constant(3));
        Console.WriteLine($"\nТест 5 - Полином в степени: {expr5}");
        var coeffs5 = expr5.GetPolynomialCoefficients();
        PrintCoefficients(coeffs5, "Коэффициенты");

        // Тест 6: Сложное выражение
        var expr6 = (x + y) * (x - y) + 2 * x * y;
        Console.WriteLine($"\nТест 6 - Сложное выражение: {expr6}");
        var coeffs6 = expr6.GetPolynomialCoefficients();
        PrintCoefficients(coeffs6, "Коэффициенты");

        // Тест 7: Проверка вычислений через коэффициенты
        Console.WriteLine($"\nПроверка вычислений через коэффициенты:");
        var poly = new Polynomial(coeffs1);
        Console.WriteLine($"Исходное выражение: {expr1}");
        Console.WriteLine($"Полином из коэффициентов: {poly}");
        Console.WriteLine($"Вычисление при x=2: {expr1.Compute(new Dictionary<string, double> { ["x"] = 2 })}");
        Console.WriteLine($"Вычисление через полином при x=2: {poly.Compute(new Dictionary<string, double> { ["x"] = 2 })}");

        // Тест 8: Выражение с тремя переменными
        var z = new Variable("z");
        var expr8 = x * y + y * z + z * x;
        Console.WriteLine($"\nТест 8 - Полином трех переменных: {expr8}");
        var coeffs8 = expr8.GetPolynomialCoefficients();
        PrintCoefficients(coeffs8, "Коэффициенты");

        // ТЕСТИРОВАНИЕ УНАРНОГО МИНУСА
        Console.WriteLine("\n\nТЕСТИРОВАНИЕ УНАРНОГО МИНУСА:");
        Console.WriteLine("==============================");

        // Тест 1: Унарный минус с константой
        var unary1 = -new Constant(5);
        Console.WriteLine($"\nТест 1 - Унарный минус с константой: {unary1}");
        Console.WriteLine($"Переменные: [{string.Join(", ", unary1.Variables)}]");
        Console.WriteLine($"Является константой: {unary1.IsConstant} (ожидается: True)");
        Console.WriteLine($"Является полиномом: {unary1.IsPolynomial} (ожидается: True)");
        Console.WriteLine($"Степень полинома: {unary1.PolynomialDegree} (ожидается: 0)");
        Console.WriteLine($"Вычисление: {unary1.Compute(new Dictionary<string, double>())} (ожидается: -5)");
        var coeffsUnary1 = unary1.GetPolynomialCoefficients();
        PrintCoefficients(coeffsUnary1, "Коэффициенты унарного минуса константы");

        // Тест 2: Унарный минус с переменной
        var unary2 = -x;
        Console.WriteLine($"\nТест 2 - Унарный минус с переменной: {unary2}");
        Console.WriteLine($"Переменные: [{string.Join(", ", unary2.Variables)}]");
        Console.WriteLine($"Является константой: {unary2.IsConstant} (ожидается: False)");
        Console.WriteLine($"Является полиномом: {unary2.IsPolynomial} (ожидается: True)");
        Console.WriteLine($"Степень полинома: {unary2.PolynomialDegree} (ожидается: 1)");
        Console.WriteLine($"Вычисление при x=3: {unary2.Compute(new Dictionary<string, double> { ["x"] = 3 })} (ожидается: -3)");
        var coeffsUnary2 = unary2.GetPolynomialCoefficients();
        PrintCoefficients(coeffsUnary2, "Коэффициенты унарного минуса переменной");

        // Тест 3: Унарный минус с полиномом
        var unary3 = -(x * x + 2 * x + 1);
        Console.WriteLine($"\nТест 3 - Унарный минус с полиномом: {unary3}");
        Console.WriteLine($"Переменные: [{string.Join(", ", unary3.Variables)}]");
        Console.WriteLine($"Является константой: {unary3.IsConstant} (ожидается: False)");
        Console.WriteLine($"Является полиномом: {unary3.IsPolynomial} (ожидается: True)");
        Console.WriteLine($"Степень полинома: {unary3.PolynomialDegree} (ожидается: 2)");
        Console.WriteLine($"Вычисление при x=2: {unary3.Compute(new Dictionary<string, double> { ["x"] = 2 })} (ожидается: -9)");
        var coeffsUnary3 = unary3.GetPolynomialCoefficients();
        PrintCoefficients(coeffsUnary3, "Коэффициенты унарного минуса полинома");

        // Тест 4: Комбинация унарного минуса с другими операциями
        var unary4 = -x + y;
        Console.WriteLine($"\nТест 4 - Комбинация унарного минуса с сложением: {unary4}");
        Console.WriteLine($"Вычисление при x=2, y=5: {unary4.Compute(new Dictionary<string, double> { ["x"] = 2, ["y"] = 5 })} (ожидается: 3)");

        var unary5 = -x * y;
        Console.WriteLine($"\nТест 5 - Комбинация унарного минуса с умножением: {unary5}");
        Console.WriteLine($"Вычисление при x=3, y=4: {unary5.Compute(new Dictionary<string, double> { ["x"] = 3, ["y"] = 4 })} (ожидается: -12)");

        var unary6 = -(-x);
        Console.WriteLine($"\nТест 6 - Двойной унарный минус: {unary6}");
        Console.WriteLine($"Вычисление при x=7: {unary6.Compute(new Dictionary<string, double> { ["x"] = 7 })} (ожидается: 7)");

        // Тест 7: Унарный минус в сложных выражениях
        var unary7 = -(x + y) * (x - y);
        Console.WriteLine($"\nТест 7 - Унарный минус в сложном выражении: {unary7}");
        Console.WriteLine($"Вычисление при x=3, y=2: {unary7.Compute(new Dictionary<string, double> { ["x"] = 3, ["y"] = 2 })} (ожидается: -5)");
        var coeffsUnary7 = unary7.GetPolynomialCoefficients();
        PrintCoefficients(coeffsUnary7, "Коэффициенты сложного выражения с унарным минусом");

        // Тест 8: Унарный минус с делением
        var unary8 = -(x / 2);
        Console.WriteLine($"\nТест 8 - Унарный минус с делением: {unary8}");
        Console.WriteLine($"Вычисление при x=6: {unary8.Compute(new Dictionary<string, double> { ["x"] = 6 })} (ожидается: -3)");

        z = new Variable("x");
        z = new Variable("y");
        c = new Constant(3);

        Console.WriteLine("Тестирование выражений:");
        Console.WriteLine("=======================");

        z = new Variable("z");

        // Тест 1: Простой полином
        var expr11 = x * x + 2 * x + (-1);
        Console.WriteLine(expr11);
        Console.WriteLine($"\nТест 1 - Квадратный полином: {expr11}");
        Console.WriteLine($"Переменные: [{string.Join(", ", expr11.Variables)}]");
        Console.WriteLine($"Является константой: {expr11.IsConstant} (ожидается: False)");
        Console.WriteLine($"Является полиномом: {expr11.IsPolynomial} (ожидается: True)");
        Console.WriteLine($"Степень полинома: {expr11.PolynomialDegree} (ожидается: 2)");
        Console.WriteLine($"Вычисление при x=3: {expr11.Compute(new Dictionary<string, double> { ["x"] = 3 })} (ожидается: 14)");

        // Тест 2: Деление на константу (должно быть полиномом)
        var expr12 = (x * x + y) / 5;
        Console.WriteLine($"\nТест 2 - Деление на константу: {expr12}");
        Console.WriteLine($"Переменные: [{string.Join(", ", expr12.Variables)}] (ожидается: [\"x\", \"y\"])");
        Console.WriteLine($"Является константой: {expr12.IsConstant} (ожидается: False)");
        Console.WriteLine($"Является полиномом: {expr12.IsPolynomial} (ожидается: True)");
        Console.WriteLine($"Степень полинома: {expr12.PolynomialDegree} (ожидается: 2)");
        Console.WriteLine($"Вычисление при x=2, y=3: {expr12.Compute(new Dictionary<string, double> { ["x"] = 2, ["y"] = 3 })} (ожидается: 1.4)");

        // Тест 3: Деление на переменную (НЕ полином)
        var expr13 = (x + 1) / y;
        Console.WriteLine($"\nТест 3 - Деление на переменную: {expr13}");
        Console.WriteLine($"Переменные: [{string.Join(", ", expr13.Variables)}] (ожидается: [\"x\", \"y\"])");
        Console.WriteLine($"Является константой: {expr13.IsConstant} (ожидается: False)");
        Console.WriteLine($"Является полиномом: {expr13.IsPolynomial} (ожидается: False)");
        Console.WriteLine($"Степень полинома: {expr13.PolynomialDegree} (ожидается: -1)");

        // Тест 4: Степень с целым показателем (полином)
        var expr14 = new IDegree(x, new Constant(3));
        Console.WriteLine($"\nТест 4 - Степень с целым показателем: {expr14}");
        Console.WriteLine($"Переменные: [{string.Join(", ", expr14.Variables)}] (ожидается: [\"x\"])");
        Console.WriteLine($"Является константой: {expr14.IsConstant} (ожидается: False)");
        Console.WriteLine($"Является полиномом: {expr14.IsPolynomial} (ожидается: True)");
        Console.WriteLine($"Степень полинома: {expr14.PolynomialDegree} (ожидается: 3)");
        Console.WriteLine($"Вычисление при x=2: {expr14.Compute(new Dictionary<string, double> { ["x"] = 2 })} (ожидается: 8)");

        // Тест 5: Корень (НЕ полином)
        var expr15 = Sqrt(x);
        Console.WriteLine($"\nТест 5 - Квадратный корень: {expr15}");
        Console.WriteLine($"Переменные: [{string.Join(", ", expr15.Variables)}] (ожидается: [\"x\"])");
        Console.WriteLine($"Является константой: {expr15.IsConstant} (ожидается: False)");
        Console.WriteLine($"Является полиномом: {expr15.IsPolynomial} (ожидается: False)");
        Console.WriteLine($"Степень полинома: {expr15.PolynomialDegree} (ожидается: -1)");

        // Тест 7: Сложное выражение с умножением переменных
        var expr17 = x * y * z;
        Console.WriteLine($"\nТест 7 - Умножение трех переменных: {expr17}");
        Console.WriteLine($"Переменные: [{string.Join(", ", expr17.Variables)}] (ожидается: [\"x\", \"y\", \"z\"])");
        Console.WriteLine($"Является константой: {expr17.IsConstant} (ожидается: False)");
        Console.WriteLine($"Является полиномом: {expr17.IsPolynomial} (ожидается: True)");
        Console.WriteLine($"Степень полинома: {expr17.PolynomialDegree} (ожидается: 3)");
        Console.WriteLine($"Вычисление при x=2, y=3, z=4: {expr17.Compute(new Dictionary<string, double> { ["x"] = 2, ["y"] = 3, ["z"] = 4 })} (ожидается: 24)");

        // Тест 8: Отрицательная степень (НЕ полином)
        var expr18 = new IDegree(x, new Constant(-2));
        Console.WriteLine($"\nТест 8 - Отрицательная степень: {expr18}");
        Console.WriteLine($"Переменные: [{string.Join(", ", expr18.Variables)}] (ожидается: [\"x\"])");
        Console.WriteLine($"Является константой: {expr18.IsConstant} (ожидается: False)");
        Console.WriteLine($"Является полиномом: {expr18.IsPolynomial} (ожидается: False)");
        Console.WriteLine($"Степень полинома: {expr18.PolynomialDegree} (ожидается: -1)");

        // Тест 9: Дробная степень (НЕ полином)
        var expr19 = new IDegree(x, new Constant(1.5));
        Console.WriteLine($"\nТест 9 - Дробная степень: {expr19}");
        Console.WriteLine($"Переменные: [{string.Join(", ", expr19.Variables)}] (ожидается: [\"x\"])");
        Console.WriteLine($"Является константой: {expr19.IsConstant} (ожидается: False)");
        Console.WriteLine($"Является полиномом: {expr19.IsPolynomial} (ожидается: False)");
        Console.WriteLine($"Степень полинома: {expr19.PolynomialDegree} (ожидается: -1)");

        // Тест 10: Смешанное выражение
        var expr110 = (x * x + 2 * x * y + y * y) / 4;
        Console.WriteLine($"\nТест 10 - Смешанное выражение: {expr110}");
        Console.WriteLine($"Переменные: [{string.Join(", ", expr110.Variables)}] (ожидается: [\"x\", \"y\"])");
        Console.WriteLine($"Является константой: {expr110.IsConstant} (ожидается: False)");
        Console.WriteLine($"Является полиномом: {expr110.IsPolynomial} (ожидается: True)");
        Console.WriteLine($"Степень полинома: {expr110.PolynomialDegree} (ожидается: 2)");
        Console.WriteLine($"Вычисление при x=2, y=2: {expr110.Compute(new Dictionary<string, double> { ["x"] = 2, ["y"] = 2 })} (ожидается: 4)");
    }
}
