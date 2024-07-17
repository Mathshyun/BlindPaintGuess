using System.Collections;
using UnityEngine;

public class PlayManager : MonoBehaviour
{
    public static PlayManager Instance { get; private set; }
    
    public const int MaxProgress = 10;
    public const float SkipPenalty = 10f;
    
    public int progress;
    public float time;
    public string topic;

    [SerializeField] private Draw draw;

    private bool _active;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _active = false;
        UIManager.Instance.SetUIActive(false);
        StartCoroutine(StartCountdown());
    }

    private void Update()
    {
        if (!_active) return;
        
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
            progress++;
            if (progress >= MaxProgress)
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
            time += SkipPenalty;
            NextTopic();
        }

        time += Time.deltaTime;
    }

    private IEnumerator StartCountdown()
    {
        // TODO: Implement countdown logic
        
        yield return null;

        _active = true;
        StartGame();
    }

    private void StartGame()
    {
        progress = 0;
        time = 0f;
        UIManager.Instance.SetUIActive(true);
        NextTopic();
    }

    private void NextTopic()
    {
        ClearDraw();
        topic = GameManager.Instance.GetNextWord();
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
