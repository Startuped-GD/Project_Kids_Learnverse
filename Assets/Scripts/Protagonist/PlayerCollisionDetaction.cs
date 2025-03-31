using UnityEngine;

public class PlayerCollisionDetaction : MonoBehaviour
{
    private Lv1Managment level1;
    public GameObject detactedLocationSwitcher;
    public IPTCoin coinManage;
    public AudioManagement audioManager;
    public PlayerDetactNPC playerNPCDetaction; 

    private void Start()
    {
        level1 = GameObject.Find("Lv1 Manager").GetComponent<Lv1Managment>();
    }

    private void OnTriggerEnter2D(Collider2D collisionDetails)
    {
        if(collisionDetails != null)
        {
            if(collisionDetails.CompareTag("Location Icon"))
            {
                Debug.Log("Location Icon");
                detactedLocationSwitcher = collisionDetails.gameObject;
                LocationSwitcherOutside locationSwitcher = detactedLocationSwitcher.GetComponent<LocationSwitcherOutside>();
                locationSwitcher.LocationSwitchButton(true);
            }

            if (collisionDetails.CompareTag("Location Icon Second"))
            {
                Debug.Log("Location Icon Second");
                LocationSwitcherInside locationSwitcher = collisionDetails.transform.GetComponent<LocationSwitcherInside>();
                StartCoroutine(locationSwitcher.LocationSwitchConfirm(detactedLocationSwitcher,collisionDetails.gameObject));
            }

            if (collisionDetails.CompareTag("Bubble"))
            {
                Debug.Log("Its Bubble");
                Destroy(collisionDetails.gameObject);
                level1.IncreaseBubbleCollection();
                audioManager.ItemCollectionAudio(); 
            }

            if (collisionDetails.CompareTag("Coin"))
            {
                Debug.Log("Its Coin");
                Destroy(collisionDetails.gameObject);
                coinManage.IncreaseCoin(100);
                audioManager.ItemCollectionAudio(); 
            }

            if (collisionDetails.CompareTag("NPC") && !playerNPCDetaction.isNpcDetacted)
            {
                Debug.Log("NPC IS DETACTED!");
                playerNPCDetaction.isNpcDetacted = true; 
                playerNPCDetaction.detactedNPC = collisionDetails.gameObject;
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collisionDetails)
    {
        if(collisionDetails != null)
        {
            if(collisionDetails.CompareTag("Location Icon"))
            {
                Debug.Log("Exit Location Icon"); 
            }

            if (collisionDetails.CompareTag("Location Icon"))
            {
                Debug.Log("Location Icon");
                LocationSwitcherOutside locationSwitcher = detactedLocationSwitcher.GetComponent<LocationSwitcherOutside>();
                locationSwitcher.LocationSwitchButton(false);
            }

            if (collisionDetails.CompareTag("NPC") && playerNPCDetaction.isNpcDetacted)
            {
                Debug.Log("NPC IS OUT");
                playerNPCDetaction.isNpcDetacted = false;
                playerNPCDetaction.detactedNPC = null;
            }
        }
    }
}
