using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_OPP_NEW
{
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
}
