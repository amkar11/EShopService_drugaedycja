using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain.ProductProvidersExceptions
{
    public class ProductDoesNotExistException : Exception
    {
        public ProductDoesNotExistException() { }
        public ProductDoesNotExistException(string message) : base(message) { }
        public ProductDoesNotExistException(string message, Exception innerException) : base(message, innerException) { }
    }
}
