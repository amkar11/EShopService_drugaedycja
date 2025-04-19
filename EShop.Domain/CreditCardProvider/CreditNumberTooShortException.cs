using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain.CreditCardProvider
{
    public class CreditNumberTooShortException : Exception
    {
        public CreditNumberTooShortException() { }
        public CreditNumberTooShortException(string message) : base(message) { }
        public CreditNumberTooShortException(string message, Exception innerException) : base(message, innerException) { }
    }
}
