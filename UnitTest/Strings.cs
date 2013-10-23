using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class Strings
    {
        readonly string TEXT_MEDIUM = resText.Medium;
        readonly string TEXT_LARGE = resText.Large;

        [TestMethod]
        public void RepeatString_Performance()
        {
            SklLib.Strings.RepeatString("#-", 1000000);
        }

        [TestMethod]
        public void Split()
        {
            CollectionAssert.AreEqual(new string[] { "abcde", "fghij" }, SklLib.Strings.Split("abcdefghij", 5));
            CollectionAssert.AreEqual(new string[] { "abcdefghij" }, SklLib.Strings.Split("abcdefghij", 20));
            CollectionAssert.AreEqual(new string[] { "abc", "def", "ghi" , "j" }, SklLib.Strings.Split("abcdefghij", 3));
            CollectionAssert.AreEqual(new string[] { "abcdefghij" }, SklLib.Strings.Split("abcdefghij", 0));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Split_Negative_Chunk()
        {
            SklLib.Strings.Split("a", -1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Split_Null_String()
        {
            SklLib.Strings.Split(null, 0);
        }

        [TestMethod]
        public void Split_Performance()
        {
            SklLib.Strings.Split(TEXT_LARGE, 2);
        }
    }
}
