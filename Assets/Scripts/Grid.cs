using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    Vector3[,] cellCenters;
    Vector2Int gridSize;
    float skew;
    Vector2 cellSize;
    Vector2 originPosition;

    public Grid(Vector2Int gridSize, Vector2 cellSize, Vector2 originPosition, float skew)
    {
        this.gridSize = gridSize;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        this.skew = skew;

        cellCenters = new Vector3[gridSize.x, gridSize.y];
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector3 cellCenterPosition = GetCellPosition(x, y) + cellSize * 0.5f + new Vector2(y * skew, 0);
                cellCenters[x, y] = cellCenterPosition;
            }
        }
    }

    Vector2 GetCellPosition(int x, int y)
    {
        return new Vector2(x * cellSize.x, y * cellSize.y) + originPosition;
    }

    public Vector2 GetCellCenterPosition(int x, int y)
    {
        return cellCenters[x, y];
    }

    public Vector2Int GetCellByPosition(Vector2 position)
    {
        int y = 0;
        while ((cellSize.y * (y + 1) < position.y - originPosition.y) && (y < gridSize.y - 1))
        {
            y++;
        }

        int x = 0;
        while ((y * skew + cellSize.x * (x + 1) < position.x - originPosition.x) && (x < gridSize.x - 1))
        {
            x++;
        }        

        return new Vector2Int(x, y);
    }
}
