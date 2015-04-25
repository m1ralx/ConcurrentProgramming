using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentProgramming
{
    public class NonSafetyStruct
    {
        private readonly List<string> _strings;
        public NonSafetyStruct(string[] strings)
        {
            _strings = strings.ToList();
        }

        public Tuple<int, string> ReplaceFirst(string word, string replace)
        {
            var firstInclusionIndex = _strings  .IndexOf(_strings.First(str => str.Contains(word)));
            if (firstInclusionIndex == -1) return null;
            _strings[firstInclusionIndex] = _strings[firstInclusionIndex].Replace(word, replace);
            return Tuple.Create(firstInclusionIndex, _strings[firstInclusionIndex]);
        }
    }
}
