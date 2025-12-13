using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_OPP_NEW
{
    public class Variable : Expr
    {
        string _name;

        public Variable(string name) { _name = name; }

        //public static implicit operator Variable(string name) => new Variable(name);

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
}
