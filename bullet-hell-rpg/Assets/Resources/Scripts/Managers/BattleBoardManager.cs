using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBoardManager : MonoBehaviour
{

    public static BattleBoardManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(this);
        }
    }

    public const int BULLET_POOL_COUNT = 2000;

    public const int PARTITION_HOR_COUNT = 16;
	public const int PARTITION_VER_COUNT = 32;

    public Transform boardTopLeft;
    public Transform boardBottomRight;

    public Transform bulletParent;
    public Transform enemyParent;

    public IPartitionedInstance[,] grid = new IPartitionedInstance[PARTITION_VER_COUNT, PARTITION_HOR_COUNT];

    public float boardTop => boardTopLeft.position.y;
    public float boardBottom => boardBottomRight.position.y;
    public float boardRight => boardBottomRight.position.x;
    public float boardLeft => boardTopLeft.position.x;

    public float cellWidth => (boardRight - boardLeft) / PARTITION_HOR_COUNT;
    public float cellHeight => (boardBottom - boardTop) / PARTITION_VER_COUNT;

    // Publics

    public void AddInstanceToGrid(IPartitionedInstance instance)
    {
        Vector2Int cellPos = GetCellPosition(instance.Position);

        if (CellIsOutsideGrid(cellPos)) return;

        // Set this instance as the frontmost item in the grid cell array

        instance.Prev = null;
        instance.Next = grid[cellPos.y, cellPos.x];

        grid[cellPos.y, cellPos.x] = instance;

        if (instance.Next != null)
        {
            instance.Next.Prev = instance;
        }

    }

    public void HandleMovedInstance(IPartitionedInstance movedInstance, Vector2 prevPos)
    {
        Vector2Int prevCell = GetCellPosition(prevPos);
        Vector2Int nextCell = GetCellPosition(movedInstance.Position);

        if (CellIsOutsideGrid(nextCell)) return;
        if (prevCell == nextCell) return;

        // Adjust neighboring linked bullets

        if (movedInstance.Prev != null)
        {
            movedInstance.Prev.Next = movedInstance.Next;
        }

        if (movedInstance.Next != null)
        {
            movedInstance.Next.Prev = movedInstance.Prev;
        }

        // If it's the head of a list, remove it

        if (grid[prevCell.y, prevCell.x] == movedInstance)
        {
            grid[prevCell.y, prevCell.x] = movedInstance.Next;
        }

        AddInstanceToGrid(movedInstance);
    }

    public void HandleDestroyedInstance(IPartitionedInstance destroyedInstance)
    {
        Vector2Int cell = GetCellPosition(destroyedInstance.Position);

        // Adjust neighboring linked bullets

        if (destroyedInstance.Prev != null)
        {
            destroyedInstance.Prev.Next = destroyedInstance.Next;
        }

        if (destroyedInstance.Next != null)
        {
            destroyedInstance.Next.Prev = destroyedInstance.Prev;
        }

        // If it's the head of a list, remove it

        if (grid[cell.y, cell.x] == destroyedInstance)
        {
            grid[cell.y, cell.x] = destroyedInstance.Next;
        }

        Destroy(destroyedInstance.GameObject);
    }

    // Private



    // Utility

    /// <summary>
    /// Gets the cell coordinate based on the real world position.
    /// </summary>
    public Vector2Int GetCellPosition(Vector2 realPosition)
    {
        int xx = Mathf.FloorToInt((realPosition.x - boardLeft) / cellWidth);
        int yy = Mathf.FloorToInt((realPosition.y - boardTop) / cellHeight);

        return new Vector2Int(xx, yy);
    }

    /// <summary>
    /// Checks if the cell is out of bounds. Better to handle OOB cases properly than clamping cell values.
    /// </summary>
    public bool CellIsOutsideGrid(Vector2Int cell)
    {
        return cell.x < 0 || cell.y < 0 || cell.x > PARTITION_HOR_COUNT - 1 || cell.y > PARTITION_VER_COUNT - 1;
    }

    /// <summary>
    /// Checks if two rects are colliding. Useful for approximate collision checking.
    /// </summary>
    public bool RectsAreColliding(Vector2 firstPos, Vector2 firstSize, Vector2 secondPos, Vector2 secondSize)
    {
        Rect firstRect = new Rect(firstPos, firstSize);
        Rect secondRect = new Rect(secondPos, secondSize);

        return firstRect.Overlaps(secondRect);
    }

    /// <summary>
    /// Checks if two circles are colliding. Useful for precise collision checking.
    /// </summary>
    public bool CirclesAreColliding(Vector2 firstPos, float firstRadius, Vector2 secondPos, float secondRadius)
    {
        return (secondPos - firstPos).magnitude <= firstRadius + secondRadius;
    }
	
}
