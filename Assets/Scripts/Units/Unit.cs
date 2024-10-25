using System;
using UnityEngine;

namespace Units
{
    public class Unit : MonoBehaviour
    {
        public string Name { get; protected set; }
        
        private Collider _collider;
        
        public int Health
        {
            get => _health;
            set => SetHealth(value);
        }
        public int Defense => defense + defenseMod;
        public int Strength => strength + strengthMod;
        public int Magic => magic + magicMod;

        public int MaxHealth => maxHealth;
        private int _health;
        [SerializeField] private int maxHealth;
        [SerializeField] private int defense;
        [SerializeField] private int strength;
        [SerializeField] private int magic;
        
        [HideInInspector] public int defenseMod;
        [HideInInspector] public int strengthMod;
        [HideInInspector] public int magicMod;

        public event Action<Unit> OnHealthChanged;
        public event Action<Unit> OnDeath;
        public event Action<Unit> OnRevived;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            
            _health = MaxHealth;

            OnDeath += _ => EnterDeathState();
            OnRevived += _ => LeaveDeathState();
        }

        private void SetHealth(int value)
        {
            value = Math.Clamp(value, 0, MaxHealth);
            
            OnHealthChanged?.Invoke(this);
            if (_health <= 0 && value > 0)
                OnRevived?.Invoke(this);
            if (value <= 0)
                OnDeath?.Invoke(this);
            
            _health = value;
        }

        private void EnterDeathState()
        {
            transform.position -= new Vector3(0, 0.5f, 0);
            
            _collider.enabled = false;
        }
        private void LeaveDeathState()
        {
            transform.position += new Vector3(0, 0.5f, 0);
            
            _collider.enabled = true;
        }
    }
}