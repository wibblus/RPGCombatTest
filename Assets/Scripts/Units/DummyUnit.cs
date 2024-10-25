using UnityEngine;

namespace Units
{
    public class DummyUnit : Unit, ICombatUnit
    {
        public void Initialize()
        {
            Name = gameObject.name;
        }
    }
}