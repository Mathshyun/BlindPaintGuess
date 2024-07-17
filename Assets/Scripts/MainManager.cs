using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    private void Update()
    {
        // Start
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameManager.Instance.isPracticeMode = false;
            SceneManager.LoadScene("Scenes/Play");
        }
        
        // Practice
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Instance.isPracticeMode = true;
            SceneManager.LoadScene("Scenes/Play");
        }
        
        // How to Play
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene("Scenes/HowToPlay");
        }
        
        // Credits
        if (Input.GetKeyDown(KeyCode.F2))
        {
            SceneManager.LoadScene("Scenes/Credits");
        }
        
        // Quit
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
