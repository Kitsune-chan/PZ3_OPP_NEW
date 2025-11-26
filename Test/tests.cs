using PZ3_OPP_NEW;
using System.Reflection.Metadata;
using Constant = PZ3_OPP_NEW.Constant;

namespace Test
{
    public class tests
    {
        Expr x = new Variable("x");
        Expr y = new Variable("y");
        Expr c = new Constant(3);
        Expr unary1 = -new Constant(5);


        [Fact]
        public void Test1() //проверка конструктора
        {
            
            double expected = 42.5;
            var constant = new Constant(expected);
            var result = constant.Compute(new Dictionary<string, double>());
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test2() //свойства константы
        {
            Assert.True(unary1.IsConstant);
            Assert.True(unary1.IsPolynomial);
            Assert.Empty(unary1.Variables);
            Assert.Equal(0, unary1.PolynomialDegree);
            Assert.Equal(-5, unary1.Compute(new Dictionary<string, double>()));

        }

     }
}
