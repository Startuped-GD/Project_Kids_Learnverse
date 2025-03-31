using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    // Spawning
    public GameObject npcPrefab;
    private GameObject generatedNPC; 
    private bool canSpawn;

    public float minDistance;
    private Transform playerTransform;

    private Transform npcParent; 

    // Start is called before the first frame update
    void Start()
    {
        canSpawn = false;

        // Find references 
        playerTransform = GameObject.FindWithTag("Player").transform;
        npcParent = GameObject.FindWithTag("NPC Parent").transform; 
    }

    // Update is called once per frame
    void Update()
    {
        // Check Distance from player 
        Distance_From_Player(); 

        // Spawn
        if(canSpawn)
        {
            if(generatedNPC == null)
            {
                SpawnNPC();
            }
        }
    }

    private void SpawnNPC()
    {
        generatedNPC = Instantiate(npcPrefab, this.transform.position, Quaternion.identity);
        generatedNPC.transform.parent = npcParent;
    }

    private void Distance_From_Player()
    {
        // Get current positions 
        Vector2 spawnerPosition = this.transform.position;
        Vector2 playerPosition = playerTransform.position;

        // Calculate Distance 
        float Distance = Vector2.Distance(spawnerPosition, playerPosition); 

       /* Debug.Log(Distance);*/
        // Player in range
        if (Distance < minDistance && Distance > 8.5f)
        {
            canSpawn = true;
        }
        // Player out of range 
        else
        {
            canSpawn = false;
        }
    }
}
