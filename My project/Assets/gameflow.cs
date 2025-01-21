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

    [Header("Game Messages")]
    public string cipherMessage = "opdtryqzcrlxpdmfwrlctllyoczxlytl";
    private string correctDecryption = "designforgamesbulgariaandromania";


    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip timerTickAudio;
    public AudioClip correctAnswerAudio;
    public AudioClip wrongAnswerAudio;

    [Header("Algorithm Details")]
    public string algorithmDescription = "The cipher is a simple Caesar cipher with a shift of 10.\n\nHow it Works:\n- Each letter is shifted by 10 places in the alphabet.\n- Example:\n  Plaintext: HELLO\n  Ciphertext: ROVVY";

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
    }

    public void StartGame()
    {

        cipherText.text = cipherMessage;

        cipherText.gameObject.SetActive(true);
        inputField.gameObject.SetActive(true);
        submitButton.gameObject.SetActive(true);
        feedbackText.gameObject.SetActive(true);

        feedbackText.text = "";


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
            feedbackText.text = "Correct! You've decrypted the message!";
            feedbackText.color = Color.green;
            audioSource.PlayOneShot(correctAnswerAudio);
            EndGame("Congratulations! You've decrypted the cipher!", Color.green);
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

        timerScript.isTimerRunning = false;

        feedbackText.text = endMessage;
        feedbackText.color = color;
        feedbackText.gameObject.SetActive(true);
        inputField.text = "";
    }

}