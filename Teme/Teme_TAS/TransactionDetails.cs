using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//------------------------------------------------------------
//------------------Transaction Details Class-----------------
//------------------------------------------------------------

namespace bankAccount
{
    // Class to represent transaction details
    public class TransactionDetails
    {
        public DateTime TransactionDate { get; } // Date of the transaction
        public decimal Amount { get; } // Amount involved in the transaction
        public string Description { get; set; } // Description of the transaction

        public TransactionDetails(DateTime transactionDate, decimal amount, string description)
        {
            TransactionDate = transactionDate;
            Amount = amount;
            Description = description;
        }
    }
}
