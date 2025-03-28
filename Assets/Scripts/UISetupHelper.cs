using UnityEngine;

/// <summary>
/// Helper script to add the UI setup to the scene
/// </summary>
public class UISetupHelper : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void InitializeUI()
    {
        // Check if we are in the GraphicsTest scene
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "GraphicsTest")
        {
            // Create a new GameObject to hold our setup script
            GameObject setupObject = new GameObject("DeerUI_Setup");
            
            // Add the UI setup script to it
            setupObject.AddComponent<UISetupScript>();
            
            Debug.Log("Deer UI setup initialized for GraphicsTest scene");
        }
    }
} 