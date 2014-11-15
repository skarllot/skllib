using Microsoft.VisualStudio.TestTools.UnitTesting;
using SklLib.Measurement;
using System;

namespace UnitTest.Measurement
{
    [TestClass]
    public class InformationSizeTest
    {
        [TestMethod]
        public void Gibibytes()
        {
            InformationSize a = new InformationSize();
            a.Gibibytes = 15;

            Assert.AreEqual<decimal>(15M, a.Gibibytes);
        }

        [TestMethod]
        public void Mebibytes()
        {
            InformationSize a = new InformationSize();
            a.Mebibytes = 15;

            Assert.AreEqual<decimal>(15M, a.Mebibytes);
        }

        [TestMethod]
        public void Operator_Sum()
        {
            InformationSize a = new InformationSize(15, ByteIEC.Mebibyte);
            InformationSize b = new InformationSize(50, ByteIEC.Mebibyte);
            InformationSize c = a + b;

            Assert.AreEqual<decimal>(65M, c.Mebibytes);
        }

        [TestMethod]
        public void ToString1()
        {
            InformationSize a = new InformationSize();
            a.Mebibytes = 15.317M;
            string result = a.ToString("G|M-");
            string expectedNumber = a.Mebibytes.ToString("G");

            Assert.AreEqual<string>(expectedNumber + " MiB", result);
        }

        [TestMethod]
        public void ToString2()
        {
            InformationSize a = new InformationSize();
            a.Mebibytes = 15.317M;
            string result = a.ToString("G|M+");
            string expectedNumber = a.Mebibytes.ToString("G");

            Assert.AreEqual<string>(expectedNumber + " Mebibytes", result);
        }

        [TestMethod]
        public void ToString3()
        {
            InformationSize a = new InformationSize();
            a.Mebibytes = 15.317M;
            string result = a.ToString("N2|M-");
            string expectedNumber = a.Mebibytes.ToString("N2");

            Assert.AreEqual<string>(expectedNumber + " MiB", result);
        }

        [TestMethod]
        public void ToString4()
        {
            InformationSize a = new InformationSize();
            a.Mebibytes = 15.317M;
            string result = a.ToString("G|K-");
            string expectedNumber = a.Kibibytes.ToString("G");

            Assert.AreEqual<string>(expectedNumber + " KiB", result);
        }

        [TestMethod]
        public void ToString5()
        {
            InformationSize a = new InformationSize();
            a.Mebibytes = 15.317M;
            string result = a.ToString("N1|K-");
            string expectedNumber = a.Kibibytes.ToString("N1");

            Assert.AreEqual<string>(expectedNumber + " KiB", result);
        }

        [TestMethod]
        public void ToString6()
        {
            InformationSize a = new InformationSize();
            a.Gibibytes = 4000;
            string result = a.ToString("G|>-");
            string expectedNumber = a.Tebibytes.ToString("G");

            Assert.AreEqual<string>(expectedNumber + " TiB", result);
        }

        [TestMethod]
        public void ToString7()
        {
            InformationSize a = new InformationSize();
            a.Gibibytes = 4000;
            string result = a.ToString("N0|>-");
            string expectedNumber = a.Tebibytes.ToString("N0");

            Assert.AreEqual<string>(expectedNumber + " TiB", result);
        }

        [TestMethod]
        public void ToString8()
        {
            InformationSize a = new InformationSize();
            a.Gibibytes = 4000;
            string result = a.ToString("G|0-");
            string expectedNumber = a.Gibibytes.ToString("G");

            Assert.AreEqual<string>(expectedNumber + " GiB", result);
        }

        [TestMethod]
        public void ToStringFormatProvider()
        {
            IFormatProvider format1 = System.Globalization.CultureInfo.GetCultureInfo("en-US");
            IFormatProvider format2 = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");

            InformationSize a = new InformationSize();
            a.Gibibytes = 4000;

            string result = a.ToString("G|>-", format1);
            string expectedNumber = a.Tebibytes.ToString("G", format1);
            Assert.AreEqual<string>("3.90625 TiB", result);

            result = a.ToString("G|>-", format2);
            expectedNumber = a.Tebibytes.ToString("G", format2);
            Assert.AreEqual<string>("3,90625 TiB", result);
        }
    }
}
