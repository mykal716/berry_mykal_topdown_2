using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMovement : MonoBehaviour
{
    public float detectionRadius = 3;
    public float movementSpeed = 5;
    public bool canMove = false;
    public bool movementDirection = false;
    public bool isFollowing = false;

    private Animator myAnimator;

    public Transform playerTarget;
    private Rigidbody2D myRB;
    private CircleCollider2D detectionZone;
    private Vector2 up;
    private Vector2 down;
    private Vector2 zero;
    
    // Start is called before the first frame update
    void Start()
    {
        up = new Vector2(0, movementSpeed);
        down = new Vector2(0, -movementSpeed);
        zero = new Vector2(0, 0);

        playerTarget = GameObject.Find("playerSprite").transform;

        myRB = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        detectionZone = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        detectionZone.radius = detectionRadius;

        if (isFollowing == false)
        {
            myAnimator.SetBool("isWalking", false);
            myRB.velocity = zero;
        }

        else if (isFollowing == true)
        {
            Vector3 lookPos = playerTarget.position - transform.position;
            float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
            myRB.rotation = angle;
            lookPos.Normalize();

            myAnimator.SetBool("isWalking", true);

            myRB.MovePosition(transform.position + (lookPos * movementSpeed * Time.deltaTime));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("bullet"))
        {
            Debug.Log("I've been hit");
            Destroy(collision.gameObject);
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerCollision2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("player"))
        {
            isFollowing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("player"))
        {
            isFollowing = false;
        }
    }
}
