using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool isPracticeMode;
    
    public int MaxProgress => maxProgress;
    public float SkipPenalty => skipPenalty;

    [SerializeField] private int maxProgress;
    [SerializeField] private float skipPenalty;
    
    private readonly List<string> _words = new();
    private int _currentWordIndex;

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
            Debug.LogError("Please connect a second display and restart the game.");
            UnityEngine.SceneManagement.SceneManager.LoadScene("Scenes/Alert");
            return;
        }
        Display.displays[1].Activate();
#endif

        LoadWords();
    }

    private void LoadWords()
    {
        var sr = new StreamReader(Application.streamingAssetsPath + "/words.txt");

        _words.Clear();
        while (!sr.EndOfStream)
        {
            _words.Add(sr.ReadLine());
        }
        
        // Shuffle the words
        for (var i = _words.Count - 1; i >= 0; i--)
        {
            var randomIndex = Random.Range(0, i + 1);
            (_words[randomIndex], _words[i]) = (_words[i], _words[randomIndex]);
        }
        
        _currentWordIndex = 0;
    }
    
    public string GetNextWord()
    {
        var ret = _words[_currentWordIndex];

        _currentWordIndex++;
        _currentWordIndex %= _words.Count;

        return ret;
    }
}
