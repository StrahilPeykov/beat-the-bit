using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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

    public static string Encrypt(string input, int shift)
    {
        string result = string.Empty;

        // Loop through each character in the input string
        foreach (char c in input)
        {
            // If the character is an uppercase letter
            if (char.IsUpper(c))
            {
                // Shift the character and wrap around if necessary
                char encryptedChar = (char)(((c - 'A' + shift) % 26 + 26) % 26 + 'A');
                result += encryptedChar;
            }
            // If the character is a lowercase letter
            else if (char.IsLower(c))
            {
                // Shift the character and wrap around if necessary
                char encryptedChar = (char)(((c - 'a' + shift) % 26 + 26) % 26 + 'a');
                result += encryptedChar;
            }
            else
            {
                // If it's not a letter, leave it unchanged
                result += c;
            }
        }

        return result;
    }

    [Header("Game Messages")]

    (string, string, int)[][] cipherPools = new (string, string, int)[][]
        {
        // Easy Ciphers
        new (string, string, int)[]
        {
            ("tue", Encrypt("tue", 3), 3),
            ("code", Encrypt("code", 1), 1),
            ("data", Encrypt("data", 2), 2),
            ("java", Encrypt("java", 2), 2),
            ("node", Encrypt("node", 1), 1),
            ("byte", Encrypt("byte", 2), 2),
            ("algo", Encrypt("algo", 1), 1),
        },

        // Medium Ciphers
        new (string, string, int)[]
        {
            ("linux", Encrypt("linux", 8), 8),
            ("cloud", Encrypt("cloud", 7), 7),
            ("python", Encrypt("python", 6), 6),
            ("logic", Encrypt("logic", 9), 9),
            ("queue", Encrypt("queue", 10), 10),
            ("stack", Encrypt("stack", 8), 8),
            ("binary", Encrypt("binary", 7), 7),
            ("token", Encrypt("token", 9), 9)
        },

        // Hard Ciphers
        new (string, string, int)[]
        {
            ("algorithm", Encrypt("algorithm", 27), 27),
            ("database", Encrypt("database", 28), 28),
            ("encryption", Encrypt("encryption", 29), 29),
            ("hardware", Encrypt("hardware", 53), 53),
            ("programming", Encrypt("programming", 54), 54),
            ("interface", Encrypt("interface", 55), 55),
            ("compiler", Encrypt("compiler", 56), 56),
            ("algorithmic", Encrypt("algorithmic", 27), 27),
            ("processor", Encrypt("processor", 28), 28),
            ("networking", Encrypt("networking", 29), 29)
        }
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
        cipherText.text = "> Encrypted password: " + selectedCipher;
        shiftText.text = $"> Shift: {currentShift}"; // Display the shift value
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
        int randomIndex = UnityEngine.Random.Range(0, cipherPools[currentDifficulty].Length);
        selectedCipher = cipherPools[currentDifficulty][randomIndex].Item2;
        correctDecryption = cipherPools[currentDifficulty][randomIndex].Item1;
        Debug.Log(correctDecryption);
        currentShift = cipherPools[currentDifficulty][randomIndex].Item3;
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
            inputField.text = "";
            feedbackText.text = "Nice! Just a couple of more firewalls to break!";
            // feedbackText.color = Color.green;
            audioSource.PlayOneShot(correctAnswerAudio);

            if (currentDifficulty < 2) // If not the last difficulty
            {
                currentDifficulty++;
                StartGame(); // Move to the next difficulty
            }
            else
            {
                EndGame("We're in! We hacked their network! Congrats!", Color.green);
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
// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;

// public class GameFlowManager : MonoBehaviour
// {
//     [Header("UI Elements")]
//     public TextMeshProUGUI cipherText;
//     public TMP_InputField inputField;
//     public Timer timerScript;
//     public Button submitButton;
//     public TextMeshProUGUI feedbackText;
//     public TextMeshProUGUI timerText;
//     public TextMeshProUGUI shiftText;
//     public PointSystem pointsys;// New TextMeshPro element for shift info

//     public static string Encrypt(string input, int shift)
//     {
//         string result = string.Empty;

//         // Loop through each character in the input string
//         foreach (char c in input)
//         {
//             // If the character is an uppercase letter
//             if (char.IsUpper(c))
//             {
//                 // Shift the character and wrap around if necessary
//                 char encryptedChar = (char)(((c - 'A' + shift) % 26 + 26) % 26 + 'A');
//                 result += encryptedChar;
//             }
//             // If the character is a lowercase letter
//             else if (char.IsLower(c))
//             {
//                 // Shift the character and wrap around if necessary
//                 char encryptedChar = (char)(((c - 'a' + shift) % 26 + 26) % 26 + 'a');
//                 result += encryptedChar;
//             }
//             else
//             {
//                 // If it's not a letter, leave it unchanged
//                 result += c;
//             }
//         }

//         return result;
//     }

//     [Header("Game Messages")]

//     (string, string, int)[] csTerms = new (string, string, int)[] {
//             ("tue", Encrypt("tue", 3), 3),
//             ("code", Encrypt("code", 5), 5),
//             ("data", Encrypt("data", 2), 2),
//             ("java", Encrypt("java", 4), 4),
//             ("node", Encrypt("node", 6), 6),
//             ("byte", Encrypt("byte", 7), 7),
//             ("algo", Encrypt("algo", 1), 1),
//             ("linux", Encrypt("linux", 8), 8),
//             ("cloud", Encrypt("cloud", 3), 3),
//             ("python", Encrypt("python", 5), 5),
//             ("hash", Encrypt("hash", 2), 2),
//             ("bit", Encrypt("bit", 9), 9),
//             ("debug", Encrypt("debug", 4), 4),
//             ("debugging", Encrypt("debugging", 3), 3),
//             ("logic", Encrypt("logic", 6), 6),
//             ("queue", Encrypt("queue", 2), 2),
//             ("stack", Encrypt("stack", 7), 7),
//             ("binary", Encrypt("binary", 5), 5),
//             ("token", Encrypt("token", 3), 3)
//         };
//     private string[][] cipherPools =
//     {


//         new string[] // Easy ciphers
//         {
//             "khoor", // Shift 3
//             "zruog", // Shift 3
//             "ifmmp", // Shift 1
//         },
//         new string[] // Medium ciphers
//         {
//             "olssv, dvysk! 123", // Shift 7, punctuation ignored
//             "rovyhfyhgrlx",      // Shift 10
//             "wkh txlfn eurzq ira mxpsv ryhu", // Shift 3, spaces preserved
//         },
//         new string[] // Hard ciphers
//         {
//             "ifmmpxpsme",   // Variable shift
//             "vwxymnzop",    // Reverse alphabet + shift 5
//             "gdkknvnqkc",   // Shift -2
//         }
//     };

//     private string[][] decryptionPools =
//     {
//         new string[] // Easy decryptions
//         {
//             "hello",
//             "world",
//             "jumps"
//         },
//         new string[] // Medium decryptions
//         {
//             "hello, world! 123",
//             "designfortech",
//             "the quick brown fox jumps over"
//         },
//         new string[] // Hard decryptions
//         {
//             "helloworld",
//             "trickypuzzle",
//             "encrypted"
//         }
//     };

//     private int[][] shiftValues =
//     {
//         new int[] { 3, 3, 1 }, // Shifts for Easy
//         new int[] { 7, 10, 3 }, // Shifts for Medium
//         new int[] { 1, 5, -2 }  // Shifts for Hard
//     };

//     private int currentDifficulty = 0; // 0 = Easy, 1 = Medium, 2 = Hard
//     private string selectedCipher;
//     private string correctDecryption;
//     private int currentShift; // Stores the current shift value

//     [Header("Audio Settings")]
//     public AudioSource audioSource;
//     public AudioClip timerTickAudio;
//     public AudioClip correctAnswerAudio;
//     public AudioClip wrongAnswerAudio;

//     void Start()
//     {
//         InitializeWelcomeScreen();
//     }

//     void InitializeWelcomeScreen()
//     {
//         cipherText.gameObject.SetActive(false);
//         inputField.gameObject.SetActive(false);
//         submitButton.gameObject.SetActive(false);
//         feedbackText.gameObject.SetActive(false);
//         shiftText.gameObject.SetActive(false); // Hide shift info initially
//     }

//     public void StartGame()
//     {
//         RandomizeCipher();
//         cipherText.text = selectedCipher;
//         shiftText.text = $"> Shift: {currentShift}"; // Display the shift value
//         cipherText.gameObject.SetActive(true);
//         shiftText.gameObject.SetActive(true);
//         inputField.gameObject.SetActive(true);
//         submitButton.gameObject.SetActive(true);
//         feedbackText.gameObject.SetActive(true);
//         timerScript.StartTimer(30);
//         feedbackText.text = "";
//     }

//     void RandomizeCipher()
//     {
//         int randomIndex = Random.Range(0, cipherPools[currentDifficulty].Length);
//         selectedCipher = cipherPools[currentDifficulty][randomIndex];
//         correctDecryption = decryptionPools[currentDifficulty][randomIndex];
//         currentShift = shiftValues[currentDifficulty][randomIndex];
//     }

//     public void CheckDecryption()
//     {
//         string playerGuess = inputField.text;

//         if (string.IsNullOrEmpty(playerGuess))
//         {
//             feedbackText.text = "Please enter a guess.";
//             feedbackText.color = Color.yellow;
//             return;
//         }

//         if (playerGuess == correctDecryption)
//         {
//             pointsys.AddPoints(10);
//             feedbackText.text = "Nice! Just a couple of more firewalls to break!";
//             feedbackText.color = Color.green;
//             audioSource.PlayOneShot(correctAnswerAudio);

//             if (currentDifficulty < 2) // If not the last difficulty
//             {
//                 currentDifficulty++;
//                 StartGame(); // Move to the next difficulty
//             }
//             else
//             {
//                 EndGame("We're in! We hacked their network! Congrats!", Color.green);
//             }
//         }
//         else
//         {
//             feedbackText.text = "Incorrect. Try again!";
//             feedbackText.color = Color.red;
//             audioSource.PlayOneShot(wrongAnswerAudio);
//             inputField.text = "";
//         }
//     }

//     void EndGame(string endMessage, Color color)
//     {
//         cipherText.gameObject.SetActive(false);
//         inputField.gameObject.SetActive(false);
//         submitButton.gameObject.SetActive(false);
//         shiftText.gameObject.SetActive(false); // Hide shift info on end

//         timerScript.isTimerRunning = false;

//         feedbackText.text = endMessage;
//         feedbackText.color = color;
//         feedbackText.gameObject.SetActive(true);
//         inputField.text = "";
//     }
// }
