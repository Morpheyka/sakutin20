using System;

namespace Wombat
{
    abstract class Entity
    {
        public int health;

        public void TakeDamage(int damage)
        {
            ApplyDamage(damage);

            if (health <= 0)
                Console.WriteLine("I'm died");
        }

        protected abstract void ApplyDamage(int damage);
    }

    class Wombat : Entity
    {
        public int armor;

        protected override void ApplyDamage(int damage) =>
            health -= damage - armor;
    }

    class Human : Entity
    {
        public int agility;

        protected override void ApplyDamage(int damage) =>
            health -= damage / agility;
    }
}