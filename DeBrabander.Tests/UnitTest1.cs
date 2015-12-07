using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DeBrabander.Utils;

namespace DeBrabander.Tests
{
    [TestClass]
    public class UnitTest1
    {
        // Test om na te gaan of het hashen werkt.
        [TestMethod]
        public void TestMethod1()
        {
            string original = "qwerty";
            string hashed = SecurityUtil.hashPassword(original);
            Assert.AreNotEqual(original, hashed);
            Assert.AreEqual(SecurityUtil.hashPassword(original), hashed);
        }
    }
}
