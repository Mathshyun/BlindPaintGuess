using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    
    [SerializeField] private TMP_Text timeText1;
    [SerializeField] private TMP_Text timeText2;
    [SerializeField] private TMP_Text topicText1;
    [SerializeField] private TMP_Text progressText1;
    [SerializeField] private TMP_Text progressText2;

    private void Awake() => Instance = this;
    
    private void Update()
    {
        timeText1.text = $"{(int)PlayManager.Instance.time / 60}:{(int)PlayManager.Instance.time % 60}";
        timeText2.text = $"{(int)PlayManager.Instance.time / 60}:{(int)PlayManager.Instance.time % 60}";
    }

    public void SetAllText()
    {
        topicText1.text = PlayManager.Instance.topic;
        progressText1.text = $"{PlayManager.Instance.progress}/{PlayManager.MaxProgress}";
        progressText2.text = $"{PlayManager.Instance.progress}/{PlayManager.MaxProgress}";
    }

    public void SetUIActive(bool active)
    {
        timeText1.gameObject.SetActive(active);
        timeText2.gameObject.SetActive(active);
        topicText1.gameObject.SetActive(active);
        progressText1.gameObject.SetActive(active);
        progressText2.gameObject.SetActive(active);
    }
}
