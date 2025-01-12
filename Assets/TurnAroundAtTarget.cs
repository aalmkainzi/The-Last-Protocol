using UnityEngine;
using UnityEngine.AI;
using PrimeTween;
public class TurnAroundAtTarget : MonoBehaviour
{
    NavMeshAgent agent;
    public Vector3 targetRot;
    bool rotated = false;
    public float distanceFromTarget;
    void Start()
    {
        agent = /*transform.GetChild(0).*/GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Vector3 ignrY = transform.position;
        ignrY.y = agent.destination.y;
        if(!rotated && Vector3.Distance(agent.destination, ignrY) <= distanceFromTarget)
        {
            rotated = true;
            Tween.Rotation(transform, targetRot, 2.0f);
        }
    }
}
