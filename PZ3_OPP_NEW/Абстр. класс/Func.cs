using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_OPP_NEW
{
    public abstract class Function : Expr
    {
        protected IExpr Funk { get; }

        public Function(IExpr funk)
        {
            Funk = funk;
        }

    }
}
