using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class Level : MonoBehaviour
{
    public static Grid grid;
    public static int enemiesOnLevel;

    [SerializeField] Transform gridOrigin; 
    [SerializeField] Vector2Int gridSize;
    [SerializeField] Vector2 gridCellSize;
    [SerializeField] float gridSkew;

    [SerializeField] MMFeedbacks reloadFeedbacks;

    bool isReloading;

    void Awake()
    {
        grid = new Grid(gridSize, gridCellSize, gridOrigin.position, gridSkew);
    }

    private void Start()
    {
        enemiesOnLevel = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    private void Update()
    {
        if(enemiesOnLevel == 0)
        {
            Reload();
        }
    }

    public void Reload() {
        if (!isReloading)
        {
            isReloading = true;

            reloadFeedbacks.PlayFeedbacks();
        }
    }
}
