using Inventory.Model;
using Inventory.Ui;
using Inventory.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField]
        private UIInventoryPage inventoryUI;
        [SerializeField]
        private GameObject hotbar;

        [SerializeField]
        private InventorySO inventoryData;

        public List<InventoryItem> initialItems = new List<InventoryItem>();

        private string mainScene;

        public void Awake()
        {
            if (SaveData.instance != null)
                mainScene = SaveData.instance.mainScene;
        }


        private void Start()
        {
            PrepareInventoryData();
            PrepareUI();
        }

        public void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == mainScene && !SaveData.firstLoad)
            {
                FindUIAndHotbar();
            }
            else
            {
                inventoryUI = null;
                hotbar = null;
            }
        }

        private void FindUIAndHotbar()
        {
            var uiGameObject = GameObject.FindGameObjectWithTag("UIInventoryPage");
            inventoryUI = uiGameObject.GetComponent<UIInventoryPage>();
            hotbar = GameObject.FindGameObjectWithTag("Hotbar");
            HotbarController hbc = hotbar.GetComponent<HotbarController>();
            HotbarView hbv = hotbar.GetComponent<HotbarView>();

            hbv.RefreshHotbar();
            hbc.UpdateHotbarFromInventory();

        }

        private void PrepareInventoryData()
        {
            inventoryData.Initialize();
            inventoryData.OnInventoryUpdated += UpdateInventoryUI;
            foreach (var item in initialItems)
            {
                if (item.IsEmpty)
                    continue;
                inventoryData.AddItem(item);
            }
        }

        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            inventoryUI.ResetAllItems(); 
            foreach (var item in inventoryState)
            {
              inventoryUI.UpdateData(item.Key,
                item.Value.item.ItemImage,
                item.Value.quantity);
            }
        }

        private void PrepareUI()
        {
            inventoryUI.InitializeInventoryUI(inventoryData.Size);
            inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
            inventoryUI.OnSwapItems += HandleSwapItems;
            inventoryUI.OnStartDragging += HandleDragging;
            inventoryUI.OnItemActionRequested += HandleItemActionRequest;
        }

        private void HandleItemActionRequest(int itemIndex)
        {

        }

        private void HandleDragging(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;
            inventoryUI.CreateDraggedItem(inventoryItem.item.ItemImage,
            inventoryItem.quantity);
        }

        private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
        {
            inventoryData.SwapItems(itemIndex_1, itemIndex_2);
        }

        private void HandleDescriptionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                inventoryUI.ResetSelection();
                return;
            }
            ItemSO item = inventoryItem.item;
            inventoryUI.UpdateDescription(itemIndex, item.ItemImage,
            item.name, item.Description);
        }

        private void FixedUpdate()
        {

        }
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (inventoryUI.isActiveAndEnabled == false)
                {
                    inventoryUI.Show();
                    hotbar.SetActive(false);
                    foreach (var item in inventoryData.GetCurrentInventoryState())
                    {
                        inventoryUI.UpdateData(item.Key,
                        item.Value.item.ItemImage,
                        item.Value.quantity);
                    }
                }
                else
                {
                    inventoryUI.Hide();
                    hotbar.SetActive(true);
                }



            }
        }
    }
}