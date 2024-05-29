using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleOffense : MonoBehaviour
{
    public int money;

    public void CompleteOffense()
    {
        FindAnyObjectByType<UICounters>().money += money;
        gameObject.SetActive(false);
    }
}
