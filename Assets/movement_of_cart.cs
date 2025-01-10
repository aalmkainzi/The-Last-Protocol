using UnityEngine;

public class movement_of_cart : MonoBehaviour
{
    public float speed = 10f; 
    private Rigidbody rb;

    void Start()
    {
        
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
       
        rb.MovePosition(rb.position + transform.forward * speed * Time.fixedDeltaTime);
    }
}


