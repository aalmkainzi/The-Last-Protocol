#if false
using UnityEngine;

public class TowerTile : MonoBehaviour
{
    public FixedTower placedTower;
    GameObject cube;
    void Start()
    {
        cube = transform.GetChild(0).gameObject;
    }

    void Update()
    {
    }

    public GameObject PlaceTower(Tower prefab)
    {
        if(placedTower == null)
        {
            GameObject newTower = Instantiate(prefab.gameObject, transform);
            placedTower = newTower.GetComponent<FixedTower>();
            GameplayManager.instance.placedTowers.Add(placedTower);
            cube.SetActive(false);
            return newTower;
        }
        return null;
    }

    private void OnTriggerStay(Collider other)
    {
        if(GameplayManager.instance.currentTowerTile == null)
        {
            GameplayManager.instance.currentTowerTile = this;
        }
        // make layer only player
    }

    private void OnTriggerExit(Collider other)
    {
        if (GameplayManager.instance.currentTowerTile == this)
        {
            GameplayManager.instance.currentTowerTile = null;
            GameplayManager.instance.DisableUI();
        }
    }
}
#endif