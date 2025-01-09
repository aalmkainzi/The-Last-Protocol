using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
    [SerializeField] LayerMask ground;
    List<Color> ogColors;
    Renderer[] rends;
    bool alreadyRed = false;
    private void Start()
    {
        ogColors = new();
        rends = transform.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rends)
        {
            foreach(Material m in r.materials)
            {
                ogColors.Add(m.color);
            }
        }
    }
    void Update()
    {
        Vector3 pos = transform.position;
        bool hitDown = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 100, ground);
        if(!hitDown)
        {
            Physics.Raycast(transform.position, Vector3.up, out hit, 100, ground);
            pos.y += hit.distance;
        }
        else
        {
            pos.y -= hit.distance;
        }
        transform.position = pos;
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
        Debug.Log("INSIDE COLLIDER " + i++ + " " + other.gameObject.name);
        if (!alreadyRed)
        {
            alreadyRed = true;
            MakeChildrenRed();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        alreadyRed = false;
        ResetChildrenColors();
    }
}
