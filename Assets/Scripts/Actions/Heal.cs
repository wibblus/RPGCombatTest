using System.Collections;
using UnityEngine;
using Units;

namespace Actions
{
    public class Heal : CombatAction
    {
        public override string Name => "Heal";
        // constructor
        public Heal(ActionHandler issuer) : base(issuer) { }
        
        // properties
        private Unit _target;
        private int _healAmount;
        
        // keep resulting values in case of undo
        private int _healResult;
        
        public override IEnumerator Perform()
        {
            InputManager.Instance.EnableUnitSelection();
            yield return new WaitUntil(() => InputManager.Instance.SelectedUnit is not null);
            
            _target = InputManager.Instance.SelectedUnit;
            
            // action logic
            
            var t = 0.0f;
            
            var startPos = Issuer.transform.position;
            while (t < 1.0f)
            {
                Issuer.transform.position = startPos + new Vector3(0f, Mathf.Sin(t*Mathf.PI), 0f);
                
                t += Time.deltaTime*2;
                yield return null;
            }

            _healAmount = Issuer.unit.Magic;
            var totalHeal = _healAmount;
            
            var prevHealth = _target.Health;
            // heal target unit
            _target.Health += totalHeal;
            // track true health difference
            _healResult = prevHealth - _target.Health;
            
            Debug.Log("Healed " + totalHeal + " !");
            
            yield return new WaitForSeconds(0.25f);
            
            startPos = _target.transform.position;
            while (t > 0.0f)
            {
                _target.transform.position = startPos + new Vector3(0f, Mathf.Sin(t*Mathf.PI), 0f);
                
                t -= Time.deltaTime*2;
                yield return null;
            }

            Complete();
        }
        
        public override void Undo()
        {
            _target.Health -= _healResult;
        }
    }
}