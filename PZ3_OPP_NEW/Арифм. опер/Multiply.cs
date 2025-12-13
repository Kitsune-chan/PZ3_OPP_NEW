using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_OPP_NEW
{
    public class Multiply : BinaryOperation
    {
        private IExpr _Left;
        private IExpr _Right;

        public Multiply(IExpr x_var, IExpr y_var) : base(x_var, y_var)
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
                resultVars.Add(key, coefficient);
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
}
