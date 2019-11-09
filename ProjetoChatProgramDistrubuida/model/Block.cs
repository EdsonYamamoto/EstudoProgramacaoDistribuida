using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoChatProgramDistrubuida.model
{
    public class Block
    {
        string version { set; get; }
        string previousHash { set; get; }
        string merkleRoot { set; get; }
        string hash { set; get; }
        string timestamp { set; get; }
        string bits { set; get; }
        string nonce { set; get; }
        string transactionCount { set; get; }

        public List<Transaction> Transactions { get; set; }
    }

    public class Transaction
    {
        public string From { get; }
        public string To { get; }
        public double Amount { get; }
        public Transaction(string from, string to, double amount)
        {
            From = from;
            To = to;
            Amount = amount;
        }
    }
}
