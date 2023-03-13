using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class InventoryPage : MonoBehaviour
    {

        public bool isPlaying;


        [SerializeField]
        private UIInventoryItem itemPrefab;               //We use prefabs to generate the items when we collect an item.

        [SerializeField]
        private RectTransform contentPanel;             //content of our inventory panel from the canvas This variable basically stores the GUI.

        [SerializeField]
        private InventoryDescription itemDescr;

        [SerializeField]
        private MouseFollower mouseFollower;

        /*public Sprite image, image2;
        public int quantity
        public string title, description;
        */

        private int currentlyDraggedItemIndex = -1;         //The -1 is to make sure that its outside of the bouds of our list, so we are not dragging any item.


        public event Action<int> OnDescriptionRequested, OnItemActionRequested, OnStartDragging;                                    //Event actions, we pass the index (int) of the item.

        public event Action<int, int> OnSwapItems;                                          //We pass 2 indexes(ints) of the 2 items we want to swap;


        private void Awake()
        {
            isPlaying = false;
            //Hide();
            itemDescr.ResetDescription();
            mouseFollower.Toggle(false);
        }




        List<UIInventoryItem> listOfItems = new List<UIInventoryItem>();    //A list of items that make up our inventory space.

        internal void ResetAllItems()
        {
            foreach (var item in listOfItems)
            {
                item.ResetData();
            }
        
        }

        public void InitializeInventory(int invSize)
        {
            for (int i = 0; i < invSize; i++)                           //Simple loop to loop through the items in the inventory.
            {
                UIInventoryItem uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);        //This creates the item in the itemslot of our inventory.
                uiItem.transform.SetParent(contentPanel);

                listOfItems.Add(uiItem);

                uiItem.OnItemBeginDrag += HandleBeginDrag;


                // Here we assign methods to the events we created in the InventoryItem script


                //uiItem.OnItemClicked += HandleItemSelection;
                uiItem.OnItemBeginDrag += HandleBeginDrag;
                uiItem.OnItemDroppedOn += HandleSwap;

                uiItem.OnItemEndDrag += HandleEndDrag;

                uiItem.OnRightMouseButtonClick += HandleShowItemActions;



            }
        }

        internal void UpdateDescription(int itemIndex, Sprite itemImage, string name, string description)
        {
            itemDescr.SetDescription(itemImage, name, description);
            

        }

        public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)
        {
            if (listOfItems.Count > itemIndex)                                       //If we have this item in the list of our items we acces this item and set its data.
            {
                listOfItems[itemIndex].SetData(itemImage, itemQuantity);
            }
        }


        private void HandleShowItemActions(UIInventoryItem inventoryItem)
        {
            int index = listOfItems.IndexOf(inventoryItem);

            if (index == -1)
            {

                return;

            }
            OnItemActionRequested?.Invoke(index);


        }

        private void HandleEndDrag(UIInventoryItem inventoryItem)
        {
            ResetDraggedItem();
        }

        private void HandleSwap(UIInventoryItem inventoryItem)
        {

            int index = listOfItems.IndexOf(inventoryItem);

            if (index == -1)
            {

                return;

            }

            OnSwapItems?.Invoke(currentlyDraggedItemIndex, index);
            HandleItemSelection(inventoryItem);




        }

        private void ResetDraggedItem()
        {
            mouseFollower.Toggle(false);
            currentlyDraggedItemIndex = -1;
        }

        private void HandleBeginDrag(UIInventoryItem inventoryItem)
        {
            int index = listOfItems.IndexOf(inventoryItem);
            if (index == -1)
                return;

            currentlyDraggedItemIndex = index;

            HandleItemSelection(inventoryItem);

            OnStartDragging?.Invoke(index);


        }

        public void CreateDraggedItem(Sprite sprite, int quantity)
        {
            mouseFollower.Toggle(true);
            mouseFollower.SetData(sprite, quantity);
        }

        private void HandleItemSelection(UIInventoryItem inventoryItem)
        {
            int index = listOfItems.IndexOf(inventoryItem);


            if (index == -1)
                return;
            OnDescriptionRequested?.Invoke(index);
        }




        public void Show()              //Shows the inventory.
        {
            gameObject.SetActive(true);
            ResetSelection();

        }

        public void ResetSelection()
        {
            itemDescr.ResetDescription();

        }





        public void Hide()              //Hides the inventory.
        {
            gameObject.SetActive(false);
            ResetDraggedItem();

        }


    }
}