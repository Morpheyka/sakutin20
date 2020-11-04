using System;

namespace Wombat
{
    abstract class Unit
    {
        private int _health;

        public void TakeDamage(int damage)
        {
            _health -= CalculateDamage(damage);

            if (_health <= 0)
                Console.WriteLine("I'm died");
        }

        protected abstract int CalculateDamage(int damage);
    }

    class Wombat : Unit
    {
        private int _armor;

        protected override int CalculateDamage(int damage)
        {
            return damage - _armor;
        }
    }

    class Human : Unit
    {
        private int _agility;

        protected override int CalculateDamage(int damage)
        {
            return damage / _agility;
        }
    }
}
