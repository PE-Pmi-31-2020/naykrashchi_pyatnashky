using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL;

namespace Unit_Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void HashAndUnhash()
        {
            Game g = new Game(4);
            string hash = g.Hash_layout();
            g.Unhash(hash);
            Assert.AreEqual(hash, g.Hash_layout());
        }
    }
}
