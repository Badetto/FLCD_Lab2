using System;
using System.Text;

namespace SymbolTable
{
    internal class SymbolTable
    {
        private readonly List<List<string>> items;
        private readonly int size;

        public SymbolTable(int _size)
        {
            size = _size;
            items = new List<List<string>>();
            for (int i = 0; i < size; ++i)
            {
                items.Add(new List<string>());
            }
        }

        public int GetSize()
        {
            return size;
        }

        private int Hash(string key)
        {
            int sum = 0;
            foreach (var chr in key)
            {
                sum += (int)chr;
            }
            return sum % size;
        }

        public bool Add(string key)
        {
            int hashValue = Hash(key);
            if (!items[hashValue].Contains(key))
            {
                items[hashValue].Add(key);
                return true;
            }
            return false;
        }

        public bool Contains(string key)
        {
            int hashValue = Hash(key);
            return items[hashValue].Contains(key);
        }

        public bool Remove(string key)
        {
            int hashValue = Hash(key);

            if (items[hashValue].Contains(key))
            {
                items[hashValue].Remove(key);
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            for (int i = 0; i < size; ++i)
            {
                result.Append(i).Append(": [");
                string separator = "";
                foreach (string item in items[i])
                {
                    result.Append(separator);
                    separator = ", ";
                    result.Append(item);
                }
                result.Append("]\n");
            }
            return result.ToString();
        }
    }
}
