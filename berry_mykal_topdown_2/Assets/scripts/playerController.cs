using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public GameObject bullet;
    private Rigidbody2D myRB;
    public Sprite invincibility;

    private AudioSource mySpeaker;
    public AudioClip pickup;
    public AudioClip death;
    public AudioClip shoot;

    private bool canShoot = true;
    public float shootCooldownTime;
    private float timeDifference;
    public bool invincible = false;
    public float invincibleCooldownTime;
    private float timeDifference2;

    public float speed = 10;
    public float bulletLifespan = 1;
    public float bulletSpeed = 15;
    public int playerHealth = 3;
    
    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        mySpeaker = GetComponent<AudioSource>();
        playerHealth = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth <= 0)
        {
            transform.SetPositionAndRotation(new Vector2(), new Quaternion());
            playerHealth = 3;
        }

        Vector2 velocity = myRB.velocity;

        velocity.x = Input.GetAxisRaw("Horizontal") * speed;
        velocity.y = Input.GetAxisRaw("Vertical") * speed;

        myRB.velocity = velocity;

        if (canShoot)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                GameObject b = Instantiate(bullet, new Vector2(transform.position.x, transform.position.y + 1), transform.rotation);
                b.GetComponent<Rigidbody2D>().velocity = new Vector2(0, bulletSpeed);

                mySpeaker.clip = shoot;
                mySpeaker.play();

                Destroy(b, bulletLifespan);
                canShoot = false;
            }

            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                GameObject b = Instantiate(bullet, new Vector2(transform.position.x, transform.position.y - 1), transform.rotation);
                b.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -bulletSpeed);

                mySpeaker.clip = shoot;
                mySpeaker.play();

                Destroy(b, bulletLifespan);
                canShoot = false;
            }

            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                GameObject b = Instantiate(bullet, new Vector2(transform.position.x - 1, transform.position.y), transform.rotation);
                b.GetComponent<Rigidbody2D>().velocity = new Vector2(-bulletSpeed, 0);

                mySpeaker.clip = shoot;
                mySpeaker.play();

                Destroy(b, bulletLifespan);
                canShoot = false;
            }

            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                GameObject b = Instantiate(bullet, new Vector2(transform.position.x + 1, transform.position.y), transform.rotation);
                b.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletSpeed, 0);

                mySpeaker.clip = shoot;
                mySpeaker.play();

                Destroy(b, bulletLifespan);
                canShoot = false;
            }
        }

        if(canShoot == false)
        {
            timeDifference += Time.deltaTime;

            if (timeDifference >= shootCooldownTime)
            {
                canShoot = true;
                timeDifference = 0;
            }
        }

        if (invincible == true)
        {
            timeDifference += Time.deltaTime;

            if (timeDifference2 >= invincibleCooldownTime)
            {
                invincible = false;
                timeDifference = 0;
                GetComponent<SpiritRenderer>().color = UnityEngine.color.white;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("enemy") && !invincible)
        {
            mySpeaker.clip = death;
            mySpeaker.Play();
            playerHealth--;
        }

        else if((collision.gameObject.name == "pickup"))
        {
            // This also means playerHealth = playerHealth + 1;
            //playerHealth++;
 
            mySpeaker.clip = pickup;
            mySpeaker.Play();
            invincible = true;
            GetComponent<SpiritRenderer>().color = UnityEngine.Color.yellow;
            collision.gameObject.SetActive(false);
        }
    }
}