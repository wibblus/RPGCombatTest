using System;
using Units;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    [SerializeField] private InputAction selectAction;
    private Camera _mainCamera;
    
    public Unit SelectedUnit { get; private set; }
    private bool _inUnitSelection;

    protected override void Awake()
    {
        base.Awake();
        
        _mainCamera = Camera.main;
        
        selectAction.performed += _ => OnSelect();
    }
    
    public void InitiateAction(int index)
    {
        if (CombatManager.Instance.IsActionPlaying) return;
        
        var actionHandler = CombatManager.Instance.GetCurrentActionHandler();
        var type = actionHandler.KnownActions[index].GetType();
        
        // create new instance of the CombatAction type specified for this button index.
        var action = type.GetConstructor(new [] { typeof(ActionHandler) })?
            .Invoke(new object[] { actionHandler }) as CombatAction;
        
        // pass onto the CombatManager to be performed
        CombatManager.Instance.PerformAction(action);
    }
    
    public void EnableUnitSelection()
    {
        SelectedUnit = null;
        _inUnitSelection = true;
    }

    private void OnEnable()
    {
        selectAction.Enable();
    }
    private void OnDisable()
    {
        selectAction.Disable();
    }

    private void OnSelect()
    {
        if (!_inUnitSelection) return;
        
        var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var hit)) return;
        
        Debug.Log(hit.collider.name);
        
        SelectedUnit = hit.collider.GetComponent<Unit>();
        if (SelectedUnit != null)
            _inUnitSelection = false;
    }
}