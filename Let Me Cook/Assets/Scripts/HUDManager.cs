using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HUDManager : MonoBehaviour
{
    public Canvas HUDCanva;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onShopClickActive() {
       //open the shop canva
       HUDCanva.gameObject.SetActive(true);
    }

}
