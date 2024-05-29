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
        if (leaves)
        {
            GameObject temp = Instantiate(listPrefab, Offenses.transform, false);
            temp.GetComponentInChildren<TextMeshProUGUI>().text = "Fallen Leaves";
            temp.GetComponent<HandleOffense>().money = 20;
            temp.SetActive(true);
        }
        if (sideLeaves)
        {
            GameObject temp = Instantiate(listPrefab, Offenses.transform, false);
            temp.GetComponentInChildren<TextMeshProUGUI>().text = "Sidewalk Blocked (leaves)";
            temp.GetComponent<HandleOffense>().money = 30;
            temp.SetActive(true);
        }
        if (sideCans)
        {
            GameObject temp = Instantiate(listPrefab, Offenses.transform, false);
            temp.GetComponentInChildren<TextMeshProUGUI>().text = "Sidewalk Blocked (trash)";
            temp.GetComponent<HandleOffense>().money = 30;
            temp.SetActive(true);
        }
        if (trashCans)
        {
            GameObject temp = Instantiate(listPrefab, Offenses.transform, false);
            temp.GetComponentInChildren<TextMeshProUGUI>().text = "Trash Cans";
            temp.GetComponent<HandleOffense>().money = 30;
            temp.SetActive(true);
        }
        if (largePlant)
        {
            GameObject temp = Instantiate(listPrefab, Offenses.transform, false);
            temp.GetComponentInChildren<TextMeshProUGUI>().text = "Large Plant";
            temp.GetComponent<HandleOffense>().money = 30;
            temp.SetActive(true);
        }
        if (paintColor)
        {
            GameObject temp = Instantiate(listPrefab, Offenses.transform, false);
            temp.GetComponentInChildren<TextMeshProUGUI>().text = "Off Paint Color";
            temp.GetComponent<HandleOffense>().money = 30;
            temp.SetActive(true);
        }
        if (furniture)
        {
            GameObject temp = Instantiate(listPrefab, Offenses.transform, false);
            temp.GetComponentInChildren<TextMeshProUGUI>().text = "Unauthorized Furniture";
            temp.GetComponent<HandleOffense>().money = 50;
            temp.SetActive(true);
        }
        if (decorations)
        {
            GameObject temp = Instantiate(listPrefab, Offenses.transform, false);
            temp.GetComponentInChildren<TextMeshProUGUI>().text = "Out of Season Decorations";
            temp.GetComponent<HandleOffense>().money = 60;
            temp.SetActive(true);
        }
        if (driveway)
        {
            GameObject temp = Instantiate(listPrefab, Offenses.transform, false);
            temp.GetComponentInChildren<TextMeshProUGUI>().text = "Driveway";
            temp.GetComponent<HandleOffense>().money = 50;
            temp.SetActive(true);
        }
        if (pet)
        {
            GameObject temp = Instantiate(listPrefab, Offenses.transform, false);
            temp.GetComponentInChildren<TextMeshProUGUI>().text = "Pet";
            temp.GetComponent<HandleOffense>().money = 100;
            temp.SetActive(true);
        }
        if (trash)
        {
            GameObject temp = Instantiate(listPrefab, Offenses.transform, false);
            temp.GetComponentInChildren<TextMeshProUGUI>().text = "Trash on Ground";
            temp.GetComponent<HandleOffense>().money = 60;
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
