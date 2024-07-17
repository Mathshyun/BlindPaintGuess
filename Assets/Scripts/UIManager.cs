using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private PlayManager playManager;
    [SerializeField] private TMP_Text timeText1;
    [SerializeField] private TMP_Text timeText2;
    [SerializeField] private TMP_Text topicText1;
    [SerializeField] private TMP_Text progressText1;
    [SerializeField] private TMP_Text progressText2;

    private void Update()
    {
        timeText1.text = $"{(int)playManager.time / 60}:{(int)playManager.time % 60}";
        timeText2.text = $"{(int)playManager.time / 60}:{(int)playManager.time % 60}";
    }

    public void UpdateAllText()
    {
        topicText1.text = playManager.topic;
        progressText1.text = $"{playManager.progress}/{PlayManager.MaxProgress}";
        progressText2.text = $"{playManager.progress}/{PlayManager.MaxProgress}";
    }
}
