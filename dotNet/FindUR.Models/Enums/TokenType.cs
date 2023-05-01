using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Enums
{
    public enum TokenType
    {
        NotSet = 0,
        NewUser,
        ResetPassword,
        BankAccount,
        CreditCardAccount,
        Facebook,
        Instagram,
        Twitter
    }
}
