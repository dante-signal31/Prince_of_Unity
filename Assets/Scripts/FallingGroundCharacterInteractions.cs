using UnityEngine;

namespace Prince
{
    public class FallingGroundCharacterInteractions : MonoBehaviour
    {
        [Header("WIRING")]
        [Tooltip("Needed to know current falling ground state")]
        [SerializeField] private FallingGroundStatus fallingGroundStatus;
    }
}