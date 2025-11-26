using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_OPP_NEW
{
    public class Polynomial : IExpr
    {
        private IReadOnlyDictionary<string, double> _coefficients;

        public Polynomial(IReadOnlyDictionary<string, double> coefficients)
        {
            _coefficients = coefficients;
        }

        public IEnumerable<string> Variables
        {
            get
            {
                var allVars = new HashSet<string>();
                foreach (var term in _coefficients.Keys)
                {
                    if (!string.IsNullOrEmpty(term))
                    {
                        var vars = term.Split('*')
                            .Select(v => v.Contains('^') ? v.Split('^')[0] : v);
                        foreach (var varName in vars)
                        {
                            allVars.Add(varName);
                        }
                    }
                }
                return allVars;
            }
        }

        public bool IsConstant => _coefficients.All(kvp => string.IsNullOrEmpty(kvp.Key)) && _coefficients.Count == 1;

        public bool IsPolynomial => true;

        public int PolynomialDegree
        {
            get
            {
                int maxDegree = 0;
                foreach (var term in _coefficients.Keys)
                {
                    if (string.IsNullOrEmpty(term))
                        continue;

                    int termDegree = 0;
                    var parts = term.Split('*');
                    foreach (var part in parts)
                    {
                        if (part.Contains('^'))
                        {
                            termDegree += int.Parse(part.Split('^')[1]);
                        }
                        else
                        {
                            termDegree += 1;
                        }
                    }
                    maxDegree = Math.Max(maxDegree, termDegree);
                }
                return maxDegree;
            }
        }

        public double Compute(IReadOnlyDictionary<string, double> variableValues)
        {
            double result = 0;
            foreach (var kvp in _coefficients)
            {
                if (string.IsNullOrEmpty(kvp.Key))
                {
                    result += kvp.Value;
                }
                else
                {
                    double termValue = kvp.Value;
                    var parts = kvp.Key.Split('*');
                    foreach (var part in parts)
                    {
                        if (part.Contains('^'))
                        {
                            var subParts = part.Split('^');
                            string varName = subParts[0];
                            int power = int.Parse(subParts[1]);
                            if (!variableValues.ContainsKey(varName))
                                throw new ArgumentException($"Значение переменной {varName} не указано");
                            termValue *= Math.Pow(variableValues[varName], power);
                        }
                        else
                        {
                            if (!variableValues.ContainsKey(part))
                                throw new ArgumentException($"Значение переменной {part} не указано");
                            termValue *= variableValues[part];
                        }
                    }
                    result += termValue;
                }
            }
            return result;
        }

        public IReadOnlyDictionary<string, double> GetPolynomialCoefficients() => _coefficients;

        public override string ToString()
        {
            var terms = new List<string>();
            foreach (var kvp in _coefficients.OrderByDescending(kvp =>
            {
                if (string.IsNullOrEmpty(kvp.Key)) return 0;
                int degree = 0;
                var parts = kvp.Key.Split('*');
                foreach (var part in parts)
                {
                    if (part.Contains('^'))
                        degree += int.Parse(part.Split('^')[1]);
                    else
                        degree += 1;
                }
                return degree;
            }))
            {
                if (Math.Abs(kvp.Value) < 1e-10)
                    continue;

                string termStr;
                if (string.IsNullOrEmpty(kvp.Key))
                {
                    termStr = kvp.Value.ToString();
                }
                else
                {
                    string coeffStr = Math.Abs(kvp.Value - 1) < 1e-10 ? "" :
                                     Math.Abs(kvp.Value + 1) < 1e-10 ? "-" :
                                     $"{kvp.Value}*";
                    termStr = $"{coeffStr}{kvp.Key}";
                }

                terms.Add(termStr);
            }

            if (terms.Count == 0)
                return "0";

            return string.Join(" + ", terms).Replace("+ -", "- ");
        }
    }
}
