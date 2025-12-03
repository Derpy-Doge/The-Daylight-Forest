using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField]
    private UIInventoryPage inventoryUI;

    public int inventorysize = 10;

    private void Start()
    {
        // Temporarily disable this to prevent freezing
        // inventoryUI.InitializeInventoryUI(inventorysize);
    }

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
