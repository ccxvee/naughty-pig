using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;

    SpriteRenderer player;
    int initSortingOrder;

    private void Awake()
    {
        initSortingOrder = spriteRenderer.sortingOrder;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (player == null)
            {
                player = collision.gameObject.GetComponent<SpriteRenderer>();
            }

            if (transform.position.y < player.transform.position.y)
            {
                spriteRenderer.sortingOrder = player.sortingOrder + 1;
            }
            else
            {
                spriteRenderer.sortingOrder = initSortingOrder;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            spriteRenderer.sortingOrder = initSortingOrder;
        }
    }
}
