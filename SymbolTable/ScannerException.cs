using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolTable
{
    public class ScannerException : Exception
    {
        public ScannerException(string message) : base(message)
        {
        }
    }
}
