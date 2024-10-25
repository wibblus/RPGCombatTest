using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject combatUIRoot;

    [SerializeField] private Button[] actionButtons;
    private TextMeshProUGUI[] _actionButtonTexts;
    
    [SerializeField] private TextMeshProUGUI unitNameText;
    [SerializeField] private TextMeshProUGUI unitHealthText;
    
    [SerializeField] private RectTransform cursor;
    
    private Camera _mainCamera;

    protected override void Awake()
    {
        base.Awake();
        
        _actionButtonTexts = new TextMeshProUGUI[actionButtons.Length];
        for (var i = 0; i < actionButtons.Length; i++)
            _actionButtonTexts[i] = actionButtons[i].GetComponentInChildren<TextMeshProUGUI>();
        
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        cursor.position += new Vector3(0, Mathf.Sin(Time.realtimeSinceStartup*3) / 50f, 0);
    }

    public void EnableCombatUI()
    {
        combatUIRoot.SetActive(true);
    }

    public void DisableCombatUI()
    {
        combatUIRoot.SetActive(false);
    }

    public void HideCursor()
    {
        cursor.gameObject.SetActive(false);
    }

    public void UpdateTurn()
    {
        var actionHandler = CombatManager.Instance.GetCurrentActionHandler();
        
        for (var i = 0; i < actionButtons.Length; i++)
        {
            var button = actionButtons[i];

            if (actionHandler.KnownActions.Count > i)
            {
                button.gameObject.SetActive(true);
                _actionButtonTexts[i].text = actionHandler.KnownActions[i].Name;
            }
            else 
                button.gameObject.SetActive(false);
        }

        var unit = actionHandler.unit;
        
        unitNameText.text = unit.Name;
        unitHealthText.text = unit.Health + " / " + unit.MaxHealth;
        
        cursor.gameObject.SetActive(true);
        cursor.position = _mainCamera.WorldToScreenPoint(actionHandler.transform.position + new Vector3(0, 2, 0));
    }
}