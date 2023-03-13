using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]           //Whenever we add this script to an object, it will add a boxcollider2d.
public class Item : MonoBehaviour
{

    /**
     * Each item in my game has a unique type that allows different interactions with it.
     * We assign this type upon creation of each object.
     * 
     **/

    [field: SerializeField]
    public ItemSO InventoryItem { get; private set; }

    [field: SerializeField]
    public int Quantity { get; set; } = 1;

    [SerializeField]
    private float duration = 0.3f;


    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = InventoryItem.ItemImage;
    }

    public void DestroyItem()
    {
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(ItemPickup());

    }

    private IEnumerator ItemPickup()
    {
        Vector3 StartScale = transform.localScale;
        Vector3 endScale = Vector3.zero;
        float currentTime = 0;
        while(currentTime < duration)
        {
            currentTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(StartScale, endScale, currentTime / duration);
            yield return null;
        }
        Destroy(gameObject);
    }
    


}
