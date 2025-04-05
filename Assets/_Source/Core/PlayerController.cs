using UnityEngine;

namespace Core
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float swingForce = 5f;
        private Rigidbody2D _rb;
        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
        }
        private void Update() 
        {
            var input = Input.GetAxis("Horizontal");
            _rb.AddForce(new Vector2(input * swingForce, 0));
        }
    }
}
