using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//------------------------------------------------------------
//----------------Not Enough Funds Exception------------------
//------------------------------------------------------------

namespace bankAccount
{
    // Custom exception for handling insufficient funds scenarios
    public class NotEnoughFundsException : ApplicationException
    {
        public NotEnoughFundsException() : base("Not enough funds in the account to perform this operation.") { }
    }
}
