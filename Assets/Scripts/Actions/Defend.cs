using System.Collections;
using UnityEngine;
using Units;

namespace Actions
{
    public class Defend : CombatAction
    {
        public override string Name => "Defend";
        // constructor
        public Defend(ActionHandler issuer) : base(issuer) { }
        
        public override IEnumerator Perform()
        {
            var t = 0.0f;
            
            var startRot = Issuer.transform.rotation;
            
            while (t < 1.0f)
            {
                Issuer.transform.Rotate(Vector3.up, Time.deltaTime * 360);
                
                t += Time.deltaTime*2;
                yield return null;
            }
            Issuer.transform.rotation = startRot;

            Issuer.unit.defenseMod = 3;
            
            yield return new WaitForSeconds(0.25f);

            Complete();
        }
        
        public override void Undo()
        {
            
        }
    }
}