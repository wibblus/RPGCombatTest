using System.Collections;
using UnityEngine;
using Units;

namespace Actions
{
    public class Attack : CombatAction
    {
        public override string Name => "Attack";
        // constructor
        public Attack(ActionHandler issuer) : base(issuer) { }
        
        // properties
        private Unit _target;
        private int _damage;
        private int _damageVariance;
        
        // keep resulting values in case of undo
        private int _damageResult;
        
        public override IEnumerator Perform()
        {
            // tell InputManager to begin checking for unit selection
            InputManager.Instance.EnableUnitSelection();
            // only proceed with the action once a target has been selected
            yield return new WaitUntil(() => InputManager.Instance.SelectedUnit is not null);
            
            _target = InputManager.Instance.SelectedUnit;
            
            // action logic
            
            var t = 0.0f;
            
            var startPos = Issuer.transform.position;
            var targetPos = _target.transform.position + _target.transform.forward;

            while (t < 1.0f)
            {
                Issuer.transform.position = Vector3.Lerp(startPos, targetPos, t);
                
                t += Time.deltaTime*2;
                yield return null;
            }

            _damage = Issuer.unit.Strength;
            var totalDamage = Mathf.Max(0, _damage + Random.Range(-_damageVariance, _damageVariance+1) - _target.Defense);
            
            var prevHealth = _target.Health;
            // damage target unit
            _target.Health -= totalDamage;
            // track true health difference
            _damageResult = prevHealth - _target.Health;
            
            Debug.Log("Dealt " + totalDamage + " damage !");

            // pause before returning
            yield return new WaitForSeconds(0.25f);
            
            while (t > 0.0f)
            {
                Issuer.transform.position = Vector3.Lerp(startPos, targetPos, t);
                
                t -= Time.deltaTime*3;
                yield return null;
            }

            Complete();
        }
        
        public override void Undo()
        {
            _target.Health += _damageResult;
        }
    }
}