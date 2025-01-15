using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class TowerPlacerL2 : MonoBehaviour
{
    [SerializeField] LayerMask ground;
    List<Color> ogColors;
    Renderer[] rends;
    bool alreadyRed = false;

    bool canPlace = true;
    [SerializeField] GameObject prefab;
    GameObject towerVisual;
    public bool readyToPlace = false;

    private void Awake()
    {
        DisableTowerPlacer();

        towerVisual = transform.Find("TowerVisual").gameObject;

        ogColors = new();
    }

    void EnableTowerPlacer()
    {
        readyToPlace = true;
        GetComponent<SphereCollider>().enabled = true;
    }
    void DisableTowerPlacer()
    {
        readyToPlace = false;
        GetComponent<SphereCollider>().enabled = false;
    }

    public void SetTowerPrefab(GameObject prefab)
    {
        EnableTowerPlacer();

        if (towerVisual.transform.childCount > 0)
        {
            Destroy(towerVisual.transform.GetChild(0).gameObject);
        }

        this.prefab = prefab;
        GameObject visual = Instantiate(prefab, towerVisual.transform);
        visual.GetComponentInChildren<FixedTowerL2>().enabled = false;

        ogColors.Clear();
        rends = visual.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rends)
        {
            foreach (Material m in r.materials)
            {
                ogColors.Add(m.color);
            }
        }
    }

    void Update()
    {
        if (readyToPlace)
        {
            Vector3 pos = transform.position;
            bool hitDown = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 100, ground);

            if (!hitDown)
            {
                bool hitUp = Physics.Raycast(transform.position, Vector3.up, out hit, 100, ground);
                pos.y += hit.distance;
            }
            else
            {
                pos.y -= hit.distance;
            }
            transform.position = pos;

            // Debug.Log("canplace? " + canPlace);
            if (canPlace)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Destroy(towerVisual.transform.GetChild(0).gameObject);
                    DisableTowerPlacer();

                    Debug.Log("NEW TOWER PLACED AT " + transform.position);
                    GameObject newTower = Instantiate(prefab, transform.position, Quaternion.identity);
                    newTower.transform.position = transform.position;
                    FixedTowerL2 placedTower = newTower.GetComponent<FixedTowerL2>();
                    GameplayManagerL2.instance.placedTowers.Add(placedTower);
                }
            }
        }
    }

    void MakeChildrenRed()
    {
        foreach (Renderer r in rends)
        {
            foreach (Material m in r.materials)
                m.color = Color.red;
        }
    }

    void ResetChildrenColors()
    {
        for (int i = 0; i < rends.Length; i++)
        {
            for (int j = 0; j < rends[i].materials.Length; j++)
                rends[i].materials[j].color = ogColors[i + j];
        }
    }
    int i = 0;
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("WENT INSIDE COLLIDER " + other.gameObject.name);
        canPlace = false;
        if (!alreadyRed)
        {
            alreadyRed = true;
            MakeChildrenRed();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        canPlace = true;
        alreadyRed = false;
        ResetChildrenColors();
    }
}
