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
        [field:SerializeField] public EventReference RockFallOnPlayerSound { get; private set; }
        [field:SerializeField] public EventReference RopeSwingSound { get; private set; }
        [field:SerializeField] public EventReference RopeMovingDownSound { get; private set; }

        
        [field: Header("Ending SFX")]
        [field:SerializeField] public EventReference RocksStartFallingSound { get; private set; }
        [field:SerializeField] public EventReference GiantRockKillsPlayerSound { get; private set; }
        [field:SerializeField] public EventReference LastMessageSound { get; private set; }
        [field:SerializeField] public EventReference CorpseSound { get; private set; }
        
        [field: Header("Random SFX")]
        [field:SerializeField] public EventReference DropsFallingSound { get; private set; }
        [field:SerializeField] public EventReference BatsSoundSound { get; private set; }

        [field: Header("Helps SFX")]
        [field:SerializeField] public EventReference Help1Sound { get; private set; }
        [field:SerializeField] public EventReference Help2Sound { get; private set; }
        [field:SerializeField] public EventReference HelpDyingSound { get; private set; }
        
        

    }
}