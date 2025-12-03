using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_OPP_NEW
{
    public class ILog : BinaryOperation
    {
        public ILog(IExpr base_x, IExpr degr_y) : base(base_x, degr_y) { }

        public override IEnumerable<string> Variables
        {
            get
            {
                var baseVars = Left.Variables;
                var argumentVars = Right.Variables;
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

}
