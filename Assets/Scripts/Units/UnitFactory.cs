using UnityEngine;

namespace Units
{
    public class UnitFactory : MonoBehaviour
    {
        [SerializeField] private GameObject unitPrefab;
        
        public ICombatUnit PlaceUnit(Vector3 position)
        {
            var instance = Instantiate(unitPrefab, position, unitPrefab.transform.rotation);
            var unit = instance.GetComponent<ICombatUnit>();
            
            unit.Initialize();
            
            return unit;
        }
    }
}