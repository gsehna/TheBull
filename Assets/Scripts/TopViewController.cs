using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopViewController : MonoBehaviour
{
    [Header("Movement")]
    public Vector2 direction;
    public float minRunningSpeed;
    public float maxRunningSpeed;
    public float rotationSpeed;

    [Header("Adrenaline")]
    [Range(0, 100)]
    public float adrenaline;
    [HideInInspector]
    public float adrenalineGoal;
    public float adrenalineLoss;

    [Header("Score")]
    public float score;
    public float multiplier;

    private new Camera camera;
    private new SpriteRenderer renderer;

    private void Awake()
    {
        camera = Camera.main;
        renderer = GetComponent<SpriteRenderer>();

        adrenalineGoal = adrenaline;
    }

    private void Update()
    {
        // Movement
        Vector2 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDirection = (mousePosition - (Vector2)transform.position).normalized;
        direction = Vector3.Slerp(direction, lookDirection, rotationSpeed * Time.deltaTime);
        renderer.flipX = direction.x < 0;
        transform.Translate(direction * Mathf.Lerp(minRunningSpeed, maxRunningSpeed, adrenaline / 100f) * Time.deltaTime);

        // Adrenaline
        if (adrenalineGoal < adrenaline)
        {
            adrenaline -= adrenalineLoss * 5 * Time.deltaTime;
        }
        else
        {
            adrenalineGoal -= adrenalineLoss * Time.deltaTime;
            adrenaline = adrenalineGoal;
        }
    }

    public void AddAdrenaline(float value)
    {
        adrenalineGoal += value;
        adrenalineGoal = Mathf.Clamp(adrenalineGoal, 0, 100);
        if (adrenalineGoal > adrenaline)
        {
            adrenaline = adrenalineGoal;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.transform.tag)
        {
            case "Building":
                direction = Vector2.Reflect(direction, collision.contacts[0].normal);
                AddAdrenaline(-15);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Destroyable":
                DestroyableObject destroyable = collision.GetComponent<DestroyableObject>();
                AddAdrenaline(destroyable.adrenalineGain);
                destroyable.Destroy();
                break;
        }
    }
}
