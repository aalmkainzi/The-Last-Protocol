using System;
using UnityEngine;
using UnityEngine.AI;

public class CommandSpider : MonoBehaviour
{
    public LayerMask groundLayerMask;
    public float currentSpeed;
    public GameObject destinationMarker;

    private Vector3 previousPosition;
    private NavMeshAgent navmeshAgent;
    private Animator animator;

    private void Awake()
    {
        navmeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleMouseInput();
        CheckCurrentSpeed();
    }

    private void HandleMouseInput()
    {
        //Move spider to destination clicked
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100, groundLayerMask))
            {
                navmeshAgent.destination = hit.point;

                GameObject marker = Instantiate(destinationMarker, hit.point, Quaternion.identity);
                Destroy(marker, 1f);
            }
        }
    }

    private void CheckCurrentSpeed()
    {
        Vector3 curMove = transform.position - previousPosition;
        currentSpeed = curMove.magnitude / Time.deltaTime;
        previousPosition = transform.position;

        if (currentSpeed >= .1)
        {
            animator.SetBool("Moving", true);
            //use the currentSpeed variable to control the animation
            //playback speed to give a more realistic look
            animator.speed = currentSpeed;
            animator.speed = Mathf.Clamp(animator.speed, 0.5f, 1);
        }
        else
        {
            animator.SetBool("Moving", false);
            //Gradually set animation speed back to the defualt of 1 
            animator.speed = animator.speed + 0.1f * Time.deltaTime;
            animator.speed = Mathf.Clamp01(animator.speed);
        }
    }
}
