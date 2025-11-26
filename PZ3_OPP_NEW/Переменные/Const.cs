using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_OPP_NEW
{
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
            return $"({_value.ToString()})";
        }
    }
}
