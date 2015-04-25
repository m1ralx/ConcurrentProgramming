using System;
using System.Linq;
using System.Threading.Tasks;
using ConcurrentProgramming;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConcurrentTests
{
    [TestClass]
    public class UnitTest1
    {
        public void Replace(NonSafetyStruct structure, string word, string replace)
        {
            for (var i = 0; i < 1000; i++)
            {
                structure.ReplaceFirst(word, replace);
            }
        }
        [TestMethod]
        public void NonSafetyTest()
        {
            var rand = new Random();
            string[] baseStrings = { "Richard", "York"}; //, "Of", "Gave", "Battle", "In", "Vain" };
            var strings = Enumerable.Range(0, 1000).Select(i => baseStrings[rand.Next(baseStrings.Length-1)]).ToArray();
            var structure = new NonSafetyStruct(strings);
            Task.Factory.StartNew(() => Replace(structure, "Richard", "York"));
            Task.Factory.StartNew(() => Replace(structure, "York", "Richard"));
            Console.ReadKey();
        }
    }
}
