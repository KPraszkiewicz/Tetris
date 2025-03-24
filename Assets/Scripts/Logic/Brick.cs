using System;
using UnityEngine;
using UnityEngine.Events;

namespace Logic
{
    public class Brick : MonoBehaviour
    {

        public Action<Brick> OnStopped;

        private int[,] table;

        public void StartMoving(float speed)
        {
            Rigidbody2D rigidbody2d = GetComponent<Rigidbody2D>();
            rigidbody2d.linearVelocity = Vector2.down * speed;
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == 10)
            {
                // TODO: Fix tunneling
                return;
            }
            Debug.Log("kolizja");
            OnStopped?.Invoke(this);
        }

        
    }
}
