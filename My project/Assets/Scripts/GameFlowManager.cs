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


    [Header("Game Messages")]
    public string welcomeMessage = "Welcome to the Cipher Challenge!";
    public string instructions = "Instructions:\n- Decrypt the cipher displayed on the screen.\n- Enter your guess in the input box.\n- Use the 'Reveal Algorithm' button for help if needed.";
    public string cipherMessage = "Cipher: opdtryqzcrlxpdmfwrlctllyoczxlytl";
    private string correctDecryption = "designforgamesbulgariaandromania";

    void Start()
        {
            InitializeWelcomeScreen();
        }

        void InitializeWelcomeScreen()
        {
            welcomeText.text = welcomeMessage + "\n\n" + instructions;
            welcomeText.gameObject.SetActive(true);
            cipherText.gameObject.SetActive(false);
            inputField.gameObject.SetActive(false);
            submitButton.gameObject.SetActive(false);
            feedbackText.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(true);

            SetButtonText(nextButton, "Next");
            SetButtonText(submitButton, "Submit");

            nextButton.onClick.AddListener(ShowCipherScreen);
        }

        void ShowCipherScreen()
        {
            welcomeText.gameObject.SetActive(false);
            cipherText.text = cipherMessage;
            cipherText.gameObject.SetActive(true);
            inputField.gameObject.SetActive(true);
            submitButton.gameObject.SetActive(true);
            feedbackText.gameObject.SetActive(true);
            feedbackText.text = "";
            nextButton.gameObject.SetActive(false);
            submitButton.onClick.AddListener(CheckDecryption);
        }
        private void SetButtonText(Button button, string text)
        {
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = text;
            }
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
            }
            else
            {
                feedbackText.text = "Incorrect. Try again!";
                feedbackText.color = Color.red;
                inputField.text = "";
            }
        }
}
