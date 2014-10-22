using Microsoft.VisualStudio.TestTools.UnitTesting;
using SklLib;
using System;

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
            "#-".RepeatString(1000000);
        }

        [TestMethod]
        public void Split()
        {
            CollectionAssert.AreEqual(new string[] { "abcde", "fghij" }, "abcdefghij".Split(5));
            CollectionAssert.AreEqual(new string[] { "abcdefghij" }, "abcdefghij".Split(20));
            CollectionAssert.AreEqual(new string[] { "abc", "def", "ghi" , "j" }, "abcdefghij".Split(3));
            CollectionAssert.AreEqual(new string[] { "abcdefghij" }, "abcdefghij".Split(0));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Split_Negative_Chunk()
        {
            "a".Split(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Split_Null_String()
        {
            string str = null;
            str.Split(0);
        }

        [TestMethod]
        public void Split_Performance()
        {
            TEXT_LARGE.Split(2);
        }
    }
}
