using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_OPP_NEW
{
    public class Degree : BinaryOperation
    {
        public Degree(IExpr x_Var, IExpr y_Var) : base(x_Var, y_Var) { }

        public override IEnumerable<string> Variables
        {
            get
            {
                var baseVars = Left.Variables;
                var exponentVars = Right.Variables;
                return baseVars.Concat(exponentVars).Distinct();
            }
        }

        public override bool IsConstant => Left.IsConstant && Right.IsConstant;

        public override bool IsPolynomial
        {
            get
            {
                if (Left.IsConstant && Right.IsConstant) return true;

                if (!Right.IsConstant) return false; // Показатель должен быть константой

                if (Right is Constant constY)
                {
                    try
                    {
                        double exponentValue = constY.Compute(new Dictionary<string, double>());
                        bool isInteger = Math.Abs(exponentValue - Math.Round(exponentValue)) < 1e-10;
                        bool isNonNegative = exponentValue >= 0;

                        return isInteger && isNonNegative && Left.IsPolynomial;
                    }
                    finally { }
                }

                return false;
            }
        }

        public override int PolynomialDegree
        {
            get
            {
                if (!IsPolynomial) return -1;

                if (Left.IsConstant && Right.IsConstant) return 0;

                try
                {
                    double exponentValue = Right.Compute(new Dictionary<string, double>());
                    int exponentInt = (int)Math.Round(exponentValue);
                    int baseDegree = Left.PolynomialDegree;

                    return baseDegree * exponentInt;
                }
                finally { }
            }
        }

        public override double Compute(IReadOnlyDictionary<string, double> variableValues)
        {
            double base_X = Left.Compute(variableValues);
            double degr_Y = Right.Compute(variableValues);

            if (Math.Abs(base_X) < 1e-10 && degr_Y < 0)
            {
                throw new ArgumentException($"Ноль в отрицательной степени {degr_Y}.");
            }

            if (base_X < 0 && Math.Abs(degr_Y % 1) > 1e-10)
            {
                throw new ArgumentException($"Отрицательное число {base_X} в дробной степени {degr_Y}");
            }

            return Math.Pow(base_X, degr_Y);
        }

        public override IReadOnlyDictionary<string, double> GetPolynomialCoefficients()
        {
            if (!IsPolynomial)
                throw new InvalidOperationException("Выражение не является полиномом");

            // Для степеней с целым неотрицательным показателем
            if (Right is Constant constExponent)
            {
                double exponentValue = constExponent.Compute(new Dictionary<string, double>());
                int exponentInt = (int)Math.Round(exponentValue);

                if (exponentInt == 0)
                {
                    return new Dictionary<string, double> { { "", 1.0 } };
                }

                // Возводим полином в степень путем многократного умножения
                var result = Left.GetPolynomialCoefficients();

                for (int i = 1; i < exponentInt; i++)
                {
                    var temp = new Multiply(new Polynomial(result), new Polynomial(Left.GetPolynomialCoefficients()));
                    result = temp.GetPolynomialCoefficients();
                }

                return result;
            }

            throw new InvalidOperationException("Невозможно вычислить коэффициенты для данной степени");
        }

        public override string ToString()
        {
            if (Right is Constant constX)
            {
                double Res = constX.Compute(new Dictionary<string, double>());

                if (Math.Abs(Res - 0.5) < 1e-10) { return $"Sqrt({Left})"; }
            }

            return $"{Left}^{Right}";
        }
    }

}
