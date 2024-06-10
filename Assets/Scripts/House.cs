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
    public bool weeds, leaves, sideLeaves, sideCans, trashCans, 
        furniture, decorations, pet, trash, canHavePet;
    public bool paint1, paint2, paint3;

    bool grassDone, weedsDone, leavesDone, sideCansDone, trashCansDone, furnitureDone,
        decorationsDone, petDone, trashDone, paint1Done, paint2Done, paint3Done;

    bool ready = false;


    void Update()
    {
        if(!ready)
        {
            int day = FindAnyObjectByType<UICounters>().day;
            if (grass)
            {
                GameObject temp = Instantiate(listPrefab, Offenses.transform, false);
                temp.GetComponentInChildren<TextMeshProUGUI>().text = "Grass";
                temp.GetComponent<HandleOffense>().money = 20;
                temp.SetActive(true);
            }

            if (Random.Range(0, 100) % 5 == 0)
                weeds = true;
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

            if(day % 7 == 4)
            {
                if (Random.Range(0, 100) % 2.5 == 0)
                    sideCans = true;
            }
            if (sideCans)
            {
                GameObject temp = Instantiate(listPrefab, Offenses.transform, false);
                temp.GetComponentInChildren<TextMeshProUGUI>().text = "Sidewalk Blocked (trash)";
                temp.GetComponent<HandleOffense>().money = 30;
                temp.SetActive(true);
            }

            if (!sideCans)
            {
                if (Random.Range(0, 100) % 10 == 0)
                    trashCans = true;
            }
            if (trashCans)
            {
                GameObject temp = Instantiate(listPrefab, Offenses.transform, false);
                temp.GetComponentInChildren<TextMeshProUGUI>().text = "Trash Cans";
                temp.GetComponent<HandleOffense>().money = 30;
                temp.SetActive(true);
            }
            /*if (largePlant)
            {
                GameObject temp = Instantiate(listPrefab, Offenses.transform, false);
                temp.GetComponentInChildren<TextMeshProUGUI>().text = "Large Plant";
                temp.GetComponent<HandleOffense>().money = 30;
                temp.SetActive(true);
            }*/


            if (Random.Range(0, 100) % 10 == 0)
                paint1 = true;
            if (paint1)
            {
                GameObject temp = Instantiate(listPrefab, Offenses.transform, false);
                temp.GetComponentInChildren<TextMeshProUGUI>().text = "Off Paint Color1";
                temp.GetComponent<HandleOffense>().money = 30;
                temp.SetActive(true);
            }
            if (Random.Range(0, 100) % 10 == 0)
                paint2 = true;
            if (paint2)
            {
                GameObject temp = Instantiate(listPrefab, Offenses.transform, false);
                temp.GetComponentInChildren<TextMeshProUGUI>().text = "Off Paint Color2";
                temp.GetComponent<HandleOffense>().money = 30;
                temp.SetActive(true);
            }
            if (Random.Range(0, 100) % 10 == 0)
                paint3 = true;
            if (paint3)
            {
                GameObject temp = Instantiate(listPrefab, Offenses.transform, false);
                temp.GetComponentInChildren<TextMeshProUGUI>().text = "Off Paint Color3";
                temp.GetComponent<HandleOffense>().money = 30;
                temp.SetActive(true);
            }

            if (Random.Range(0, 100) % 10 == 0)
                furniture = true;
            if (furniture)
            {
                GameObject temp = Instantiate(listPrefab, Offenses.transform, false);
                temp.GetComponentInChildren<TextMeshProUGUI>().text = "Unauthorized Furniture";
                temp.GetComponent<HandleOffense>().money = 50;
                temp.SetActive(true);
            }

            if (Random.Range(0, 100) % 20 == 0)
                decorations = true;
            if (decorations)
            {
                GameObject temp = Instantiate(listPrefab, Offenses.transform, false);
                temp.GetComponentInChildren<TextMeshProUGUI>().text = "Out of Season Decorations";
                temp.GetComponent<HandleOffense>().money = 60;
                temp.SetActive(true);
            }
            /*if (driveway)
            {
                GameObject temp = Instantiate(listPrefab, Offenses.transform, false);
                temp.GetComponentInChildren<TextMeshProUGUI>().text = "Driveway";
                temp.GetComponent<HandleOffense>().money = 50;
                temp.SetActive(true);
            }*/

            if(canHavePet)
            {
                if (Random.Range(0, 100) % 5 == 0)
                    decorations = true;
            }
            if (pet)
            {
                GameObject temp = Instantiate(listPrefab, Offenses.transform, false);
                temp.GetComponentInChildren<TextMeshProUGUI>().text = "Pet";
                temp.GetComponent<HandleOffense>().money = 100;
                temp.SetActive(true);
            }

            if(day % 7 == 0 || day % 7 == 1)
            {
                if (Random.Range(0, 100) % 20 == 0)
                    trash = true;
            }
            else
            {
                if (Random.Range(0, 100) % 5 == 0)
                    trash = true;
            }
            if (trash)
            {
                GameObject temp = Instantiate(listPrefab, Offenses.transform, false);
                temp.GetComponentInChildren<TextMeshProUGUI>().text = "Trash on Ground";
                temp.GetComponent<HandleOffense>().money = 60;
                temp.SetActive(true);
            }
            ready = true;
        }

        if (Input.GetMouseButtonDown(0) && FindObjectOfType<Player>().canMove)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject.tag == "House" && hit.collider.gameObject.name == gameObject.name)
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

    public void closeWindow()
    {
        UI_PopUp.SetActive(false);
    }

    public void NewDay()
    {
        if(weeds)
        {
            if(weedsDone)
            {
                if (Random.Range(0, 100) % 1.34 == 0)
                    weeds = false;
            }
            else
            {
                if (Random.Range(0, 100) % 5 == 0)
                    weeds = false;
            }
        }
        else
        {
            if (Random.Range(0, 100) % 5 == 0)
                weeds = true;
        }
        if (weeds)
        {
            GameObject temp = Instantiate(listPrefab, Offenses.transform, false);
            temp.GetComponentInChildren<TextMeshProUGUI>().text = "Weeds";
            temp.GetComponent<HandleOffense>().money = 10;
            temp.SetActive(true);
        }

        if(sideCans)
        {
            if(sideCansDone)
            {
                if (Random.Range(0, 100) % 1.11 == 0)
                    sideCans = false;
            }
            else
            {
                if (Random.Range(0, 100) % 1.43 == 0)
                    sideCans = false;
            }
        }
        if (sideLeaves)
        {
            GameObject temp = Instantiate(listPrefab, Offenses.transform, false);
            temp.GetComponentInChildren<TextMeshProUGUI>().text = "Sidewalk Blocked (leaves)";
            temp.GetComponent<HandleOffense>().money = 30;
            temp.SetActive(true);
        }
    }
}
