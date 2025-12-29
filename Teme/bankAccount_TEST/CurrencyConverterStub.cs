using bankAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//------------------------------------------------------------
//------------------Currency Converter Stub-------------------
//------------------------------------------------------------

namespace bankAccount
{
    public class CurrencyConverterStub : ICurrencyConverter
    {
        private readonly decimal _fixedRate;

        // Constructor to initialize the stub with a fixed conversion rate
        public CurrencyConverterStub(decimal eurToRonRate)
        {
            _fixedRate = eurToRonRate;
        }

        // Method to get the fixed Euro to RON conversion rate without any external calls
        public decimal GetEurToRonRate()
        {
            return _fixedRate;
        }
    }
}
