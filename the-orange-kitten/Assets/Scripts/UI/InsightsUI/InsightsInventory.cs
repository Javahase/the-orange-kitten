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
    private const int PageSize = 9;
    
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

    private void Start()
    {
        slotElementPrefab.gameObject.SetActive(false);
        _slots = Enumerable.Range(0, PageSize)
                            .Select(_ => slotElementPrefab.InstantiateWith(RecipeInsight.EMPTY, transform))
                            .ToList();
        SetPage(null, 0);
    }
    
    /// <summary>
    /// Deselects all <see cref="SlotElement"/>s.
    /// </summary>
    public void DeselectAll()
    {
        _slots.ForEach(slot => slot.SetSelected(false));
    }
    
    /// <summary>
    /// Sets the displayed page to <see cref="page"/>.
    /// </summary>
    /// <param name="currSelected"></param>
    /// <param name="page"></param>
    public void SetPage(RecipeInsight? currSelected, int page)
    {
        // TODO: Delete currSelected.
        if (page < 0 || page > GetMaxPageCount()) return;
        var insights = InsightService.GetInstance().GetPaged(PageSize, page);
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

    private static int GetMaxPageCount()
    {
        return (int)Math.Floor(InsightService.GetInstance().GetAllInsights().Count / (float)PageSize);
    }

    public List<SlotElement> GetSlotElements()
    {
        return _slots;
    }

}