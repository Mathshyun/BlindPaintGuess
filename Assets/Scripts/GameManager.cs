using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool IsPracticeMode { get; private set; } = false;

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
            // TODO: Implement the logic for a single display
            Debug.LogError("Single display");
        }
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
