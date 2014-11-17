using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SklLib.Formatting;

namespace UnitTest.Formatting
{
    [TestClass]
    public class NumbersTest
    {
        System.Globalization.NumberFormatInfo formatPtBr =
            System.Globalization.CultureInfo.GetCultureInfo("pt-BR").NumberFormat;
        SklLib.Globalization.NumberWriteInfo format2PtBr =
            SklLib.Globalization.NumberWriteInfo.GetCultureBasedInfo("pt-BR");

        [TestMethod]
        public void SpellNumber1()
        {
            string result = Numbers.SpellNumber(457826362.943M, false, format2PtBr, formatPtBr);
            string expected = "quatrocentos e cinquenta e sete milhões e oitocentos e vinte e seis mil e trezentos e sessenta e dois e novecentos e quarenta e três milésimos";
            Assert.AreEqual<string>(expected, result);
        }

        [TestMethod]
        public void SpellNumber2()
        {
            string result = Numbers.SpellNumber(457000000.003M, false, format2PtBr, formatPtBr);
            string expected = "quatrocentos e cinquenta e sete milhões e três milésimos";
            Assert.AreEqual<string>(expected, result);
        }

        [TestMethod]
        public void SpellNumber3()
        {
            string result = Numbers.SpellNumber(457000362.903M, false, format2PtBr, formatPtBr);
            string expected = "quatrocentos e cinquenta e sete milhões e trezentos e sessenta e dois e novecentos e três milésimos";
            Assert.AreEqual<string>(expected, result);
        }

        [TestMethod]
        public void SpellNumber4()
        {
            string result = Numbers.SpellNumber(40000000.1M, false, format2PtBr, formatPtBr);
            string expected = "quarenta milhões e um décimo";
            Assert.AreEqual<string>(expected, result);
        }

        [TestMethod]
        public void SpellNumber5()
        {
            string result = Numbers.SpellNumber(100010001M, false, format2PtBr, formatPtBr);
            string expected = "cem milhões e dez mil e um";
            Assert.AreEqual<string>(expected, result);
        }

        [TestMethod]
        public void SpellNumber6()
        {
            string result = Numbers.SpellNumber(112, false, format2PtBr, formatPtBr);
            string expected = "cento e doze";
            Assert.AreEqual<string>(expected, result);
        }

        [TestMethod]
        public void SpellNumber7()
        {
            string result = Numbers.SpellNumber(.042M, false, format2PtBr, formatPtBr);
            string expected = "quarenta e dois milésimos";
            Assert.AreEqual<string>(expected, result);
        }

        [TestMethod]
        public void SpellNumber8()
        {
            string result = Numbers.SpellNumber(0M, false, format2PtBr, formatPtBr);
            string expected = "zero";
            Assert.AreEqual<string>(expected, result);
        }

        [TestMethod]
        public void SpellNumber9()
        {
            string result = Numbers.SpellNumber(3M, true, format2PtBr, formatPtBr);
            string expected = "três Reais";
            Assert.AreEqual<string>(expected, result);
        }

        [TestMethod]
        public void SpellNumber10()
        {
            string result = Numbers.SpellNumber(173.41M, true, format2PtBr, formatPtBr);
            string expected = "cento e setenta e três Reais e quarenta e um Centavos";
            Assert.AreEqual<string>(expected, result);
        }

        [TestMethod]
        public void SpellNumber11()
        {
            string result = Numbers.SpellNumber(7300.013M, true, format2PtBr, formatPtBr);
            string expected = "sete mil e trezentos Reais e um Centavo";
            Assert.AreEqual<string>(expected, result);
        }
    }
}
