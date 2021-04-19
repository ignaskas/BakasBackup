using System;
using System.Collections;
using System.Collections.Generic;
// using UnityEditor.UI;
using UnityEngine.UI;
using UnityEngine;

public class menuBehaviur : MonoBehaviour
{
    public int pointsLeft = 10;
    public int intelStat;
    public int strStat;
    public int agyStat;

    public Text txtint;
    public Text txtstr;
    public Text txtagy;
    public Text txtPointsLeft;
    public Text MainMenuText;

    public Button buttonint;
    public Button buttonstr;
    public Button buttonagy;
    public Button buttonStartGame;

    public GameObject didintSpendAllPoints;
    public GameObject pointsLeftToSpend;
    public GameObject pointsLeftToSpendBackdrop;
    public GameObject storyPlate;
    public GameObject key;
    public GameObject sword;
    public GameObject staff;
    public GameObject blaster;
    public GameObject flaskEmpty;
    public GameObject flaskFull;
    public GameObject coinPurse;
    public GameObject armor;
    public GameObject blurMap;

    public Main_Behaviour mainBehaviourRef;


    // private void Awake()
    // {
    //     CanvasScaler canvasScale = GameObject.Find("Canvas").gameObject.GetComponent<CanvasScaler>();
    //     canvasScale.scaleFactor = CalculateUiScale();
    // }

    void Start()
    {
        didintSpendAllPoints.gameObject.SetActive(false);
        storyPlate.gameObject.SetActive(false);
        key.gameObject.SetActive(true);
        sword.gameObject.SetActive(false);
        staff.gameObject.SetActive(false);
        blaster.gameObject.SetActive(false);
        flaskEmpty.gameObject.SetActive(true);
        flaskFull.gameObject.SetActive(false);
        coinPurse.gameObject.SetActive(false);
        armor.gameObject.SetActive(false);
        foreach (var buttons in mainBehaviourRef.menuCards)
        {
            buttons.gameObject.SetActive(false);
        }
        // Debug.Log("My folders location on a phone   " + Application.persistentDataPath);
    }
    
    private void OnEnable()
    {
        buttonint.onClick.AddListener(() => ButtonCallBackInt());
        buttonstr.onClick.AddListener(() => ButtonCallBackStr());
        buttonagy.onClick.AddListener(() => ButtonCallBacksAgy());
        buttonStartGame.onClick.AddListener(() => ButtonCallBackStartGame());
    }

    // public void Update()
    // {
    //     Debug.Log(buttonint.transform.position.x + " " + buttonint.transform.position.y);
    // }
    
    // Start the game only if all attribute points were spent and remove all menu elements off screen if not display a message
    private void ButtonCallBackStartGame()
    {
        if (pointsLeft != 0)
        {
            didintSpendAllPoints.gameObject.SetActive(true);
        }
        else
        {
            mainBehaviourRef.CreateObjects();
            foreach (var buttons in mainBehaviourRef.menuCards)
            {
                buttons.gameObject.SetActive(true);
            }
            blurMap.gameObject.SetActive(false);
            pointsLeftToSpend.gameObject.SetActive(false);
            didintSpendAllPoints.gameObject.SetActive(false);
            // buttonint.transform.position = new Vector3(1205.0f, 370.0f, gameObject.transform.position.z);
            // buttonstr.transform.position = new Vector3(1205.0f, 436.0f, gameObject.transform.position.z);
            // buttonagy.transform.position = new Vector3(1205.0f, 512.0f, gameObject.transform.position.z);
            buttonStartGame.gameObject.SetActive(false);
            pointsLeftToSpendBackdrop.gameObject.SetActive(false);
            storyPlate.gameObject.SetActive(true);
            MainMenuText.gameObject.SetActive(false);
        }
    }

    // adds 1 point to attribute agy every time a button is pressed
    private void ButtonCallBacksAgy()
    {
        if (pointsLeft > 0)
        {
            agyStat = Add(agyStat);
            txtagy.text = "Agility: " + agyStat.ToString();
            RemovePoint();
        }
        txtPointsLeft.text = "Points Left: " + pointsLeft.ToString();
    }
    
    // adds 1 points to attribute str every time a button is pressed
    private void ButtonCallBackStr()
    {
        if (pointsLeft > 0)
        {
            strStat = Add(strStat);
            txtstr.text = "Strength: " + strStat.ToString();
            RemovePoint();
        }
        txtPointsLeft.text = "Points Left: " + pointsLeft.ToString();
    }
    
    // adds 1 points to attribute int every time a button is presses
    private void ButtonCallBackInt()
    {
        if (pointsLeft > 0)
        {
            intelStat = Add(intelStat);
            txtint.text = "Intelligence: " + intelStat.ToString();
            RemovePoint();
        }
        txtPointsLeft.text = "Points Left: " + pointsLeft.ToString();
    }

    // removes 1 point from attribute pool every time a button is clicked
    private void RemovePoint()
    {
        pointsLeft -= 1;
    }

    private int Add(int currentPoints)
    {
        return currentPoints + 1;
    }

    /*OnLoad get current device screen resolution scale UI to fit by comparing it to 1920 x 1080 base resolution
     _________________________________________
     |                                       |
     |                                       |
     |                                       |
     |                                       |
     |                                       |
     |                                       |
     _________________________________________
     game is always in portrait mode
     */
    private float CalculateUiScale()
    {
        int screenWidth = Screen.currentResolution.width;
        int screenHeight = Screen.currentResolution.height;
        Vector2 scalerReferenceResolution = new Vector2(x: 1920f, y: 1080f);
        int scalerMatchWidthOrHeight = 0;
        // Debug.Log("width" + screenWidth + "Height" +  screenHeight);
        
        return Mathf.Pow(screenWidth / scalerReferenceResolution.x, 1f - scalerMatchWidthOrHeight) *
               Mathf.Pow(screenHeight / scalerReferenceResolution.y, scalerMatchWidthOrHeight);
    }
}