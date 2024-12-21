using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public Toggle muteToggle;
    public Image toggleImage;
    public Sprite firstImage;    
    public Sprite secondImage;  

    public void ToggleMute()
    {
        if (muteToggle.isOn)
        {
            AudioListener.volume = 0;
            toggleImage.sprite = secondImage;   
        }
        else
        {
            AudioListener.volume = 1;
            toggleImage.sprite = firstImage;   
        }
    }
}
