
using UnityEngine;

public class player_move : MonoBehaviour
{
    public float moveSpeed = 5f; 
    public float rotationSpeed = 100f; 
    public float mouseSensitivity = 2f;
    public AudioSource movementSound; 
    private Rigidbody rb;
    private bool isMoving = false; 
    private float xRotation = 0f;
    public Transform playerCamera; 

    void Start()
    {
       
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody not found! Please add a Rigidbody component to the player.");
        }

     
        if (movementSound == null)
        {
            Debug.LogWarning("AudioSource for movement sound is not assigned. Please assign it in the Inspector.");
        }

        
        Cursor.lockState = CursorLockMode.Locked; // قفل الماوس داخل الشاشة
        Cursor.visible = false; // إخفاء مؤشر الماوس
    }

    void Update()
    {
       
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity; // دوران حول المحور Y
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity; // دوران حول المحور X

       
        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY; 
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f); 
    }

    void FixedUpdate()
    {
       
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical"); 

        Vector3 movement = transform.forward * moveVertical + transform.right * moveHorizontal;
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        
        if (movement != Vector3.zero)
        {
            if (!isMoving)
            {
                isMoving = true; 
                if (movementSound != null && !movementSound.isPlaying)
                {
                    movementSound.Play();
                }
            }
        }
        else
        {
            if (isMoving)
            {
                isMoving = false; 
                if (movementSound != null && movementSound.isPlaying)
                {
                    movementSound.Stop();
                }
            }
        }
    }
}
