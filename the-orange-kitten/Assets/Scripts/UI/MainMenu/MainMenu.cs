using System.Collections;
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

    void Start()
    {
        fadeTransition.OnInitialize();
        btnProceed.onClick.AddListener(() =>
            {
                DataCollector.GetInstance().Init(inputField.text);
                fadeTransition.gameObject.SetActive(true);
                StartCoroutine(SwitchScene());
            }
        );
    }

    private IEnumerator SwitchScene()
    {
        yield return fadeTransition.FadeOut();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}