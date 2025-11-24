using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class UIInventoryPage : MonoBehaviour
{
    [SerializeField]
    private UIInventoryItem itemPrefab;

    [SerializeField]
    private RectTransform contentPanel;

    List<UIInventoryItem> listOfUIItems = new List<UIInventoryItem>();


    public void InitializeInventoryUI(int inventorysize)
    { 
        for (int i = 0; 1 < inventorysize; i++)
        { 
            UIInventoryItem uiItem =
            Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            uiItem.transform.SetParent(contentPanel);
            listOfUIItems.Add(uiItem);
        }

     }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }


}



