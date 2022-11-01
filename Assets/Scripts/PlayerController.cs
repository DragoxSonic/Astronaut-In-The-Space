using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 8f;
    Rigidbody2D rigidboby;
    Animator animator;
    Vector3 startPosition;

    [Header("Movimiento")]
    private float movimientoHorizontal = 0f;
    public float velocidadDeMovimiento;
    [Range(0, 0.3f)][SerializeField]private float suavizadoDeMovimiento;
    private Vector3 velocidad = Vector3.zero;
    private bool mirandoDerecha = true;
    private Rigidbody2D rb2D;

    const string STATE_ALIVE = "isAlive";
    const string STATE_ON_THE_GROUND = "isOnTheGround";

    //[SerializeFiled] es para poder visualizar variables, sin manupularlas
    [SerializeField]private int healthPonits, manaPoints;

    public const int INITIAL_HEALTH = 100, INITIAL_MANA = 15, MAX_HEALTH = 200, MAX_MANA = 30, MIN_HEALTH = 10, MIN_MANA = 0;

    public const int SUPERJUMP_COST = 5;
    public const float SUPERJUMP_FORCE = 1.5f;

    public LayerMask groundMask;

    // Start is called before the first frame update

    void Awake()
    {
        rigidboby = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        rb2D = GetComponent <Rigidbody2D>();

        //Esto hace que el personaje vuelva a la posicion inicial
        startPosition = this.transform.position;

        healthPonits = INITIAL_HEALTH;
        manaPoints = INITIAL_MANA;
    }

    public void StartGame()
    {
        animator.SetBool(STATE_ALIVE, true);
        animator.SetBool(STATE_ON_THE_GROUND, false);

        //Genera un pequeño delay en el frame de la reaparicion del personaje despues de morir
        Invoke("RestartPosition", 0.2f);

    }

    void RestartPosition()
    {
        this.transform.position = startPosition;
        this.rigidboby.velocity = Vector2.zero;

        GameObject mainCamera = GameObject.Find("Main Camera");
        mainCamera.GetComponent<CameraFollow>().ResetCameraPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump(false);
        }
        if (Input.GetButtonDown("SuperJump"))
        {
            Jump(true);
        }

        movimientoHorizontal = Input.GetAxisRaw("Horizontal") * velocidadDeMovimiento;

        animator.SetBool(STATE_ON_THE_GROUND, IsTouchingTheGruond());

        //Debug.DrawRay(this.transform.position, Vector2.down * 1.5f, Color.red);
    }

    private void FixedUpdate()
    {
        if (GameManager.sharedInstance.currentGameState == GameState.inGame)
        {

            {
                Mover(movimientoHorizontal * Time.fixedDeltaTime);
            }
        }
    }

    void Jump(bool superjump)
    {
        if (IsTouchingTheGruond() && GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            float jumpForceFactor = jumpForce;

            if (superjump && manaPoints >= SUPERJUMP_COST)
            {
                manaPoints -= SUPERJUMP_COST;
                jumpForceFactor *= SUPERJUMP_FORCE;
            }
            GetComponent<AudioSource>().Play();
            rigidboby.AddForce(Vector2.up * jumpForceFactor, ForceMode2D.Impulse);
        }
    }

    bool IsTouchingTheGruond()
    {
        if (Physics2D.Raycast(this.transform.position, Vector2.down, 1.5f, groundMask)) {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Mover(float mover)
    {
        Vector3 velocidadObjetivo = new Vector2(mover, rigidboby.velocity.y);
        rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, velocidadObjetivo, ref velocidad, suavizadoDeMovimiento);

        if(mover > 0 && !mirandoDerecha)
        {
            //girar
            Girar();
        }
        else if(mover < 0 && mirandoDerecha)
        {
            //girar
            Girar();
        }
    }

    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    public void Die()
    {
        float travelledDistance = GetTravelledDistance();
        float previuosMaxDistance = PlayerPrefs.GetFloat("maxscore", 0f);
        if(travelledDistance> previuosMaxDistance)
        {
            PlayerPrefs.SetFloat("maxscore", travelledDistance);
        }
        this.animator.SetBool(STATE_ALIVE, false);
        GameManager.sharedInstance.GameOver();
    }

    public void CollectHelath(int points)
    {
        this.healthPonits += points;
        if(this.healthPonits >+MAX_HEALTH)
        {
            this.healthPonits = MAX_HEALTH;
        }

        if(this.healthPonits<= 0)
        {
            Die();
        }
    }
    public void CollectMana(int points)
    {
        this.manaPoints += points;
        if(this.manaPoints >= MAX_MANA)
        {
            this.manaPoints = MAX_MANA;
        }
    }

    public int GetHealth()
    {
        return healthPonits;
    }
    public int GetMana()
    {
        return manaPoints;
    }

    public float GetTravelledDistance()
    {
        return this.transform.position.x - startPosition.x;
    }
}
