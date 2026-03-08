using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Part of the UI that manages the displaying of <see cref="RecipeInsight"/>s.
/// </summary>
public class InsightsInventory : MonoBehaviour
{
    /// <summary>
    /// The page size of a page of the insights inventory.
    /// </summary>
    public const int PageSize = 9;
    
    /// <summary>
    /// A <see cref="SlotElement"/>-prefab that gets copied.
    /// </summary>
    [SerializeField] private SlotElement slotElementPrefab;
    
    /// <summary>
    /// The current page that is shown to the player.
    /// </summary>
    private int _currentPage;
    
    /// <summary>
    /// Nine <see cref="SlotElement"/>s.
    /// </summary>
    private List<SlotElement> _slots;

    private InsightService _insightService;

    private void Start()
    {
        _insightService = InsightService.GetInstance();
        slotElementPrefab.gameObject.SetActive(false);
        _slots = Enumerable.Range(0, PageSize)
                            .Select(_ => slotElementPrefab.InstantiateWith(RecipeInsight.EMPTY, transform))
                            .ToList();
        _insightService.OnInsightGained += (insight) =>
        {
            DataLog.GetInstance().Log(DataLog.DataEvent.InsightGenerated, "New Insight: " + insight.info);
        };
    }
    
    /// <summary>
    /// Deselects all <see cref="SlotElement"/>s.
    /// </summary>
    public void DeselectAll()
    {
        _slots.ForEach(slot => slot.SetSelected(false));
    }

    public void ToggleSlot(int slot)
    {
        var slotElement = _slots[slot];
        var selected = slotElement.IsSelected();
        DeselectAll();
        slotElement.SetSelected(!selected);
    }
    
    /// <summary>
    /// Sets the displayed page to <see cref="page"/>.
    /// </summary>
    /// <param name="page">The page that should be displayed</param>
    public void SetPage(int page)
    {
        if (page < 0 || page >= GetMaxPageCount()) return;
        var insights = _insightService.GetPaged(PageSize, page);
        for (var i = 0; i < _slots.Count; i++)
        {
            _slots[i].SetSelected(false);
            _slots[i].SetInsight(insights.ElementAtOrElse(i, RecipeInsight.EMPTY));
        }
        _currentPage = page;
    }

    public void AddPage(int pagesToAdd) => SetPage((_currentPage + pagesToAdd) % GetMaxPageCount());

    public int GetCurrentPage() => _currentPage;

    private int GetMaxPageCount()
        => (int)Math.Ceiling(_insightService.GetAllInsights().Count / (float)PageSize);

    public SlotElement GetSlot(int index) => _slots[index];

    public List<SlotElement> GetSlotElements() => _slots;
    

}