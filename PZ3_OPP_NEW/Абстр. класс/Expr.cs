using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_OPP_NEW
{
    public abstract class Expr : IExpr
    {
        public static implicit operator Expr(int value) => new Constant(value);
        public static implicit operator Expr(double value) => new Constant(value);

        public abstract IEnumerable<string> Variables { get; }
        public abstract bool IsConstant { get; }
        public abstract bool IsPolynomial { get; }
        public abstract int PolynomialDegree { get; }

        public abstract double Compute(IReadOnlyDictionary<string, double> variableValues);
        public abstract IReadOnlyDictionary<string, double> GetPolynomialCoefficients();
        public static Expr operator -(Expr a, Expr b) => new Subtract(a, b);
        public static Expr operator +(Expr a, Expr b) => new Add(a, b);
        public static Expr operator *(Expr a, Expr b) => new Multiply(a, b);
        public static Expr operator /(Expr a, Expr b) => new Divide(a, b);
        public static Expr operator ^(Expr a, Expr b) => new Degree(a, b);
        public static Expr log(Expr a, Expr b) => new ILog(a, b);
        public static Expr operator -(Expr a) => new UnaryMinus(a);
        public abstract override string ToString();
    }
}
