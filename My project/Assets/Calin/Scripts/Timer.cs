// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine.Events;
// using TMPro;

// public class Timer : MonoBehaviour
// {
//     [SerializeField] private TextMeshProUGUI timerText;
//     [SerializeField] private float remainingTime;
//     [SerializeField] private UnityEvent onTimerEnd;
//     [SerializeField] private AudioSource tickSound; // Reference to an AudioSource for the tick sound

//     private bool isTimerRunning = false;
//     private float tickTimer = 1f; // Tracks time between ticks

//     // Update is called once per frame
//     public void Update()
//     {
//         if (!isTimerRunning) return;

//         if (remainingTime > 0)
//         {
//             remainingTime -= Time.deltaTime;

//             tickTimer -= Time.deltaTime;
//             if (tickTimer <= 0f)
//             {
//                 PlayTickSound();
//                 tickTimer = 1f; // Reset tick timer for the next second
//             }

//             UpdateTimerDisplay();
//         }
//         else
//         {
//             // Ensure the timer doesn't go below 0
//             remainingTime = 0;
//             isTimerRunning = false;

//             // Update the display to show "00:00" explicitly
//             UpdateTimerDisplay();

//             // Invoke the event when the timer ends
//             onTimerEnd?.Invoke();
//         }

//     }

//     // Public method to start the timer
//     public void StartTimer(float duration)
//     {
//         remainingTime = duration;
//         isTimerRunning = true;
//         tickTimer = 1f; // Reset tick timer
//     }

//     // Play the tick sound
//     private void PlayTickSound()
//     {
//         if (tickSound != null)
//         {
//             tickSound.Play();
//         }
//         else
//         {
//             // Debug.LogWarning("Tick sound is not assigned!");
//         }
//     }

//     // Helper method to update the timer display
//     private void UpdateTimerDisplay()
//     {
//         if (remainingTime > 10)
//         {
//             timerText.color = Color.white;
//         }
//         else if (remainingTime > 5)
//         {
//             timerText.color = Color.yellow;
//         }
//         else
//         {
//             timerText.color = Color.red;
//         }

//         int minutes = Mathf.FloorToInt(remainingTime / 60);
//         int seconds = Mathf.FloorToInt(remainingTime % 60);
//         timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
//     }
// }