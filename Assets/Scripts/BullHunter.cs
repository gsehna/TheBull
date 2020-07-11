using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BullHunter : DestroyableObject
{
    public float lockRange;
    public float minRange;
    public float shotDelay;
    public Transform target;
    public GameObject dart;

    private bool locked = false;
    private LineRenderer line;
    private Transform gunPosition;
    private float timer = 0f;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        gunPosition = transform.GetChild(0);
    }

    private void Update()
    {
        float distance = Vector2.Distance(transform.position, target.position);
        Vector2 direction = target.position - gunPosition.position;

        if (target.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = Vector3.one;
        }

        if (!locked && distance <= lockRange &&
            !Physics2D.Raycast(gunPosition.position, direction.normalized, direction.magnitude, LayerMask.GetMask("Building")))
        {
            locked = true;
        }

        if (locked)
        {
            line.startColor = line.endColor = Color.yellow;

            if (Physics2D.Raycast(gunPosition.position, direction.normalized, direction.magnitude, LayerMask.GetMask("Building")))
            {
                locked = false;
                timer = 0f;
                line.SetPositions(new Vector3[]
                {
                    Vector3.zero,
                    Vector3.zero
                });
            }

            if (distance >= minRange)
            {
                timer += Time.deltaTime;
                if (timer >= shotDelay - 0.5f)
                {
                    line.startColor = line.endColor = Color.red;
                }

                if (timer >= shotDelay)
                {
                    GameObject newDart = Instantiate(dart, gunPosition.position, Quaternion.identity);
                    newDart.transform.up = (target.position - gunPosition.position).normalized;
                    timer = 0f;

                    if (distance > lockRange)
                    {
                        locked = false;
                        line.SetPositions(new Vector3[]
                        {
                            Vector3.zero,
                            Vector3.zero
                        });
                    }
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (locked)
        {
            line.SetPositions(new Vector3[]
            {
                gunPosition.position,
                target.position
            });
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, lockRange);
        Gizmos.DrawWireSphere(transform.position, minRange);
    }
}
