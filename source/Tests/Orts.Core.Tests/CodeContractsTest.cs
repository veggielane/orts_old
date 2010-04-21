using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Orts.Core.Tests
{
    public class CodeContractsTest
    {

        public void Test2()
        {
            Test(null);
        }

        public string Test(string input)
        {
            Contract.Requires(input != null);
            Contract.Ensures(Contract.Result<string>() != null);

            input = input + "ha";
            return input;
        }
    }
}
