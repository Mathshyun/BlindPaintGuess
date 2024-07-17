using System;
using System.Collections;
using UnityEngine;

public class PlayManager : MonoBehaviour
{
    public static PlayManager Instance { get; private set; }

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
        if (GameManager.Instance.IsPracticeMode)
        {
            Active = true;
            Paused = false;
            Topic = "Practice";
            UIManager.Instance.SetUIActive(true);
            UIManager.Instance.SetStatusActive(false);
            ClearDraw();
            return;
        }
        
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

            if (!GameManager.Instance.IsPracticeMode)
            {
                // Pause
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Paused = true;
                    UIManager.Instance.SetStatusText("Paused\n\n[ESC] Resume    [Enter] Quit");
                    UIManager.Instance.SetStatusActive(true);
                }
        
                // Correct
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    Progress++;
                    if (Progress >= GameManager.Instance.MaxProgress)
                    {
                        Active = false;
                        var record = TimeSpan.FromSeconds(Time).ToString("mm':'ss");
                        UIManager.Instance.SetAllText();
                        UIManager.Instance.SetStatusText($"Clear!\nRecord: {record}\n\n[Enter] Quit");
                        UIManager.Instance.SetStatusActive(true);
                        Cursor.visible = true;
                    }
                    else
                    {
                        NextTopic();
                    }
                }

                // Skip
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Time += GameManager.Instance.SkipPenalty;
                    NextTopic();
                }

                Time += UnityEngine.Time.deltaTime;
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    // TODO: Quit practice mode
                }
            }
        }
        else
        {
            // Resume
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Paused = false;
                UIManager.Instance.SetStatusActive(false);
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
        UIManager.Instance.SetStatusActive(true);
        UIManager.Instance.SetStatusText("3");
        yield return new WaitForSeconds(1f);
        UIManager.Instance.SetStatusText("2");
        yield return new WaitForSeconds(1f);
        UIManager.Instance.SetStatusText("1");
        yield return new WaitForSeconds(1f);
        StartGame();
    }

    private void StartGame()
    {
        Progress = 0;
        Time = 0f;
        Active = true;
        Cursor.visible = false;
        UIManager.Instance.SetStatusActive(false);
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
