using System;
using System.Collections.Generic;
using UnityEngine;
using Units;

public class ActionHandler : MonoBehaviour
{
    [HideInInspector] public Unit unit;

    public readonly List<CombatAction> KnownActions = new();
    public bool IsActionable { get; private set; }

    private void Awake()
    {
        unit = GetComponent<Unit>();
        IsActionable = true;
        
        // ensure that dead units cannot act
        unit.OnDeath += _ => IsActionable = false;
        unit.OnRevived += _ => IsActionable = true;
    }
}