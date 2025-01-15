using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Wagon : MonoBehaviour
{
    public int health;

    int loadedBatteries = 0;
    public Vector3[] wayPoints;
    int curWayPoint = -1;
    NavMeshAgent agent;
    bool playerNear = false;
    Player player;
    Animator anim;
    AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if(loadedBatteries > 0)
        {
            if (Vector3.Distance(transform.position, wayPoints[curWayPoint]) <= 0.25f)
            {
                if(loadedBatteries > curWayPoint + 1)
                {
                    curWayPoint++;
                    if (curWayPoint >= wayPoints.Length)
                    {
                        WinScreen();
                    }
                    agent.SetDestination(wayPoints[curWayPoint]);
                    anim.SetBool("Walking", true);
                    audioSource.Play();
                }
                else
                {
                    anim.SetBool("Walking", false);
                    audioSource.Stop();
                }
            }
        }

        if(playerNear)
        {
            if(player.holdingBattery && Input.GetKeyDown(KeyCode.F))
            {
                LoadBattery();
            }
        }
    }

    void playWalkSound()
    {
        audioSource.Play();
    }

    public void WinScreen()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void LoadBattery()
    {
        Debug.Log("LOADING BATTERY");
        if (curWayPoint == -1)
        {
            curWayPoint = 0;
            agent.SetDestination(wayPoints[curWayPoint]);
            anim.SetBool("Walking", true);
            audioSource.Play();
        }
        loadedBatteries++;

        player.holdingBattery = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (wayPoints == null) return;
        Gizmos.color = new Color(1, 0, 1, 1);
        foreach (var point in wayPoints)
        {
            Gizmos.DrawSphere(point, 1.0f);
        }
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        if(health <= 0)
        {
            // GameplayManager2.
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        playerNear = true;
    }

    private void OnTriggerExit(Collider other)
    {
        playerNear = false;
    }
}
