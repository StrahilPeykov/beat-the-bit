using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;


public class SceneEventHandler : MonoBehaviour
{
    [SerializeField] private UnityEvent onSceneLoaded; // Serialized UnityEvent to set in the Editor

    private void OnEnable()
    {
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Trigger the serialized UnityEvent
        Debug.Log($"Scene Loaded: {scene.name}");
        onSceneLoaded?.Invoke();
    }
}