using PZ3_OPP_NEW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Program
{
    public static Degree Sqrt(IExpr expr) => new Degree(expr, new Constant(0.5));

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

        var coefficients = (-((x + y)) * (x - y)).GetPolynomialCoefficients();

        Console.WriteLine(string.Join("" ,(-((x + y)) * (x - y)).GetPolynomialCoefficients()));

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
        var expr5 = new Degree(x + 1, new Constant(3));
        Console.WriteLine($"\nТест 5 - Полином в степени: {expr5}");
        var coeffs5 = expr5.GetPolynomialCoefficients();
        PrintCoefficients(coeffs5, "Коэффициенты");

        // Тест 6: Сложное выражение
        var expr6 = (x + y) * (x - y) + 2 * x * y;
        Console.WriteLine($"\nТест 6 - Сложное выражение: {expr6}");
        var coeffs6 = expr6.GetPolynomialCoefficients();
        PrintCoefficients(coeffs6, "Коэффициенты");

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
        var expr14 = new Degree(x, new Constant(3));
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
        var expr18 = new Degree(x, new Constant(0));
        Console.WriteLine($"\nТест 8 - Отрицательная степень: {expr18}");
        Console.WriteLine($"Переменные: [{string.Join(", ", expr18.Variables)}] (ожидается: [\"x\"])");
        Console.WriteLine($"Является константой: {expr18.IsConstant} (ожидается: False)");
        Console.WriteLine($"Является полиномом: {expr18.IsPolynomial} (ожидается: False)");
        Console.WriteLine($"Степень полинома: {string.Join("" ,expr18.GetPolynomialCoefficients())} (ожидается: -1)");

        // Тест 9: Дробная степень (НЕ полином)
        var expr19 = new Degree(x, new Constant(1.5));
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
