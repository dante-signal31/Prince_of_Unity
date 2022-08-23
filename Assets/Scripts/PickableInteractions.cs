using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Prince
{
    public class PickableInteractions : MonoBehaviour
    {
        [Header("WIRING:")] 
        [Tooltip("Needed to know when we are picking something.")] 
        [SerializeField] private CharacterStatus characterStatus;
        [Tooltip("Needed to signal if there is a pickable in range.")]
        [SerializeField] private Animator stateMachine;
        [Tooltip("Needed to know which pickables we can take.")]
        [SerializeField] private PickableSensor pickableSensor;
        [Tooltip("Needed to set new life values.")]
        [SerializeField] private HealthController healthController;

        private bool _alreadyTakingSomething;
        private HashSet<PickableCharacterInteractions> _pickablesInRange;

        /// <summary>
        /// Used by small potions to restore one health point.
        /// </summary>
        public void RestoreOneHealthPoint()
        {
            healthController.HealLifePoint();
        }

        /// <summary>
        /// Used by big potions to restore all health points and add an extra one.
        /// </summary>
        public void EnhanceHealth()
        {
            healthController.AddMaximumLifePoint();
            healthController.HealLifePoint();
        }

        /// <summary>
        /// Used by sword to give sword to Prince character.
        /// </summary>
        public void TakeSword()
        {
            characterStatus.HasSword = true;
        }

        public void PickingAnimationStarted()
        {
            stateMachine.SetBool("PickingAnimationPlaying", true);
        }

        public void PickingAnimationEnded()
        {
            stateMachine.SetBool("PickingAnimationPlaying", false);
        }

        /// <summary>
        /// Called from pickable sensor to warn a pickable is in range or have left it.
        /// </summary>
        /// <param name="_">Actually not used, but needed to match event signature. </param>
        public void UpdatePickableSet(PickableCharacterInteractions _)
        {
            stateMachine.SetBool("PickablesInRange", pickableSensor.AnythingPickable);
            _pickablesInRange = pickableSensor.PickablesInRange;
        }

        private void Update()
        {
            if (characterStatus.CurrentState == CharacterStatus.States.TakingPickable)
            {
                if (!_alreadyTakingSomething)
                {
                    // In a first approach I iterated through sensor pickableInRange directly. Problem
                    // with that is that TakingPickable state ends when pickable animation ends and they
                    // are disabled so they are no longer detected by sensor. Problem is that that can take
                    // its time, so if we iterate through sensor list we can get here a second time if in
                    // the next frame pickable has not been disabled yet although we have already processed it.
                    List<PickableCharacterInteractions> pickablesList = _pickablesInRange.ToList();
                    foreach (PickableCharacterInteractions pickable in pickablesList)
                    {
                        pickable.Take(this);
                        _pickablesInRange.Remove(pickable);
                    }
                    _alreadyTakingSomething = false;
                }
            }
        }
    }
}