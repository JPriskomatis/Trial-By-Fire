using Inventory.Model;
using Inventory.UI;                         
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {



        GameManagement gameManagement;                          //We reference GameManagement script so we can use its functions in this script.



        [SerializeField]
        private InventoryPage inventory;                        //Takes it from the InventoryPage script to pass in the size of the inventory.

        [SerializeField]
        private InventorySO inventoryData;

        public List<InventoryItem> initialItems = new List<InventoryItem>();



        private void Start()
        {
            //gameManagement = GetComponent<GameManagement>();
            //
            PrepareUI();       //The initial value of the size of the inventory, will add code to make it dynamically change as we get items in-game. *IT IS ALREADY DYNAMIC*
            PrepareInventoryData();

        }

        private void PrepareInventoryData()
        {
            inventoryData.Initialzie();
            inventoryData.OnInventoryUpdated += UpdateInventoryUI;
            foreach (InventoryItem item in initialItems)
            {
                if (item.IsEmpty)
                    continue;
                inventoryData.AddItem(item);

            }
        }

        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            inventory.ResetAllItems();
            foreach (var item in inventoryState)
            {
                inventory.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
            }
        }

        private void PrepareUI()
        {

            inventory.InitializeInventory(inventoryData.Size);
            inventory.OnDescriptionRequested += HandleDescriptionRequest;
            inventory.OnSwapItems += HandleSwapItems;
            inventory.OnStartDragging += HandleDragging;
            inventory.OnItemActionRequested += HandleItemActionRequest;

        }

        private void HandleItemActionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;
            IItemAction itemAction = inventoryItem.item as IItemAction;
            if(itemAction != null)
            {
                itemAction.PerformAction(gameObject, null);
            }

            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
            if (destroyableItem != null)
            {
                inventoryData.RemoveItem(itemIndex, 1);
            }
        }

        private void HandleDragging(int itemIndex)
        {

            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;
            inventory.CreateDraggedItem(inventoryItem.item.ItemImage, inventoryItem.quantity);

        }

        private void HandleSwapItems(int itemIndex1, int itemIndex2)
        {
            inventoryData.SwapItems(itemIndex1, itemIndex2);


        }

        private void HandleDescriptionRequest(int itemIndex)
        {

            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                inventory.ResetSelection();
                return;
            }
            ItemSO item = inventoryItem.item;

            string description = PrepareDescription(inventoryItem);
            inventory.UpdateDescription(itemIndex, item.ItemImage, item.name, description);

        }

        private string PrepareDescription(InventoryItem inventoryItem)
        {
            StringBuilder sb = new StringBuilder();             //we use string builder to create text
            sb.Append(inventoryItem.item.Description);
            sb.AppendLine();                                    //new line
            for (int i = 0; i < inventoryItem.itemState.Count; i++)         //goes through all the items states
            {
                sb.Append($"{inventoryItem.itemState[i].itemParameter.ParameterName} " + 
                    $": {inventoryItem.itemState[i].value} / " +  //dollar sign to ensure we can pass parameters
                    $" {inventoryItem.item.DefaultParametersList[i].value}");           //it will describe the durability in a form of like "durability: 50/100".
                sb.AppendLine();
            }

            return sb.ToString();           //To create it as a string
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {

                if (inventory.isActiveAndEnabled == false)          //That means if our Inventory is not open in-game, then display it.
                {
                    inventory.Show();
                    inventory.isPlaying = true;

                    foreach (var item in inventoryData.GetCurrentInventoryState())              //Returns a dictionary, so each item will be a key-value pair.
                    {
                        inventory.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);     //key is the Index which item shold be added.
                                                                                                            //the Value is basically the value of the item (lke the image or quantity).
                    }


                }
                else
                {

                    inventory.Hide();                               //If the inventory is open in-game and the user pressees the I key, then close the inventory (we also have an X button on the inventory to close it with mouse).
                    inventory.isPlaying = false;
                }

            }


        }

    }
}