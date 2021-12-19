using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using HinputClasses;

public class Player : MonoBehaviour
{
    private GameController gameController;

    public ESpriteDirection direction;

    public SpriteRenderer WeaponRenderer;

    public Disable WeaponDisabler;
    public Collider2D PlayerCollectionCollider;

    private Rigidbody2D rigidbody2DComponent;
    private Animator animator;
    private SpriteRenderer playerSpriteRenderer;
    private BoxCollider2D boxCollider;

    public float Speed;

    public UserInterface UserInterfaceScript;

    private bool canAttack;
    public const float secondBetweenAttacks = 0.5f;

    public int NumberOfApplesRed;
    public int NumberOfBerriesPurple;

    private bool areCollecting;
    private List<int> collectionInstances;

    static public int PlayerY = 0;

    private int heartContainers;
    private int health;

    private int focus;

    private bool hurtFrames;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            gameController = GameController.Instance;
        }
        catch
        {
            Debug.LogError($"{nameof(Player)} Cannot find: {typeof(GameController)}");
        }
        try
        {
            rigidbody2DComponent = gameObject.GetComponent<Rigidbody2D>();
        }
        catch
        {
            Debug.LogError($"{nameof(Player)} Cannot find: {typeof(Rigidbody2D)}");
        }
        try
        {
            playerSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }
        catch
        {
            Debug.LogError($"{nameof(Player)} Cannot find: {typeof(SpriteRenderer)}");
        }
        try
        {
            animator = gameObject.GetComponent<Animator>();
        }
        catch
        {
            Debug.LogError($"{nameof(Player)} Cannot find: {typeof(Animator)}");
        }
        boxCollider = gameObject.GetComponent<BoxCollider2D>();

        direction = ESpriteDirection.South;

        canAttack = true;

        PlayerCollectionCollider.enabled = false;

        collectionInstances = new List<int>();

        if (WeaponRenderer == null)
        {
            throw new System.NullReferenceException($"{nameof(Player)} Cannot find: {nameof(WeaponRenderer)}:{typeof(SpriteRenderer)}");
        }

        if (PlayerCollectionCollider == null)
        {
            throw new System.NullReferenceException($"{nameof(Player)} Cannot find: {nameof(PlayerCollectionCollider)}:{typeof(Collider2D)}");
        }

        health = 0;
        heartContainers = 0;
        focus = 0;
        AdjustHealthContainers(5);
        AdjustHealth(3);
        AdjustFocus(0);
    }

    // Update is called once per frame
    void Update()
    {
        //ActivateFocusMode();

        
        PlayerY = (int)transform.position.y;

        if (canAttack && Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Attack");
            canAttack = false;
            StartCoroutine("CanAttackNow");
            this.WeaponRenderer.enabled = true;
            WeaponDisabler.DisableEverything(true);

            if (!areCollecting)
            {
                PlayerCollectionCollider.enabled = true;
                areCollecting = true;
                collectionInstances.Clear();
                StartCoroutine("StopCollection");
            }
        }

        Pickups();
    }

    private void FixedUpdate()
    {
        PlayerMovement();
        playerSpriteRenderer.sortingOrder = ((int)(transform.position.y * 10) * -1) + 100;
    }

    private void PlayerMovement()
    {
        float h = Input.GetAxis("Horizontal");
        /*h += Input.GetKeyDown(KeyCode.D) ? Time.deltaTime : 0;
        h -= Input.GetKeyDown(KeyCode.A) ? Time.deltaTime : 0;
        h = h > 1 ? 1 : h;
        h = h < -1 ? -1 : h;*/

        float v = Input.GetAxis("Vertical");
        /*v += Input.GetKeyDown(KeyCode.W) ? Time.deltaTime : 0;
        v -= Input.GetKeyDown(KeyCode.S) ? Time.deltaTime : 0;
        v = v > 1 ? 1 : v;
        v = v < -1 ? -1 : v;*/

        //Debug.Log($"{h} {v}");
        if (h > 0)
        {
            direction = ESpriteDirection.East;
            WeaponRenderer.sortingOrder = playerSpriteRenderer.sortingOrder - 1;
        }
        else if (h < 0)
        {
            direction = ESpriteDirection.West;
            WeaponRenderer.sortingOrder = playerSpriteRenderer.sortingOrder - 1;
        }
        else if (v < 0)
        {
            direction = ESpriteDirection.South;
            WeaponRenderer.sortingOrder = playerSpriteRenderer.sortingOrder + 1;
        }
        else if (v > 0)
        {
            direction = ESpriteDirection.North;
            WeaponRenderer.sortingOrder = playerSpriteRenderer.sortingOrder - 1;
        }
        animator.SetInteger("Direction", (int)direction);

        int speed = 800;
        float x = h * speed * Time.deltaTime;
        float y = v * speed * Time.deltaTime;

        var movement = new Vector2(x, y);
        rigidbody2DComponent.AddForce(movement);

        animator.SetFloat("Speed", rigidbody2DComponent.velocity.magnitude);
        Speed = rigidbody2DComponent.velocity.magnitude;
    }

    void Pickups()
    {
        if (areCollecting)
        {
            ContactFilter2D contactFilter = new ContactFilter2D
            {
                useTriggers = true
            };
            List<Collider2D> results = new List<Collider2D>();
            int numberOfResults = PlayerCollectionCollider.OverlapCollider(contactFilter, results);
            foreach (var collider2d in results)
            {
                if(!collectionInstances.Contains(collider2d.gameObject.GetInstanceID()))
                {
                    collectionInstances.Add(collider2d.gameObject.GetInstanceID());
                    if (collider2d.gameObject.CompareTag("Pickup-Tree-Red"))
                    {
                        NumberOfApplesRed += 3;
                        AdjustHealth(3);
                    }
                    else if(collider2d.gameObject.CompareTag("Pickup-Bush-Red"))
                    {
                        NumberOfApplesRed += 1;
                        AdjustHealth(1);
                    }
                    else if (collider2d.gameObject.CompareTag("Pickup-Bush-Purple"))
                    {
                        NumberOfBerriesPurple += 1;
                        AdjustFocus(1);
                    }
                    else if (collider2d.gameObject.CompareTag("Pickup-Tree-Purple"))
                    {
                        NumberOfBerriesPurple += 5;
                        AdjustFocus(5);
                    }
                }
                
            }
        }
    }

    void ActivateFocusMode()
    {
        if(Input.GetButtonUp("Fire2"))
        {

            //gameController.Scene.ToggleCamera();
        }
    }

    private IEnumerator CanAttackNow()
    {
        yield return new WaitForSeconds(Player.secondBetweenAttacks);
        canAttack = true;
        this.WeaponRenderer.enabled = false;
        WeaponDisabler.DisableEverything(false);
    }

    private IEnumerator StopCollection()
    {
        yield return new WaitForSeconds(Player.secondBetweenAttacks);
        PlayerCollectionCollider.enabled = false;
        areCollecting = false;
    }

    private void AdjustHealth(int number)
    {
        int newHealth = health + number;
        health = newHealth > heartContainers ? heartContainers : newHealth;
        health = newHealth < 0 ? 0 : newHealth;

        UserInterfaceScript.UpdateHealth(health);
        
    }

    private void AdjustHealthContainers(int number)
    {
        bool currentlyMoreHealth = health > heartContainers;
        int newHealth = heartContainers + number;
        heartContainers = newHealth > 15 ? heartContainers : newHealth;
        heartContainers = newHealth < 0 ? 0 : newHealth;

        if(currentlyMoreHealth && health < heartContainers)
        {
            health = heartContainers;
        }

        UserInterfaceScript.UpdateHeartContainers(heartContainers);
    }

    private void AdjustFocus(int number)
    {
        int newFocus = focus + number;
        focus = newFocus > 100 ? 100 : newFocus;
        focus = newFocus < 0 ? 0 : newFocus;

        UserInterfaceScript.AdjustFocus(number);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.isTrigger)
        {
            return;
        }

        if(collision.gameObject.CompareTag("Enemy-Bat"))
        {
            TakeDamage(-1);
            
        }
    }

    bool TakeDamage(int number)
    {
        bool didTakeDamage = false;
        if(!hurtFrames)
        {
            didTakeDamage = true;
            AdjustHealth(number);
            hurtFrames = true;
            StartCoroutine("AllowDamageAgain");
        }

        return didTakeDamage;
    }

    private IEnumerator AllowDamageAgain()
    {
        yield return new WaitForSeconds(1);
        hurtFrames = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(boxCollider.IsTouching(collision))
        {
            if (collision.gameObject.CompareTag("Level"))
            {
                int.TryParse(collision.gameObject.name, out int level);
                gameController.Scene.MoveLevel(level);
            }
        }

    }


}
