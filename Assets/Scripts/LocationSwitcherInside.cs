using System.Collections;
using UnityEngine;

public class LocationSwitcherInside : MonoBehaviour
{
    public Transform newLocation;
    private PlayerLocationSwitch playerLocationSwitcher;
    public PlayerDetactNPC playerDetactNPC;

    private Transform buildingSpawnPos;

    private GameObject imageFadeEffectObject;
    private Player_Control playerControllSystem;
    private Lv1Managment level1; 

    private void Start()
    {
        buildingSpawnPos = GameObject.Find("Buidling Spawner").transform;

        playerLocationSwitcher = GameObject.FindWithTag("Player").GetComponent<PlayerLocationSwitch>();
        playerDetactNPC = GameObject.FindWithTag("Player").GetComponent<PlayerDetactNPC>();
        playerControllSystem = GameObject.FindWithTag("Player").GetComponent<Player_Control>(); 

        imageFadeEffectObject = GameObject.Find("Canvas").transform.Find("Player Transition Fade").gameObject;
        level1 = GameObject.Find("Lv1 Manager").GetComponent<Lv1Managment>(); 
    }

    public IEnumerator LocationSwitchConfirm(GameObject locationSwitchOutside,GameObject locationSwitchInside)
    {
        playerControllSystem.CanPlayerMove(false);
        imageFadeEffectObject.SetActive(true);

        // Get back position from location switcher outside
        LocationSwitcherOutside locationSwitch = locationSwitchOutside.GetComponent<LocationSwitcherOutside>();
        newLocation = locationSwitch.playerBackSpawnPos; 
       
        yield return new WaitForSeconds(1f);

        // Tranform Player 
        playerLocationSwitcher.TransformPlayer(newLocation, true);

        level1.BubbleResetPlacing(); 

        yield return new WaitForSeconds(1.3f);

        // Remove current building
        Destroy(buildingSpawnPos.transform.GetChild(0).gameObject);
        Destroy(locationSwitchInside);

        imageFadeEffectObject.SetActive(false);
        playerControllSystem.CanPlayerMove(true);
        playerDetactNPC.isStart = true; 
    }
}
