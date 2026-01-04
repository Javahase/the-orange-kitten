using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InsightsInventory : MonoBehaviour
{

    const int PAGE_SIZE = 9;

    
    [SerializeField] private SlotElement _slotElementPrefab;
    private int _currentPage = 0;
    private List<SlotElement> _slots;

    void Start()
    {
        _slotElementPrefab.gameObject.SetActive(false);
        _slots = Enumerable.Range(0, PAGE_SIZE)
                            .Select(_ => _slotElementPrefab.InstantiateWith(RecipeInsight.EMPTY, transform))
                            .ToList();
        SetPage(null, 0);
    }

    public void DeselectAll()
    {
        _slots.ForEach(slot => slot.SetSelected(false));
    }

    public void SetPage(RecipeInsight? currSelected, int page)
    {
        if (page < 0 || page > GetMaxPageCount()) return;
        List<RecipeInsight> insights = InsightService.GetInstance().GetPaged(PAGE_SIZE, page);
        var maxSlotUsed = insights.Count;
        for (var i = 0; i < maxSlotUsed; i++)
        {
            _slots[i].SetInsight(insights[i]);
            _slots[i].SetSelected(insights[i].Equals(currSelected));
        }
        _currentPage = page;
    }

    // TODO: This throws a DivideByZeroException for some reason?
    public void AddPage(RecipeInsight? currSelected, int pagesToAdd = 1) => SetPage(currSelected, (_currentPage + pagesToAdd) % GetMaxPageCount());

    public int GetCurrentPage() => _currentPage;

    public int GetMaxPageCount()
    {
        return (int)Math.Floor(InsightService.GetInstance().GetAllInsights().Count / 9f);
    }

    public List<SlotElement> GetSlotElements()
    {
        return _slots;
    }

}