using Inventory.Model;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class HotbarView : MonoBehaviour
    {
        [SerializeField] private HotbarController hotbarController;
        [SerializeField] private List<Image> slotImages; // uh i guess make sure there are 6 elements i nthe list 
        [SerializeField] private Sprite emptySprite;
        [SerializeField] private List<GameObject> selectionHighlights; // optional: one per slot

        private void Awake()
        {
            RefreshHotbar();
        }

        private void OnEnable()
        {
            if (hotbarController != null)
                hotbarController.OnHotbarSelectionChanged += OnHotbarSelectionChanged;
            RefreshHotbar();
        }

        private void OnDisable()
        {
            if (hotbarController != null)
                hotbarController.OnHotbarSelectionChanged -= OnHotbarSelectionChanged;
        }

        private void OnHotbarSelectionChanged(int index, InventoryItem item, bool selected)
        {
            RefreshHotbar();
        }

        public void RefreshHotbar()
        {
            if (hotbarController == null) return;

            List<InventoryItem> hotbarItems = hotbarController.GetHotbarItems() ?? new List<InventoryItem>();

            int slotCount = slotImages?.Count ?? 0;
            for (int i = 0; i < slotCount; i++)
            {
                var image = slotImages[i];
                if (image == null) continue;

                Sprite spriteToUse = null;

                if (i < hotbarItems.Count)
                {
                    var invItem = hotbarItems[i];
                    if (!invItem.IsEmpty && invItem.item != null && invItem.item.ItemImage != null)
                    {
                        image.enabled = true;
                        spriteToUse = invItem.item.ItemImage;
                    }
                    else
                    {
                        image.enabled = false;
                    }
                }

                image.sprite = spriteToUse;
            }

            UpdateSelectionVisuals(hotbarController != null ? hotbarController.SelectedIndex : -1);
        }

        private void UpdateSelectionVisuals(int selectedIndex)
        {
            if (selectionHighlights == null || selectionHighlights.Count == 0) return;

            int highlightsCount = selectionHighlights.Count;
            
            int normalizedIndex = (selectedIndex >= 0 && selectedIndex < highlightsCount) ? selectedIndex : -1;

            for (int i = 0; i < highlightsCount; i++)
            {
                var highlight = selectionHighlights[i];
                if (highlight == null) continue;
                highlight.SetActive(i == normalizedIndex);
            }
        }
    }
}
