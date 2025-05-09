using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestEntryUI : MonoBehaviour
{
    public TextMeshProUGUI questNameText;
    public GameObject detailsPanel;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI recipientText;

    private bool isExpanded = false;

    public void Initialize(QuestData quest)
    {
        questNameText.text = quest.questName;
        descriptionText.text = $"Description: {quest.description}";
        recipientText.text = $"To: {quest.recipientName}";
        detailsPanel.SetActive(false);
    }

    public void ToggleDetails()
    {
        isExpanded = !isExpanded;
        detailsPanel.SetActive(isExpanded);
    }
}