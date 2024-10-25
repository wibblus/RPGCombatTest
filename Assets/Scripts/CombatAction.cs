using System.Collections;
using UnityEngine;

public abstract class CombatAction
{
    public virtual string Name => "Action";
    public readonly ActionHandler Issuer;

    protected CombatAction(ActionHandler issuer)
    {
        Issuer = issuer;
    }
    
    public abstract IEnumerator Perform();
    public abstract void Undo();

    protected static void Complete()
    {
        CombatManager.Instance.BeginNextTurn();
    }
}