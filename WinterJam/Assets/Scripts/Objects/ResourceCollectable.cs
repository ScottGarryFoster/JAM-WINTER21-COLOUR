using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCollectable : MonoBehaviour
{
    public bool SingleTile = true;

    public Sprite ConsumedSprite;

    private Sprite resourceSprite;

    private SpriteRenderer spriteRendererComponent;

    private Collider2D colliderComponent;

    private GameController gameController;

    private AudioSource audioSource;

    private bool collected;
    // Start is called before the first frame update
    void Start()
    {
        spriteRendererComponent = gameObject.GetComponent<SpriteRenderer>();
        colliderComponent = gameObject.GetComponent<Collider2D>();
        audioSource = gameObject.GetComponent<AudioSource>();
        resourceSprite = spriteRendererComponent.sprite;

        gameController = GameController.Instance;
        spriteRendererComponent.sortingOrder = ((int)transform.position.y * -1) + 100;
    }

    void Update()
    {
        if(SingleTile)
        {
            return;
        }
        if(Player.PlayerY < (int)transform.position.y)
        {
            spriteRendererComponent.sortingOrder = ((int)(transform.position.y * 10) * -1) + 100;
        }
        else
        {
            spriteRendererComponent.sortingOrder = ((int)(transform.position.y * 10) * -1) + 101;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collected)
        {
            return;
        }
        bool didCollect = false;
        didCollect = collision.gameObject.CompareTag("Player-Sword");

        if(didCollect)
        {
            collected = true;
            spriteRendererComponent.sprite = ConsumedSprite;
            colliderComponent.enabled = false;
            gameController.AudioManager.PlayDiegeticClip(EDiegeticClipType.HarvestTree, audioSource);
        }
    }
}
