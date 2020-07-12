using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public bool startedGame = false;
    public bool tookBull = false;

    public GameObject startGameInstructions;
    public TextMeshProUGUI instructTxt;

    private string text1 = "CLICK!";
    private string text2 = "ESCAPE TO BEGIN";

    private void Start()
    {
        instructTxt.text = text1;
    }

    public void StartGame()
    {
        startedGame = true;
        startGameInstructions.SetActive(false);
    }

    public void TakeBull()
    {
        instructTxt.text = text2;
        tookBull = true;
    }
}
