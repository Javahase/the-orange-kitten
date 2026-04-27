using System.Collections;
using System.Collections.Generic;
using Data.Collector;
using Data.Log;
using Sequence.Types.Transitions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private FadeTransition fadeTransition;

    [SerializeField] private Button btnProceed;

    [SerializeField] private Button btnCredits;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Image backgroundComponent;
    [SerializeField] private List<Sprite> backgroundPool;

    void Start()
    {
        fadeTransition.OnInitialize();
        btnProceed.onClick.AddListener(() =>
            {
                DataCollector.GetInstance().Init(inputField.text);
                DataLog.GetInstance().Init(inputField.text);
                fadeTransition.gameObject.SetActive(true);
                StartCoroutine(SwitchScene());
            }
        );
        btnCredits.onClick.AddListener(Application.Quit);
        backgroundComponent.sprite = backgroundPool[Random.Range(0, backgroundPool.Count)];
    }

    private IEnumerator SwitchScene()
    {
        yield return fadeTransition.FadeOut();
        yield return SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }
}