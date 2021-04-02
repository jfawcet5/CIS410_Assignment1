using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 0.0f;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    private Rigidbody rb;

    private float movementX;
    private float movementY;
    private float jumpForce = 5;
    private float groundDistance = 0.5f;

    private int count;

    private bool jump;
    private bool canDoubleJump;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;

        SetCountText();
        winTextObject.SetActive(false);

        jump = false;
        canDoubleJump = true;
    }

    private void Update()
    {
        if (transform.position.y <= -3f)
        {
            SceneManager.LoadScene("MiniGame");
        }
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        Debug.Log(movementVector);

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void OnJump(InputValue val)
    {
        Debug.Log(val.Get<float>());
        if (val.Get<float>() == 1)
        {
            jump = true;
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString(); 

        if (count >= 24)
        {
            winTextObject.SetActive(true);
        }
    }

    private bool isGrounded()
    {
        if (Physics.Raycast(transform.position, -Vector3.up, groundDistance + .02f))
        {
            canDoubleJump = true;
            return true;
        }
        return false;
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);

        if (jump)
        {
            Vector3 jumpMovement = Vector3.up * jumpForce;
            //Debug.Log(jumpMovement);
            //Debug.Log(rb.velocity);
            if (isGrounded())
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z) + jumpMovement;
                //Debug.Log(rb.velocity);
            }
            else
            {
                if (canDoubleJump)
                {
                    rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z) + jumpMovement;
                    //Debug.Log(rb.velocity);
                    canDoubleJump = false;
                }
            }
            jump = false;
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
        }
        
    }
}
