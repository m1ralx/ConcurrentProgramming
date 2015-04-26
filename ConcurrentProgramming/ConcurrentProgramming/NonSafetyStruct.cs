using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentProgramming
{
    public class NonSafetyStruct : IStruct
    {
        private readonly List<string> _strings;
        public NonSafetyStruct(string[] strings)
        {
            _strings = strings.ToList();
        }

        public Tuple<int, string> ReplaceFirst(string word, string replace)
        {
            int firstInclusionIndex;
            try
            {
                firstInclusionIndex = _strings.IndexOf(_strings.First(str => str.Contains(word)));
                _strings[firstInclusionIndex] = _strings[firstInclusionIndex].Replace(word, replace);
            }
            catch
            {
                return null;
            }
            return Tuple.Create(firstInclusionIndex, _strings[firstInclusionIndex]);
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
