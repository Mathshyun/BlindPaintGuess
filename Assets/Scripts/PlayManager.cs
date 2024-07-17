using System.Collections;
using UnityEngine;

public class PlayManager : MonoBehaviour
{
    public static PlayManager Instance { get; private set; }
    
    public const int MaxProgress = 10;
    public const float SkipPenalty = 10f;

    public int Progress { get; private set; }
    public float Time { get; private set; }
    public string Topic { get; private set; }

    public bool Active { get; private set; }
    public bool Paused { get; private set; }

    [SerializeField] private Draw draw;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Active = false;
        Paused = false;
        UIManager.Instance.SetUIActive(false);
        StartCoroutine(StartCountdown());
    }

    private void Update()
    {
        if (!Active) return;

        if (!Paused)
        {
            // Clear
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                ClearDraw();
            }

            // Pause
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // TODO: Pause game
            }
        
            // Correct
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Progress++;
                if (Progress >= MaxProgress)
                {
                    // TODO: Finish game
                }
                else
                {
                    NextTopic();
                }
            }

            // Skip
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Time += SkipPenalty;
                NextTopic();
            }

            Time += UnityEngine.Time.deltaTime;
        }
        else
        {
            // Resume
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Paused = false;
            }
            
            // Quit
            if (Input.GetKeyDown(KeyCode.Return))
            {
                // TODO: Quit game
            }
        }
    }

    private IEnumerator StartCountdown()
    {
        // TODO: Implement countdown logic
        
        yield return null;

        Active = true;
        StartGame();
    }

    private void StartGame()
    {
        Progress = 0;
        Time = 0f;
        UIManager.Instance.SetUIActive(true);
        NextTopic();
    }

    private void NextTopic()
    {
        ClearDraw();
        Topic = GameManager.Instance.GetNextWord();
        UIManager.Instance.SetAllText();
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
