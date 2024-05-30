using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseManager : MonoBehaviour
{
    public House[] houses;

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

        for(int i = 0; i < houses.Length; i++)
        {
            for(int j = 0; j < petHouses.Count; j++)
            {
                if (i == petHouses[j])
                {
                    houses[i].canHavePet = true;
                }
            }
        }
    }
}
