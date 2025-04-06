using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Rocks
{
    [CreateAssetMenu(fileName = "RockUtilityConfig", menuName = "RockUtilityConfig")]
    public class RockUtilityConfig: ScriptableObject
    {
        [field: SerializeField] public List<GameObject> RocksPrefabs {get; private set;}
    }
}