using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private GameStageController gsc;
    private Rigidbody rb;

    public float movementSpeed = 1.2f;

	// Use this for initialization
	void Start () {
        gsc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameStageController>();
        rb = GetComponent<Rigidbody>();
	}

    // Update is called once per frame
    void FixedUpdate()
    {

        if (gsc.GetIsGameStarted())
        {
            PlayerControl();

        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Check witn name, not nice....
        if(other.gameObject.name == "EndTile(Clone)")
        {
            //Win the game
            Debug.Log("WIN");
        }
    }

    void PlayerControl()
    {
        if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            //Move up
            rb.velocity = new Vector3(rb.velocity.x, 0, movementSpeed);
        }
        if (!Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
        {
            //Move down
            rb.velocity = new Vector3(rb.velocity.x, 0, -movementSpeed);
        }
        if(!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            //Stand still if nothing is pressed
            rb.velocity = new Vector3(rb.velocity.x, 0, 0);
        }
        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            //Move left
            rb.velocity = new Vector3(-movementSpeed, 0, rb.velocity.z);
        }
        if (!Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            //Move right
            rb.velocity = new Vector3(movementSpeed, 0, rb.velocity.z);
        }
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            //Stand still if nothing is pressed
            rb.velocity = new Vector3(0, 0, rb.velocity.z);
        }
    }
}
