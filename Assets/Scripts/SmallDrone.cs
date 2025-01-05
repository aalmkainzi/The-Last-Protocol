using UnityEngine;
// using DG.Tweening;
using PrimeTween;
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
        GetComponent<AudioSource>().clip = GameplayManager.instance.booms[Random.Range(0, GameplayManager.instance.booms.Length)];
        GetComponent<AudioSource>().Play();
        StartCoroutine(SpinInSpiral());
        particle1.SetActive(true);
    }

    protected override void Attack()
    {
        agent.enabled = false;
        blowUpOnRadio = true;

        PrimeTween.Tween.Position(transform, radarTower.transform.position, duration: 1.0f);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (blowUpOnRadio && other.gameObject.CompareTag("radioTower"))
        {
            radarTower.TakeDamage(power);
            Die();
        }
    }
}
