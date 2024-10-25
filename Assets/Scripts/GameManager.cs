using System;
using UnityEngine;
using Units;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UnitFactory allyUnitFactory;
    [SerializeField] private UnitFactory enemyUnitFactory;
    [SerializeField] private UnitFactory dummyUnitFactory;
    
    
    private void Start()
    {
        allyUnitFactory.PlaceUnit(new Vector3(0, 0, 0));
        allyUnitFactory.PlaceUnit(new Vector3(2, 0, 0));
        allyUnitFactory.PlaceUnit(new Vector3(4, 0, 0));
        
        enemyUnitFactory.PlaceUnit(new Vector3(0, 0, 6));
        dummyUnitFactory.PlaceUnit(new Vector3(2, 0, 6));
        enemyUnitFactory.PlaceUnit(new Vector3(4, 0, 6));
        
        CombatManager.Instance.BeginCombat();
    }
}