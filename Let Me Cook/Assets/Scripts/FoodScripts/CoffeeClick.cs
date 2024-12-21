using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CoffeeClick : MonoBehaviour
{
    public Canvas CoffeeCanvas;            // Reference to the Canvas GameObject
    private CanvasGroup coffeeCanvasGroup; // Reference to the CanvasGroup component

    private void Start()
    {
        // Get the CanvasGroup component attached to the CoffeeCanvas
        coffeeCanvasGroup = CoffeeCanvas.GetComponent<CanvasGroup>();
        
        // If there is no CanvasGroup, add one dynamically
        if (coffeeCanvasGroup == null)
        {
            coffeeCanvasGroup = CoffeeCanvas.gameObject.AddComponent<CanvasGroup>();
        }
    }

    private void OnMouseDown()
    {
        // Make the canvas visible and interactable
        coffeeCanvasGroup.alpha = 1f;
        coffeeCanvasGroup.interactable = true;
        coffeeCanvasGroup.blocksRaycasts = true;
    }


}
