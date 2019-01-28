using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float speed;
    public float jumpHeight;
    public float maxVelocity;
    
    private Rigidbody2D rb;
    private Animator anim;
    private bool fall = false;
    private bool control = true;
    public Interactable interact;

    private Vector3 move;

    public static PlayerMovement instance;

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
                
            //if not, set instance to this
            instance = this;
            
        //If instance already exists and it's not this:
        else if (instance != this)
                
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);    
    }
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Velocity", rb.velocity.x);
        if (GameManager.instance.GetFlag("HAS_CONTROL")) {
            move = new Vector3(Input.GetAxis("Horizontal"), 0).normalized;
            rb.velocity += (Vector2)(move * speed);
            
            if (rb.velocity.magnitude > maxVelocity)
            {
                rb.velocity = rb.velocity.normalized * maxVelocity;
            }
    
            if (Input.GetButtonDown("Use") && interact != null)
            {
                interact.Interact();
            }
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            fall = false;
        }
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            fall = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Interactable"))
        {
            interact = other.gameObject.GetComponent<Interactable>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        interact = null;
    }
}
