using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private const float FadeTime = 1f;
    
    [SerializeField] private TMP_Text timeText1;
    [SerializeField] private TMP_Text timeText2;
    [SerializeField] private TMP_Text topicText1;
    [SerializeField] private TMP_Text topicText2;
    [SerializeField] private TMP_Text progressText1;
    [SerializeField] private TMP_Text progressText2;
    [SerializeField] private TMP_Text statusText1;
    [SerializeField] private TMP_Text statusText2;
    
    private Coroutine _statusFadeCoroutine;

    private void Awake() => Instance = this;
    
    private void Update()
    {
        if (!PlayManager.Instance.Active) return;
        
        var text = TimeSpan.FromSeconds(PlayManager.Instance.Time).ToString("mm':'ss");
        timeText1.text = text;
        timeText2.text = text;
    }

    private IEnumerator StatusFade(Color color)
    {
        var time = 0f;
        while (time < FadeTime)
        {
            time += Time.deltaTime;
            var alpha = 1f - time / FadeTime;
            statusText1.color = new Color(color.r, color.g, color.b, alpha);
            statusText2.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        statusText1.gameObject.SetActive(false);
        statusText2.gameObject.SetActive(false);
    }

    public void SetAllText()
    {
        topicText1.text = PlayManager.Instance.Topic;
        topicText2.text = PlayManager.Instance.Topic;
        if (!PlayManager.Instance.IsReviewing)
        {
            progressText1.text = $"{PlayManager.Instance.Progress}/{PlayManager.Instance.MaxProgress}";
            progressText2.text = $"{PlayManager.Instance.Progress}/{PlayManager.Instance.MaxProgress}";
        }
        else
        {
            progressText1.text = $"{PlayManager.Instance.Progress + 1}/{PlayManager.Instance.MaxProgress}";
            progressText2.text = $"{PlayManager.Instance.Progress + 1}/{PlayManager.Instance.MaxProgress}";
        }
    }
    
    public void SetStatusText(string text)
    {
        if (_statusFadeCoroutine != null) StopCoroutine(_statusFadeCoroutine);
        
        statusText1.color = Color.white;
        statusText2.color = Color.black;
        statusText1.text = text;
        statusText2.text = text;
    }

    public void SetStatusFadeOnce(string text, Color color)
    {
        if (_statusFadeCoroutine != null) StopCoroutine(_statusFadeCoroutine);
        
        statusText1.color = color;
        statusText2.color = color;
        statusText1.text = text;
        statusText2.text = text;
        
        statusText1.gameObject.SetActive(true);
        statusText2.gameObject.SetActive(true);
        
        _statusFadeCoroutine = StartCoroutine(StatusFade(color));
    }

    public void SetStatusActive(bool active)
    {
        statusText1.gameObject.SetActive(active);
        statusText2.gameObject.SetActive(active);
    }

    public void SetUIActive(bool active)
    {
        timeText1.gameObject.SetActive(active);
        timeText2.gameObject.SetActive(active);
        topicText1.gameObject.SetActive(active);
        progressText1.gameObject.SetActive(active);
        progressText2.gameObject.SetActive(active);
    }

    public void SetTopicText2Active(bool active) => topicText2.gameObject.SetActive(active);
}
