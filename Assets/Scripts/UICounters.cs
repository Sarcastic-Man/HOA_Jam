using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UICounters : MonoBehaviour
{
    public GameObject dayCounter, moneyCounter, timerCounter, endOfDay;
    public int day, money;
    public float timer;
    int minutes, seconds;

    private void Update()
    {
        if(timer <= 0)
        {
            FindAnyObjectByType<Player>().canMove = false;
            FindAnyObjectByType<HouseManager>().CloseWindow();
            endOfDay.SetActive(true);
        }
        else
        {
            dayCounter.GetComponentInChildren<TextMeshProUGUI>().text = day.ToString();
            moneyCounter.GetComponentInChildren<TextMeshProUGUI>().text = (money.ToString() + ".00");

            timer -= Time.deltaTime;
            minutes = (int)timer / 60;
            seconds = (int)timer % 60;
            if (seconds < 10)
            {
                timerCounter.GetComponentInChildren<TextMeshProUGUI>().text = minutes.ToString() + ":0" + seconds.ToString();
            }
            else
            {
                timerCounter.GetComponentInChildren<TextMeshProUGUI>().text = minutes.ToString() + ":" + seconds.ToString();
            }
        }
    }
}
