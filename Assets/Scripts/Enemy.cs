using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float runningSpeed = 1.5f;

    Rigidbody2D rigidBody;

    public bool facingRight = false;

    private Vector3 starPosition;

    public int enemyDamage = 10;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        starPosition = this.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = starPosition;
    }

    private void FixedUpdate()
    {
        float currentRunningSpeed = runningSpeed;

        if (facingRight)
        {
            //Mirando hacia la derecha
            currentRunningSpeed = runningSpeed;
            //Permite el giro del objeto
            this.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            //Mirando hacia la izquierda
            currentRunningSpeed = -runningSpeed;
            this.transform.eulerAngles = Vector3.zero;
        }

        if(GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            rigidBody.velocity = new Vector2(currentRunningSpeed, rigidBody.velocity.y);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Coin")
        {
            return;
        }
        if(collision.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().CollectHelath(-enemyDamage);
            return;
        }
        //Rotacion del enemigo
        facingRight = !facingRight;
    }
}
