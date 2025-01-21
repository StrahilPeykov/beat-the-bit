using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameFlowManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI cipherText;
    public TMP_InputField inputField;
    public Timer timerScript;
    public Button submitButton;
    public TextMeshProUGUI feedbackText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI shiftText;
    public PointSystem pointsys;// New TextMeshPro element for shift info

    [Header("Game Messages")]
    private string[][] cipherPools =
    {
        new string[] // Easy ciphers
        {
            "khoor", // Shift 3
            "zruog", // Shift 3
            "ifmmp", // Shift 1
        },
        new string[] // Medium ciphers
        {
            "olssv, dvysk! 123", // Shift 7, punctuation ignored
            "rovyhfyhgrlx",      // Shift 10
            "wkh txlfn eurzq ira mxpsv ryhu", // Shift 3, spaces preserved
        },
        new string[] // Hard ciphers
        {
            "ifmmpxpsme",   // Variable shift
            "vwxymnzop",    // Reverse alphabet + shift 5
            "gdkknvnqkc",   // Shift -2
        }
    };

    private string[][] decryptionPools =
    {
        new string[] // Easy decryptions
        {
            "hello",
            "world",
            "jumps"
        },
        new string[] // Medium decryptions
        {
            "hello, world! 123",
            "designfortech",
            "the quick brown fox jumps over"
        },
        new string[] // Hard decryptions
        {
            "helloworld",
            "trickypuzzle",
            "encrypted"
        }
    };

    private int[][] shiftValues =
    {
        new int[] { 3, 3, 1 }, // Shifts for Easy
        new int[] { 7, 10, 3 }, // Shifts for Medium
        new int[] { 1, 5, -2 }  // Shifts for Hard
    };

    private int currentDifficulty = 0; // 0 = Easy, 1 = Medium, 2 = Hard
    private string selectedCipher;
    private string correctDecryption;
    private int currentShift; // Stores the current shift value

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip timerTickAudio;
    public AudioClip correctAnswerAudio;
    public AudioClip wrongAnswerAudio;

    void Start()
    {
        InitializeWelcomeScreen();
    }

    void InitializeWelcomeScreen()
    {
        cipherText.gameObject.SetActive(false);
        inputField.gameObject.SetActive(false);
        submitButton.gameObject.SetActive(false);
        feedbackText.gameObject.SetActive(false);
        shiftText.gameObject.SetActive(false); // Hide shift info initially
    }

    public void StartGame()
    {
        RandomizeCipher();
        cipherText.text = selectedCipher;
        shiftText.text = $"Shift: {currentShift}"; // Display the shift value
        cipherText.gameObject.SetActive(true);
        shiftText.gameObject.SetActive(true);
        inputField.gameObject.SetActive(true);
        submitButton.gameObject.SetActive(true);
        feedbackText.gameObject.SetActive(true);
        timerScript.StartTimer(30);
        feedbackText.text = "";
    }

    void RandomizeCipher()
    {
        int randomIndex = Random.Range(0, cipherPools[currentDifficulty].Length);
        selectedCipher = cipherPools[currentDifficulty][randomIndex];
        correctDecryption = decryptionPools[currentDifficulty][randomIndex];
        currentShift = shiftValues[currentDifficulty][randomIndex];
    }

    public void CheckDecryption()
    {
        string playerGuess = inputField.text;

        if (string.IsNullOrEmpty(playerGuess))
        {
            feedbackText.text = "Please enter a guess.";
            feedbackText.color = Color.yellow;
            return;
        }

        if (playerGuess == correctDecryption)
        {
            pointsys.AddPoints(10);
            feedbackText.text = "Correct! You've decrypted the message!";
            feedbackText.color = Color.green;
            audioSource.PlayOneShot(correctAnswerAudio);

            if (currentDifficulty < 2) // If not the last difficulty
            {
                currentDifficulty++;
                StartGame(); // Move to the next difficulty
            }
            else
            {
                EndGame("Congratulations! You've completed all levels!", Color.green);
            }
        }
        else
        {
            feedbackText.text = "Incorrect. Try again!";
            feedbackText.color = Color.red;
            audioSource.PlayOneShot(wrongAnswerAudio);
            inputField.text = "";
        }
    }

    void EndGame(string endMessage, Color color)
    {
        cipherText.gameObject.SetActive(false);
        inputField.gameObject.SetActive(false);
        submitButton.gameObject.SetActive(false);
        shiftText.gameObject.SetActive(false); // Hide shift info on end

        timerScript.isTimerRunning = false;

        feedbackText.text = endMessage;
        feedbackText.color = color;
        feedbackText.gameObject.SetActive(true);
        inputField.text = "";
    }
}
