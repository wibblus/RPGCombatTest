using Actions;
using UnityEngine;

namespace Units
{
    public class EnemyUnit : Unit, ICombatUnit
    {
        public void Initialize()
        {
            Name = "Enemy Unit";
            var actionHandler = gameObject.AddComponent<ActionHandler>();
            
            actionHandler.KnownActions.Add(new Attack(actionHandler));
            actionHandler.KnownActions.Add(new Defend(actionHandler));
        }
    }
}