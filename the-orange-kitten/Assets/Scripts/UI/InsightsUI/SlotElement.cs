using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotElement : MonoBehaviour
{

    private RecipeInsight _insight;
    [SerializeField] private TMP_Text _textField;
    [SerializeField] private Image _panel;

    private bool _selected = false;

    public SlotElement InstantiateWith(RecipeInsight insight, Transform parent, bool selected = false)
    {
        var slotElement = Instantiate(gameObject, parent).GetComponent<SlotElement>();
        slotElement._panel.gameObject.SetActive(false);
        slotElement.SetInsight(insight);
        slotElement.SetSelected(selected);
        slotElement.gameObject.SetActive(true);
        return slotElement;
    }

    public void SetSelected(bool selected)
    {
        if (_insight.Equals(RecipeInsight.EMPTY))
            return;
        _selected = selected;
        _panel.gameObject.SetActive(selected);
    }

    public bool IsSelected() => _selected;

    public void SetInsight(RecipeInsight insight)
    {
        _insight = insight;
        _textField.text = insight.info;
    }

    public RecipeInsight GetInsight() => _insight;
}
