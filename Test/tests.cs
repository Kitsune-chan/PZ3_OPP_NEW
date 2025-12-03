using PZ3_OPP_NEW;
using System.Reflection.Metadata;
using Constant = PZ3_OPP_NEW.Constant;

using static PZ3_OPP_NEW.Expr;

namespace Test
{
    public class arithmeticOperations
    {
        Expr x = new Variable("x");
        Expr y = new Variable("y");
        Expr z = new Variable("z");
        Expr c = new Constant(3);
        Expr unary1 = -new Constant(5);


        [Fact]
        public void TestConstant() //свойства константы
        {
            //base
            double expected = 42.5;
            var constant = new Constant(expected);
            var result = constant.Compute(new Dictionary<string, double>());
            Assert.Equal(expected, result);

            Assert.True(unary1.IsConstant);
            Assert.True(unary1.IsPolynomial);
            Assert.Empty(unary1.Variables);
            Assert.Equal(0, unary1.PolynomialDegree);
            Assert.Equal(-5, unary1.Compute(new Dictionary<string, double>()));

        }

        [Fact]
        public void TestVariable() //свойства variable
        {
            //base
            string expected = "[x]";
            var alternating = new Variable(expected);
            var result = $"{string.Join("", alternating.Variables)}";
            Assert.Equal(expected, result);

            Assert.False(x.IsConstant);
            Assert.True(x.IsPolynomial);
            Assert.Equal("[x]", $"[{string.Join(", ", x.Variables)}]");
            Assert.Equal(1, x.PolynomialDegree);
            Assert.Equal(3, x.Compute(new Dictionary<string, double> { ["x"] = 3 }));

        }

        [Fact]
        public void TestAddFunc() //свойства Add
        {
            //base
            Assert.False((x + y).IsConstant);
            Assert.False((x + unary1).IsConstant);
            Assert.True((unary1 + unary1).IsConstant);
            Assert.True((x + y).IsPolynomial);
            Assert.True((x + unary1).IsPolynomial);
            Assert.True((unary1 + unary1).IsPolynomial);
            Assert.Equal("[x, y]", $"[{string.Join(", ", (x + y).Variables)}]");
            Assert.Equal(1, (x + y).PolynomialDegree);
            Assert.Equal(1, (x + unary1).PolynomialDegree);
            Assert.Equal(0, (unary1 + unary1).PolynomialDegree);
            Assert.Equal(5, (x + y).Compute(new Dictionary<string, double> { ["x"] = 3, ["y"] = 2 }));
            Assert.Equal(0, (x + unary1).Compute(new Dictionary<string, double> { ["x"] = 5 }));

        }

        [Fact]
        public void TestSubtractFunc() //свойства Subtract
        {
            //base
            Assert.False((x - y).IsConstant);
            Assert.False((x - unary1).IsConstant);
            Assert.True((unary1 - unary1).IsConstant);
            Assert.True((x - y).IsPolynomial);
            Assert.True((x - unary1).IsPolynomial);
            Assert.True((unary1 - unary1).IsPolynomial);
            Assert.Equal("[x, y]", $"[{string.Join(", ", (x - y).Variables)}]");
            Assert.Equal(1, (x - y).PolynomialDegree);
            Assert.Equal(1, (x - unary1).PolynomialDegree);
            Assert.Equal(0, (unary1 - unary1).PolynomialDegree);
            Assert.Equal(1, (x - y).Compute(new Dictionary<string, double> { ["x"] = 3, ["y"] = 2 }));
            Assert.Equal(10, (x - unary1).Compute(new Dictionary<string, double> { ["x"] = 5 }));

        }

        [Fact]
        public void TestMultiplyFunc() //свойства Multiply
        {
            //base
            Assert.False((x * y).IsConstant);
            Assert.False((x * unary1).IsConstant);
            Assert.True((unary1 * unary1).IsConstant);
            Assert.True((x * y).IsPolynomial);
            Assert.True((x * unary1).IsPolynomial);
            Assert.True((unary1 * unary1).IsPolynomial);
            Assert.Equal("[x, y]", $"[{string.Join(", ", (x * y).Variables)}]");
            Assert.Equal(2, (x * y).PolynomialDegree);
            Assert.Equal(1, (x * unary1).PolynomialDegree);
            Assert.Equal(0, (unary1 * unary1).PolynomialDegree);
            Assert.Equal(6, (x * y).Compute(new Dictionary<string, double> { ["x"] = 3, ["y"] = 2 }));
            Assert.Equal(-25, (x * unary1).Compute(new Dictionary<string, double> { ["x"] = 5 }));

            //expressions
            //1)
            Assert.False(((x + y) * (x - y)).IsConstant);
            Assert.True(((x + y) * (x - y)).IsPolynomial);
            Assert.Equal(2, ((x + y) * (x - y)).PolynomialDegree);
            Assert.Equal(5, ((x + y) * (x - y)).Compute(new Dictionary<string, double> { ["x"] = 3, ["y"] = 2 }));
            
            //2)
            Assert.False(((x + unary1) * (x - unary1)).IsConstant);
            Assert.True(((x + unary1) * (x - unary1)).IsPolynomial);
            Assert.Equal(2, ((x + unary1) * (x - unary1)).PolynomialDegree);
            Assert.Equal(24, ((x + unary1) * (x - unary1)).Compute(new Dictionary<string, double> { ["x"] = 7 }));


        }

        [Fact]
        public void TestDivideFunc() //свойства Divide
        {
            //base
            Assert.False((x / y).IsConstant);
            Assert.False((x / unary1).IsConstant);
            Assert.True((unary1 / unary1).IsConstant);
            Assert.False((x / y).IsPolynomial);
            Assert.False((unary1 / x).IsPolynomial);
            Assert.True((x / unary1).IsPolynomial);
            Assert.True((unary1 / unary1).IsPolynomial);
            Assert.Equal("[x, y]", $"[{string.Join(", ", (x / y).Variables)}]");
            Assert.Equal(-1, (x / y).PolynomialDegree);
            Assert.Equal(1, (x / unary1).PolynomialDegree);
            Assert.Equal(0, (unary1 / unary1).PolynomialDegree);
            Assert.Equal(1.5, (x / y).Compute(new Dictionary<string, double> { ["x"] = 3, ["y"] = 2 }));
            Assert.Equal(-1, (x / unary1).Compute(new Dictionary<string, double> { ["x"] = 5 }));

            //expressions
            //1)
            Assert.False(((x * y) / unary1).IsConstant);
            Assert.True(((x * y) / unary1).IsPolynomial);
            Assert.Equal(2, ((x * y) / unary1).PolynomialDegree);
            Assert.Equal(-3, ((x * y) / unary1).Compute(new Dictionary<string, double> { ["x"] = 3, ["y"] = 5 }));

            //2)
            Assert.False(((x + y) * (x / z)).IsConstant);
            Assert.True(((x + y) * (x / c)).IsPolynomial);
            Assert.False(((x + y) * (x / z)).IsPolynomial);
            Assert.True(((x + y) * (x / c)).IsPolynomial);
            Assert.Equal(-1, ((x + y) * (x / z)).PolynomialDegree);
            Assert.Equal(2, ((x + y) * (x / c)).PolynomialDegree);
            Assert.Equal(18, ((x + y) * (x / z)).Compute(new Dictionary<string, double> { ["x"] = 4, ["y"] = 5, ["z"] = 2 }));
            Assert.Equal(12, ((x + y) * (x / c)).Compute(new Dictionary<string, double> { ["x"] = 4, ["y"] = 5, ["z"] = 2 }));

            var func = x / 0; // Только логарифм

            // Act & Assert
            var exception = Assert.Throws<DivideByZeroException>(
                () => func.GetPolynomialCoefficients()
            );

            Assert.Contains("Деление на ноль", exception.Message);


        }

        [Fact]
        public void TestUnaryMinusFunc() //свойства UnaryMinus
        {
            //base
            Assert.False((-x).IsConstant);
            Assert.True((-unary1).IsConstant);
            Assert.True((-x).IsPolynomial);
            Assert.True((-unary1).IsPolynomial);
            Assert.Equal("[x, y]", $"[{string.Join(", ", (x - (-y)).Variables)}]");
            Assert.Equal(1, (x - (-y)).PolynomialDegree);
            Assert.Equal(5, (x - (-y)).Compute(new Dictionary<string, double> { ["x"] = 3, ["y"] = 2 }));
            Assert.Equal(1, ((-x) / unary1).Compute(new Dictionary<string, double> { ["x"] = 5 }));

            //expressions
            //1)
            Assert.False((-(x * y) / unary1).IsConstant);
            Assert.True((-(x * y) / unary1).IsPolynomial);
            Assert.Equal(2, (-(x * y) / unary1).PolynomialDegree);
            Assert.Equal(3, (-(x * y) / unary1).Compute(new Dictionary<string, double> { ["x"] = 3, ["y"] = 5 }));

            //2)
            Assert.False((-x + y * unary1).IsConstant);
            Assert.True((-x + y * unary1).IsPolynomial);
            Assert.Equal(1, (-x + y * unary1).PolynomialDegree);
            Assert.Equal(-29, (-x + y * unary1).Compute(new Dictionary<string, double> { ["x"] = 4, ["y"] = 5 }));
        }
    }

    public class degreeFunctions
    {
        Expr x = new Variable("x");
        Expr y = new Variable("y");
        Expr z = new Variable("z");
        Expr c = new Constant(3);
        Expr unary1 = -new Constant(5);

        [Fact]   
        public void TestDegreeFunc() //свойства Degree
        {
            //base
            Assert.False((x^(y + z)).IsConstant);
            Assert.True((c ^ unary1).IsConstant);
            Assert.True((x ^ c).IsPolynomial);
            Assert.False((x ^ (y + z)).IsPolynomial);
            Assert.True((c ^ unary1).IsPolynomial);
            Assert.False((x ^ unary1).IsPolynomial);
            Assert.Equal("[x, y, z]", $"[{string.Join(", ", (x^(y + z)).Variables)}]");
            Assert.Equal(-1, (x ^ unary1).PolynomialDegree);
            Assert.Equal(3, (x ^ c).PolynomialDegree);
            Assert.Equal(-1, (x ^ y).PolynomialDegree);
            Assert.Equal(0, (c ^ unary1).PolynomialDegree);
            Assert.Equal(0, (c ^ c).PolynomialDegree);
            Assert.Equal(-1, (c ^ y).PolynomialDegree);
            Assert.Equal(27, (x ^ (y + z)).Compute(new Dictionary<string, double> { ["x"] = 3, ["y"] = 1, ["z"] = 2 }));
            Assert.Equal(152.0/3.0, ((((x + y) ^ c) + (y ^ c)) / c).Compute(new Dictionary<string, double> { ["x"] = 2, ["y"] = 3 }));

            //expressions
            //1)
            Assert.False((x ^ 2 + y ^ 2 - c).IsConstant);
            Assert.True((unary1 ^ c).IsConstant);
            Assert.True(((x ^ 2) + (y ^ 2) - c).IsPolynomial);
            Assert.True((unary1 ^ c).IsPolynomial);
            Assert.Equal(2, ((x ^ 2) + (y ^ 2) - c).PolynomialDegree);
            Assert.Equal(22, ((x ^ 2) + (y ^ 2) - c).Compute(new Dictionary<string, double> { ["x"] = 3, ["y"] = 4 }));
            Assert.Equal(-125, (unary1 ^ c).Compute(new Dictionary<string, double> { }));

            var basef = new Constant(1e-17);
            var argumentf = new Constant(-2);
            var func = new Degree(basef, argumentf);
            var variables = new Dictionary<string, double>();

       
            var exception = Assert.Throws<ArgumentException>(
                () => func.Compute(variables)
            );

            Assert.Contains("Ноль в отрицательной степени -2", exception.Message);

            //
            basef = new Constant(-2);
            argumentf = new Constant(1.0/5.0);
            func = new Degree(basef, argumentf);
            variables = new Dictionary<string, double>();

            // Act & Assert
            exception = Assert.Throws<ArgumentException>(
                () => func.Compute(variables)
            );

            Assert.Contains("Отрицательное число -2 в дробной степени", exception.Message);

        }

        [Fact]
        public void TestLogFunc() //свойства Log
        {
            //base
            Assert.False((log(x, y)).IsConstant);
            Assert.True((log(3, c)).IsConstant);
            Assert.False((log(x, y)).IsPolynomial);
            Assert.False((log(3, c)).IsPolynomial);
            Assert.Equal("[x, y, z]", $"[{string.Join(", ", (log(x, y + z)).Variables)}]");
            Assert.Equal(-1, (log(x, y)).PolynomialDegree);
            Assert.Equal(-1, (log(x, c)).PolynomialDegree);
            Assert.Equal(-1, (log(3, c)).PolynomialDegree);
            Assert.Equal(18, (log(c, 9) * (x ^ y)).Compute(new Dictionary<string, double> { ["x"] = 3, ["y"] = 2 }));

            var logExpr = log(x, y);

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(
                () => logExpr.GetPolynomialCoefficients()
            );

            Assert.Contains("Логарифм не является полиномом", exception.Message);
        }

        [Fact]
        public void Compute_BaseLessThanOrEqualToZero_ThrowsArgumentException()
        {
            var baseExpr = new Constant(0);
            var argumentExpr = new Constant(10);
            var log = new ILog(baseExpr, argumentExpr);
            var variables = new Dictionary<string, double>();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(
                () => log.Compute(variables)
            );

            Assert.Contains("Основание должно быть больше 0", exception.Message);
        }

        [Fact]
        public void Compute_BaseEqualsOne_ThrowsArgumentException()
        {
            // Arrange
            var baseExpr = new Constant(1);
            var argumentExpr = new Constant(10);
            var log = new ILog(baseExpr, argumentExpr);
            var variables = new Dictionary<string, double>();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(
                () => log.Compute(variables)
            );

            Assert.Contains("Основание не может быть 1", exception.Message);
        }

        [Fact]
        public void Compute_ArgumentLessThanOrEqualToZero_ThrowsArgumentException()
        {
            var baseExpr = new Constant(2);
            var argumentExpr = new Constant(0);
            var log = new ILog(baseExpr, argumentExpr);
            var variables = new Dictionary<string, double>();

            var exception = Assert.Throws<ArgumentException>(
                () => log.Compute(variables)
            );

            Assert.Contains("Аргумент должен быть больше 0", exception.Message);
        }
    }

    public class TestGetPolynomialCoefficients
    {
        Expr x = new Variable("x");
        Expr y = new Variable("y");
        Expr z = new Variable("z");
        Expr w = new Variable("w");
        Expr c = new Constant(3);
        Expr unary1 = -new Constant(5);

        [Fact]
        public void TestCoefficientsForFuncs()
        {
            //base
            Assert.Equal("[x^2, -1][x*y, 0][y^2, 1]", string.Join("", (-((x + y)) * (x - y)).GetPolynomialCoefficients()));
            Assert.Equal("[x^2, 1][y^2, 1]", string.Join("", ((x ^ 2) + (y ^ 2)).GetPolynomialCoefficients()));
            Assert.Equal("[x^3, 1]", string.Join("", (x ^ c).GetPolynomialCoefficients()));
            Assert.Equal("[x, -5]", string.Join("", (unary1 * x).GetPolynomialCoefficients()));
            Assert.Equal("[x, 1][y, 1][z, 1][w, -1]", string.Join("", (x + y + z - w).GetPolynomialCoefficients()));
            Assert.Equal($"[x, {1.0/6.0}][x^2, {1.0/3.0}]", string.Join("", (x / 6 + (x ^ 2) / 3).GetPolynomialCoefficients()));

            var func = log(x, y) * z; 

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(
                () => func.GetPolynomialCoefficients()
            );

            Assert.Contains("Выражение не является полиномом", exception.Message);

            func = log(x, y) + z;

            exception = Assert.Throws<InvalidOperationException>(
                () => func.GetPolynomialCoefficients()
            );

            func = log(x, y) - z;

            exception = Assert.Throws<InvalidOperationException>(
                () => func.GetPolynomialCoefficients()
            );

            Assert.Contains("Выражение не является полиномом", exception.Message);

            func = x / y;

            exception = Assert.Throws<InvalidOperationException>(
                () => func.GetPolynomialCoefficients()
            );

            Assert.Contains("Выражение не является полиномом", exception.Message);
        }
                
    }
}
