using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class Bomb : MonoBehaviour
{
    [SerializeField] MMFeedbacks preExplosionFeedbacks;
    [SerializeField] MMFeedbacks explosionFeedbacks;
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] LayerMask blockMask;
    [SerializeField] float expolosionsDelay;

    private void Awake()
    {
        preExplosionFeedbacks.Initialization();
        explosionFeedbacks.Initialization();
    }

    void Start()
    {
        preExplosionFeedbacks.PlayFeedbacks();
    }

    public void OnComplete()
    {
        GetComponent<SpriteRenderer>().enabled = false;

        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        explosionFeedbacks.PlayFeedbacks();

        StartCoroutine(CreateExplosions(transform.up));
        StartCoroutine(CreateExplosions(-transform.up));
        StartCoroutine(CreateExplosions(transform.right));
        StartCoroutine(CreateExplosions(-transform.right));

        Player.isBombPlaced = false;
       
        Destroy(gameObject, 3f);
    }

    private IEnumerator CreateExplosions(Vector3 direction)
    {
        for (int i = 1; i < 3; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, i, blockMask);

            if (hit.collider)
            {
                break; 
            }
            else
            {
                Vector2Int currentCell = Level.grid.GetCellByPosition(transform.position + (i * direction));
                Vector2 explosionPosition = Level.grid.GetCellCenterPosition(currentCell.x, currentCell.y);

                Instantiate(explosionPrefab, explosionPosition, Quaternion.identity);
            }

            yield return new WaitForSeconds(expolosionsDelay);
        }

    }
}
