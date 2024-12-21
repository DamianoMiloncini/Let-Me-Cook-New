using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class EndLevelUI : MonoBehaviour
{
 
    public TMP_Text timeRecordedText;
    public TMP_Text customersServedText;
    public TMP_Text profitText;
    public TMP_Text currLevel;

    public static float timeRecorded;
    public static int customersServed;
    public static int profit;
    public static int currentLevel;

    void Start()
    {
        timeRecordedText.text = $"Time Recorded: {timeRecorded:F2}";
        customersServedText.text = $"Customers Served: {customersServed}";
        profitText.text = $"Profit: ${profit}";
        currLevel.text = $"Level {currentLevel}";
    }

    public static void SetEndLevelData(float time, int customers, int profits, int level)
    {
        timeRecorded = time;
        customersServed = customers;
        profit = profits;
        currentLevel = level;
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
