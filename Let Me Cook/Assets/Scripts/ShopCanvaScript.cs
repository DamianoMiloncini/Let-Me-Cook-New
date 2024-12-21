using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class ShopCanvaScript : MonoBehaviour
{
    public Canvas ShopCanva;
    //public GridLayoutGroup groupItems;

    private GameObject itemClicked;

    //image 
    private Image itemClicked_image;
    //description
    private TextMeshProUGUI itemClicked_description;
    private TextMeshProUGUI itemClicked_price;
    public Button buyButton;
    private int currentMoney = 0;
    public GameManager gameManager;

    //private Transform itemImageTransform;
    // Start is called before the first frame update
    void Start()
    {
    if (gameManager == null)
    {
        gameManager = GameManager.Instance; // Access the singleton instance
    }
        Transform informationView_panel = transform.Find("InformationView");

        itemClicked = informationView_panel.gameObject;

        itemClicked_description = informationView_panel.Find("DescriptionText").GetComponent<TextMeshProUGUI>();
        itemClicked_price = informationView_panel.Find("price").GetComponent<TextMeshProUGUI>();

        itemClicked_image = informationView_panel.Find("ItemImage").GetComponent<Image>();
        
        //getting the image
        //itemClicked = panel.Find("ItemImage").GetComponent<GameObject>();
       // itemImageTransform = panel.Find("ItemImage").GetComponent<Transform>();
        //debug type shi
        // if(itemClicked_description) {
        //     Debug.Log("true");
        //     Debug.Log(itemClicked_description.text);
        // }
        // else {
        //     Debug.Log("false");
        // }

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void onShopClickDeactivate() {
       //open the shop canva
       ShopCanva.gameObject.SetActive(false);
    }

    //handle when the user clicks on an item in the grid layout
    public void onItemClick(GameObject clicked_item) {
        //display the image on the InformationView panel
        //Debug.Log(clicked_item.name);
        //find the image
        Image clicked_image = clicked_item.GetComponent<Image>();
        //find the description
        TextMeshProUGUI description = clicked_item.transform.Find("textDescription").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI price = clicked_item.transform.Find("price").GetComponent<TextMeshProUGUI>();
        itemClicked_image.sprite = clicked_image.sprite;
        itemClicked_description.text = description.text;
        itemClicked_price.text = price.text;
        buyButton.onClick.RemoveAllListeners(); // Clear old listeners
        buyButton.onClick.AddListener(() => purchaseItem(clicked_item));
        //Debug.Log("OnItemClick" + clicked_item);
    }

    public void purchaseItem(GameObject objectName) {  
     gameManager.ActivateEquipments(objectName);
    }
}
