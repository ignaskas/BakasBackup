using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingLogic : MonoBehaviour
{
    public Main_Behaviour mainBehaviour;
    public Button restart;
    [SerializeField]
    private int chanceToWin = 50;

    //show restart button add click listener to it
    public void GameEnding()
    {
        this.gameObject.SetActive(true);
        restart.onClick.AddListener(buttonCallBackRestart);
        RandomOutcomeGenerator(mainBehaviour.PlayerPoint, chanceToWin);
    }

    //restart the game
    private void buttonCallBackRestart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
    
    //generate a int value from 0 to 100 compare it to chanceToWin variable to determine the outcome 
    private void RandomOutcomeGenerator(Main_Behaviour.GameState ending, int winChance)
    {
        var endingRoll = UnityEngine.Random.Range(0, 100);// Random.Range is inclusive only if its int for some reason
        
        switch (ending.Name)
        {
            case "miningCampStealEnding":
                if (endingRoll >= winChance)
                {
                    mainBehaviour.storyPlate.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Story_Board/youdie");
                }
                break;
            case "failToWinEnding":
                if (endingRoll >= winChance)
                {
                    mainBehaviour.storyPlate.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Story_Board/youdie");
                }
                break;
            case "FleeEnding":
                if (endingRoll >= winChance)
                {
                    mainBehaviour.storyPlate.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Story_Board/youdie");
                }
                break;
            case "mountainsEnding":
                if (endingRoll >= winChance)
                {
                    mainBehaviour.storyPlate.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Story_Board/youdie");
                }
                break;
            default:
                Debug.Log("Wrong State past to ending script" + ending.Name);
                break;
        }
    } 
    
}
