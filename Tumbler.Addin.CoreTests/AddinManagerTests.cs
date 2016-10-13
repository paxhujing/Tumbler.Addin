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

        public AddinManagerTests()
        {
            _manager.Initialize();
        }

        [TestMethod()]
        public void InitializeTest()
        {
            //_manager.Initialize();
            Assert.AreEqual<Int32>(6, _manager.Count);
        }

        [TestMethod()]
        public void GetNodeTest()
        {
            _manager.Initialize();
            //AddinTreeNode badAddin = _manager.GetNode("OpenFile");
            //Assert.IsNull(badAddin, "Should be null");

            //AddinTreeNode goodAddin = _manager.GetNode("Addins/Menu/File/OpenFile");
            //Assert.IsNotNull(goodAddin, "Should be not null");
        }

        [TestMethod()]
        public void BuildTest()
        {
            IAddin[] addins = _manager.BuildFirstLevelAddins();
            Assert.Fail();
        }

        [TestMethod()]
        public void UninstallTest()
        {
            AddinTreeNode node = _manager.GetNode("./Addins/Toolbar");
            _manager.Uninstall((AddinNode)node);
            Assert.Fail();
        }

        [TestMethod()]
        public void BuildFirstLevelAddinsTest()
        {
            _manager.BuildFirstLevelAddins();
            Assert.Fail();
        }

        [TestMethod()]
        public void DestroyTest()
        {
            IAddin[] addins = _manager.BuildFirstLevelAddins();
            foreach (IAddin addin in addins)
            {
                _manager.Destroy(addin);
            }
            Assert.Fail();
        }

        [TestMethod()]
        public void GetAddinStateTest()
        {
            IAddin[] addins = _manager.BuildFirstLevelAddins();
            AddinState? state = null;
            foreach (IAddin addin in addins)
            {
                state = _manager.GetAddinState(addin);
            }
            Assert.Fail();
        }

        [TestMethod()]
        public void GetAddinStateTest1()
        {
            //_manager.BuildFirstLevelAddins();
            AddinState? state = null;
            foreach (String fullPath in new String[] { "/Addins/Menu", "/Addins/Toolbar" })
            {
                state = _manager.GetAddinState(fullPath);
            }
            Assert.Fail();
        }

        [TestMethod()]
        public void SetAddinStateTest()
        {
            IAddin[] addins = _manager.BuildFirstLevelAddins();
            foreach (IAddin addin in addins)
            {
                _manager.SetAddinState(addin, AddinState.Disable);
            }
            Assert.Fail();
        }

        [TestMethod()]
        public void SendMessageTest()
        {
            //IAddin[] addins = _manager.BuildFirstLevelAddins();
            _manager.SendMessage("./Addins/Toolbar", new System.Collections.Hashtable());
            Assert.Fail();
        }
    }
}