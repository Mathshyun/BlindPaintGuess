using UnityEngine;

public class PlayManager : MonoBehaviour
{
    public const int MaxProgress = 10;
    public const float SkipPenalty = 10f;
    
    public int progress = 0;
    public float time = 0f;
    public string topic = "";
    public string[] words;

    [SerializeField] private Draw draw;

    private void Awake()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            ClearDraw();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // TODO: Pause game
        }
        
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // TODO: Correct answer
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // TODO: Skip
        }
    }

    private void ClearDraw()
    {
        draw.FinishLine();
        foreach (Transform child in draw.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
