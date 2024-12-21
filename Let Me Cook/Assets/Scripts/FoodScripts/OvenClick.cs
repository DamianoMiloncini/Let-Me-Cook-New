using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OvenClick : MonoBehaviour
{
    public Canvas OvenCanvas;         
    private CanvasGroup ovenCanvasGroup;

    private void Start()
    {

        ovenCanvasGroup = OvenCanvas.GetComponent<CanvasGroup>();

 
        if (ovenCanvasGroup == null)
        {
            ovenCanvasGroup = OvenCanvas.gameObject.AddComponent<CanvasGroup>();
        }
    }

    private void OnMouseDown()
    {
        // Make the canvas visible and interactable
        ovenCanvasGroup.alpha = 1f;
        ovenCanvasGroup.interactable = true;
        ovenCanvasGroup.blocksRaycasts = true;
    }


}
