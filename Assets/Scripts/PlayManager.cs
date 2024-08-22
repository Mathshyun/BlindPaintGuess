using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayManager : MonoBehaviour
{
    public static PlayManager Instance { get; private set; }

    public int Progress { get; private set; }
    public int MaxProgress { get; private set; }
    public float Time { get; private set; }
    public string Topic { get; private set; }
    public bool Active { get; private set; }
    public bool Paused { get; private set; }
    public bool IsReviewing { get; private set; }

    [SerializeField] private Draw draw;
    [SerializeField] private Transform drawingsParent;

    private struct Drawing
    {
        public string Topic;
        public bool IsSkipped;
        public GameObject DrawObject;
    }

    private List<Drawing> _drawings = new();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Progress = 0;
        Time = 0f;
        Paused = false;
        IsReviewing = false;
        
        if (GameManager.Instance.isPracticeMode)
        {
            MaxProgress = 0;
            Active = true;
            Topic = "Practice";
            Cursor.visible = false;
            UIManager.Instance.SetAllText();
            UIManager.Instance.SetUIActive(true);
            UIManager.Instance.SetTopicText2Active(true);
            UIManager.Instance.SetStatusActive(false);
            ClearDraw();
            return;
        }

        MaxProgress = GameManager.Instance.MaxProgress;
        Active = false;
        _drawings.Clear();
        UIManager.Instance.SetUIActive(false);
        UIManager.Instance.SetTopicText2Active(false);
        StartCoroutine(StartCountdown());
    }

    private void Update()
    {
        if (!Active)
        {
            // Is reviewing
            if (IsReviewing)
            {
                // Next
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (Progress < MaxProgress - 1)
                    {
                        Progress++;
                        SetReviewStatus();
                    }
                }
                
                // Previous
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (Progress > 0)
                    {
                        Progress--;
                        SetReviewStatus();
                    }
                }
                
                // Quit
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Quit();
                }
            }
            
            // Cleared
            if (Progress >= GameManager.Instance.MaxProgress)
            {
                // Review
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Progress = 0;
                    MaxProgress = _drawings.Count;
                    IsReviewing = true;
                    ClearDraw();
                    SetReviewStatus();
                    UIManager.Instance.SetTopicText2Active(true);
                }
                
                // Quit
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    Quit();
                }
            }
            
            return;
        }

        if (!Paused)
        {
            // Clear
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                UIManager.Instance.SetStatusFadeOnce("Clear", new Color(0.5f, 0.5f, 0.5f));
                ClearDraw();
            }

            // Play mode
            if (!GameManager.Instance.isPracticeMode)
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
                    SaveDrawing();
                    Progress++;
                    if (Progress >= GameManager.Instance.MaxProgress)
                    {
                        Active = false;
                        var record = TimeSpan.FromSeconds(Time).ToString("mm':'ss");
                        UIManager.Instance.SetAllText();
                        UIManager.Instance.SetStatusText($"Clear!\nRecord: {record}\n\n[Space] Review    [Enter] Quit");
                        UIManager.Instance.SetStatusActive(true);
                        Cursor.visible = true;
                    }
                    else
                    {
                        UIManager.Instance.SetStatusFadeOnce("Correct!", new Color(0.2f, 0.8f, 0.2f));
                        NextTopic();
                    }
                }

                // Skip
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    SaveDrawing(true);
                    Time += GameManager.Instance.SkipPenalty;
                    UIManager.Instance.SetStatusFadeOnce("Skip", new Color(0.5f, 0.5f, 0.5f));
                    NextTopic();
                }

                Time += UnityEngine.Time.deltaTime;
            }
            // Practice mode
            else
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Quit();
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
                Quit();
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
        Active = true;
        Cursor.visible = false;
        UIManager.Instance.SetStatusActive(false);
        UIManager.Instance.SetUIActive(true);
        NextTopic();
    }

    private void SaveDrawing(bool isSkipped = false)
    {
        Drawing drawing;

        var drawObject = Instantiate(draw.gameObject, drawingsParent);
        drawObject.layer = 0;
        drawObject.SetActive(false);

        drawing.Topic = Topic;
        drawing.IsSkipped = isSkipped;
        drawing.DrawObject = drawObject;
        
        _drawings.Add(drawing);
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

    private void SetReviewStatus()
    {
        var currentDrawing = _drawings[Progress];
        Topic = currentDrawing.Topic;
        foreach (var drawing in _drawings)
        {
            drawing.DrawObject.SetActive(false);
        }
        currentDrawing.DrawObject.SetActive(true);
        UIManager.Instance.SetAllText();
        UIManager.Instance.SetStatusFadeOnce(currentDrawing.IsSkipped ? "Skipped" : "Correct",
            new Color(0.5f, 0.5f, 0.5f));
    }

    private void Quit()
    {
        Cursor.visible = true;
        SceneManager.LoadScene("Scenes/Main");
    }
}
