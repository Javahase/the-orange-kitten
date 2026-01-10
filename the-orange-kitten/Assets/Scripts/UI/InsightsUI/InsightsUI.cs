using UnityEngine;
using UnityEngine.UI;

public class InsightsUI : SingletonObject<InsightsUI>
{
    
    public InsightsInventory inventory;
    [SerializeField] private Button btnContradict;
    private bool _contradictionMode = false;

    private RecipeInsight? _selectedInsight = null;
    private GameSequenceController _sequenceController;

    private void Start()
    {
        gameObject.SetActive(false);
        _sequenceController = GameSequenceController.GetInstance();
        InitInsightInventory();
    }

    private void InitInsightInventory()
    {
        for (var i = 0; i < InsightsInventory.PageSize; i++)
        {
            var i1 = i;
            var slotElem = inventory.GetSlot(i);
            var button = slotElem.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                inventory.ToggleSlot(i1);
                _selectedInsight = slotElem.GetInsight();
                btnContradict.enabled = _contradictionMode;
            });
        }
    }

    public void Open(bool contradictionMode)
    {
        _contradictionMode = contradictionMode;
        btnContradict.gameObject.SetActive(contradictionMode);
        inventory.SetPage(0);
        gameObject.SetActive(true);
    }

    public void Close()
    {
        inventory.DeselectAll();
        _selectedInsight = null;
        gameObject.SetActive(false);
    }

    public void PresentStatement()
    {
        if (_sequenceController.CurrentSequence is RecipeSequence recipeSequence)
        {
            var selected = _selectedInsight!.Value;
            Close();
            recipeSequence.PresentStatement(selected);
        }
    }

}
