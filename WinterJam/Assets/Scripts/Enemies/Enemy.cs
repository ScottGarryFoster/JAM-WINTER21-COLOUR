using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Enemy : MonoBehaviour
{
    public int Speed;
    public int AttackSpeed;
    public int EnemyHealth;

    private Animator animator;

    private BoxCollider2D boxCollider2D;
    private Rigidbody2D rigidbody2DComponent;

    private ESpriteDirection currentDirection;
    private ESpriteDirection currentDirection2;
    private float timeUntilChangeDirection;
    private float idleMovementDistanceTraveled;

    private GameObject playerObject;
    private bool areInRange;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        rigidbody2DComponent = gameObject.GetComponent<Rigidbody2D>();
        boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!areInRange)
        {
            MoveRandomly();
            animator.SetBool("Attacking", false);
        }
        else
        {
            MoveToPlayer();
        }
    }

    private void MoveToPlayer()
    {
        Vector3 direction = (playerObject.transform.position - transform.position).normalized;

        float xDiff = System.Math.Abs(playerObject.transform.position.x - transform.position.x);
        float yDiff = System.Math.Abs(playerObject.transform.position.y - transform.position.y);

        if (yDiff > xDiff)
        {
            if (direction.y > 0)
            {
                animator.SetInteger("Direction", 0);
            }
            else if (direction.y < 0)
            {
                animator.SetInteger("Direction", 2);
            }
        }
        else
        {
            if (direction.x > 0)
            {
                animator.SetInteger("Direction", 1);
            }
            else if (direction.x < 0)
            {
                animator.SetInteger("Direction", 3);
            }
        }

        rigidbody2DComponent.AddForce(direction * AttackSpeed * Time.deltaTime);
        animator.SetBool("Attacking", true);
    }

    private void MoveRandomly()
    {
        if (timeUntilChangeDirection <= 0)
        {
            GetNewRandomDirection();
        }
        if (rigidbody2DComponent.velocity.magnitude < 0.1f)
        {
            idleMovementDistanceTraveled += Time.deltaTime;
            if (idleMovementDistanceTraveled > 1)
            {
                GetNewRandomDirection();
            }
        }


        timeUntilChangeDirection -= Time.deltaTime;

        float x = 0;
        float y = 0;
        if (currentDirection == ESpriteDirection.North || currentDirection2 == ESpriteDirection.North)
        {
            y += Speed * Time.deltaTime;
        }
        else if (currentDirection == ESpriteDirection.South || currentDirection2 == ESpriteDirection.South)
        {
            y -= Speed * Time.deltaTime;
        }

        if (currentDirection == ESpriteDirection.East || currentDirection2 == ESpriteDirection.East)
        {
            x += Speed * Time.deltaTime;
        }
        else if (currentDirection == ESpriteDirection.West || currentDirection2 == ESpriteDirection.West)
        {
            x -= Speed * Time.deltaTime;
        }
        var v = new Vector2(x, y);


        rigidbody2DComponent.AddForce(v);

        animator.SetInteger("Direction", (int)currentDirection);
    }

    void GetNewRandomDirection()
    {
        int randomDirection = Random.Range(0, 4);
        currentDirection = (ESpriteDirection)randomDirection;

        int stray = Random.Range(0, 3);
        if(stray > 0)
        {
            switch(currentDirection)
            {
                case ESpriteDirection.North:
                case ESpriteDirection.South:
                    switch(stray)
                    {
                        case 1:
                        currentDirection2 = ESpriteDirection.East;
                        break;
                        case 2:
                        currentDirection2 = ESpriteDirection.West;
                        break;
                    }
                
                break;

                case ESpriteDirection.East:
                case ESpriteDirection.West:
                switch (stray)
                {
                    case 1:
                    currentDirection2 = ESpriteDirection.North;
                    break;
                    case 2:
                    currentDirection2 = ESpriteDirection.South;
                    break;
                }

                break;
            }
        }
        else
        {
            currentDirection2 = ESpriteDirection.None;
        }

        timeUntilChangeDirection = Random.Range(3, 8);
        idleMovementDistanceTraveled = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            playerObject = collision.gameObject;
            areInRange = true;
        }

        bool haveCollidedWithMainCollider = boxCollider2D.IsTouching(collision);
        if (haveCollidedWithMainCollider)
        {
            if (collision.gameObject.CompareTag("Player-Sword"))
            {
                Vector3 direction = (transform.position - playerObject.transform.position).normalized;
                rigidbody2DComponent.AddForce(direction * (AttackSpeed * 2));
                animator.SetTrigger("TakenDamage");

                TakeDamage(-1);
            }
        }
    }

    private void TakeDamage(int number)
    {
        EnemyHealth += number;
        if(EnemyHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            areInRange = false;
        }
    }
}
