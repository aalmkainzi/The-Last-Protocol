using UnityEngine;
// using DG.Tweening;
using PrimeTween;
public class EnemyDroneL2 : EnemyL2
{
    GameObject particle1;
    bool blowUpOnWagon = false;
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
        GetComponent<AudioSource>().clip = GameplayManagerL2.instance.booms[Random.Range(0, GameplayManagerL2.instance.booms.Length)];
        GetComponent<AudioSource>().Play();
        StartCoroutine(SpinInSpiral());
        particle1.SetActive(true);
    }

    protected override void Attack()
    {
        agent.enabled = false;
        blowUpOnWagon = true;

        PrimeTween.Tween.Position(transform, wagon.transform.position, duration: 1.0f);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (blowUpOnWagon && other.gameObject.CompareTag("Wagon"))
        {
            wagon.TakeDamage(power);
            Die();
        }
    }
}
