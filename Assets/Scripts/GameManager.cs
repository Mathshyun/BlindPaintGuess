using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
#if !UNITY_EDITOR
        if (Display.displays.Length == 1)
        {
            // TODO: Implement the logic for a single display
            Debug.LogError("Single display");
        }
#endif
        
        
    }
}
