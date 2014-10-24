using Microsoft.VisualStudio.TestTools.UnitTesting;
using SklLib;
using System;

namespace UnitTest
{
    [TestClass]
    public class ExceptionExtension
    {
        [TestMethod]
        public void CreateDump()
        {
            try { "".Remove(10, 10); }
            catch (Exception ex) {
                ex.CreateDump();
            }
        }
    }
}
