using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain.CreditCardProvider
{
    public class CreditNumberTooLongException : Exception
    {
        public CreditNumberTooLongException() { }
        public CreditNumberTooLongException(string message) : base(message) { }
        public CreditNumberTooLongException(string message, Exception innerException) : base(message, innerException) { }
    }
}
