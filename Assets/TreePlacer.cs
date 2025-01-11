using UnityEngine;
using static UnityEditor.PlayerSettings;

public class TreePlacer : MonoBehaviour
{
    public GameObject[] prefabs;
    public float minX;
    public float maxX;
    public float minZ;
    public float maxZ;
    public LayerMask groundLayer;

    Quaternion RandomRoation()
    {
        return Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
    }
    public void PlaceTrees(int nb, float minScale, float maxScale)
    {
        for (int i = 0; i < nb; i++)
        {
            float x = Random.Range(minX, maxX);
            float z = Random.Range(minZ, maxZ);
            Physics.Raycast(new Vector3(x, 40, z), Vector3.down, out RaycastHit hit, 100, groundLayer);
            float y = 40 - hit.distance;
            GameObject newTree = Instantiate(prefabs[Random.Range(0, prefabs.Length)], new Vector3(x, y, z), RandomRoation(), transform);
            float newScale = Random.Range(minScale, maxScale);
            newTree.transform.localScale = new Vector3(newScale, newScale, newScale);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.5f, 0.5f, 0, 0.5f);
        Gizmos.DrawCube(new Vector3((minX + maxX) / 2.0f, 4.0f, (minZ + maxZ) / 2.0f), new Vector3(maxX - minX , 3.0f, maxZ - minZ));
    }
}
