using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_OPP_NEW
{
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
}
