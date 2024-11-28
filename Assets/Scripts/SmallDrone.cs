using UnityEngine;
using DG.Tweening;

public class SmallDrone : Enemy
{
    GameObject particle1;
    bool blowUpOnRadio = false;
    protected override void Start()
    {
        base.Start();
        particle1 = transform.Find("Particle").gameObject;

        particle1.SetActive(false);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void Die()
    {
        base.Die();
        StartCoroutine(SpinInSpiral());
        particle1.SetActive(true);
    }

    protected override void Attack()
    {
        agent.enabled = false;
        blowUpOnRadio = true;
        transform.DOMove(radarTower.transform.position, 1.0f, false);
        Debug.Log("agent enabled: " + agent.enabled);
        Debug.Log("agnet dst " + agent.destination);
        // StartCoroutine(BlowUpOnImpact());
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Colliding with " + other.gameObject.name);
        if (blowUpOnRadio && other.gameObject.CompareTag("radioTower"))
        {
            radarTower.TakeDamage(power);
            Die();
        }
    }

/*    IEnumerator BlowUpOnImpact()
    {
        while(true)
        {
            if(Vector3.Distance(transform.position, radarTower.transform.position) <= 0.75f)
            {
                radarTower.TakeDamage(5);
                Die();
            }
        }
    }*/


}
