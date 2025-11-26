using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_OPP_NEW
{
    public abstract class UnaryOperation : Expr
    {
        protected IExpr Oper { get; }

        public UnaryOperation(IExpr oper)
        {
            Oper = oper;
        }

    }
}
