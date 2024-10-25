using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : Singleton<CombatManager>
{
    private readonly Stack<CombatAction> _actionStack = new();
    private readonly List<ActionHandler> _turnOrder = new();
    private int _currentTurnIndex;

    public bool IsActionPlaying { get; private set; }

    public void BeginCombat()
    {
        // refresh turn order to include current active Units with ActionHandlers
        _turnOrder.Clear();
        foreach (var unit in FindObjectsOfType<ActionHandler>())
            _turnOrder.Add(unit);
        _currentTurnIndex = -1;

        BeginNextTurn();
        
        UIManager.Instance.EnableCombatUI();
    }

    public void EndCombat()
    {
        _turnOrder.Clear();
        _actionStack.Clear();
        
        UIManager.Instance.DisableCombatUI();
    }

    public void BeginNextTurn()
    {
        var startIndex = _currentTurnIndex;
        do
        {
            IncrementTurn();
            // prevent endless loop
            if (_currentTurnIndex == startIndex) break;
        }
        while (!GetCurrentActionHandler().IsActionable);

        IsActionPlaying = false;
        
        UIManager.Instance.UpdateTurn();
    }

    public void PerformAction(CombatAction action)
    {
        if (action == null) return;
        
        StartCoroutine(action.Perform());
        _actionStack.Push(action);
        IsActionPlaying = true;
        
        UIManager.Instance.HideCursor();
    }

    public void UndoLastAction()
    {
        if (_actionStack.Count == 0) return;

        var action = _actionStack.Pop();
        action.Undo();
        
        DecrementTurn();
        UIManager.Instance.UpdateTurn();
        
        Debug.Log("Undid " + action.Name + ", performed by " + action.Issuer);
    }

    public ActionHandler GetCurrentActionHandler()
    {
        return _turnOrder[_currentTurnIndex];
    }

    private void IncrementTurn()
    {
        _currentTurnIndex++;
        if (_currentTurnIndex >= _turnOrder.Count)
            _currentTurnIndex -= _turnOrder.Count;
    }
    private void DecrementTurn()
    {
        _currentTurnIndex--;
        if (_currentTurnIndex < 0)
            _currentTurnIndex += _turnOrder.Count;
    }
}