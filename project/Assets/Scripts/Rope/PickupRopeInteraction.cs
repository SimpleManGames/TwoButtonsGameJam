namespace Game
{
    using UnityEngine;

    [RequireComponent(typeof(Interactable))]
    public class PickupRopeInteraction : MonoBehaviour
    {
        [SerializeField]
        private float holdStrength = 10f;
        
        [SerializeField]
        private float dampingFactor = 10f;
        
        [SerializeField]
        private new Rigidbody2D rigidbody2D;
        
        private Interactable _interactable;

        private bool _held;
        
        private Transform _heldTransform;
        
        private void Start()
        {
            _interactable.onInteract += OnInteract; 
        }

        private void FixedUpdate()
        {
            if (_heldTransform == null)
                return;
            
            Vector2 direction = _heldTransform.position - transform.position;
            rigidbody2D.AddForce(direction * holdStrength, ForceMode2D.Force);
            rigidbody2D.AddForce(-rigidbody2D.linearVelocity * dampingFactor, ForceMode2D.Force);
        }

        private bool OnInteract(Transform interactee)
        {
            if (_held)
            {
                gameObject.layer = 0;
                _heldTransform = null;
            }
            else
            {
                gameObject.layer = LayerMask.NameToLayer("FloatRigidbodyIgnore");
                _heldTransform = interactee.Find("HoldPosition").transform;
            }
            _held = !_held;
            return true;
        }

        private void OnValidate()
        {
            _interactable = GetComponent<Interactable>();
        }
    }
}
