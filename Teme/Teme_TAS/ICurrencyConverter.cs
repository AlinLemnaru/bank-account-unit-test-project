using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//------------------------------------------------------------
//----------------Currency Converter Interface----------------
//------------------------------------------------------------

namespace bankAccount
{
    // Interface for currency conversion from RON to Multiple Currencies
    // We return the rate for conversion
    public interface ICurrencyConverter
    {
        public decimal GetEurToRonRate(); // Euro -> Ron conversion rate
    }
}
