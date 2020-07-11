using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    private TopViewController bull;
    private Image adrenalineFill;
    private Image adrenalineGoalFill;

    private void Awake()
    {
        bull = FindObjectOfType<TopViewController>();
        adrenalineFill = transform.Find("Adrenaline Bar/Adrenaline").GetComponent<Image>();
        adrenalineGoalFill = transform.Find("Adrenaline Bar/Adrenaline Goal").GetComponent<Image>();
    }

    private void Update()
    {
        adrenalineFill.fillAmount = bull.adrenaline / 100f;
        adrenalineGoalFill.fillAmount = bull.adrenalineGoal / 100f;
    }
}
