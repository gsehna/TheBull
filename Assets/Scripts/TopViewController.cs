using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopViewController : MonoBehaviour
{
    [Header("Properties")]
    public Vector2 direction;
    public float runningSpeed;
    public float rotationSpeed;

    private new Camera camera;

    private void Awake()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        Vector2 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDirection = (mousePosition - (Vector2)transform.position).normalized;
        direction = Vector3.Slerp(direction, lookDirection, rotationSpeed * Time.deltaTime);
        transform.Translate(direction * runningSpeed * Time.deltaTime);
    }
}
