using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BingoUtils.Domain.Exceptions
{
    public class InvalidLanguageException : Exception
    {
        public InvalidLanguageException(string language, string missingKey)
         : base(string.Format("The language {0} is missing key {1}", language, missingKey))
        {

        }
    }
}
