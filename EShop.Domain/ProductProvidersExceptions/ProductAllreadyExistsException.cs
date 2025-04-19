using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain.ProductProvidersExceptions
{
    public class ProductAllreadyExistsException : Exception
    {
        public ProductAllreadyExistsException() { }
        public ProductAllreadyExistsException(string message) : base(message) { }
        public ProductAllreadyExistsException(string message, Exception innerException) : base(message, innerException) { }
    }
}
