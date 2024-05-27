using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

public class CloseMenu : MonoBehaviour
{
    public void Close()
    {
        gameObject.SetActive(false);
        FindObjectOfType<Player>().canMove = true;
    }
}
