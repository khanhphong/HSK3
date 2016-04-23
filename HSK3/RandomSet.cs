using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSK3
{
    class RandomSet
    {
        List<int> _setOfNumbers = new List<int>();

        public List<int> SetOfNumbers
        {
            get { return _setOfNumbers; }
            set { _setOfNumbers = value; }
        }

        Random _random = new Random();

        public RandomSet() { }
        public RandomSet(int min, int max)
        {
            for (int i = min; i <= max; i++)
            {
                _setOfNumbers.Add(i);
            }
        }

        public int Next()
        {
            if (_setOfNumbers.Count > 0)
            {
                int nextNumberIndex = _random.Next(_setOfNumbers.Count);
                int val = _setOfNumbers[nextNumberIndex];
                _setOfNumbers.RemoveAt(nextNumberIndex);
                return val;
            }

            return -1;
        }
    }
}
