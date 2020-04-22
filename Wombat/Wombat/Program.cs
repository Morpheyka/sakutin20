using System;

namespace Wombat
{
    abstract class Unit
    {
        protected int health;

        public void TakeDamage(int damage)
        {
            ApplyDamage(damage);

            if (health <= 0)
                Console.WriteLine("I'm died");
        }

        protected abstract void ApplyDamage(int damage);
    }

    class Wombat : Unit
    {
        private int _armor;

        protected override void ApplyDamage(int damage) =>
            health -= damage - _armor;
    }

    class Human : Unit
    {
        private int _agility;

        protected override void ApplyDamage(int damage) =>
            health -= damage / _agility;
    }
}
