using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public float Followspeed;           //How fast camera will move to the target
    public Transform target;            //Postition of the player
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y, -10);        //Current position of the player, in the X we write -10f because in 2d the camera position stays -10, if it was 0 we would not be able to see anything

        transform.position = Vector3.Slerp(transform.position, newPos, Followspeed * Time.deltaTime); //Change camera position same as target position, Slerp changes the current position to the target position.

    }
}
