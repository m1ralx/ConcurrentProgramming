using System;
using System.Linq;
using System.Threading.Tasks;
using ConcurrentProgramming;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading;

namespace ConcurrentTests
{
    [TestClass]
    public class UnitTest1
    {
        public void Replace(IStruct structure, string word, string replace, int repeates=4)
        {
            for (var i = 0; i < repeates; i++)
            {
                structure.ReplaceFirst(word, replace);
            }
        }
        [TestMethod]
        public void NonSafetyStructTest()
        {
            string[] strings = {
                                   "A", "A", "A", "A",  
                                   "A", "A", "A", "A", 
                                   "A", "A", "A", "A"
                               };
            var structure = new NonSafetyStruct(strings);
            var taskB = Task.Factory.StartNew(() => Replace(structure, "B", "A", 8));
            var taskA = Task.Factory.StartNew(() => Replace(structure, "A", "B", 12));
            taskA.Wait();
            taskB.Wait();
            var expected = new List<string> { "C", "C", "C", "C", "B", "B", "B", "B", "B", "B", "B", "B" };
            for (var i = 0; i < expected.Count; i++)
                Assert.AreEqual(expected[i], structure[i]);
        }

        [TestMethod]
        public void SafetyStructTest()
        {
            string[] strings = {
                                   "A", "A", "A", "A",  
                                   "A", "A", "A", "A", 
                                   "A", "A", "A", "A"
                               };
            var structure = new SafetyStruct(strings);
            var taskB = Task.Factory.StartNew(() => Replace(structure, "B", "A", 8));
            var taskA = Task.Factory.StartNew(() => Replace(structure, "A", "B", 12));
            taskA.Wait();
            taskB.Wait();
            var expected = new List<string> { "C", "C", "C", "C", "C", "C", "C", "C", "B", "B", "B", "B"};
            for (var i = 0; i < expected.Count; i++)
                Assert.AreEqual(expected[i], structure[i]);
        }
    }
}
