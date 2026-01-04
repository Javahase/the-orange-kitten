using UnityEngine;
using UnityEngine.UI;

public class InsightsUI : SingletonObject<InsightsUI>
{
    
    public InsightsInventory inventory;
    [SerializeField] private Button _btnContradict;
    [SerializeField] private Button _btnLeft;
    [SerializeField] private Button _btnRight;
    private bool _contradictionMode = false;

    private RecipeInsight? _selectedInsight = null;

    void Start()
    {
        gameObject.SetActive(false);
        inventory.GetSlotElements().ForEach(slotElement =>
        {
           var button = slotElement.GetComponent<Button>();
           button.onClick.AddListener(() =>
           {
                var selected = slotElement.IsSelected();
                inventory.DeselectAll();
                slotElement.SetSelected(!selected);
               _selectedInsight = slotElement.GetInsight();
               _btnContradict.enabled = _contradictionMode;
           });
        });
        _btnLeft.onClick.AddListener(() =>
        {
           inventory.AddPage(_selectedInsight, 1); 
        });
        _btnRight.onClick.AddListener(() =>
        {
           inventory.AddPage(_selectedInsight, -1);
        });
    }

    public void Open(bool contradictionMode)
    {
        _contradictionMode = contradictionMode;
        _btnContradict.gameObject.SetActive(contradictionMode);
        inventory.SetPage(_selectedInsight, 0);
        gameObject.SetActive(true);
    }

    public void Close()
    {
        inventory.DeselectAll();
        _selectedInsight = null;
        gameObject.SetActive(false);
    }

}
