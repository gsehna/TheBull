using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepDart : MonoBehaviour
{
    public float velocity;

    private void Update()
    {
        transform.Translate(transform.up * velocity * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Building")
        {
            Destroy(gameObject);
        }
    }
}
