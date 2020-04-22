using System;

namespace Wombat
{
    abstract class Unit
    {
        private int _health;

        public void TakeDamage(int damage)
        {
            ApplyDamage(damage);

            if (_health <= 0)
                Console.WriteLine("I'm died");
        }

        protected abstract void ApplyDamage(int damage);
    }

    class Wombat : Unit
    {
        private int _armor;

        protected override void ApplyDamage(int damage) =>
            _health -= damage - _armor;
    }

    class Human : Unit
    {
        private int _agility;

        protected override void ApplyDamage(int damage) =>
            _health -= damage / _agility;
    }
}
