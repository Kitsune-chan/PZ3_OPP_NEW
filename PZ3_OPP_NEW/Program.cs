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

    static void Main() { }
        
}
