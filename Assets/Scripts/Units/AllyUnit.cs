using Actions;
using UnityEngine;

namespace Units
{
    public class AllyUnit : Unit, ICombatUnit
    {
        public void Initialize()
        {
            Name = "Ally Unit";
            var actionHandler = gameObject.AddComponent<ActionHandler>();
            
            actionHandler.KnownActions.Add(new Attack(actionHandler));
            actionHandler.KnownActions.Add(new Defend(actionHandler));
            actionHandler.KnownActions.Add(new Heal(actionHandler));
        }
    }
}