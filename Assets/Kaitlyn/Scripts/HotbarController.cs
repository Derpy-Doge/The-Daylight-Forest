using Inventory.Model;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.UI
{
    public class HotbarController : MonoBehaviour
    {
        [SerializeField] public InventorySO inventory;
        [SerializeField] public int hotbarSize = 6;
        [SerializeField] private List<InventoryItem> barItems;

        public int SelectedIndex { get; private set; } = -1; // -1 is no selection
        public bool IsItemSelected => SelectedIndex >= 0 && SelectedIndex < hotbarSize && HasItemAt(SelectedIndex);

        public event Action<int, InventoryItem, bool> OnHotbarSelectionChanged;

        public bool handsEmpty = true;
        public bool usingPlow = false;
        public bool usingWateringCan = false;
        public bool usingSeed1 = false;
        public bool usingSeed2 = false;
        public bool usingPlant1 = false; ///uhhhh i mightve lost the plot atp
        public bool usingPlant2 = false;

        public void Awake()
        {
            barItems = GetHotbarItems();
            UpdateHotbarFromInventory();
        }

        private void Update()
        {
            barItems = GetHotbarItems();

            GetHotbarItems();
        }

        public void OnEnable()
        {
            if (inventory != null)
                inventory.OnInventoryUpdated += OnInventoryUpdated;
        }

        private void OnDisable()
        {
            if (inventory != null)
                inventory.OnInventoryUpdated -= OnInventoryUpdated;
        }

        private void OnInventoryUpdated(System.Collections.Generic.Dictionary<int, InventoryItem> _)
        {
            UpdateHotbarFromInventory();
        }

        public void UpdateHotbarFromInventory()
        {
            if (SelectedIndex < 0 || SelectedIndex >= hotbarSize)
            {
                if (SelectedIndex != -1) Deselect();
                handsEmpty = true;
                return;
            }

            if (!HasItemAt(SelectedIndex))
            {
                Deselect();
                handsEmpty = true;
            }
            else
            {
                OnHotbarSelectionChanged?.Invoke(SelectedIndex, inventory.GetItemAt(SelectedIndex), true);
            }
        }

        public List<InventoryItem> GetHotbarItems()
        {
            List<InventoryItem> hotbarItems = new List<InventoryItem>();


            if (inventory == null)
            {
                Debug.Log("no inventory");
                return hotbarItems;
            }

            foreach (var item in inventory.inventoryItems)
            {
                if(hotbarItems.Count < 6)
                {
                    hotbarItems.Add(item);
                }
                
            }
            return hotbarItems;
        }

        public void OnHotbarButtonPressed(int slotIndex)
        {
            SelectSlot(slotIndex);
        }

        public void SelectSlot(int slotIndex)
        {
            if (inventory == null) return;
            if (slotIndex < 0 || slotIndex >= hotbarSize) return;

            if (!HasItemAt(slotIndex)) // if the slot has nothing selecting wont do anything
            {
                Debug.Log("nothing here");
                Deselect();
                return;
            }

            if (SelectedIndex == slotIndex) // if you click on the same slot twice it deselects
            {
                Deselect();
                return;
            }

            SelectedIndex = slotIndex;
            string itemName = barItems[slotIndex].item.name;
            SelectedItemMatchesName(itemName);

            OnHotbarSelectionChanged?.Invoke(SelectedIndex, inventory.GetItemAt(SelectedIndex), true);
        }

        public void Deselect()
        {
            if (SelectedIndex == -1) return;

            SelectedIndex = -1;
            handsEmpty = true;
            usingPlow = false;
            usingWateringCan = false;
            usingSeed1 = false;
            usingSeed2 = false;
            usingPlant1 = false;
            usingPlant2 = false;
            OnHotbarSelectionChanged?.Invoke(-1, InventoryItem.GetEmptyItem(), false);
        }

        private bool HasItemAt(int index)
        {
            if (inventory == null) return false;
            if (index < 0 || index >= inventory.inventoryItems.Count) return false;

            return !inventory.GetItemAt(index).IsEmpty;
        }

        public bool SelectedItemMatchesName(string name) //chekcs if the item you selected matches the given name
        {
            if (!IsItemSelected) return false;
            if (inventory == null) return false;

            var selectedItem = inventory.GetItemAt(SelectedIndex);
            if (selectedItem.IsEmpty) return false;
            if (selectedItem.item == null) return false;

            
            if(string.Equals(selectedItem.item.name, name, StringComparison.Ordinal))
            {
                Debug.Log(selectedItem.item.name);
                if(selectedItem.item.name == "Hands(plow)")
                {
                    handsEmpty = false;
                    usingPlow = true;
                    usingWateringCan = false;
                    usingSeed1 = false;
                    usingSeed2 = false;
                    usingPlant1 = false;
                    usingPlant2 = false;
                }
                if (selectedItem.item.name == "Watering Can")
                {
                    handsEmpty = false;
                    usingWateringCan = true;
                    usingPlow = false;
                    usingSeed1 = false;
                    usingSeed2 = false;
                    usingPlant1 = false;
                    usingPlant2 = false;
                }
                if (selectedItem.item.name == "Seed1")
                {
                    handsEmpty = false;
                    usingSeed1 = true;
                    usingPlow = false;
                    usingWateringCan = false;
                    usingSeed2 = false;
                    usingPlant1 = false;
                    usingPlant2 = false;
                }
                if (selectedItem.item.name == "Seed2")
                {
                    handsEmpty = false;
                    usingSeed2 = true;
                    usingPlow = false;
                    usingWateringCan = false;
                    usingSeed1 = false;
                    usingPlant1 = false;
                    usingPlant2 = false;
                }
                if (selectedItem.item.name == "Plant1")
                {
                    handsEmpty = false;
                    usingPlant1 = true;
                    usingPlow = false;
                    usingWateringCan = false;
                    usingSeed1 = false;
                    usingSeed2 = false;
                    usingPlant2 = false;
                }
                if (selectedItem.item.name == "Plant2")
                {
                    handsEmpty = false;
                    usingPlant2 = true;
                    usingPlow = false;
                    usingWateringCan = false;
                    usingSeed1 = false;
                    usingSeed2 = false;
                    usingPlant1 = false;
                }
                return true;
            }

            return false;
        }

        public void UseItem()
        {
            if (!IsItemSelected || inventory == null) return;

            var current = inventory.GetItemAt(SelectedIndex);
            var newQty = Math.Max(0, current.quantity - 1);
            var updated = current.ChangeQuantity(newQty);

            if(current.quantity == 0)
            {
                current.IsEmpty.Equals(true);
            }
            else
            {
                inventory.inventoryItems[SelectedIndex] = updated;
                inventory.InformAboutChange();
            }
        }
    }
}
