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
    public float dartLoss;

    [Header("Score")]
    public float score;
    public float multiplier;

    [Header("Refs")]
    public MainManager mg;
    public DestroyableObject[] gates;
    public Animator animator;

    private new Camera camera;
    private new SpriteRenderer renderer;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;

        camera = Camera.main;
        renderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        adrenalineGoal = adrenaline;
    }

    private void Update()
    {
        if (!mg.tookBull)
        {
            if(Input.GetMouseButtonDown(0))
            {
                mg.TakeBull();
                animator.SetTrigger("StartRunning");
            }
            return;
        }

        // Movement
        Vector2 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDirection = (mousePosition - (Vector2)transform.position).normalized;
        direction = Vector3.Slerp(direction, lookDirection, rotationSpeed * Time.deltaTime);
        renderer.flipX = direction.x < 0;
        transform.Translate(direction * Mathf.Lerp(minRunningSpeed, maxRunningSpeed, adrenaline / 100f) * Time.deltaTime);

        if (!mg.startedGame) // Don't do anything if game hasn't started yet
            return;

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
            case "Start Gate":
                foreach (DestroyableObject g in gates)
                {
                    g.Destroy();
                }
                mg.StartGame();
                break;
            case "Dart":
                Destroy(collision.gameObject);
                AddAdrenaline(dartLoss);
                break;
        }
    }
}
