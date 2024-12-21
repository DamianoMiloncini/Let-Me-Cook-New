using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OvenUiHandle : MonoBehaviour
{

    public Canvas OvenCanvas;      
    public CanvasGroup OvenCanvasGroup;  


    public static bool isBeingUsed = false;

    // Coffee Prefabs
    public GameObject pizza;
    public GameObject burger;
    public GameObject egg;

    private GameObject equippedItem;

    // Players Transform
    public Transform rightHand;

    // Particle System
    public ParticleSystem coinsParticleSystem;

    // HUD UIs
    public Canvas OvenHUD;
    public Slider ovenProgressSlider;    
    public Image OvenHUD_Image;        
    public Sprite completedImage;

    // Machine UIs
    public TextMeshProUGUI errorText;
    public Image completed_progress_image;
    public Slider machineSlider;
    public Image machineImage;
    public Button cancelButton;
    public Button claimButton;
    public CanvasGroup progressPanel;
    public TextMeshProUGUI indicatorText;
    public CanvasGroup FoodPanel;

    // CanvasGroups
    public CanvasGroup pizzaGroup;
    public CanvasGroup burgerGroup;
    public CanvasGroup eggGroup;

    // Selected option
    private int selectedOption = 10;
    private bool optionSelected = false;
    private GameObject selectedFood;

    private Coroutine ovenCoroutine;
    private Coroutine machineCoroutine;

    private GameObject currentCanvas;

    private GameManager gameManager;

    private void Start()
    {
        // Ensure the CanvasGroup component is attached to the CoffeeCanvas GameObject
        if (OvenCanvasGroup == null)
        {
            OvenCanvasGroup = OvenCanvas.GetComponent<CanvasGroup>();
        }
    }

    public void FoodChoice(GameObject clicked_item)
    {
        UnselectAll();

        // Getting importnat information
        CanvasGroup panelCanvasGroup = clicked_item.GetComponentInParent<CanvasGroup>();
        FoodOption foodOption = clicked_item.GetComponent<FoodOption>();

        // Only select the current option
        selectedOption = foodOption.foodIndex;
        panelCanvasGroup.alpha = 1f;
        selectedFood = clicked_item;
        optionSelected = true;
    }

    public void StartFoodProcess()
    {

        if (optionSelected)
        {
            errorText.gameObject.SetActive(false);
            indicatorText.gameObject.SetActive(false);

            FoodPanel.interactable = false;
            progressPanel.alpha = 1f;
            // Get the Image component of the clicked item
            OvenHUD.gameObject.SetActive(true);
            Image clicked_image = selectedFood.GetComponent<Image>();

            if (clicked_image != null && OvenHUD_Image != null)
            {
                OvenHUD_Image.sprite = clicked_image.sprite;
                machineImage.sprite = clicked_image.sprite;
                Debug.Log("Image updated successfully.");

                // Start or restart the coffee progress coroutine
                if (ovenCoroutine != null)
                {
                    StopCoroutine(ovenCoroutine);
                }
                ovenCoroutine = StartCoroutine(AnimateFoodProgress());
                machineCoroutine = StartCoroutine(UIAnimationProgress());
            }
            else
            {
                Debug.LogWarning("Clicked item or target image is missing.");
            }
        }
        else
        {
            errorText.gameObject.SetActive(true);
        }
    }


    private IEnumerator AnimateFoodProgress()
    {
        // Make sure the slider is visible and reset its value
        ovenProgressSlider.value = 0f;
        ovenProgressSlider.gameObject.SetActive(true);



        // Set the duration of the coffee-making process
        float duration = 10f;
        float elapsed = 0f;

        // Progress the slider over time
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            ovenProgressSlider.value = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }

        // Ensure slider reaches 100% at the end
        ovenProgressSlider.value = 1f;
        Debug.Log("Food Process Complete");

        //CoffeeHUD_Image.sprite = completedProgressSprite;

        // Hide the progress slider and deactivate the canvas after the process completes
        ovenProgressSlider.gameObject.SetActive(false);
        OvenHUD_Image.sprite = completedImage;
        //CoffeeCanvas.gameObject.SetActive(false);  // Deactivate the canvas fully
    }

    private IEnumerator UIAnimationProgress()
    {
        // Make sure the slider is visible and reset its value
        machineSlider.value = 0f;
        machineSlider.gameObject.SetActive(true);



        // Set the duration of the coffee-making process
        float duration = 10f;
        float elapsed = 0f;

        // Progress the slider over time
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            machineSlider.value = Mathf.Clamp01(elapsed / duration);
            cancelButton.gameObject.SetActive(true);
            yield return null;
        }

        // Ensure slider reaches 100% at the end
        machineSlider.value = 1f;

        cancelButton.gameObject.SetActive(false);
        claimButton.gameObject.SetActive(true);
    }

    public void CloseMenu()
    {
        // Set the CanvasGroup alpha to 0 to make it invisible
        OvenCanvasGroup.alpha = 0f;
        OvenCanvasGroup.interactable = false;
        OvenCanvasGroup.blocksRaycasts = false;
        //CoffeeCanvas.gameObject.SetActive(false);
    }

    private void UnselectAll()
    {
        pizzaGroup.alpha = 0.2f;
        burgerGroup.alpha = 0.2f;
        eggGroup.alpha = 0.2f;
    }

    public void CancelProcess()
    {
        StopCoroutine(ovenCoroutine);
        StopCoroutine(machineCoroutine);
        UnselectAll();
        optionSelected = false;
        selectedFood = null;
        selectedOption = 10;
        progressPanel.alpha = 0f;
        OvenHUD.gameObject.SetActive(false);
        indicatorText.gameObject.SetActive(true);
        FoodPanel.interactable = true;
    }

    public void EquipItem(GameObject itemPrefab)
    {
        // Instantiate and parent the item to the right hand slot
        equippedItem = Instantiate(itemPrefab, rightHand.position, rightHand.rotation);
        equippedItem.transform.SetParent(rightHand);
        equippedItem.transform.localRotation = Quaternion.Euler(180f, 0f, 0f);

    }

    public void ClaimFood()
    {
        Debug.Log("Food Claimed!");
        Debug.Log(selectedOption);
        GameObject food = CreatedFood(selectedOption);
        Debug.Log(food);
        claimButton.gameObject.SetActive(false);
        EquipItem(food);
        GameManager.Instance.equipped_food = food;
        CancelProcess();
        CloseMenu();
        //ShowParticleEffect();
    }

    public GameObject CreatedFood(int option)
    {
        switch (option)
        {
            case 0:
                return pizza;
                break;
            case 1:
                return burger;
                break;
            case 2:
                return egg;
                break;
            default:
                return null;
                break;
        }
    }

}
