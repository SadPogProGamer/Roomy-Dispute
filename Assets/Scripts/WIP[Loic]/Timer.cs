
using TMPro; // Required for TextMeshPro
using UnityEngine;

public class Timer : MonoBehaviour
{
    private float StartRoundTime = 61f;
    [SerializeField]
    private float RoundTime = 61f;

    private int Round = 1;

    [SerializeField]
    private TextMeshProUGUI timerText; // Link this in the Inspector

    void Update()
    {
        RoundTime -= Time.deltaTime;

        if (RoundTime <= 0.0f && Round < 4)
        {
            RoundTime = StartRoundTime - 10f * Round;
            Round++;
            //player gains money
        }
        else if (RoundTime <= 0.0f && Round >= 4 && Round != 9)
        {
            RoundTime = StartRoundTime - 30f -5f * (Round-3);
            Round++;
            //player gains money
        }
        else if (RoundTime <= 0.0f && Round == 9)
        {
            TimerEnd();
            //game is over
        }

        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(RoundTime / 60);
        int seconds = Mathf.FloorToInt(RoundTime % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void TimerEnd()
    {
        timerText.text = "Timer Ended";
        //implement podium feature/ winning thingy idk
    }
}
