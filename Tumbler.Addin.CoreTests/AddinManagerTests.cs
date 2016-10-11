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
            Assert.AreEqual<Int32>(5, _manager.Count);
        }

        [TestMethod()]
        public void GetNodeTest()
        {
            _manager.Initialize();
            AddinTreeNode badAddin = _manager.GetNode("OpenFile");
            Assert.IsNull(badAddin, badAddin.FullPath);

            AddinTreeNode goodAddin = _manager.GetNode("Addins/Menu/File/OpenFile");
            Assert.IsNotNull(goodAddin, goodAddin.FullPath);
        }
    }
}