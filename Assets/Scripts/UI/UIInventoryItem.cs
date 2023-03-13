using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory.UI
{
    /**
     * In this script we edit functionalities of the inventory items.
     * In RPG games we care alot about our inventory. Its important to know what you have, how many
     * what these items do, and organize them as you want.
     * In this script all these functionalities take place.
     * 
     * 
     * We use this. indicate that we are setting the parameters of THIS class
     * 
     * 
     */
    public class UIInventoryItem : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler
    {


        [SerializeField]
        private Image itemImage;                        //The image of the field

        [SerializeField]
        private TMP_Text quantityText;                  //Displays how many of said item the character owns
        /*
         * The following event action is allowing us to call the methods described(OnItemClickedOnItemClicked, etc.)
         * This basically sends a message to another script that "knows what to do" (In this case it will be the InventoryPage script
         * The awesome feature about event Actions is that they are Serialized, meaning that Unity understands them and we can
         * edit them through the inspect (where I have)
         * I added the InventoryItem script the ItemUI prefab and added the required events through that.
         * 
         * 
         * 
         * 
         */

        public event Action<UIInventoryItem> OnItemClicked, OnItemDroppedOn, OnItemBeginDrag, OnItemEndDrag, OnRightMouseButtonClick;                  //Action allows us to call a method assigned here.



        private bool empty = true;

        private void Awake()
        {
            ResetData();
        }


        public void ResetData()                             //Makes the itemslot empty.
        {
            itemImage.gameObject.SetActive(false);
            empty = true;
        }



        public void SetData(Sprite sprite, int quantity)
        {
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = sprite;
            quantityText.text = quantity + "";         //empty string to turn this into a string.
            empty = false;
        }



        public void OnPointerClick(PointerEventData pointerData)
        {
            if (pointerData.button == PointerEventData.InputButton.Right)
            {

                OnRightMouseButtonClick?.Invoke(this);                               //If user clicks the left mouse then we invoke this method.
            }
            else
            {
                OnItemClicked?.Invoke(this);                                        //If user clicks the left button then he is just selecting the item.
            }

        }

        public void OnDrop(PointerEventData eventData)                              //When we drop our item on another item (swapping items basically).
        {
            OnItemDroppedOn?.Invoke(this);
        }

        public void OnBeginDrag(PointerEventData eventData)                                     //Checks that if the item is not empty, then the event action from above is shoot
                                                                                                //and if the OnItemBeginDrag is not null, then we ready this method to be called.
        {
            if (empty)
                return;
            else
                OnItemBeginDrag?.Invoke(this);
        }

        public void OnEndDrag(PointerEventData eventData)                                       // If we have stopped dragging our items on a fine position (not on another item basically).
        {
            OnItemEndDrag?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData)
        {

        }

    }
}