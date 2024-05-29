using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UICounters : MonoBehaviour
{
    public GameObject dayCounter, moneyCounter, timerCounter;
    public int day, money;
    public float timer;
    int minutes, seconds;

    private void Update()
    {
        dayCounter.GetComponentInChildren<TextMeshProUGUI>().text = day.ToString();
        moneyCounter.GetComponentInChildren<TextMeshProUGUI>().text = (money.ToString() + ".00");

        timer -= Time.deltaTime;
        minutes = (int)timer / 60;
        seconds = (int)timer % 60;
        timerCounter.GetComponentInChildren<TextMeshProUGUI>().text = minutes.ToString() + ":" + seconds.ToString();

        if(timer <= 0)
        {
            //Load vote scene
        }
    }
}
