using System;
using System.Linq;
using System.Collections.Generic;

namespace Rollback
{
    class Program
    {
        static void Main(string[] args)
        {
            Bank bank = new Bank();

            bank.CreateAccount(1000);
            bank.CreateAccount(2000);
            bank.CreateAccount(0);
            bank.Undo();

            bank.Transfer(0, 1, money: 500);
            bank.Undo();

            bank.CloseAccount(0);
            bank.Undo();
        }
    }

    class Bank
    {
        private readonly List<Account> _accounts;
        private readonly Stack<ICommand> _executedCommands;

        public Bank()
        {
            _accounts = new List<Account>(10);
            _executedCommands = new Stack<ICommand>(20);
        }


        public void CreateAccount(uint money)
        {
            _executedCommands.Push(new CreateAccount(_accounts, money));
            _executedCommands.Peek().Execute();
        }

        public void Transfer(uint ownerID, uint targetID, uint money)
        {
            _executedCommands.Push(new MoneyTransfer(GetAccountByID(ownerID), GetAccountByID(targetID), money));
            _executedCommands.Peek().Execute();
        }

        public void CloseAccount(uint id)
        {
            _executedCommands.Push(new CloseAccount(_accounts, id));
            _executedCommands.Peek().Execute();
        }

        private Account GetAccountByID(uint id) =>
            _accounts.FirstOrDefault(account => account.GetID() == id)
            ?? throw new ArgumentException(paramName: nameof(id), message: "You try get non-existent account.");

        public void Undo() => _executedCommands.Pop().Undo();
    }

    class Account : IBankAccount
    {
        private readonly uint _id;
        private uint _money;

        public Account(uint id) =>
            (this._id) = (id);

        public Account(uint id, uint money) : this(id) =>
            (this._id, _money) = (id, money);

        public uint GetID() => _id;
        public uint GetMoneyAmount() => _money;

        public void AddMoney(uint value) => _money += value;
        public void TakeMoney(uint value) => _money -= value;
    }

    interface IBankAccount
    {
        uint GetID();
        uint GetMoneyAmount();
    }

    interface ICommand
    {
        void Execute();
        void Undo();
    }

    class CreateAccount : ICommand
    {
        private readonly List<Account> _accounts;
        private readonly uint _createdID;
        private readonly uint _money;

        public CreateAccount(List<Account> accounts, uint money) =>
            (_accounts, _createdID, _money) = (accounts, (uint)accounts.Count, money);

        public void Execute() =>
            _accounts.Add(new Account(_createdID, _money));

        public void Undo()
        {
            int deletedAccountIndex = _accounts.FindIndex(account => account.GetID() == _createdID);
            _accounts.RemoveAt(deletedAccountIndex);
        }
    }

    class MoneyTransfer : ICommand
    {
        private readonly Account _ower;
        private readonly Account _target;
        private readonly uint _value;

        public MoneyTransfer(Account ower, Account target, uint value) =>
            (_ower, _target, _value) = (ower, target, value);

        public void Execute()
        {
            _ower.TakeMoney(_value);
            _target.AddMoney(_value);
        }

        public void Undo()
        {
            _ower.AddMoney(_value);
            _target.TakeMoney(_value);
        }
    }

    class CloseAccount : ICommand
    {
        private readonly List<Account> _accounts;
        private readonly uint _accountId;
        private readonly uint _money;

        public CloseAccount(List<Account> accounts, uint accountId)
        {
            _accounts = accounts;
            _accountId = accountId;
            _money = _accounts.Find(account => account.GetID() == accountId).GetMoneyAmount();
        }

        public void Execute()
        {
            int deletedAccountIndex = _accounts.FindIndex(account => account.GetID() == _accountId);
            _accounts.RemoveAt(deletedAccountIndex);
        }

        public void Undo()
        {
            _accounts.Add(new Account((uint)_accounts.Count, _money));
        }
    }
}