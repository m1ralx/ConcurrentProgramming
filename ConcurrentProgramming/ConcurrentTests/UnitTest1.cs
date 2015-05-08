using System;
using System.Linq;
using System.Threading.Tasks;
using ConcurrentProgramming;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace ConcurrentTests
{
    [TestClass]
    public class UnitTest1
    {
        public void Replace(IStruct structure, string word, string replace, int repeates=1)
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
                                   "A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A"
                               };
            var structure = new NonSafetyStruct(strings);
            Task[] tasks = new Task[5];
            for (var i = 0; i < 5; i++)
                tasks[i] = Task.Factory.StartNew(() => Replace(structure, "A", "B", 5));
            Task.WaitAll(tasks);
            var expected = new List<string> { "B B B B B B B B B B B B B B B B B B B B B B B B B A A A A A"};
            Assert.AreNotEqual(expected[0], structure[0]);
        }

        [TestMethod]
        public void SafetyStructTest()
        {
            string[] strings = {
                                   "A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A"
                               };
            var structure = new SafetyStruct(strings);
            Task[] tasks = new Task[5];
            for (var i = 0; i < 5; i++)
                tasks[i] = Task.Factory.StartNew(() => Replace(structure, "A", "B", 5));
            Task.WaitAll(tasks);
            var expected = new List<string> { "B B B B B B B B B B B B B B B B B B B B B B B B B A A A A A" };
            Assert.AreEqual(expected[0], structure[0]);
        }

        [TestMethod]
        public void BenchmarkTest()
        {
            var rand = new Random();
            var baseStrings = new [] {"apple", "banana", "orange"};
            var strEnum = Enumerable.Range(0, 10000).Select(i => baseStrings[rand.Next(3)]).ToArray();
            var strings = strEnum.ToArray();
            var structure = new SafetyStruct(strings);
            var watch = new Stopwatch();
            watch.Start();
            Replace(structure, "apple", "banana", 1500);
            Replace(structure, "banana", "apple", 1600);
            Replace(structure, "orange", "banana", 1000);
            watch.Stop();
            var t1 = watch.ElapsedTicks;
            strings = strEnum.ToArray();
            var structure1 = new SafetyStruct(strings);
            
            //watch = new Stopwatch();
            watch.Restart();
            var tasks = new List<Task>
            {
                Task.Factory.StartNew(() => Replace(structure1, "apple", "banana", 1500)),
                Task.Factory.StartNew(() => Replace(structure1, "banana", "apple", 1600)),
                Task.Factory.StartNew(() => Replace(structure1, "orange", "banana", 1000))
            };
            Task.WaitAll(tasks.ToArray());
            watch.Stop();
            var t2 = watch.ElapsedTicks;
            //"C:\Users\Alexey\Desktop\classNames.txt"
            FileStream fs = new FileStream("C:/Users/Alexey/Desktop/TestResult.txt", FileMode.Create);
            TextWriter tmp = Console.Out;
            StreamWriter sw = new StreamWriter(fs);
            Console.SetOut(sw);
            Console.WriteLine(t1 + " " + t2);
            Console.SetOut(tmp);
            sw.Close();
            Assert.IsTrue(t1 > t2);
            //for (var i = 0; i < structure._strings.Count; i++)
            //    Assert.AreEqual(structure._strings[i], structure1._strings[i]);
        }
    }
}
