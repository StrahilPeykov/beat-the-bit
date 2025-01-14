using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameFlowManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI welcomeText;
    public TextMeshProUGUI cipherText;
    public TMP_InputField inputField;
    public Button nextButton;
    public Button submitButton;
    public TextMeshProUGUI feedbackText;
    public TextMeshProUGUI timerText;
    public Button retryButton;
    public Button revealAlgorithmButton;
    public GameObject algorithmPopup;
    public TextMeshProUGUI algorithmPopupText;

    [Header("Game Messages")]
    public string welcomeMessage = "Welcome to the Cipher Challenge!";
    public string instructions = "Instructions:\n- Decrypt the cipher displayed on the screen.\n- Enter your guess in the input box.\n- Use the 'Reveal Cipher' button for help if needed.";
    public string cipherMessage = "Cipher: opdtryqzcrlxpdmfwrlctllyoczxlytl";
    private string correctDecryption = "designforgamesbulgariaandromania";

    [Header("Timer Settings")]
    public float roundTimer = 60f;
    private float timer = 1f;
    private bool isTimerRunning = false;

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
        roundTimer = 60f;
        timer = 1f;
        isTimerRunning = false;

        welcomeText.text = welcomeMessage + "\n\n" + instructions;
        welcomeText.gameObject.SetActive(true);
        cipherText.gameObject.SetActive(false);
        inputField.gameObject.SetActive(false);
        submitButton.gameObject.SetActive(false);
        feedbackText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(false);
        revealAlgorithmButton.gameObject.SetActive(false);
        algorithmPopup.SetActive(false);

        SetButtonText(nextButton, "Next");
        SetButtonText(submitButton, "Submit");
        SetButtonText(retryButton, "Retry");
        SetButtonText(revealAlgorithmButton, "Reveal Cipher");

        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(StartGame);
    }

    void StartGame()
    {
        welcomeText.gameObject.SetActive(false);
        cipherText.text = cipherMessage;
        cipherText.gameObject.SetActive(true);
        inputField.gameObject.SetActive(true);
        submitButton.gameObject.SetActive(true);
        feedbackText.gameObject.SetActive(true);
        feedbackText.text = "";
        timerText.gameObject.SetActive(true);
        isTimerRunning = true;
        nextButton.gameObject.SetActive(false);
        revealAlgorithmButton.gameObject.SetActive(true);

        submitButton.onClick.RemoveAllListeners();
        submitButton.onClick.AddListener(CheckDecryption);

        revealAlgorithmButton.onClick.RemoveAllListeners();
        revealAlgorithmButton.onClick.AddListener(ShowAlgorithmPopup);

    }

    void Update()
    {
        if (isTimerRunning)
        {
            timer -= Time.deltaTime;

            if (timer <= 0 && roundTimer > 0)
            {
                audioSource.PlayOneShot(timerTickAudio, 0.30f);
                timer = 1f;
                roundTimer -= 1f;
                DisplayTime(roundTimer);
            }
            if (roundTimer <= 0)
            {
                isTimerRunning = false;
                EndGame("Time's up! You've run out of time!", Color.red);
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        int minutes = Mathf.FloorToInt(timeToDisplay / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        Color greenColor;

        if (timeToDisplay <= 15f)
        {
            timerText.color = Color.red;
        }
        else
        {
            if (ColorUtility.TryParseHtmlString("#20C20E", out greenColor))
                {
                    timerText.color = greenColor;
                }
        }

    }

    void ShowAlgorithmPopup()
    {
        if (!algorithmPopup.activeSelf)
        {
            algorithmPopupText.text = algorithmDescription;
            algorithmPopup.SetActive(true);

            cipherText.gameObject.SetActive(false);
            inputField.gameObject.SetActive(false);
            submitButton.gameObject.SetActive(false);
            feedbackText.gameObject.SetActive(false);
        }
        else
        {
            algorithmPopup.SetActive(false);
            algorithmPopupText.text = "";
            cipherText.gameObject.SetActive(true);
            inputField.gameObject.SetActive(true);
            submitButton.gameObject.SetActive(true);
            feedbackText.gameObject.SetActive(true);
        }
    }
    void HideAlgorithmPopup()
    {
        algorithmPopup.SetActive(false);
    }

    void CheckDecryption()
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
            isTimerRunning = false;
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
        isTimerRunning = false;

        welcomeText.gameObject.SetActive(false);
        cipherText.gameObject.SetActive(false);
        inputField.gameObject.SetActive(false);
        submitButton.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);

        feedbackText.text = endMessage;
        feedbackText.color = color;
        feedbackText.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(true);

        retryButton.onClick.RemoveAllListeners();
        retryButton.onClick.AddListener(InitializeWelcomeScreen);
        inputField.text = "";
    }

    private void SetButtonText(Button button, string text)
    {
        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            buttonText.text = text;
        }
    }
}
