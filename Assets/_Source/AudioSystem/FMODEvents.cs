using FMODUnity;
using UnityEngine;

namespace AudioSystem
{
    [CreateAssetMenu(menuName = "AudioSystem/FMOD Events")]
    public class FMODEvents: ScriptableObject
    {
        [field: Header("Music|Ambient")]
        [field:SerializeField] public EventReference MainMenuMusic { get; private set; }
        [field:SerializeField] public EventReference CaveAmbient { get; private set; }
        
        [field: Header("SFX")]
        [field:SerializeField] public EventReference BatsSound { get; private set; }
        [field:SerializeField] public EventReference RockFallOnPlayerSound { get; private set; }
        [field:SerializeField] public EventReference RopeSwing { get; private set; }
        [field:SerializeField] public EventReference RopeMovingDown { get; private set; }
        [field:SerializeField] public EventReference PlayerFallToGround { get; private set; }
        
        [field: Header("Random SFX")]
        [field:SerializeField] public EventReference DropsFalling { get; private set; }
    }
}