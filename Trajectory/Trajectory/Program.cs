using System;
using System.Collections.Generic;

namespace TrajectorySimulation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            UnitsManager units = new UnitsManager();
            units.TryAdd(new Unit((uint)units.UnitsCount, new Position(5, 5)));
            units.TryAdd(new Unit((uint)units.UnitsCount, new Position(10, 10)));
            units.TryAdd(new Unit((uint)units.UnitsCount, new Position(15, 15)));

            Random random = new Random();

            while (true)
            {
                units.CheckCollisions();

                foreach (Unit unit in units.AllUnits)
                {
                    unit.Step(new Position(random.Next(-1, 1), random.Next(-1, 1)));

                    if (unit.IsAlive)
                        unit.Respond();
                }
            }
        }
    }

    public class UnitsManager
    {
        public IReadOnlyList<Unit> AllUnits => _units;
        public int UnitsCount => _units.Count;
        private readonly List<Unit> _units;

        public UnitsManager() =>
            (_units) = (new List<Unit>(10));

        public bool TryAdd(Unit unit)
        {
            _ = unit ?? throw new ArgumentNullException(paramName: nameof(unit),
                message: "Added unit is NULL.");

            if (_units.Contains(unit))
                return false;

            _units.Add(unit);
            return true;
        }

        public void CheckCollisions()
        {
            int unitsCount = _units.Count;

            for (int i = 0; i < unitsCount - 1; i++)
            {
                if (_units[i].Position == _units[i + 1].Position)
                {
                    _units[i].Die();
                    _units[i + 1].Die();
                }

                for (int j = i + 1; j < unitsCount; j++)
                {
                    if (_units[j].Position == _units[i].Position)
                    {
                        _units[j].Die();
                        _units[i].Die();
                    }
                }
            }
        }
    }

    public class Unit
    {
        public Position Position { get; private set; }
        public bool IsAlive { get; private set; }
        public readonly uint id;

        public Unit(uint id) =>
            (this.id, Position, IsAlive) = (id, new Position(0, 0), true);

        public Unit(uint id, Position position)
        {
            this.id = id;
            this.Position = position ?? throw new ArgumentNullException(paramName: nameof(position),
                message: "Position is NULL.");
            IsAlive = true;
        }

        public void Step(Position offset)
        {
            Position.x += (offset.x + Position.x) < 0 ? 0 : offset.x;
            Position.y += (offset.y + Position.y) < 0 ? 0 : offset.y;
        }

        public void Respond()
        {
            Console.SetCursorPosition(Position.x, Position.y);
            Console.Write(id);
        }

        public void Die()
        {
            IsAlive = false;
        }
    }

    public class Position
    {
        public int x;
        public int y;

        public Position(int x, int y) =>
            (this.x, this.y) = (x, y);

        public static bool operator ==(Position obj1, Position obj2) =>
            (obj1.x, obj1.y) == (obj2.x, obj2.y);

        public static bool operator !=(Position obj1, Position obj2) =>
            (obj1.x, obj1.y) != (obj2.x, obj2.y);

        public override bool Equals(object obj) =>
            (obj is Position otherPosition)
             ? this == otherPosition
             : false;

        public override int GetHashCode() => x.GetHashCode() ^ y.GetHashCode();
    }
}