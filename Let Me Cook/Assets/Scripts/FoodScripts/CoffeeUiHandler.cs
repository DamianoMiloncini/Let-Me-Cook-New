using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoffeeUiHandler : MonoBehaviour
{
    
    public Canvas CoffeeCanvas;            // Canvas with the UI
    public CanvasGroup CoffeeCanvasGroup;  // CanvasGroup to control transparency


    public static bool isBeingUsed = false;

    // Coffee Prefabs
    public GameObject RegularCoffee;
    public GameObject Espresso;
    public GameObject Cappucino;

    private GameObject equippedItem;

    // Players Transform
    public Transform rightHand;

    // Particle System
    public ParticleSystem coinsParticleSystem;

    // HUD UIs
    public Canvas CoffeeHUD;
    public Slider coffeeProgressSlider;    // Progress slider over the coffee machine
    public Image CoffeeHUD_Image;          // Image in the UI
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
    public CanvasGroup DrinkPanel;

    // CanvasGroups
    public CanvasGroup regularCoffeeGroup;
    public CanvasGroup espressoCoffeeGroup;
    public CanvasGroup cappucinoCoffeeGroup;

    // Selected option
    private int selectedOption = 10;
    private bool optionSelected = false;
    private GameObject selectedCoffee;

    private Coroutine coffeeCoroutine;
    private Coroutine machineCoroutine;

    private GameObject currentCanvas;

    private GameManager gameManager;

    private void Start()
    {
        // Ensure the CanvasGroup component is attached to the CoffeeCanvas GameObject
        if (CoffeeCanvasGroup == null)
        {
            CoffeeCanvasGroup = CoffeeCanvas.GetComponent<CanvasGroup>();
        }
    }

    public void CoffeeChoice(GameObject clicked_item)
    {
        UnselectAll();

        // Getting importnat information
        CanvasGroup panelCanvasGroup = clicked_item.GetComponentInParent<CanvasGroup>();
        CoffeeOption coffeeOption = clicked_item.GetComponent<CoffeeOption>();

        // Only select the current option
        selectedOption = coffeeOption.coffeeIndex;
        panelCanvasGroup.alpha = 1f;
        selectedCoffee = clicked_item;
        optionSelected = true;
    }

    public void StartCoffeeProcess()
    {

        if (optionSelected)
        {
            errorText.gameObject.SetActive(false);
            indicatorText.gameObject.SetActive(false);

            DrinkPanel.interactable = false;
            progressPanel.alpha = 1f;
            // Get the Image component of the clicked item
            CoffeeHUD.gameObject.SetActive(true);
            Image clicked_image = selectedCoffee.GetComponent<Image>();

            if (clicked_image != null && CoffeeHUD_Image != null)
            {
                CoffeeHUD_Image.sprite = clicked_image.sprite;
                machineImage.sprite = clicked_image.sprite;
                Debug.Log("Image updated successfully.");

                // Start or restart the coffee progress coroutine
                if (coffeeCoroutine != null)
                {
                    StopCoroutine(coffeeCoroutine);
                }
                coffeeCoroutine = StartCoroutine(AnimateCoffeeProgress());
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

    private void machineHUD()
    {

    }

    private IEnumerator AnimateCoffeeProgress()
    {
        // Make sure the slider is visible and reset its value
        coffeeProgressSlider.value = 0f;
        coffeeProgressSlider.gameObject.SetActive(true);



        // Set the duration of the coffee-making process
        float duration = 10f;
        float elapsed = 0f;

        // Progress the slider over time
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            coffeeProgressSlider.value = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }

        // Ensure slider reaches 100% at the end
        coffeeProgressSlider.value = 1f;
        Debug.Log("Coffee Process Complete");

        //CoffeeHUD_Image.sprite = completedProgressSprite;

        // Hide the progress slider and deactivate the canvas after the process completes
        coffeeProgressSlider.gameObject.SetActive(false);
        CoffeeHUD_Image.sprite = completedImage;
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
        CoffeeCanvasGroup.alpha = 0f;
        CoffeeCanvasGroup.interactable = false;
        CoffeeCanvasGroup.blocksRaycasts = false;
        //CoffeeCanvas.gameObject.SetActive(false);
    }

    private void UnselectAll()
    {
        regularCoffeeGroup.alpha = 0.2f;
        espressoCoffeeGroup.alpha = 0.2f;
        cappucinoCoffeeGroup.alpha = 0.2f;
    }

    public void CancelProcess()
    {
        StopCoroutine(coffeeCoroutine);
        StopCoroutine(machineCoroutine);
        UnselectAll();
        optionSelected = false;
        selectedCoffee = null;
        selectedOption = 10;
        progressPanel.alpha = 0f;
        CoffeeHUD.gameObject.SetActive(false);
        indicatorText.gameObject.SetActive(true);
        DrinkPanel.interactable = true;
    }

    public void EquipItem(GameObject itemPrefab)
    {
        // Instantiate and parent the item to the right hand slot
        equippedItem = Instantiate(itemPrefab, rightHand.position, rightHand.rotation);
        equippedItem.transform.SetParent(rightHand);
        equippedItem.transform.localRotation = Quaternion.Euler(180f, 0f, 0f);
        
    }

    public void ClaimCoffee()
    {
        Debug.Log("Coffee Claimed!");
        Debug.Log(selectedOption);
        GameObject coffee = CreatedCoffee(selectedOption);
        Debug.Log(coffee);
        claimButton.gameObject.SetActive(false);
        EquipItem(coffee);
        GameManager.Instance.equipped_food = coffee;
        CancelProcess();
        CloseMenu();
        ShowParticleEffect();
    }

    public GameObject CreatedCoffee(int option)
    {
        switch (option)
        {
            case 0:
                return RegularCoffee;
                break;
            case 1:
                return Espresso;
                break;
            case 2:
                return Cappucino;
                break;
            default:
                return null;
                break;
        }
    }

    public void ShowParticleEffect()
    {
        // Position the particle system in the middle of the screen
        //Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
        //Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenCenter.x, screenCenter.y, 5f)); // Use a fixed Z-depth

        //coinsParticleSystem.transform.position = worldPosition;
        //coinsParticleSystem.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward); // Face the camera

        // Play the particle system
        //coinsParticleSystem.Play();
    }

    private void RefreshCanvas()
    {

    }
}
