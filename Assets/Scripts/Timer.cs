using UnityEngine;

public class Timer : MonoBehaviour
{
    public float startTime = 60f; // Starting time in seconds
    public bool countDown = true; // True = counts down, False = counts up
    public Transform clockHand; // Reference to the clock hand

    private float currentTime;
    private bool isRunning = false;

    void Start()
    {
        ResetTimer();
        StartTimer();
    }

    void Update()
    {
        if (isRunning)
        {
            currentTime += countDown ? -Time.deltaTime : Time.deltaTime;

            if (countDown && currentTime <= 0)
            {
                currentTime = 0;
                isRunning = false;
                OnTimerEnd();
            }

            RotateClockHand();
        }
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        currentTime = countDown ? startTime : 0;
        RotateClockHand();
    }

    private void RotateClockHand()
    {
        if (clockHand != null)
        {
            float normalizedTime = countDown ? currentTime / startTime : currentTime / startTime;
            float rotationAngle = Mathf.Lerp(0, -360f, normalizedTime);
            clockHand.localRotation = Quaternion.Euler(0, 0, rotationAngle);
        }
    }

    private void OnTimerEnd()
    {
        Debug.Log("Timer Ended!");
        // Add event logic here if needed
    }
}
