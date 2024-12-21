using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;


public class NPCBehaviour : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform[] walkAroundPoints;
    public Transform counterPoint;
    public Transform[] counterPoints;
    Animator animator;
    public Transform exitPoint;
    public int walkPointIndex = 0;
    public bool atCounter = false;
    public bool hasFood = false;
    
    public GameObject npcObject;
    public System.Action<GameObject> onDespawn;

    public Image image1;
    public Image image2;

    public Image check1;
    public Image check2;
    public Image x1;
    public Image x2;

    private GameManager gameManager;
    private PrefabThumbnail customerOrder;

    private int completed_orders = 0;

    private List<PrefabThumbnail> customer_items = new List<PrefabThumbnail>();
    private bool order1_completed = false;
    private bool order2_completed = false;

    public CanvasGroup canvasGroup; // Assign your UI element's CanvasGroup in the Inspector.
    public float fadeDuration = 5f;

    private int orders_given = 0;

    public void Initialize(GameObject npc, Transform[] walkPoints, Transform[] counters, Transform exit, System.Action<GameObject> despawnCallback)
    {
        npcObject = npc;  
        agent = npcObject.GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        walkAroundPoints = walkPoints;
        counterPoints = counters;
        counterPoint = SelectCounterLine(); 
        exitPoint = exit;
        onDespawn = despawnCallback;

        walkPointIndex = 0;
        MoveToNextWalkPoint();
    }

    void Start()
    {
        //image1.sprite = gameManager.prefabThumbnails[0].thumbnail;
        //image2.sprite = gameManager.prefabThumbnails[0].thumbnail;

        FoodSelector();

        int length = GameManager.Instance.prefabThumbnails.Count;
       // Debug.Log("Length of prefabThumbnails: " + length);

        agent = npcObject.GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            UnityEngine.Debug.LogError("Nnot found.");
        }

        if (walkAroundPoints == null || walkAroundPoints.Length == 0)
        {
            UnityEngine.Debug.LogError("pts are not set or empty.");
        }
    }

    void Update()
    {
        // Update Animator's Speed parameter based on movement
        float speed = agent.velocity.magnitude / agent.speed; // Normalize to 0-1 range
        animator.SetFloat("Speed", speed);

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && agent.hasPath)
        {
            if (!atCounter)
            {
                if (walkPointIndex < walkAroundPoints.Length)
                {
                    MoveToNextWalkPoint();
                }
                else if (!hasFood)
                {
                    GoToCounter();
                }
            }
        }

        
    }

    void GoToCounter()
    {
        UnityEngine.Debug.Log(counterPoint);

        atCounter = true; // Set the script's variable
        animator.SetBool("atCounter", true); // Update the Animator's parameter

        agent.SetDestination(counterPoint.position);
    }

    void MoveToNextWalkPoint()
    {
        if (walkPointIndex < walkAroundPoints.Length)
        {
            // Move to the next walk point
            atCounter = false; // Reset the state
            animator.SetBool("atCounter", false);

            agent.SetDestination(walkAroundPoints[walkPointIndex].position);
            walkPointIndex++;
        }
        else
        {
            GoToCounter();
        }

        
    }


    void MoveToExit()
    {
        walkPointIndex = 3;
        if (walkPointIndex > walkAroundPoints.Length)
        {
            agent.SetDestination(walkAroundPoints[walkPointIndex].position);
            walkPointIndex--;
        }
        else
        {
            agent.SetDestination(exitPoint.position);
            StartCoroutine(DespawnAfterReachingExit());
        }
    }

    IEnumerator DespawnAfterReachingExit()
    {
        // Wait until the NPC reaches the exit
        while (!agent.pathPending && agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null; // Wait for the next frame
        }

        // Trigger despawn logic
        if (onDespawn != null)
        {
            onDespawn(npcObject);
        }

        // Destroy the NPC object
        Destroy(npcObject);
    }



    Transform SelectCounterLine()
    {
       
            int randomIndex = UnityEngine.Random.Range(0, counterPoints.Length);
            return counterPoints[randomIndex];
        
        
    }

    void FoodSelector()
    {
        int length = GameManager.Instance.prefabThumbnails.Count;
        //Debug.Log(length);

        int index1 = UnityEngine.Random.Range(0, length);
        int index2 = UnityEngine.Random.Range(0, length);

        while (index2 == index1)
        {
            index2 = UnityEngine.Random.Range(0, length);
        }

        

        PrefabThumbnail order1 = GameManager.Instance.prefabThumbnails[index1];
        PrefabThumbnail order2 = GameManager.Instance.prefabThumbnails[index2];

        customer_items.Add(order1);
        customer_items.Add(order2);

        //Debug.Log("Order 1: " + order1);

        image1.sprite = order1.thumbnail;
        image2.sprite = order2.thumbnail;

    }

    private void OnMouseDown()
    {
        bool flag = false;

        if (GameManager.Instance.equipped_food != null)
        {
            for (int i = 0; i < customer_items.Count; i++)
            {
                if (customer_items[i].prefab == GameManager.Instance.equipped_food)
                {
                    flag = true;
                    //Debug.Log("Equipped item is in the list!");

                    switch (i)
                    {
                        case 0:
                            if (!order1_completed)
                            {
                                check1.gameObject.SetActive(true);
                                completed_orders++;
                                order1_completed = true;
                            }
                            break;

                        case 1:
                            if (!order2_completed)
                            {
                                check2.gameObject.SetActive(true);
                                completed_orders++;
                                order2_completed = true;
                            }
                            break;
                    }
                    break;
                }
            }

            if (!flag)
            {
                completed_orders++;
                switch (completed_orders)
                {
                    case 1: x1.gameObject.SetActive(true);break;
                    case 2: x2.gameObject.SetActive(true); break;
                }
                

                // Set NPC to leave the restaurant after incorrect order
                //StartLeavingRestaurant();
            }

            

            if (completed_orders == 2)
            {
                if (x1.gameObject.activeSelf || x2.gameObject.activeSelf)
                {
                   // Debug.Log("Order failed.");
                    StartCoroutine(FadeOutRoutine());
                    
                    
                    return;
                }
                //Debug.Log("Order done!");
                GameManager.Instance.player_money = GameManager.Instance.player_money + 25;
                StartCoroutine(FadeOutRoutine());

                // Set NPC to leave the restaurant
                
            }
        }
    }


    IEnumerator FadeOutRoutine()
    {

        float startAlpha = canvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, elapsedTime / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0; // Ensure it's fully transparent.
        canvasGroup.interactable = false; // Disable interaction.
        canvasGroup.blocksRaycasts = false; // Disable blocking raycasts.
        StartLeavingRestaurant();
    }

    void StartLeavingRestaurant()
    {
        atCounter = false; // Reset the state
        animator.SetBool("atCounter", false);

        if (walkAroundPoints != null && walkAroundPoints.Length > 0)
        {
            agent.SetDestination(walkAroundPoints[1].position);
            agent.SetDestination(walkAroundPoints[0].position);
            agent.SetDestination(exitPoint.position);
            StartCoroutine(DespawnAfterReachingExit());
        }
        else
        {
            agent.SetDestination(walkAroundPoints[1].position);
            agent.SetDestination(walkAroundPoints[0].position);
            agent.SetDestination(exitPoint.position);
            StartCoroutine(DespawnAfterReachingExit());
        }
    }


}

