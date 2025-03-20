using System;
using UnityEngine;
using UnityEngine.Events;

namespace Logic
{
    public class Brick : MonoBehaviour
    {
        public int Width = 10;
        public int Height = 10;
        public float CellSize = 10f;

        public Action<Brick> OnStopped;

        private int[,] table;

        void Start()
        {
            Rigidbody2D rigidbody2d = GetComponent<Rigidbody2D>();
            rigidbody2d.linearVelocity = Vector2.down;


        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("kolizja");
            OnStopped?.Invoke(this);
        }

        
    }
}
