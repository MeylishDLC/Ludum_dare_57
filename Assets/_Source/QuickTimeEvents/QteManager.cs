using System;
using System.Collections.Generic;
using UnityEngine;

namespace QuickTimeEvents
{
    public class QteManager: MonoBehaviour
    {
        [SerializeField] private SerializedDictionary<BaseQte, QteHandler> qtePairs;

        private void Start()
        {
            SubscribeOnEvents();
        }
        private void OnDestroy()
        {
            UnsubscribeOnEvents();
        }
        private void UnsubscribeOnEvents()
        {
            foreach (var baseQte in qtePairs.Keys)
            {
                baseQte.OnTryStartQte -= TriggerQte;
            }
        }
        private void SubscribeOnEvents()
        {
            foreach (var baseQte in qtePairs.Keys)
            {
                baseQte.OnTryStartQte += TriggerQte;
            }
        }

        private bool IsAnyQteActive()
        {
            foreach (var baseQte in qtePairs.Values)
            {
                if (baseQte.EventIsActive)
                {
                    return true;
                }
            }
            return false;
        }

        private void TriggerQte(BaseQte qte, Action successCallback)
        {
            if (IsAnyQteActive())
            {
                return;
            }
            var handler = qtePairs[qte];
            qte.StartQte();
            handler.StartQTE(successCallback);
        }
    }
}