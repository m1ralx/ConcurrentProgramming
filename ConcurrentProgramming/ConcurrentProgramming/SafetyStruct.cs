using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentProgramming
{
    public class SafetyStruct : IStruct
    {
        private readonly object[] lockers;
        private readonly List<string> _strings;
        private object locker;
        public SafetyStruct(string[] strings)
        {
            locker = new object();
            _strings = strings.ToList();
            lockers = Enumerable.Range(0, strings.Length).Select(i => new object()).ToArray();
        }

        public Tuple<int, string> ReplaceFirst(string word, string replace)
        {
            
            lock(locker)
            {
                var firstInclusionIndex = _strings.IndexOf(_strings.First(str => str.Contains(word)));
                if (firstInclusionIndex < 0)
                    return null;
                var indexOfSubstr = _strings[firstInclusionIndex].IndexOf(word);
                if (indexOfSubstr < 0)
                    return null;
                _strings[firstInclusionIndex] = _strings[firstInclusionIndex].Substring(0, indexOfSubstr) + replace +
                                                _strings[firstInclusionIndex].Substring(indexOfSubstr + word.Length);
                return Tuple.Create(firstInclusionIndex, _strings[firstInclusionIndex]);  
            }
        }
        public string this[int i]
        {
            get { return _strings[i]; }
        }
        public List<string> Take(int n)
        {
            if (n < 0 || n > _strings.Count)
                throw new ArgumentException("Bad argument n");
            return _strings.Take(n).ToList();
        }
        public List<string> TakeLast(int n)
        {
            if (n < 0 || n > _strings.Count)
                throw new ArgumentException("Bad argument n");
            return _strings.Skip(_strings.Count - n).ToList();
        }
    }
}
