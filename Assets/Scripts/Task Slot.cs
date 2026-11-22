using TMPro;
using UnityEngine;

public class TaskSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI taskNameText;
    [SerializeField] private TextMeshProUGUI taskStepText;
    [SerializeField] private TextMeshProUGUI taskRewardText;
    [SerializeField] private float rewardTextOffset;

    private void Awake()
    {
        taskNameText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTaskName(string taskName)
    {
        taskNameText.SetText(taskName);
        if (string.IsNullOrEmpty(taskNameText.text)) return;

        taskNameText.ForceMeshUpdate();

        int lastIndex = taskNameText.textInfo.characterCount - 1;
        TMP_CharacterInfo lastChar = taskNameText.textInfo.characterInfo[lastIndex];

        Vector3 localPosition = lastChar.topRight;

        taskRewardText.rectTransform.anchoredPosition = new Vector2(
            localPosition.x + rewardTextOffset,
            taskRewardText.rectTransform.anchoredPosition.y
        );
    }

    public void SetStepText(string taskStep)
    {
        taskStepText.SetText(taskStep);
    }

    public void SetRewardText(string taskReward)
    {
        taskRewardText.SetText(taskReward);
    }
}
