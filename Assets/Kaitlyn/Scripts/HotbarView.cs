using UnityEngine;
using UnityEngine.UI;
using Inventory.Model;
using System.Collections.Generic;
using System.Linq;

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

            foreach(var image in slotImages)
            {
                for (int i = 0; i <= hotbarItems.Count;) // this isnt wrking rn but its not my biggest issue rn either...
                {
                    Debug.Log("blink");
                    image.sprite = null;
                    var item = hotbarItems[i];

                    if (item.IsEmpty || item.item == null)
                    {
                        continue;
                    }
                    else
                    {
                        image.sprite = hotbarItems[i].item.ItemImage;
                    }
                }
            }


            UpdateSelectionVisuals(hotbarController != null ? hotbarController.SelectedIndex : -1);
        }

        private void UpdateSelectionVisuals(int selectedIndex)
        {
            if (selectionHighlights == null || selectionHighlights.Count == 0) return;

            int len = selectionHighlights.Count;
            for (int i = 0; i < len; i++)
            {
                var go = selectionHighlights[i];
                if (go == null) continue;
                go.SetActive(i == selectedIndex);
            }
        }
    }
}
