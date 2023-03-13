using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{


    /*
     * 
     * This script will give the proper description to each item.
     * Each item requires specific elements, these are:
     *      Image
     *      Name of Item (Title)
     *      Item' Description(basically what the item does
     *      Item's stats and attributes (What will the player gain by using this item)
     * 
     * 
     */

    public class InventoryDescription : MonoBehaviour
    {
        [SerializeField]
        private Image itemImage;

        [SerializeField]
        private TMP_Text title;

        [SerializeField]
        private TMP_Text description;

        //Item Stats


        private void Awake()
        {
            ResetDescription();
        }


        //Simple reset of the description of an item.
        public void ResetDescription()
        {
            itemImage.gameObject.SetActive(false);
            title.text = "";
            description.text = "";
        }

        //Simple function to set description to an item
        public void SetDescription(Sprite sprite, string itemName, string itemDescription)
        {
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = sprite;
            title.text = itemName;
            description.text = itemDescription;
        }
    }
}