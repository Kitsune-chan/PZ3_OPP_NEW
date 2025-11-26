using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_OPP_NEW
{
    public abstract class BinaryOperation : Expr
    {
        protected IExpr Left { get; }
        protected IExpr Right { get; }

        public BinaryOperation(IExpr x_Var, IExpr y_Var)
        {
            Left = x_Var;
            Right = y_Var;
        }
    }
}
