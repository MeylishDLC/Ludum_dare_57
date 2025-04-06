using System;
using UnityEngine;

namespace QuickTimeEvents
{
    public abstract class BaseQte: MonoBehaviour
    {
        public abstract event Action<BaseQte,Action> OnTryStartQte;
        public bool IsWorking { get; set; } = true;

        public abstract void StartQte();
    }
}