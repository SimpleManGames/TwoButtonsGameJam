namespace Game
{
    using System;

    using UnityEngine;

    public sealed class Interactable : MonoBehaviour
    {
        public Func<Transform, bool> onInteract;
        
        [SerializeField] 
        private float interactionRange = 0.1f;
        
        public bool TryToInteract(Transform interactee)
        {
            if (Vector3.Distance(transform.position, interactee.position) > interactionRange)
            {
                return false;
            }

            return onInteract != null && onInteract.Invoke(interactee);
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, interactionRange);
        }
    }
}
