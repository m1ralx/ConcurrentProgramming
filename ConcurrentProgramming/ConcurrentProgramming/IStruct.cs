using System;

namespace ConcurrentProgramming
{
    public interface IStruct
    {
        Tuple<int, string> ReplaceFirst(string word, string replace);
    }
}