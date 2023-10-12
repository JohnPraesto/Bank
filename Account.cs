using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    internal class Account
    {
        public string userName;
        public string accountName;
        public int sum;

        public Account(string username, string accountName)
        {
            this.userName = username;
            this.accountName = accountName;
        }
    }
}
