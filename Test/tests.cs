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
            string perm = "x";
            var variable = new Variable(perm);
            var result = string.Join("", variable.Variables);
            Assert.Equal(perm, result); 
            //var result = $"{string.Join("", alternating.Variables)}";
            //Assert.Equal("x", string.Concat(alternating.Variables));

            Assert.False(x.IsConstant);
            Assert.True(x.IsPolynomial);
            Assert.Equal("[x]", $"[{string.Join(", ", x.Variables)}]");
            Assert.Equal(1, x.PolynomialDegree);
            Assert.Equal(3, x.Compute(new Dictionary<string, double> { ["x"] = 3 }));

            
            var exception = Assert.Throws<ArgumentException>(
                () => (x - y).Compute(new Dictionary<string, double> { ["x"] = 3 }));

            Assert.Contains("Значение переменной y не указано", exception.Message);
        }

        [Fact]
        public void TestAddFunc() //свойства Add
        {
            var funk = x + y;
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
            Assert.Equal(-1, ((2 / x) + (3 / y)).PolynomialDegree);
            Assert.Equal(5, (x + y).Compute(new Dictionary<string, double> { ["x"] = 3, ["y"] = 2 }));
            Assert.Equal(0, (x + unary1).Compute(new Dictionary<string, double> { ["x"] = 5 }));
            Assert.Equal("(x + y)", funk.ToString());

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
            Assert.Equal(1, (unary1 * x).PolynomialDegree);
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

            var func = x / 0.0; 
            var exception = Assert.Throws<DivideByZeroException>(
                () => func.GetPolynomialCoefficients()
            );
            Assert.Contains("Деление на ноль", exception.Message);

            
            exception = Assert.Throws<DivideByZeroException>(
                () => func.Compute(new Dictionary<string, double> { })
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
        Expr c = new Constant(3.0);
        Expr unary1 = -new Constant(5.0);

        [Fact]   
        public void TestDegreeFunc() //свойства Degree
        {

            var funk1 = x ^ (0.5);
            var funk2 = x ^ (2);
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
            Assert.Equal(9, (x ^ y).Compute(new Dictionary<string, double> { ["x"] = 3, ["y"] = 2 }));
            Assert.Equal(125, (x ^ c).Compute(new Dictionary<string, double> { ["x"] = 5 }));
            Assert.Equal("Sqrt(x)", funk1.ToString());
            Assert.Equal("x^(2)", funk2.ToString());

            //expressions
            //1)
            Assert.False((x ^ 2 + y ^ 2 - c).IsConstant);
            Assert.True((unary1 ^ c).IsConstant);
            Assert.True(((x ^ 2) + (y ^ 2) - c).IsPolynomial);
            Assert.True((unary1 ^ c).IsPolynomial);
            Assert.True((unary1 ^ (c - unary1)).IsPolynomial);
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


            basef = new Constant(-2);
            argumentf = new Constant(1.0/5.0);
            func = new Degree(basef, argumentf);
            variables = new Dictionary<string, double>();

            exception = Assert.Throws<ArgumentException>(
                () => func.Compute(variables)
            );
            Assert.Contains("Отрицательное число -2 в дробной степени", exception.Message);

        }

        [Fact]
        public void TestLogFunc() //свойства Log
        {
            var func = log(c, c);
            //base
            Assert.False((log(x, y)).IsConstant);
            Assert.True((log(3, c)).IsConstant);
            Assert.False((log(x, y)).IsPolynomial);
            Assert.False((log(3, c)).IsPolynomial);
            Assert.Equal("[x, y, z]", $"[{string.Join(", ", (log(x, y + z)).Variables)}]");
            Assert.Equal("[]", $"[{string.Concat(log(c, c).Variables)}]");
            Assert.Equal(-1, (log(x, y)).PolynomialDegree);
            Assert.Equal(-1, (log(x, c)).PolynomialDegree);
            Assert.Equal(-1, (log(3, c)).PolynomialDegree);
            Assert.Equal(18, (log(c, 9) * (x ^ y)).Compute(new Dictionary<string, double> { ["x"] = 3, ["y"] = 2 }));

            var logExpr = log(x, y);

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

            var exception = Assert.Throws<ArgumentException>(
                () => log.Compute(variables)
            );
            Assert.Contains("Основание должно быть больше 0", exception.Message);
        }


        [Fact]
        public void Compute_BaseEqualsOne_ThrowsArgumentException()
        {
            var baseExpr = new Constant(1);
            var argumentExpr = new Constant(10);
            var log = new ILog(baseExpr, argumentExpr);
            var variables = new Dictionary<string, double>();

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
            Assert.Equal($"[, 1]", string.Join("", (x ^ 0).GetPolynomialCoefficients()));
            Assert.Equal("[x, 5]", string.Join("", ((x * 2) + (3 * x)).GetPolynomialCoefficients()));
            Assert.Equal("[x, -5]", string.Join("", (-((x * 2) + (3 * x))).GetPolynomialCoefficients()));
            Assert.Equal("[y, 1]", string.Join("", ((2 * y) - y).GetPolynomialCoefficients()));

            var func = log(x, y) * z; 
            var exception = Assert.Throws<InvalidOperationException>(
                () => func.GetPolynomialCoefficients()
            );
            Assert.Contains("Выражение не является полиномом", exception.Message);


            func = log(x, y) + z;
            exception = Assert.Throws<InvalidOperationException>(
                () => func.GetPolynomialCoefficients()
            );
            Assert.Contains("Выражение не является полиномом", exception.Message);

            func = x / y;
            exception = Assert.Throws<InvalidOperationException>(
                () => func.GetPolynomialCoefficients()
            );
            Assert.Contains("Выражение не является полиномом", exception.Message);

            func = (c / x) ^ y ;
            exception = Assert.Throws<InvalidOperationException>(
                () => func.GetPolynomialCoefficients()
            );
            Assert.Contains("Выражение не является полиномом", exception.Message);

            func = (c / x) - y;
            exception = Assert.Throws<InvalidOperationException>(
                () => func.GetPolynomialCoefficients()
            );
            Assert.Contains("Выражение не является полиномом", exception.Message);

            func = -(y / x);
            exception = Assert.Throws<InvalidOperationException>(
                () => func.GetPolynomialCoefficients()
            );
            Assert.Contains("Выражение не является полиномом", exception.Message);
        }
    }


    public class PolynomialClassTests
    {
        [Fact]
        public void Constructor_WithValidCoefficients_SetsCoefficients()
        {
            
            var coefficients = new Dictionary<string, double>
            {
                { "", 3.0 },
                { "x", 2.0 },
                { "x^2", 1.0 }
            };

            var polynomial = new Polynomial(coefficients);

            Assert.Equal(3, polynomial.GetPolynomialCoefficients().Count);
            Assert.Equal(3.0, polynomial.GetPolynomialCoefficients()[""]);
            Assert.Equal(2.0, polynomial.GetPolynomialCoefficients()["x"]);
            Assert.Equal(1.0, polynomial.GetPolynomialCoefficients()["x^2"]);
        }

        [Fact]
        public void Constructor_WithEmptyDictionary_CreatesEmptyPolynomial()
        {
            
            var coefficients = new Dictionary<string, double>();

            var polynomial = new Polynomial(coefficients);

            Assert.Empty(polynomial.GetPolynomialCoefficients());
        }

        [Fact]
        public void Variables_WithSingleVariable_ReturnsCorrectVariable()
        {
            
            var coefficients = new Dictionary<string, double> { { "x", 2.0 } };
            var polynomial = new Polynomial(coefficients);

            var variables = polynomial.Variables.ToList();

            Assert.Single(variables);
            Assert.Equal("x", variables[0]);
        }

        [Fact]
        public void Variables_WithMultipleVariables_ReturnsAllVariables()
        {
            
            var coefficients = new Dictionary<string, double>
            {
                { "x*y", 3.0 },
                { "y^2", 2.0 },
                { "", 1.0 }
            };
            var polynomial = new Polynomial(coefficients);

            var variables = polynomial.Variables.OrderBy(v => v).ToList();

            Assert.Equal(2, variables.Count);
            Assert.Equal("x", variables[0]);
            Assert.Equal("y", variables[1]);
        }

        [Fact]
        public void Variables_WithComplexTerm_ExtractsVariablesCorrectly()
        {
            
            var coefficients = new Dictionary<string, double>
            {
                { "x^2*y^3*z", 2.0 }
            };
            var polynomial = new Polynomial(coefficients);

            var variables = polynomial.Variables.OrderBy(v => v).ToList();

            Assert.Equal(3, variables.Count);
            Assert.Equal("x", variables[0]);
            Assert.Equal("y", variables[1]);
            Assert.Equal("z", variables[2]);
        }

        [Fact]
        public void Variables_WithConstantOnly_ReturnsEmpty()
        {
            
            var coefficients = new Dictionary<string, double> { { "", 5.0 } };
            var polynomial = new Polynomial(coefficients);

            var variables = polynomial.Variables.ToList();

            Assert.Empty(variables);
        }

        [Fact]
        public void IsConstant_WithOnlyConstantTerm_ReturnsTrue()
        {
            
            var coefficients = new Dictionary<string, double> { { "", 5.0 } };
            var polynomial = new Polynomial(coefficients);

            Assert.True(polynomial.IsConstant);
        }

        [Fact]
        public void IsConstant_WithVariableTerm_ReturnsFalse()
        {
            
            var coefficients = new Dictionary<string, double> { { "x", 5.0 } };
            var polynomial = new Polynomial(coefficients);

            Assert.False(polynomial.IsConstant);
        }

        [Fact]
        public void IsConstant_WithMultipleTermsIncludingVariable_ReturnsFalse()
        {
            
            var coefficients = new Dictionary<string, double>
            {
                { "", 3.0 },
                { "x", 2.0 }
            };
            var polynomial = new Polynomial(coefficients);

            Assert.False(polynomial.IsConstant);
        }

        [Fact]
        public void IsPolynomial_Always_ReturnsTrue()
        {

            var coefficients = new Dictionary<string, double> { { "x^2", 3.0 } };
            var polynomial = new Polynomial(coefficients);

            Assert.True(polynomial.IsPolynomial);
        }

        [Fact]
        public void PolynomialDegree_WithConstant_Returns0()
        {
            var coefficients = new Dictionary<string, double> { { "", 5.0 } };
            var polynomial = new Polynomial(coefficients);

            var degree = polynomial.PolynomialDegree;

            Assert.Equal(0, degree);
        }

        [Fact]
        public void PolynomialDegree_WithLinearTerm_Returns1()
        {
            var coefficients = new Dictionary<string, double> { { "x", 3.0 } };
            var polynomial = new Polynomial(coefficients);

            var degree = polynomial.PolynomialDegree;

            Assert.Equal(1, degree);
        }

        [Fact]
        public void PolynomialDegree_WithQuadraticTerm_Returns2()
        {
            var coefficients = new Dictionary<string, double> { { "x^2", 3.0 } };
            var polynomial = new Polynomial(coefficients);

            var degree = polynomial.PolynomialDegree;

            Assert.Equal(2, degree);
        }

        [Fact]
        public void PolynomialDegree_WithMixedTerms_ReturnsHighestDegree()
        {
            var coefficients = new Dictionary<string, double>
            {
                { "", 1.0 },
                { "x", 2.0 },
                { "x^3", 3.0 },
                { "x^2", 4.0 }
            };
            var polynomial = new Polynomial(coefficients);

            var degree = polynomial.PolynomialDegree;

            Assert.Equal(3, degree);
        }

        [Fact]
        public void PolynomialDegree_WithMultiVariableTerm_CalculatesTotalDegree()
        {
            var coefficients = new Dictionary<string, double>
            {
                { "x*y^2", 3.0 }
            };
            var polynomial = new Polynomial(coefficients);

            var degree = polynomial.PolynomialDegree;

            Assert.Equal(3, degree);
        }

        [Fact]
        public void PolynomialDegree_WithComplexMultiVariableTerm_CalculatesCorrectDegree()
        {
            var coefficients = new Dictionary<string, double>
            {
                { "x^2*y^3*z", 2.0 }
            };
            var polynomial = new Polynomial(coefficients);

            var degree = polynomial.PolynomialDegree;

            Assert.Equal(6, degree);
        }

        [Fact]
        public void Compute_WithConstant_ReturnsConstantValue()
        {
            var coefficients = new Dictionary<string, double> { { "", 5.0 } };
            var polynomial = new Polynomial(coefficients);
            var variables = new Dictionary<string, double>();

            var result = polynomial.Compute(variables);

            Assert.Equal(5.0, result, 10);
        }

        [Fact]
        public void Compute_WithSingleVariable_ReturnsCorrectValue()
        {

            var coefficients = new Dictionary<string, double> { { "x", 2.0 } };
            var polynomial = new Polynomial(coefficients);
            var variables = new Dictionary<string, double> { { "x", 3.0 } };

            var result = polynomial.Compute(variables);

            Assert.Equal(6.0, result, 10);
        }

        [Fact]
        public void Compute_WithPowerTerm_ReturnsCorrectValue()
        {

            var coefficients = new Dictionary<string, double> { { "x^3", 2.0 } };
            var polynomial = new Polynomial(coefficients);
            var variables = new Dictionary<string, double> { { "x", 2.0 } };

            var result = polynomial.Compute(variables);

            Assert.Equal(16.0, result, 10);
        }

        [Fact]
        public void Compute_WithMultiVariableTerm_ReturnsCorrectValue()
        {
            var coefficients = new Dictionary<string, double> { { "x*y", 3.0 } };
            var polynomial = new Polynomial(coefficients);
            var variables = new Dictionary<string, double>
            {
                { "x", 2.0 },
                { "y", 4.0 }
            };

            var result = polynomial.Compute(variables);

            Assert.Equal(24.0, result, 10);
        }

        [Fact]
        public void Compute_WithComplexTerm_ReturnsCorrectValue()
        {

            var coefficients = new Dictionary<string, double> { { "x^2*y^3", 2.0 } };
            var polynomial = new Polynomial(coefficients);
            var variables = new Dictionary<string, double>
            {
                { "x", 3.0 },
                { "y", 2.0 }
            };

            var result = polynomial.Compute(variables);

            Assert.Equal(144.0, result, 10);
        }

        [Fact]
        public void Compute_WithMultipleTerms_ReturnsCorrectSum()
        {
            var coefficients = new Dictionary<string, double>
            {
                { "", 1.0 },
                { "x", 2.0 },
                { "x^2", 3.0 }
            };
            var polynomial = new Polynomial(coefficients);
            var variables = new Dictionary<string, double> { { "x", 2.0 } };

            var result = polynomial.Compute(variables);

            Assert.Equal(17.0, result, 10);
        }

        [Fact]
        public void Compute_WithMissingVariable_ThrowsArgumentException()
        {
            var coefficients = new Dictionary<string, double> { { "x", 2.0 } };
            var polynomial = new Polynomial(coefficients);
            var variables = new Dictionary<string, double>();

            Assert.Throws<ArgumentException>(() => polynomial.Compute(variables));
        }

        [Fact]
        public void Compute_WithMissingVariableInMultiVariableTerm_ThrowsArgumentException()
        {
            var coefficients = new Dictionary<string, double> { { "x*y", 2.0 } };
            var polynomial = new Polynomial(coefficients);
            var variables = new Dictionary<string, double> { { "x", 2.0 } };

            Assert.Throws<ArgumentException>(() => polynomial.Compute(variables));
        }

        [Fact]
        public void Compute_WithZeroCoefficient_IgnoresTerm()
        {
            var coefficients = new Dictionary<string, double>
            {
                { "", 1.0 },
                { "x", 0.0 },
                { "x^2", 3.0 }
            };
            var polynomial = new Polynomial(coefficients);
            var variables = new Dictionary<string, double> { { "x", 2.0 } };

            var result = polynomial.Compute(variables);

            Assert.Equal(13.0, result, 10);
        }

        [Fact]
        public void GetPolynomialCoefficients_ReturnsOriginalCoefficients()
        {
            var coefficients = new Dictionary<string, double>
            {
                { "", 1.0 },
                { "x", 2.0 },
                { "x^2", 3.0 }
            };
            var polynomial = new Polynomial(coefficients);

            var result = polynomial.GetPolynomialCoefficients();

            Assert.Equal(coefficients.Count, result.Count);
            Assert.Equal(1.0, result[""]);
            Assert.Equal(2.0, result["x"]);
            Assert.Equal(3.0, result["x^2"]);
        }

        [Fact]
        public void ToString_WithConstant_ReturnsConstantString()
        {
            var coefficients = new Dictionary<string, double> { { "", 5.0 } };
            var polynomial = new Polynomial(coefficients);

            var result = polynomial.ToString();

            Assert.Equal("5", result);
        }

        [Fact]
        public void ToString_WithSingleVariable_ReturnsCorrectString()
        {
            var coefficients = new Dictionary<string, double> { { "x", 2.0 } };
            var polynomial = new Polynomial(coefficients);

            var result = polynomial.ToString();

            Assert.Equal("2*x", result);
        }

        [Fact]
        public void ToString_WithCoefficientOne_OmitsCoefficient()
        {
            var coefficients = new Dictionary<string, double> { { "x", 1.0 } };
            var polynomial = new Polynomial(coefficients);

            var result = polynomial.ToString();

            Assert.Equal("x", result);
        }

        [Fact]
        public void ToString_WithCoefficientMinusOne_ShowsMinusSign()
        {
            var coefficients = new Dictionary<string, double> { { "x", -1.0 } };
            var polynomial = new Polynomial(coefficients);

            var result = polynomial.ToString();

            Assert.Equal("-x", result);
        }

        [Fact]
        public void ToString_WithMultipleTerms_ReturnsSortedString()
        {
            var coefficients = new Dictionary<string, double>
            {
                { "", 1.0 },
                { "x", 2.0 },
                { "x^2", 3.0 }
            };
            var polynomial = new Polynomial(coefficients);

            var result = polynomial.ToString();

            Assert.Equal("3*x^2 + 2*x + 1", result);
        }

        [Fact]
        public void ToString_WithMixedTerms_ReturnsCorrectString()
        {
            var coefficients = new Dictionary<string, double>
            {
                { "x*y", 3.0 },
                { "x^2", 2.0 },
                { "", -1.0 }
            };
            var polynomial = new Polynomial(coefficients);

            var result = polynomial.ToString();

            Assert.Contains("3*x*y", result);
            Assert.Contains("2*x^2", result);
            Assert.Contains("- 1", result);
        }

        [Fact]
        public void ToString_WithNegativeTerm_ShowsCorrectSign()
        {
            var coefficients = new Dictionary<string, double>
            {
                { "x^2", 2.0 },
                { "x", -3.0 },
                { "", 1.0 }
            };
            var polynomial = new Polynomial(coefficients);

            var result = polynomial.ToString();

            Assert.Equal("2*x^2 - 3*x + 1", result);
        }

        [Fact]
        public void ToString_WithZeroCoefficient_OmitsTerm()
        {
            var coefficients = new Dictionary<string, double>
            {
                { "", 1.0 },
                { "x", 0.0 },
                { "x^2", 3.0 }
            };
            var polynomial = new Polynomial(coefficients);

            var result = polynomial.ToString();

            Assert.Equal("3*x^2 + 1", result);
        }

        [Fact]
        public void ToString_AllZeroCoefficients_ReturnsZero()
        {
            var coefficients = new Dictionary<string, double>
            {
                { "", 0.0 },
                { "x", 0.0 }
            };
            var polynomial = new Polynomial(coefficients);

            var result = polynomial.ToString();

            Assert.Equal("0", result);
        }

        [Fact]
        public void ToString_WithPowerTerm_FormatsCorrectly()
        {
            var coefficients = new Dictionary<string, double>
            {
                { "x^3", 2.0 },
                { "x^2", -1.0 }
            };
            var polynomial = new Polynomial(coefficients);

            var result = polynomial.ToString();

            Assert.Equal("2*x^3 - x^2", result);
        }

        [Fact]
        public void ToString_WithMultiVariablePowerTerm_FormatsCorrectly()
        {
            var coefficients = new Dictionary<string, double>
            {
                { "x^2*y^3", 2.0 },
                { "x*y", 3.0 }
            };
            var polynomial = new Polynomial(coefficients);

            var result = polynomial.ToString();

            Assert.Contains("2*x^2*y^3", result);
            Assert.Contains("3*x*y", result);
        }

        [Fact]
        public void ToString_WithComplexNegativeTerms_FormatsCorrectly()
        {
            var coefficients = new Dictionary<string, double>
            {
                { "x^3", -2.0 },
                { "x^2", 1.0 },
                { "x", -1.0 },
                { "", 0.5 }
            };
            var polynomial = new Polynomial(coefficients);

            var result = polynomial.ToString();

            Assert.Equal("-2*x^3 + x^2 - x + 0,5", result);
        }

        [Fact]
        public void ToString_WithDecimalCoefficient_FormatsCorrectly()
        {
            var coefficients = new Dictionary<string, double>
            {
                { "x", 0.5 },
                { "x^2", 1.5 }
            };
            var polynomial = new Polynomial(coefficients);

            var result = polynomial.ToString();

            Assert.Equal("1,5*x^2 + 0,5*x", result);
        }

        [Fact]
        public void ToString_WithNegativeDecimal_FormatsCorrectly()
        {
            var coefficients = new Dictionary<string, double>
            {
                { "x", -0.75 },
                { "", 2.25 }
            };
            var polynomial = new Polynomial(coefficients);

            var result = polynomial.ToString();

            Assert.Equal("-0,75*x + 2,25", result);
        }
    }

}
