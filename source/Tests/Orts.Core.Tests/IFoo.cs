using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Orts.Core.Tests
{
    [ContractClass(typeof(IFooContract))]
    public interface IFoo
    {
        int Foo(int i);
    }

    [ContractClassFor(typeof(IFoo))]
    internal class IFooContract :IFoo
    {

        public int J { get; set; }
        #region IFoo Members

        public int Foo(int i)
        {
            Contract.Ensures(Contract.Result<int>() >= 0);

            return default(int);
        }
        #endregion
    }

    public class Foo : IFoo
    {

        #region IFoo Members

        int IFoo.Foo(int i)
        {
            return Math.Abs(i - 5);
        }

        #endregion
    }
}
