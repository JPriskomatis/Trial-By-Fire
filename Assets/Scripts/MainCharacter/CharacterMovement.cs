using Inventory.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using UnityEngine.Tilemaps;

public class CharacterMovement : MonoBehaviour
{
    public float bsSpeed;                       //Base Speed
    public float speed;


    public Rigidbody2D rb;

    [SerializeField]
    private InventoryPage inventory;

    private Vector3 targetPosition;
    private bool isMoving = false;

    public Animator anim;

    public Tilemap map;     //To select where the player can walk on.

    public PCManagement pcManagement;

    private void Awake()
    {
        bsSpeed = 2;                        //The base speed is the default speed a character has if no additional equipment or skills are adding movement speed.
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(inventory.isPlaying)
        {
            return;
        }
        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            return;
        }
        //Code to check if you are moving
        if (Input.GetMouseButtonDown(1))
        {
            setTargetPosition();
          }

        if (isMoving)
        {
            Move();

        }
        else
            anim.SetFloat("Speed", 0);
        

       



    }

    void setTargetPosition()
    {
        targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPosition.z = transform.position.z;

        Vector3Int gridPos = map.WorldToCell(targetPosition);                    //To ensure if the map has a tile on this position in which the character can walk on.

        if (map.HasTile(gridPos))
        {
            if ((targetPosition.x >= transform.position.x))
            {
                transform.localScale = new Vector2(1, 1);
            }
            else
            {
                transform.localScale = new Vector2(-1, 1);                      //flips the character's sprite 180 degrees.
            }
            
            isMoving = true;
        }
    }



    void Move()
    {
        MovementSpeed(pcManagement.mainStats.agility, bsSpeed);                          //This calculates how fast the character will be moving. I wanted to make the character move faster the more
                                                                                         //agility they have but without breaking the game. To do so I figured a simple formula that calcuates speed
                                                                                         //based on percentage of the agility. So the more agility the character has the more speed they will gain but
                                                                                         //will ultimately be decreasing with each point.
        
        anim.SetFloat("Speed", speed * Time.deltaTime);

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        if (transform.position == targetPosition)
        {
            isMoving = false;
        }
        else
            isMoving = true;
    }

    float MovementSpeed(int agi, float basespeed)
    {
        speed = (agi * 0.1f) * basespeed + basespeed;
        return speed;
    }
}