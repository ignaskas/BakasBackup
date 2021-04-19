﻿﻿using System;
using System.Collections;
using System.Collections.Generic;
 using System.Diagnostics;
 using System.IO;
using System.Linq;
 using System.Net.Mime;
 using System.Threading;
using System.Threading.Tasks;
// using Newtonsoft.Json;
// using UnityEditor.Experimental.GraphView;
// using UnityEditorInternal;
// using StatePattern;
using UnityEngine;
using UnityEngine.Serialization;
 using UnityEngine.SocialPlatforms;
 using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
 using Debug = UnityEngine.Debug;
 using Image = UnityEngine.UI.Image;
 using Object = System.Object;
 using Random = System.Random;

public class Main_Behaviour : MonoBehaviour
{
    public Button[] menuCards;
    public GameObject storyPlate;
    public menuBehaviur menuBehaviurRef;
    public Inventory inventoryRef;
    public EndingLogic endingLogicRef;
    public AudioScript AudioScript;
    public Text[] StoryText;
    public Transform playerPosition;
    public int playerSpeed = 2; //speed at witch the player object moves all animations are affected by this

    private List<bool> CardBoolList = new List<bool>();
    private TextAsset[] StoryTextArrayName;
    private Dictionary<string, TextAsset> StoryDictionary = new Dictionary<string, TextAsset>();
    private UnityEngine.Object[] StoryTextArrayObject;
    private string[] StoryTextArray;
    //this is stupid player should be its own class and hold all the values instead of having dynamically changing elements inside static states witch code cant access at all time |<----fix this if there is time
    public GameState PlayerPoint { get; set; }
    public GameState CoinUpdate { get; set; }
    public GameState FlaskUpdate { get; set; }
    public GameState ArmorUpdate { get; set; }
    public GameState blasterUpdate { get; set; }

    public class GameState
    {
        public string Name;
        public Cards StoryCardName;
        public Vector3 Position;
        public GameState[] PossibleOutcomes;
        public Vector3[] Steps;
        public string StoryTextField;
        public AttributeCheck StatCheck;

        public GameState(string name, Cards storyCardName, Vector3 position, GameState[] possibleOutcomes, Vector3[] steps, string storyTextField, AttributeCheck statReturn)
        {
            this.Name = name;
            this.StoryCardName = storyCardName;
            this.Position = position;
            this.PossibleOutcomes = possibleOutcomes;
            this.Steps = steps;
            this.StoryTextField = storyTextField;
            this.StatCheck = statReturn;
        }
    }

    public class AttributeCheck
    {
        public bool AttributeReturn { set; get; }

        public AttributeCheck(bool attributeReturn)
        {
            AttributeReturn = attributeReturn;
        }
    }
    
    public class Cards
    {
        public Sprite CardImage;

        public Cards(Sprite cardImage)
        {
            CardImage = cardImage;
        }
    }

    private void Start()
    {
        StoryTextArrayObject = Resources.LoadAll("storyTextTest", typeof(TextAsset));
        StoryTextArrayName = Resources.LoadAll<TextAsset>("storyTextTest");
        var fillDictionary = 0;
        foreach (var iDontKnowNamesAreHard in StoryTextArrayObject)
        {
            StoryDictionary.Add(iDontKnowNamesAreHard.name, StoryTextArrayName[fillDictionary]);
            fillDictionary++;
        }
    }

    //GameStates
    public void CreateObjects()
    {
        //this is death
        GameState port = new GameState(
            "portEnding",
            new Cards(Resources.Load<Sprite>("cards_faces/ToPort")), 
            new Vector3(3.73f, 8.814f), 
            new GameState[] {},
            new []
            {
                new Vector3(0.232f, 7.24f, -1f),
                new Vector3(0.631f, 9.541f, -1f),
                new Vector3(1.633f, 9.643f, -1f),
                new Vector3(3.73f, 8.814f, -1f),
            },
            "PortEnding", 
            new AttributeCheck(true)
        );
        //this is death
        GameState miningCampSteal = new GameState(
            "miningCampStealEnding",
            new Cards(Resources.Load<Sprite>("cards_faces/StealCoins")),
            new Vector3(-4.05f, 13.9f),
            new GameState[] {},
            new []
            {
                new Vector3(0.6f, 13.13f, -1f),
                new Vector3(-0.67f, 13.77f, -1f),
                new Vector3(-4.05f, 13.9f, -1f),
            },
            "MiningCampStealEnding",
            new AttributeCheck(true)
        );
        //this is death
        GameState failToWin = new GameState(
            "failToWinEnding",
            new Cards(Resources.Load<Sprite>("cards_faces/dragon_steal_card")),
            new Vector3(1.81f, -9.61f),
            new GameState[] {},
            new []
            {
                new Vector3(1.14f, -6.68f, -1f),
                new Vector3(1.81f, -9.61f, -1f),
            },
            "Dragon_steal",
            new AttributeCheck(true)
        );
        
        //BITCOIN ending
        GameState mineBitCoins = new GameState(
            "mineBitCoinEnding",
            new Cards(Resources.Load<Sprite>("cards_faces/MineCoins")),
            new Vector3(-4.05f, 13.9f),
            new GameState[] {},
            new []
            {
                new Vector3(0.6f, 13.13f, -1f),
                new Vector3(-0.67f, 13.77f, -1f),
                new Vector3(-4.05f, 13.9f, -1f),
            },
            "BitCoinStealSucces",
            new AttributeCheck(true)
        );
        
        //Dragon ending do an if check for items
        GameState toDragon = new GameState(
            "goToDragonEnding",
            new Cards(Resources.Load<Sprite>("cards_faces/dragon_run_card")),
            new Vector3(18.64f, -1.22f),
            new GameState[] {},
            new []
            {
                new Vector3(17.63f, -3.13f, -1f),
                new Vector3(18.64f, -1.22f, -1f),
            },
            "StoryTest",//TODO: fix me
            new AttributeCheck(true)
        );
        //outskirts ending do roll on outcome
        GameState outskirts = new GameState(
            "outskirtsEnding",
            new Cards(Resources.Load<Sprite>("cards_faces/ToOutskirts")),
            new Vector3(7.16f, 1.34f),
            new GameState[] {},
            new []
            {
                new Vector3(3.42f, -0.25f, -1f),
                new Vector3(5.43f, 0.12f, -1f),
                new Vector3(7.16f, 1.34f, -1f),
            },
            "OutskirstsEnding",
            new AttributeCheck(true)
        );
        //this is death
        GameState goToCityAfterJungle = new GameState(
            "goToCityAfterJungleEnding",
            new Cards(Resources.Load<Sprite>("cards_faces/merchant_keep_card")),
            new Vector3(4.21f, -2.45f),
            new GameState[] {},//TODO: missing flee card
            new []
            {
                new Vector3(1.52f, -6.73f, -1f),
                new Vector3(4.99f, -5.4f, -1f),
                new Vector3(4.21f, -2.45f, -1f),
            },
            "CityDeath",
            new AttributeCheck(true)
        );
        
        //this is death
        GameState forrestDrink = new GameState(
            "forrestEnding",
            new Cards(Resources.Load<Sprite>("cards_faces/forest_drink_card")),
            new Vector3(-16.39f, 2.11f),
            new GameState[] {},
            new []
            {
                new Vector3(-16.39f, 2.11f, -1f),
            },
            "ForestDrink",
            new AttributeCheck(true)
        );
        
        //this is death do a roll for outcome
        GameState flee = new GameState(
            "FleeEnding",
            new Cards(Resources.Load<Sprite>("cards_faces/dragon_run_card")),
            new Vector3(18.191f, 6.756f),
            new GameState[] {},
            new []
            {
                new Vector3(16.508f, 8.654f, -1f),
                new Vector3(16.947f, 7.682f, -1f),
                new Vector3(16.947f, 7.682f, -1f),
                new Vector3(17.536f, 7.588f, -1f),
                new Vector3(18.191f, 6.756f, -1f),
            },
            "DragonRunEnding",
            new AttributeCheck(true)
        );
        //do a roll for ending
        GameState toMountains = new GameState(
            "mountainsEnding",
            new Cards(Resources.Load<Sprite>("cards_faces/ToMountains")),
            new Vector3(18.95f, 13.99f),
            new GameState[] {},
            new []
            {
                new Vector3(15.41f, 4.91f, -1f),
                new Vector3(16.23f, 7.07f, -1f),
                new Vector3(17.17f, 8.91f, -1f),
                new Vector3(17.64f, 9.9f, -1f),
                new Vector3(18.00f, 12.16f, -1f),
                new Vector3(18.64f, 12.82f, -1f),
                new Vector3(18.46f, 13.36f, -1f),
                new Vector3(18.95f, 13.99f, -1f),
            },
            "MountainsEnd",
            new AttributeCheck(true)
        );
        
        GameState poisonDragon = new GameState(
            "poisonDragon",
            new Cards(Resources.Load<Sprite>("cards_faces/dragon_poison_card")),
            new Vector3(18.64f, -1.22f),
            new GameState[] {},
            new []
            {
                new Vector3(17.63f, -3.13f, -1f),
                new Vector3(18.64f, -1.22f, -1f),
            },
            "PoisionDragon",
            new AttributeCheck(false)
        );
        
        GameState dragonArmor = new GameState(
            "dragonArmor",
            new Cards(Resources.Load<Sprite>("cards_faces/dragon_attack_win_card")),
            new Vector3(18.64f, -1.22f),
            new GameState[] {},
            new []
            {
                new Vector3(17.63f, -3.13f, -1f),
                new Vector3(18.64f, -1.22f, -1f),
            },
            "Dragon_defeated",
            new AttributeCheck(false)
        );
        
        //TODO: temp card icon
        GameState toTower = new GameState(
            "goToTower",
            new Cards(Resources.Load<Sprite>("cards_faces/dragon_poison_card")),
            new Vector3(15.73f, -2.89f),
            new GameState[] {toDragon, poisonDragon, dragonArmor},
            new []
            {
                new Vector3(4.95f, -5.46f, -1f),
                new Vector3(13.04f, -2.31f, -1f),
                new Vector3(14.19f, -2.31f, -1f),
                new Vector3(15.73f, -2.89f, -1f),
            },
            "WizardsTower",
            new AttributeCheck(true)
        );
        
        GameState blasterEnding = new GameState(
            "blasterEnding",
            new Cards(Resources.Load<Sprite>("cards_faces/dragon_use_blaster_card")),
            new Vector3(15.73f, -2.89f),
            new GameState[] {},
            new []
            {
                new Vector3(15.73f, -2.89f, -1f),

            },
            "Blaster_dragon",
            new AttributeCheck(false)
        );
        
        GameState toTowerAfterPortal = new GameState(
            "goToTowerAfterPortal",
            new Cards(Resources.Load<Sprite>("cards_faces/dragon_poison_card")),
            new Vector3(15.73f, -2.89f),
            new GameState[] {toDragon, blasterEnding},
            new []
            {
                new Vector3(16.97f, 3.45f, -1f),
                new Vector3(18.86f, 1.73f, -1f),
                new Vector3(18.68f, -0.52f, -1f),
                new Vector3(15.73f, -2.89f, -1f),

            },
            "TowerAfterPortal",
            new AttributeCheck(true)
        );
        
        GameState miningCampTalkToTheBoss = new GameState(
            "miningCampTalk",
            new Cards(Resources.Load<Sprite>("cards_faces/TalkToBoss")),
            new Vector3(-4.05f, 13.9f),
            new [] {mineBitCoins},
            new []
            {
                new Vector3(0.6f, 13.13f, -1f),
                new Vector3(-0.67f, 13.77f, -1f),
                new Vector3(-4.05f, 13.9f, -1f),
            },
            "MiningCamp_StatCheck_Pass",
            new AttributeCheck(menuBehaviurRef.strStat >= 5)
        );
        
        GameState miningCamp = new GameState(
            "miningCamp",
            new Cards(Resources.Load<Sprite>("cards_faces/ToMiningCamp")),
            new Vector3(1.81f, 13.5f),
            new [] {miningCampTalkToTheBoss, miningCampSteal},
            new []
            {
                new Vector3(0.3f, 7.01f, -1f),
                new Vector3(0.51f, 8.45f, -1f),
                new Vector3(0.92f, 10.85f, -1f),
                new Vector3(0.92f, 13.22f, -1f),
                new Vector3(1.81f, 13.5f, -1f),
            },
            "MiningCamp",
            new AttributeCheck(true)
        );
        
        GameState buyArmor = new GameState(
            "BuyArmor",
            new Cards(Resources.Load<Sprite>("cards_faces/Buy_Armor")),
            new Vector3(3.529f, -2.451f),
            new [] {toTower},
            new []
            {
                new Vector3(3.529f, -2.451f, -1f),
            },
            "BuyArmor",
            new AttributeCheck(false)
        );
        
        GameState stealArmor = new GameState(
            "StealArmor",
            new Cards(Resources.Load<Sprite>("cards_faces/StealArmor")),
            new Vector3(18.64f, -1.22f),
            new [] {toTower},
            new []//TODO: fix the Vectors steal armor should not lead to mage tower
            {
                new Vector3(4.95f, -5.46f, -1f),
                new Vector3(13.04f, -2.31f, -1f),
                new Vector3(14.19f, -2.31f, -1f),
                new Vector3(15.73f, -2.89f, -1f),
                new Vector3(17.63f, -3.13f, -1f),
                new Vector3(18.64f, -1.22f, -1f),
            },
            "StealArmor",
            new AttributeCheck(menuBehaviurRef.agyStat >= 5)
        );

        GameState shop = new GameState(
            "Shop",
            new Cards(Resources.Load<Sprite>("cards_faces/armor")),
            new Vector3(3.529f, -2.451f),
            new [] {toTower, buyArmor, stealArmor},
            new []
            {
                new Vector3(3.895f, -1.173f, -1f),
                new Vector3(4.778f, -1.948f, -1f),
                new Vector3(3.529f, -2.451f, -1f),
            },
            "Shop",
            new AttributeCheck(true)
        );
        
        GameState cityAfterQuest = new GameState(
            "cityAfterQuest",
            new Cards(Resources.Load<Sprite>("cards_faces/go_to_town_card")),
            new Vector3(4.2f, -2.56f),
            new [] {shop, outskirts, toTower},
            new []
            {
                new Vector3(-0.52f, -6.75f, -1f),
                new Vector3(4.94f, -5.64f, -1f),
                new Vector3(4.2f, -2.56f, -1f),
            },
            "City",
            new AttributeCheck(true)
        );
        
        GameState takeLootBack = new GameState(
            "takeLootBack",
            new Cards(Resources.Load<Sprite>("cards_faces/merchant_return_card")),
            new Vector3(-1f, -5.76f),
            new [] {cityAfterQuest, toTower},
            new []
            {
                new Vector3(1.45f, -6.63f, -1f),
                new Vector3(-1f, -6.63f, -1f),
                new Vector3(-1f, -5.76f, -1f),
            },
            "ReturnLoot",
            new AttributeCheck(true)
        );
        
        GameState jungle = new GameState(
            "jungle",
            new Cards(Resources.Load<Sprite>("cards_faces/SearchForTheves")),
            new Vector3(2.116f, -11.071f),
            new [] {takeLootBack, goToCityAfterJungle},
            new []
            {
                new Vector3(4.344f, -11.139f, -1f),
                new Vector3(4.962f, -12.45f, -1f),
                new Vector3(2.116f, -11.071f, -1f),

            },
            "jungle",
            new AttributeCheck(true)
        );
        //TODO: prob shold remove this idk yet
        GameState giveUpTheChase = new GameState(
            "giveUpTheChase",
            new Cards(Resources.Load<Sprite>("cards_faces/follow_lizardMen_card")),
            new Vector3(2.304f, -10.603f),
            new []{toDragon},
            new []
            {
                new Vector3(1.226f, -6.399f, -10.633f),
            }, 
            "GiveUpTheChase",
            new AttributeCheck(true)
            );
        
        GameState helpMerchant = new GameState(
            "aggreToHelp",
            new Cards(Resources.Load<Sprite>("cards_faces/follow_lizardmen_card")),
            new Vector3(2.304f, -10.633f),
            new [] {jungle, giveUpTheChase},//TODO: need 1 more choice to leave and go to city directly
            new []
            {
                new Vector3(1.226f, -6.399f, -1f),
                new Vector3(1.815f, -8.836f, -1f),
                new Vector3(2.304f, -10.633f, -1f),
            },
            "MerchantHelp",
            new AttributeCheck(true)
        );
        
        GameState ignoreMerchant = new GameState(
            "ignoreMerchant",
            new Cards(Resources.Load<Sprite>("cards_faces/Merchant_ignore")),
            new Vector3(4.2f, -2.56f),
            new [] {shop, toTower},
            new []
            {
                new Vector3(-0.52f, -6.75f, -1f),
                new Vector3(4.94f, -5.64f, -1f),
                new Vector3(4.2f, -2.56f, -1f),
            },
            "IgnoreMerchant",
            new AttributeCheck(true)
        );
        
        GameState quest = new GameState(
            "cityQuest",
            new Cards(Resources.Load<Sprite>("cards_faces/Quest_PH")),
            new Vector3(-1.25f, -5.89f),
            new [] {helpMerchant, ignoreMerchant},
            new []
            {
                new Vector3(4.72f, -5.56f, -1f),
                new Vector3(-0.59f, -6.76f, -1f),
                new Vector3(-1.25f, -5.89f, -1f),
            },
            "cityQuest",
            new AttributeCheck(true)
        );
        
        //city line coming from crossroads
        GameState city = new GameState(
            "city",
            new Cards(Resources.Load<Sprite>("cards_faces/go_to_town_card")),
            new Vector3(3.61f, -2.24f),
            new [] {quest, outskirts, shop},
            new []
            {
                new Vector3(-0.87f, 4.62f, -1f),
                new Vector3(1.48f, 2.72f, -1f),
                new Vector3(1.89f, 2.09f, -1f),
                new Vector3(3.06f, 1.93f, -1f),
                new Vector3(3.61f, 0.78f, -1f),
                new Vector3(3.61f, -2.24f, -1f),
            },
            "City",
            new AttributeCheck(true)
        );
        
        //forest crossroads merge with road story if heading to city
        GameState crossRoads = new GameState(
            "crossroads",
            new Cards(Resources.Load<Sprite>("cards_faces/CrossRoads_PH")),
            new Vector3(-0.84f, 6.34f),
            new [] {city, miningCamp, port},//TODO: reuse this for extra dragon
            new []
            {
                new Vector3(-7.15f, 3.23f, -1f),
                new Vector3(-5.19f, 6.34f, -1f),
                new Vector3(-0.84f, 6.34f, -1f),
            },
            "StoryTest",
            new AttributeCheck(true)
        );
        
        GameState forrestIntCheck = new GameState(
            "forrestIntCheck",
            new Cards(Resources.Load<Sprite>("cards_faces/forest_flask_card")),
            new Vector3(-0.84f, 6.34f),
            new [] {city, miningCamp, port},
            new []
            {
                new Vector3(-7.15f, 3.23f, -1f),
                new Vector3(-5.19f, 6.34f, -1f),
                new Vector3(-0.84f, 6.34f, -1f),
            },
            "ForrestIntCheck",
            new AttributeCheck(menuBehaviurRef.intelStat >= 5)
        );
        
        GameState forrestWalk = new GameState(
            "forrestLeave",
            new Cards(Resources.Load<Sprite>("cards_faces/forest_leave_card")),
            new Vector3(-0.84f, 6.34f),
            new [] {city, miningCamp, port},
            new []
            {
                new Vector3(-7.15f, 3.23f, -1f),
                new Vector3(-5.19f, 6.34f, -1f),
                new Vector3(-0.84f, 6.34f, -1f),
            },
            "ForestWalk",
            new AttributeCheck(true)
        );
        
        // forrest line starts here
        GameState forrest = new GameState(
            "forrest",
            new Cards(Resources.Load<Sprite>("cards_faces/forest_card")),
            new Vector3(-11.19f, 1.43f),
            new [] {forrestDrink, forrestWalk, forrestIntCheck},
            new []
            {
                new Vector3(-14.68f, -3.61f, -1f),
                new Vector3(-17.07f, -2.11f, -1f),
                new Vector3(-11.19f, 1.43f, -1f),
            },
            "ForestChoice",
            new AttributeCheck(true)
        );
        
        GameState toBattlefield = new GameState(
            "battlefield",
            new Cards(Resources.Load<Sprite>("cards_faces/ToBattleField")),
            new Vector3(15.18f, 9.28f),
            new [] {flee},
            new []
            {
                new Vector3(15.25f, 6.56f, -1f),
                new Vector3(15.18f, 9.28f, -1f),
            },
            "Battlefield",
            new AttributeCheck(true)
        );
        
        GameState portal = new GameState(
            "portal",
            new Cards(Resources.Load<Sprite>("cards_faces/portal_card")),
            new Vector3(15.13f, 3.81f),
            new [] {toTowerAfterPortal, toBattlefield, toMountains},
            new []
            {
                new Vector3(-13.33f, -7.63f, -1f),
                new Vector3(-9.36f, -13.29f, -1f),
                new Vector3(15.13f, 3.81f, -1f),
            },
            "portal",
            new AttributeCheck(true)
        );
        
        GameState road = new GameState(
            "road",
            new Cards(Resources.Load<Sprite>("cards_faces/road_card")),
            new Vector3(-0.98f, -5.85f),
            new [] {helpMerchant, ignoreMerchant},
            new []
            {
                new Vector3(-9.64f, -6.37f, -1f),
                new Vector3(-6.71f, -7.02f, -1f),
                new Vector3(-1.18f, -6.77f, -1f),
                new Vector3(-0.98f, -5.85f, -1f),
            },
            "Road",
            new AttributeCheck(true)
        );
        
        GameState blaster = new GameState(
            "blaster",
            new Cards(Resources.Load<Sprite>("cards_faces/Tech_Chest")),
            new Vector3(-12.82f, -6.55f), 
            new [] {forrest, road, portal},
            new []
            {
                new Vector3(-16.51f, -5.89f, -1f),
                new Vector3(-12.82f, -6.55f, -1f),
            },
            "BlasterChoise",
            new AttributeCheck(true)
        );
        
        GameState sword = new GameState(
            "sword",
            new Cards(Resources.Load<Sprite>("cards_faces/sturdy_chest_card")),
            new Vector3(-12.82f, -6.55f), 
            new [] {forrest, road, portal},
            new []
            {
                new Vector3(-16.51f, -5.89f, -1f),
                new Vector3(-12.82f, -6.55f, -1f),
            },
            "SwordChoise",
            new AttributeCheck(true)
        );
        
        GameState staff = new GameState(
            "staff",
            new Cards(Resources.Load<Sprite>("cards_faces/Magic_chest_card")),
            new Vector3(-12.82f, -6.55f),
            new [] {forrest, road, portal},
            new []
            {
                new Vector3(-16.51f, -5.89f, -1f), 
                new Vector3(-12.82f, -6.55f, -1f), 
            },
            "StaffChoice",
            new AttributeCheck(true)
        );
        
        GameState home = new GameState(
            "start",
            new Cards(Resources.Load<Sprite>("cards_faces/armor")),
            new Vector3(-16.42f, -5.76f),
            new [] {staff, sword, blaster},
            new []
            {
                new Vector3(-16.42f, -5.76f, -1f),
            },
            "StoryTest",
            new AttributeCheck(true)
        );
        
        PlayerPoint = home;
        CoinUpdate = buyArmor;
        ArmorUpdate = dragonArmor;
        FlaskUpdate = poisonDragon;
        blasterUpdate = blasterEnding;
    }
//Move player
    private async void MoveMe(GameState currentState)
    {
        foreach (var currentStep in currentState.Steps)//check current GameState.Steps that player has picked
        {
            StartCoroutine(MoveOverSeconds(gameObject, currentStep, playerSpeed));
            WhichSideDoIFace(transform.position.x, currentStep.x, transform.position.y, currentStep.y);
            StartCoroutine(AudioScript.PlaySound());
            await Task.Delay(playerSpeed * 1000); // wait for current animation to end before looping again
        }   
        StartCoroutine(AudioScript.PlaySound());
    }

//move over X seconds
    private IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 end, float seconds)
    {
        float elapsedTime = 0; //time
        Vector3 startingPos = objectToMove.transform.position; // get current object position
        while (elapsedTime < seconds)
        {
            objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds)); //move object from startingPos to end over X seconds
            elapsedTime += Time.deltaTime; // The completion time in seconds since the last frame
            yield return new WaitForEndOfFrame(); // wait for unity to render every thing on GUI before displaying the next frame 
        }

        objectToMove.transform.position = end;
    }
//Change the direction the player is facing based on the direction of movement
//TODO: need to convert this for a proper 3D model
    private void WhichSideDoIFace (float currentX, float nextStepX, float currentY, float nextStepY) {
        if ((nextStepX >= currentX) && (nextStepY >= currentY)) {  //we are moving NW -- top right
            gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Player/Backs");
            // gameObject.GetComponent<SpriteRenderer>().flipX = true;
            // gameObject.GetComponent<SpriteRenderer>().flipY = false;
        } else if ((nextStepX <= currentX) && (nextStepY <= currentY)) { //we are moving SE -- bottom left
            gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Player/Right");
            // gameObject.GetComponent<SpriteRenderer>().flipX = false;
            // gameObject.GetComponent<SpriteRenderer>().flipY = true;
        } else if ((nextStepX <= currentX) && (nextStepY >= currentY)) { //we are moving NE -- top left
            gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Player/Left");
            // gameObject.GetComponent<SpriteRenderer>().flipX = false;
            // gameObject.GetComponent<SpriteRenderer>().flipY = false;
        } else if ((nextStepX >= currentX) && (nextStepY <= currentY)) { //we are moving SW -- bottom right
            gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Player/Front");
            // gameObject.GetComponent<SpriteRenderer>().flipX = true;
            // gameObject.GetComponent<SpriteRenderer>().flipY = true;
        }
    }
//Generate card faces for next possible moves
    private async void ChangeButtonImg(GameState currentState)
    {
        var hideForTime = currentState.Steps.Length;
        foreach (var cards in menuCards)
        {
            cards.gameObject.SetActive(false);// hide cards while the player is moving
        }
        await Task.Delay((hideForTime * playerSpeed) * 1000);// wait for the player to finish moving to the new position before showing cards
        var i = 0;
        foreach (var buttonImage in currentState.PossibleOutcomes)
        {
            var currentCard = menuCards[i];
            if (!currentCard.gameObject.activeSelf)
            {
                currentCard.gameObject.SetActive(true);//show cards if they are currently hidden
            }
            if (!buttonImage.StatCheck.AttributeReturn)//hide the card if player does not have required stats
            {
                currentCard.gameObject.SetActive(false);
            }
            currentCard.GetComponent<Image>().sprite = buttonImage.StoryCardName.CardImage;
            i++;
        }
        
        ConvertButtonsToBoolList();
        //if no cards being shown we are at the end of the game call EndingLogic
        if (CardBoolList.All(boolList => boolList == false))
        {
            endingLogicRef.GameEnding();
            return;
        }
        CardBoolList.Clear();//clear the list if list contains any true values for the next move
        
        while (i < menuCards.Length)//hide cards if they are currently showing
        {
            menuCards[i].gameObject.SetActive(false);
            i++;
        }
    }

    /*since the buttons are bing a bitch using array.IsActive method^^^^
         writing it to a list then checking if the list contains only false*/
    private void ConvertButtonsToBoolList()
    {
        var convertToList = 0;
        while (convertToList < menuCards.Length)
        {
            CardBoolList.Add(menuCards[convertToList].IsActive());
            convertToList++;
        }
    }
    
    /*change story based on what GameState the player is currently in
     take the name of the story from StoryTextField use it in dictionary to get String use SplitToLines method to split the string at new line
                                                                                            set each text element StoryText[] to that ^^^^ */
    private async void ChangeStoryPlate(GameState currentState)
    {
        var hideForTime = currentState.Steps.Length;//should make hideForTime its own method
        storyPlate.gameObject.SetActive(false);
        await Task.Delay((hideForTime * playerSpeed) * 1000);//Wait for player to finish animations before showing cards
        
        //set story text based on info inside the text file
        int putTextInToArray = 0;
        foreach (var textLine in StoryDictionary[currentState.StoryTextField].text.SplitToLines())
        {
            StoryText[putTextInToArray].text = textLine;
            putTextInToArray++;
        }

        storyPlate.gameObject.SetActive(true);
    }

    // Register Button events
    private void OnEnable()
    {
        menuCards[0].onClick.AddListener(ButtonCallBackCard1);
        menuCards[1].onClick.AddListener(ButtonCallBackCard2);
        menuCards[2].onClick.AddListener(ButtonCallBackCard3);
        DEBUGONLY.onClick.AddListener(()=>buttonCallBackDEBUG());
    }
    
    public Button DEBUGONLY;
    public InputField DEBUGFIELD;
//THIS IS FOR DEBUG ONLY!
    private void buttonCallBackDEBUG()
    {
        var position = this.transform.position;
        var outputX = position.x.ToString();
        var outputY = position.y.ToString();
        string txtDoucumentName = Application.streamingAssetsPath + "/cords/" + "I AM LAZY" + ".txt";

        if (!File.Exists(txtDoucumentName))
        {
            File.WriteAllText(txtDoucumentName, "MY LOG \n\n");
        }

        if (DEBUGFIELD.text == "")
        {
            File.AppendAllText(txtDoucumentName, "new Vector3(" + outputX + "f, " + outputY + "f," + " " + "-1f)," + "\n");
        }
        else
        {
            File.AppendAllText(txtDoucumentName, DEBUGFIELD.text + ": " + "\n");
        }

        DEBUGFIELD.text = "";
    }
    //should make this 1 method but this is simpler
    //Card1 button actions
    private void ButtonCallBackCard1()
    {
        PlayerPoint = PlayerPoint.PossibleOutcomes[0];
        inventoryRef.InventoryCheck();
        ChangeStoryPlate(PlayerPoint);
        MoveMe(PlayerPoint);
        ChangeButtonImg(PlayerPoint);
    }

    //Card2 button actions
    private void ButtonCallBackCard2()
    {
        PlayerPoint = PlayerPoint.PossibleOutcomes[1];
        inventoryRef.InventoryCheck();
        ChangeStoryPlate(PlayerPoint);
        MoveMe(PlayerPoint);
        ChangeButtonImg(PlayerPoint);
    }
    
    //Card2 button actions
    private void ButtonCallBackCard3()
    {
        PlayerPoint = PlayerPoint.PossibleOutcomes[2];
        inventoryRef.InventoryCheck();
        ChangeStoryPlate(PlayerPoint);
        MoveMe(PlayerPoint);
        ChangeButtonImg(PlayerPoint);
    }
}

  public static class ExtensionMethods
  {
      public static IEnumerable<string> SplitToLines(this string input)
      {
          if (input == null)
          {
              yield break;
          }

          using (StringReader reader = new StringReader(input))
          {
              string line;
              while ( (line = reader.ReadLine()) != null)
              {
                  yield return line;
              }
          }
      }
  }