using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.ParserLayer.ErrorHandling
{
    public static class RuddyRexError
    {
        private static List<Exception> errors = new();
        public static void Report(Exception exception)
        {
            errors.Add(exception);
            //Console.Error.WriteLine($"[Line {line.ToString()}] Error: {where}: {message}");
        }

        public static List<Exception> GetErrors()
        {
            return errors;
        }

        public static void ClearErrors()
        {
            errors.Clear();
        }
    }
}
