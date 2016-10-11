using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tumbler.Addin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tumbler.Addin.Core.Tests
{
    [TestClass()]
    public class AddinManagerTests
    {
        AddinManager _manager = new AddinManager(@"E:\Tumbler.Addin\Tumbler.Addin.CoreTests\Addins.xml");

        [TestMethod()]
        public void InitializeTest()
        {
            _manager.Initialize();
            Assert.Fail();
        }
    }
}