using UnityEngine;

namespace Logic
{
    public class Brick : MonoBehaviour
    {
        void Start()
        {
            Rigidbody2D rigidbody2d = GetComponent<Rigidbody2D>();
            rigidbody2d.linearVelocity = Vector2.down;
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("kolizja");
        }

        
    }
}
