using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static bool isBombPlaced;

    [SerializeField] Sprite rightSprite;
    [SerializeField] Sprite leftSprite;
    [SerializeField] Sprite upSprite;
    [SerializeField] Sprite downSprite;

    [SerializeField] float moveSpeed;
    [SerializeField] GameObject bomb;
    [SerializeField] Vector2Int spawnCell;
    [SerializeField] Level level;

    Rigidbody2D rb;
    Vector2 input;
    SpriteRenderer spriteRenderer;
    bool isDead;
    PlayerInputActions playerInput;

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        playerInput = new PlayerInputActions();

        isBombPlaced = false;
    }

    private void Start()
    {
        Vector2 initPosition = Level.grid.GetCellCenterPosition(spawnCell.x, spawnCell.y);
        transform.position = initPosition;
    }

    private void Update()
    {
        if (!isDead)
        {
            Vector2 lastInput = input;
            input = playerInput.Player.Move.ReadValue<Vector2>();

            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            {
                input.y = 0;
            }
            else if (Mathf.Abs(input.x) < Mathf.Abs(input.y))
            {
                input.x = 0;
            }
            else
            {
                if (lastInput.x != 0)
                {
                    input.y = 0;
                }
                else
                {
                    input.x = 0;
                }
            }

            input.Normalize();

            if (input.x > 0)
            {
                spriteRenderer.sprite = rightSprite;
            }
            else if (input.x < 0)
            {
                spriteRenderer.sprite = leftSprite;
            }

            if (input.y > 0)
            {
                spriteRenderer.sprite = upSprite;
            }
            else if (input.y < 0)
            {
                spriteRenderer.sprite = downSprite;
            }
        }
    }

    void FixedUpdate()
    {
        Vector2Int currentCell = Level.grid.GetCellByPosition(rb.position);
      
        if (input.y != 0 || input.x != 0)
        {
            Vector2 targetPosition = Level.grid.GetCellCenterPosition(currentCell.x + (int)input.x, currentCell.y + (int)input.y);
            Vector2 direction = (targetPosition - rb.position).normalized;

            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    public void OnPlaceBomb(InputValue inputValue)
    {
        if (!isBombPlaced)
        {
            isBombPlaced = true;

            Vector2Int currentCell = Level.grid.GetCellByPosition(transform.position);
            Vector2 bombPosition = Level.grid.GetCellCenterPosition(currentCell.x, currentCell.y);

            Instantiate(bomb, bombPosition, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDead)
        {
            if (collision.gameObject.tag == "Explosion" || collision.gameObject.tag == "Enemy")
            {
                isDead = true;

                level.Reload();

                Destroy(gameObject);
            }
        }
    }
}
