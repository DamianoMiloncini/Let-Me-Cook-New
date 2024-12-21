using UnityEngine;
using System.Collections.Generic;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab;
    public Transform spawnPoint;
    public List<Transform> counterLines;
    public Transform exitPoint;
    public Transform[] walkAroundPoints;

    private float spawnInterval = 8f;
    private int maxNPCs = 2;

    private List<GameObject> activeNPCs = new List<GameObject>(); 
    private float spawnTimer;
    private int currentCounterIndex = 0; 

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval && activeNPCs.Count < maxNPCs)
        {
            
            SpawnNPC();
            spawnTimer = 0f; 
        }
    }

    void SpawnNPC()
    {
        GameObject npc = Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);
        NPCBehaviour npcBehaviour = npc.GetComponent<NPCBehaviour>();

        if (npcBehaviour != null)
        {
            Transform targetLine = counterLines[currentCounterIndex];

            currentCounterIndex = (currentCounterIndex + 1) % counterLines.Count;

            npcBehaviour.Initialize(npc, walkAroundPoints, new Transform[] { targetLine }, exitPoint, OnNPCDespawn);

            activeNPCs.Add(npc);

        }
    }


    void OnNPCDespawn(GameObject npc)
    {
        if (activeNPCs.Contains(npc))
        {
            activeNPCs.Remove(npc);

            Destroy(npc);

        }
       
    }
}
