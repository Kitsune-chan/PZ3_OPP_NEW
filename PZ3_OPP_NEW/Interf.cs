using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_OPP_NEW
{
    public interface IExpr
    {
        IEnumerable<string> Variables { get; } //переменные
        bool IsConstant { get; } //константы
        bool IsPolynomial { get; } //полиномы
        int PolynomialDegree { get; } //степень полинома
        double Compute(IReadOnlyDictionary<string, double> variableValues);

        // Новый метод для получения коэффициентов полинома
        IReadOnlyDictionary<string, double> GetPolynomialCoefficients();

    }
}
