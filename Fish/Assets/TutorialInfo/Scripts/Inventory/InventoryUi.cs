
using UnityEngine;

public class InventoryUi : MonoBehaviour
{
    public Transform itemsParent;
    public GameObject inventoryUi;
    InventorySlot[] slots;
    Inventory inventory;
 
    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChanegdCallback += UpdateUi;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    void Update ()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            inventoryUi.SetActive(!inventoryUi.activeSelf);
        }
    }
 
    void UpdateUi()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].Additem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
