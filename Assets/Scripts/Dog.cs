using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    [SerializeField] Sprite rightSprite;
    [SerializeField] Sprite leftSprite;
    [SerializeField] Sprite upSprite;
    [SerializeField] Sprite downSprite;

    [SerializeField] Sprite rightAngrySprite;
    [SerializeField] Sprite leftAngrySprite;
    [SerializeField] Sprite upAngrySprite;
    [SerializeField] Sprite downAngrySprite;

    [SerializeField] float moveSpeed;
    [SerializeField] Vector2Int[] path;
    [SerializeField] LayerMask playerMask;
    [SerializeField] float fieldOfView;

    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    int targetPoint;
    int lastPoint;
    bool isDead;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void Start()
    {
        Vector2 initPosition = Level.grid.GetCellCenterPosition(path[targetPoint].x, path[targetPoint].y);
        rb.position = initPosition;

        targetPoint = 1;
    }

    void FixedUpdate()
    {
        Vector2 targetPointPosition = Level.grid.GetCellCenterPosition(path[targetPoint].x, path[targetPoint].y);

        Vector2 direction = targetPointPosition - rb.position;
        if (direction.magnitude < 0.1f)
        {
            lastPoint = targetPoint;
            targetPoint++;

            if(targetPoint >= path.Length)
            {
                targetPoint = 0;
            }
        }
        else
        {
            rb.MovePosition(rb.position + direction.normalized * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void Update()
    {
        Vector2 offset = path[targetPoint] - path[lastPoint];

        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + offset.normalized * 0.5f, offset, fieldOfView, playerMask);

        bool isAngry = false;
        if (hit.collider?.tag == "Player")
        {
            isAngry = true;
        }

        if (offset.x > 0)
        {
            if (isAngry)
            {
                spriteRenderer.sprite = rightAngrySprite;
            }
            else
            {
                spriteRenderer.sprite = rightSprite;
            }
        }
        else if (offset.x < 0)
        {
            if (isAngry)
            {
                spriteRenderer.sprite = leftAngrySprite;
            }
            else
            {
                spriteRenderer.sprite = leftSprite;
            }
        }

        if (offset.y > 0)
        {
            if (isAngry)
            {
                spriteRenderer.sprite = upAngrySprite;
            }
            else
            {
                spriteRenderer.sprite = upSprite;
            }
        }
        else if (offset.y < 0)
        {
            if (isAngry)
            {
                spriteRenderer.sprite = downAngrySprite;
            }
            else
            {
                spriteRenderer.sprite = downSprite;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDead)
        {
            if (collision.gameObject.tag == "Explosion")
            {
                isDead = true;

                Level.enemiesOnLevel--;

                Destroy(gameObject);
            }
        }
    }
}
