using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.linearVelocity = Vector2.down;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
