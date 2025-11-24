using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField]
    private UIInventoryPage inventoryUI;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if(inventoryUI.isActiveAndEnabled == false)
            {
                inventoryUI.Show();
            }
            else
            {
                inventoryUI.Hide();
            }



        }
    }
}
