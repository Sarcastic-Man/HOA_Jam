using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

public class House : MonoBehaviour
{
    [SerializeField]
    GameObject UI_PopUp;
    [SerializeField]
    GameObject Offenses;
    [SerializeField]
    GameObject listPrefab;

    [Header("Offenses")]
    public bool grass;
    public bool weeds, leaves, sideLeaves, sideCans, trashCans, largePlant, 
        paintColor, furniture, decorations, driveway, pet, trash;

    void Start()
    {
        if(grass)
        {
            GameObject temp = Instantiate(listPrefab, Offenses.transform, false);
            temp.GetComponentInChildren<TextMeshProUGUI>().text = "Grass";
            temp.GetComponent<HandleOffense>().money = 20;
            temp.SetActive(true);
        }
        if (weeds)
        {
            GameObject temp = Instantiate(listPrefab, Offenses.transform, false);
            temp.GetComponentInChildren<TextMeshProUGUI>().text = "Weeds";
            temp.GetComponent<HandleOffense>().money = 10;
            temp.SetActive(true);
        }
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject.tag == "House")
            {
                FindObjectOfType<Player>().canMove = false;
                UI_PopUp.SetActive(true);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<Player>().canMove = false;
            UI_PopUp.SetActive(true);
        }
    }
}
