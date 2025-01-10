using UnityEngine;
using DialogueEditor;

public class DialogueTrigger : MonoBehaviour
{
    public NPCConversation myConvo;


    public void start()
    {
        ConversationManager.Instance.StartConversation(myConvo);
    }
}
