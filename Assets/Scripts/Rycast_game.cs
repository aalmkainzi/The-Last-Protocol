using DG.Tweening.Core;
using UnityEngine;

public class Rycast_game : MonoBehaviour
{

    public Transform firePoint;
    public float range = 90000f;
    // public LineRenderer laserLine;
    public float fireRate = 0.5f;
    private float nextFireTime = 0f;
    public Player player;
    int layerMask;
    ParticleSystem ps;
    ParticleSystem ps2;
    AudioSource bang;

    public LineRenderer myLine;
    private void Start()
    {
        bang = GetComponent<AudioSource>();
        ps = transform.Find("Particle").gameObject.GetComponent<ParticleSystem>();
        ps2 = ps.gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
        layerMask = (1 << LayerMask.NameToLayer("EnemyBot")) | (1 << LayerMask.NameToLayer("EnemyBotStealth"));
        player = transform.parent.parent.gameObject.GetComponent<Player>();
    }
    void Update()
    {
        Debug.DrawRay(firePoint.position, firePoint.forward * range, Color.red);
        
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Fire();
        }
    }

    GameObject FindClosest(RaycastHit[] hits)
    {
        if(hits.Length == 0) return null;
        GameObject closest = null;
        float closestDistance = Mathf.Infinity;
        for (int i = 0; i < hits.Length; i++)
        {
            GameObject hitObj = hits[i].collider.gameObject;

            float distance = Vector3.Distance(firePoint.position, hitObj.transform.position);
            if (hitObj.CompareTag("Enemy") && distance < closestDistance)
            {
                closest = hitObj;
                closestDistance = distance;
            }
        }

        return closest;
    }

    void Fire()
    {
        bang.Play();
        ps.Play();
        ps2.Play();
        
        Ray ray = new Ray(firePoint.position, firePoint.forward);
        RaycastHit[] hits = Physics.RaycastAll(ray, range, layerMask: layerMask);
        GameObject closest = FindClosest(hits);
        if (closest == null) return;

        // if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, range,  layerMask))
        {
            /*          laserLine.enabled = true;
                        laserLine.SetPosition(0, firePoint.position);
                        laserLine.SetPosition(1, hit.point);
            */
            Debug.Log("DAMAGED E");
            Enemy enemy = closest.GetComponent<Enemy>();
            enemy.TakeDamage(player.power);
        }
        //else
        {
/*            laserLine.enabled = true;
            laserLine.SetPosition(0, firePoint.position);
            laserLine.SetPosition(1, firePoint.position + firePoint.forward * range);
*/       }

        Invoke("DisableLaser", 0.1f);
    }

    void DisableLaser()
    {
        // laserLine.enabled = false;
    }
}
