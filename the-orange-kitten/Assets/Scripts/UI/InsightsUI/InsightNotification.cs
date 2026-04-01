using Services.Insights;
using TMPro;
using UnityEngine;

public class InsightNotification : SingletonObject<InsightNotification>
{
    [SerializeField] private TMP_Text descriptionText;

    private void Start()
    {
        InsightService.GetInstance().OnInsightGained += (insight, display) =>
        {
            if (display) 
                DisplayNotification(insight.info);
        };
        gameObject.SetActive(false);
    }

    public void DisplayNotification(string description)
    {
        Debug.Log("Display");
        descriptionText.text = description;
        gameObject.SetActive(true);
    }

    public void Close() => gameObject.SetActive(false);
}