using System;
using UnityEngine;

namespace Prince
{
    [ExecuteAlways]
    public class IronCurtainController : MonoBehaviour
    {
        [Header("WIRING:")] 
        [Tooltip("Needed to mode iron curtain.")]
        [SerializeField] private Transform curtainTransform;
        [Tooltip("Needed to block way when curtain is closed.")]
        [SerializeField] private BoxCollider2D curtainCollider;


        [Header("CONFIGURATION:")] 
        [Tooltip("Needed to calculate Y offsets.")]
        [SerializeField] private float colliderInitialYPosition;
        [Tooltip("Minimum Y size for curtain collider.")]
        [SerializeField] private float colliderMinimumHeight;
        [Tooltip("Maximum Y size for curtain collider.")]
        [SerializeField] private float colliderMaximumHeight;
        [Tooltip("Height when curtain is considered open.")]
        [SerializeField] private float openingHeight;
        [Tooltip("Height when curtain is considered closed.")]
        [SerializeField] private float closingHeight;
        [Tooltip("Initial opening of this gate (0 closed 1 full open).")]
        [Range(0.0f, 1.0f)] 
        [SerializeField] private float initialOpening;

        // [Header("DEBUG:")]
        // [Tooltip("Show this component logs on console window.")]
        // [SerializeField] private bool showLogs;

        /// <summary>
        /// <p>Set current opening for iron curtain.</p>
        ///
        /// <p>Opening percentage used here goes from 0 (closed) to 1 (open). Values over 1 will be
        /// converted to 1 and values under 0 will converted to 0.</p>
        /// </summary>
        /// <param name="newOpening">Opening, from 0 (closed) to 1 (open)</param>
        private void setOpening(float newOpening)
        {
            float opening = Mathf.Clamp(newOpening, 0, 1);
            float newCurtaingHeight = Mathf.Lerp(closingHeight, openingHeight, opening);
            curtainTransform.localPosition = new Vector3(curtainTransform.localPosition.x,
                newCurtaingHeight,
                curtainTransform.localPosition.z);
            setColliderSize(opening);
        }

        /// <summary>
        /// Make collider longer while curtain closes to block the way.
        /// </summary>
        /// <param name="portcullisOpening">Opening, from 0 (closed) to 1 (open)</param>
        private void setColliderSize(float portcullisOpening)
        {
            float opening = Mathf.Clamp(portcullisOpening, 0, 1);
            // Collider size rate is the inverse of opening rate.
            float colliderSize = (1 - opening);
            float newColliderHeight = Mathf.Lerp(colliderMinimumHeight, colliderMaximumHeight, colliderSize);
            curtainCollider.size = new Vector2(curtainCollider.size.x, newColliderHeight);
            curtainCollider.transform.localPosition = new Vector3(curtainCollider.transform.localPosition.x,
                colliderInitialYPosition - ((newColliderHeight - colliderMinimumHeight) / 2),
                curtainCollider.transform.localPosition.z);
        }

        private void OnValidate()
        {
            setOpening(initialOpening);
        }
    }
}