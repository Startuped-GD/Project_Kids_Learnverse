using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Linq.Expressions;

public class LocationSwitcherOutside : MonoBehaviour
{
    public string BUILDINGNAME;
    public DialogueSystemManagment dialogueManager;
    public bool canSwitchLocation { get; private set; } = false;
    public PlayerDetactNPC playerDetactNPC;

    [Space]
    // Location
    [Header("LOCATION")]
    public GameObject locationSwitcherPrefab;
    public Transform playerBackSpawnPos; 
    private Transform buildingSpawnPoint;
    private Transform buildingTransformPoint; 

    // Building Details 
    [Header("BUILDING INSIDE")]
    public GameObject buildingPrefab;
    public Sprite buildingSprite; 
    public List<Vector2> buildingBordersPosition = new();
    public Vector2 buildingEntryPosition;
    public Vector2 buildingExitPosition;
    public Vector2 bubblePosition; 
    public Vector2 bubblePosition2;
    public List<Vector2> bubblePositions = new(); 
    
    private Transform buildingSpawnPos;

    // Buttons
    private GameObject locationSwitchBtnObject;

    // Other 
    PlayerLocationSwitch playerLocationSwitcher;
    Lv1Managment level1; 
    private GameObject imageFadeEffectObject;
    private Transform npcParent;
    private Transform vehicleParent;
    private Player_Control playerControllSystem; 


    private void Start()
    {
        buildingSpawnPos = GameObject.Find("Buidling Spawner").transform; 
        locationSwitchBtnObject = GameObject.Find("Location Switch").gameObject; 

        playerLocationSwitcher = GameObject.FindWithTag("Player").GetComponent<PlayerLocationSwitch>();
        playerControllSystem = GameObject.FindWithTag("Player").GetComponent<Player_Control>(); 

        imageFadeEffectObject = GameObject.Find("Canvas").transform.Find("Player Transition Fade").gameObject;

        vehicleParent = GameObject.FindWithTag("Vehicle Parent").transform; 
        npcParent = GameObject.FindWithTag("NPC Parent").transform;

        level1 = GameObject.Find("Lv1 Manager").GetComponent<Lv1Managment>(); 
    }

    private void Update()
    {
        if(canSwitchLocation)
        {
            if (!dialogueManager.isTheyTalking)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    Debug.Log("YE WALA HI HAI BHAI!");
                    canSwitchLocation = false;
                    StartCoroutine(LocationSwitchConfirm());
                }
            }
        }
    }

    public void LocationSwitchButton(bool enable_or_Disable)
    {
        canSwitchLocation = enable_or_Disable;

        Image image = locationSwitchBtnObject.GetComponent<Image>();
        image.enabled = enable_or_Disable;

        Button childButton = locationSwitchBtnObject.transform.GetChild(0).GetComponent<Button>();
        childButton.enabled = enable_or_Disable;

        Image childImage = locationSwitchBtnObject.transform.GetChild(0).GetComponent<Image>();
        childImage.enabled = enable_or_Disable;

        Text childText = childImage.transform.GetChild(0).GetComponent<Text>();
        childText.enabled = enable_or_Disable; 
    }

    public IEnumerator LocationSwitchConfirm()
    {
        playerDetactNPC.isStart = false;
        playerControllSystem.CanPlayerMove(false); 
        // Fade anim 
        imageFadeEffectObject.SetActive(true); 

        // Spawn Building 
        SpawnBuilding();

        yield return new WaitForSeconds(0.5f);

        // Spawn building's inside location switcher
        SpawnLocationSwitcher(locationSwitcherPrefab, buildingTransformPoint); 

        yield return new WaitForSeconds(0.5f);

        // Tranform Player 
        playerLocationSwitcher.TransformPlayer(buildingSpawnPoint, false);

        // Destorye all vehicle & npc 
        for(int i =0; i < vehicleParent.childCount; i++)
        {
            Destroy(vehicleParent.GetChild(i).gameObject);
        }
        for (int i = 0; i < npcParent.childCount; i++)
        {
            Destroy(npcParent.GetChild(i).gameObject);
        }

        yield return new WaitForSeconds(1.3f);

        imageFadeEffectObject.SetActive(false);
        playerControllSystem.CanPlayerMove(true); 
    }

    private void SpawnBuilding()
    {
        // Create building 
        GameObject newBuilding = Instantiate(buildingPrefab, buildingSpawnPos.position, Quaternion.identity);
        newBuilding.transform.parent = buildingSpawnPos; 

        // Set its borders 
        Transform bordersParent = newBuilding.transform.Find("Borders").transform;
        for(int i =0; i < bordersParent.childCount; i++)
        {
            Transform newBuildingBorder = bordersParent.GetChild(i).transform;
            newBuildingBorder.transform.position = new Vector2(bordersParent.transform.position.x + buildingBordersPosition[i].x, 
                                                       bordersParent.transform.position.y + buildingBordersPosition[i].y);  
        }

        //Set its sprite 
        SpriteRenderer newbuildingSprite = newBuilding.transform.Find("Sprite").GetComponent<SpriteRenderer>();
        newbuildingSprite.sprite = buildingSprite;

        // Set building entry 
        GameObject buildingSpawn = newBuilding.transform.Find("Entry Location").gameObject;
        buildingSpawn.transform.position = new(newBuilding.transform.position.x + buildingEntryPosition.x,
                                               newBuilding.transform.position.y + buildingEntryPosition.y);
        this.buildingSpawnPoint = buildingSpawn.transform; 

        // Set building's location switcher position & spawn another location swicther 
        GameObject buildingTransform = newBuilding.transform.Find("Location Transform").gameObject;
        buildingTransform.transform.position = new(newBuilding.transform.position.x + buildingExitPosition.x,
                                            newBuilding.transform.position.y + buildingExitPosition.y);
        buildingTransformPoint = buildingTransform.transform;

        //Bubbles position
        GameObject[] bubbleObject = new GameObject[2];
        bubbleObject[0] = newBuilding.transform.Find("Bubble Pos").gameObject;
        bubbleObject[0].transform.position = new(newBuilding.transform.position.x + bubblePosition.x,
                                            newBuilding.transform.position.y + bubblePosition.y);

        bubbleObject[1] = newBuilding.transform.Find("Bubble Pos 2").gameObject;
        bubbleObject[1].transform.position = new(newBuilding.transform.position.x + bubblePosition2.x,
                                            newBuilding.transform.position.y + bubblePosition2.y);

        // Relocate bubble placer 
        if (level1.bubblesType == "Industry")
        {
            if (BUILDINGNAME == "OFFICE 1")
            {
                for (int i = 0; i < 2; i++)
                {
                    Vector2 position = new(bubbleObject[i].transform.position.x, bubbleObject[i].transform.position.y);
                    level1.BubblePlacing(i, position);
                }
            }
            if (BUILDINGNAME == "OFFICE 2")
            {
                Vector2 position = new(bubbleObject[0].transform.position.x, bubbleObject[0].transform.position.y);
                level1.BubblePlacing(2, position);
            }
        }
        else if (level1.bubblesType == "Problems")
        {
            if (BUILDINGNAME == "LIBRARY")
            {
                Debug.Log("This is library");
                Vector2 position = new(bubbleObject[0].transform.position.x, bubbleObject[0].transform.position.y);
                level1.BubblePlacing(0, position);
            }
        }
        else if (level1.bubblesType == "Solution")
        {
            if (BUILDINGNAME == "RESTO")
            {
                Debug.Log("This is resto");
                Vector2 position = new(bubbleObject[0].transform.position.x, bubbleObject[0].transform.position.y);
                level1.BubblePlacing(0, position);
            }
        }
        else if (level1.bubblesType == "USP")
        {
            if (BUILDINGNAME == "OFFICE 3")
            {
                Debug.Log("This is office 3");
                Vector2 position = new(bubbleObject[0].transform.position.x, bubbleObject[0].transform.position.y);
                level1.BubblePlacing(0, position);
            }
        }
    }

    private void SpawnLocationSwitcher(GameObject locationSwitcher , Transform spawnPoint)
    {
        Instantiate(locationSwitcher, spawnPoint.position,Quaternion.identity);
    }
}
