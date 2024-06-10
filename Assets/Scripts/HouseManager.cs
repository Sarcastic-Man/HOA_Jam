using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class HouseManager : MonoBehaviour
{
    public House[] houses;
    public UICounters ui;
    public GameObject endOfDay;

    private void Start()
    {
        List<int> petHouses = new List<int>(new int[3]);
        for (int i = 0; i < 2; i++)
        {
            int num = Random.Range(0, houses.Length);

            while (petHouses.Contains(num))
            {
                num = Random.Range(0, houses.Length);
            }
            petHouses[i] = num;
        }

        bool allHouses = false;
        if (Random.Range(0, 100) % 5 == 0)
            allHouses = true;

        for (int i = 0; i < houses.Length; i++)
        {
            if(allHouses)
            {
                houses[i].leaves = true;
            }
            for(int j = 0; j < petHouses.Count; j++)
            {
                if (i == petHouses[j])
                {
                    houses[i].canHavePet = true;
                }
            }
        }
    }

    public void CloseWindow()
    {
        for(int i = 0;i < houses.Length;i++)
        {
            houses[i].closeWindow();
        }
    }

    public void StartNewDay()
    {
        endOfDay.SetActive(false);
        Player player = FindAnyObjectByType<Player>();
        player.canMove = true;
        player.transform.position = Vector3.zero;
        ui.day++;
        ui.timer = 120;
        for (int i = 0;i < houses.Length;i++)
        {
            houses[i].NewDay();
        }
    }
}
