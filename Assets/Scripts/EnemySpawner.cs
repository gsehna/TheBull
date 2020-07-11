using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Properties")]
    public float spawnerTime;
    public float spawnerTimeDecay; //Per 1000 points
    public float minSpawnerTime;
    public int maxEntities;

    [Header("Bounds")]
    public Vector2 min;
    public Vector2 max;

    [Header("Refs")]
    public TopViewController bull;
    public Transform colliders;
    public GameObject hunter;

    private float timer;

    private void LateUpdate()
    {
        timer += Time.deltaTime;

        if (timer >= spawnerTime)
        {
            timer = 0;
            GameObject newHunter = Instantiate(hunter, Vector2.zero, Quaternion.identity);
            newHunter.GetComponent<BullHunter>().target = bull.transform;

            bool canSpawn = false;
            do
            {
                newHunter.transform.position = PickRandomLocation();
                Bounds bounds = new Bounds(newHunter.transform.position,
                                           newHunter.GetComponent<BoxCollider2D>().bounds.size);
                canSpawn = CheckLocation(bounds);
            } while (!canSpawn);
        }
    }

    private Vector2 PickRandomLocation()
    {
        Bounds prohibitedArea = new Bounds(transform.position, min);
        Bounds allowedArea = new Bounds(transform.position, max);
        Vector2 position = Vector2.zero;

        bool canSpawn = false;
        do
        {
            float x = Random.Range(allowedArea.min.x, allowedArea.max.x);
            float y = Random.Range(allowedArea.min.y, allowedArea.max.y);

            position = new Vector2(x, y);

            canSpawn = !prohibitedArea.Contains(position);
        } while (!canSpawn);

        return position;
    }

    private bool CheckLocation(Bounds bounds)
    {
        BoxCollider2D[] boxes = colliders.GetComponentsInChildren<BoxCollider2D>();

        foreach(BoxCollider2D box in boxes)
        {
            if (bounds.Intersects(box.bounds))
            {
                return false;
            }
        }

        return true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector2(min.x, min.y));
        Gizmos.DrawWireCube(transform.position, new Vector2(max.x, max.y));
    }
}
