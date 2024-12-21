using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PrefabThumbnail
{
    
    public GameObject prefab;
    public Sprite thumbnail;
}


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public TextMeshProUGUI Datetext;
    public TextMeshProUGUI timetext;
    public TextMeshProUGUI Daytext;
    private int currentMoney = 0;
    public TextMeshProUGUI moneyCounter;
    public TextMeshProUGUI itemPrice;

    public List<PrefabThumbnail> prefabThumbnails = new List<PrefabThumbnail>();

    public GameObject equipped_food;
    public Transform equipmentContainer;
    public Button next;

    // For hotbar
    public List<PrefabThumbnail> items = new List<PrefabThumbnail>(); // Array to hold items

    public Image image1;
    public Image image2;
    public Image image3;
    public Image image4;
    public Image image5;
    public Image image6;
    public Image image7;
    public Image image8;
    public Image image9;
    public Image image10;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(prefabThumbnails.Count);
        // Initialize the dictionary at runtime
        

        levelTracker();
        InvokeRepeating("UpdateDateTime", 0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectItem(0); // Key "1" corresponds to index 0
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectItem(1); // Key "2" corresponds to index 1
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectItem(2); // Key "3" corresponds to index 2
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectItem(3); // Key "4" corresponds to index 3
        if (Input.GetKeyDown(KeyCode.Alpha5)) SelectItem(4); // Key "5" corresponds to index 4
    }

    void SelectItem(int index)
    {
        //Debug.Log(index);
        /*if (items[index] != null)
        {
            equipped_food = items[index].prefab; // Set selected item
            Debug.Log("Selected item: " + equipped_food.name);
        }
        else
        {*/
            Debug.Log("Slot " + (index + 1) + " is empty.");
        ShowSelectedOption(index);
        //}
    }

    void ShowSelectedOption(int index)
    {
        UnselectAll();
        GameObject imageObject = GameObject.Find("Image" + index);
        Image image = imageObject.GetComponent<Image>();

        if (image != null)
        {
            // Get the current color
            Color color = image.color;

            // Set the alpha value
            color.a = Mathf.Clamp01(1f); // Ensure alpha is between 0 and 1

            // Apply the updated color back to the Image
            image.color = color;
        }
    }

    public void UnselectAll()
    {
        for (int i = 0; i >= items.Count; i++) // Loop through Image objects
        {
            // Dynamically find the Image by name
            GameObject imageObject = GameObject.Find("Image" + i);

            if (imageObject != null)
            {
                Image image = imageObject.GetComponent<Image>();

                if (image != null)
                {
                    // Get the current color
                    Color color = image.color;

                    // Set the alpha value
                    color.a = Mathf.Clamp01(0.2f); // Ensure alpha is between 0 and 1

                    // Apply the updated color back to the Image
                    image.color = color;
                }
                else
                {
                    Debug.LogWarning($"No Image component found on {imageObject.name}");
                }
            }
            else
            {
                Debug.LogWarning($"Image{i} not found in the scene.");
            }
        }
    }

    private void UpdateDateTime()
    {
        if (Datetext != null)
        {
            // Format the date and time for a friendly look
            Datetext.text = DateTime.Now.ToString("dddd, MMMM dd, yyyy");
            timetext.text = DateTime.Now.ToString("hh:mm:ss tt");
        }
    }
    private void levelTracker()
    {
        int sceneNumber = SceneManager.GetActiveScene().buildIndex;
        // Add a coffee-themed prefix for cuteness
        Daytext.text = $"Day {sceneNumber}";
    }

        public void nextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public bool getMoneyInt() {
        string moneyString = moneyCounter.text;
        moneyString = moneyString.Substring(0, moneyString.Length - 1);

        string itemString = itemPrice.text;
        itemString = itemString.Substring(0, itemString.Length - 1);

        //Try parsing the money string to an integer
        if (int.TryParse(moneyString, out int currentMoney))
        {
            if (int.TryParse(itemString, out int itemPrice)) {
            //Now you can check if the player has enough money
            if (currentMoney >= itemPrice)
            {
               //Deduct the money and confirm purchase
               currentMoney -= itemPrice;
               moneyCounter.text = currentMoney.ToString() + "$"; 
               Debug.Log($"Item purchased for {itemPrice} coins! Remaining money: {currentMoney}");
               return true;
            }
            else
            {
                Debug.Log("Not enough money to purchase this item!");
            }
            }
        }
        else
        {
            Debug.LogError("Failed to parse money text!");
        }
        return false;
    }
    public void ActivateEquipments(GameObject gameObject) {        
    if (getMoneyInt()) {
    string objectName = gameObject.name;
    GameObject equipment = equipmentContainer.Find(objectName)?.gameObject;
    if (equipment != null)
    {
        equipment.SetActive(true);
        Debug.Log($"{objectName} activated.");
    }
    else
    {
        Debug.LogError($"Equipment {objectName} not found in the container!");
    }
        }
    }
}
