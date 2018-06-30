using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingWithMoq
{
    public interface IVisitor
    {
        void Visit(StuffBase element);
    }
}
