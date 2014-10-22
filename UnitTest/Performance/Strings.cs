using Microsoft.VisualStudio.TestTools.UnitTesting;
using SklLib;
using SklLib.Performance;

namespace UnitTest.Performance
{
    [TestClass]
    public class Strings
    {
        [TestMethod]
        public void ToUpper()
        {
            string str = "abcdef".RepeatString(500);
            string str2 = "ABCDEF".RepeatString(500);
            str.UnsafeToUpper();
            Assert.AreEqual<string>(str2, str);
        }

        [TestMethod]
        public void NativeToUpper()
        {
            string str = "abcdef".RepeatString(500);
            string str2 = "ABCDEF".RepeatString(500);
            str = str.ToUpper();
            Assert.AreEqual<string>(str2, str);
        }
    }
}
