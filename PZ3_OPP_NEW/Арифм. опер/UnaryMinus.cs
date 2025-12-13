using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_OPP_NEW
{
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
                result.Add(key, -coefficient);
            }

            return result;
        }

        public override string ToString() => $"-({Oper})";
    }
}
