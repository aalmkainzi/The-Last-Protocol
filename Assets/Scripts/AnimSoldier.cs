using UnityEngine;
using UnityEngine.AI;

public class AnimSoldier : MonoBehaviour
{
    public Vector3 destination;
    public float speed;
    void Start()
    {
        var ag = GetComponent<NavMeshAgent>();
        ag.speed = speed;
        ag.SetDestination(destination);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1,0,1,1);
        Gizmos.DrawSphere(destination, 0.5f);
    }
}
