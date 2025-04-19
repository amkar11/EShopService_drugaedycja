using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Application
{
    public interface ICreditCardService
    {
        bool ValidateCard(string cardNumber);
        string GetCardType(string cardNumber);
    }
}

