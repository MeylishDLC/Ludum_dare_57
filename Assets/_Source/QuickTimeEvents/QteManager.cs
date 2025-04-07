using System;
using System.Collections.Generic;
using Core;
using RopeScript;
using UnityEngine;
using Zenject;

namespace QuickTimeEvents
{
    public class QteManager: MonoBehaviour
    {
        [SerializeField] public SerializedDictionary<BaseQte, QteHandler> qtePairs;

        private AnchorController _anchorController;
        private PlayerController _playerController;
        [Inject]
        public void Initialize(AnchorController anchorController, PlayerController playerControllers)
        {
            _anchorController = anchorController;
            _playerController = playerControllers;
        }
        private void Start()
        {
            _anchorController.OnEndReached += SelfDestroy;
            _playerController.OnPlayerDeath += SelfDestroy;
            SubscribeOnEvents();
        }
        private void OnDestroy()
        {
            UnsubscribeOnEvents();
            _anchorController.OnEndReached -= SelfDestroy;
            _playerController.OnPlayerDeath -= SelfDestroy;
        }
        private void SelfDestroy() => Destroy(gameObject);
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