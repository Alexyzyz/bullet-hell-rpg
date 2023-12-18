using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public const float SPEED = 5f;
	public const float RADIUS = 10f;

    /// <summary>
    /// Collisions in the center cell are checked every frame.
    /// Collisions in the cardinally neighboring cells are checked every other frame.
    /// Collisions in the diagonally neighboring cells are checked every third frame.
    /// </summary>
    private int collisionCheckNumber = 0;

    private Vector2Int cell = Vector2Int.zero;

    private void Update()
    {
        cell = BattleBoardManager.Instance.GetCellPosition(transform.position);

        HandleMovement();
        CheckCollision();
    }

    private void HandleMovement()
    {
        float xAxis = Input.GetKey(KeyCode.RightArrow) ? 1 : 0 - (Input.GetKey(KeyCode.LeftArrow) ? 1 : 0);
        float yAxis = Input.GetKey(KeyCode.UpArrow) ? 1 : 0 - (Input.GetKey(KeyCode.DownArrow) ? 1 : 0);

        if (xAxis == 0 && yAxis == 0) return;

        Vector2 direction = new Vector2(xAxis, yAxis).normalized;

        transform.position += SPEED * Time.deltaTime * (Vector3)direction;

        cell = BattleBoardManager.Instance.GetCellPosition(transform.position);
    }

    private void CheckCollision()
    {
        int collisionChecks = 0;

        CheckCell(0, 0);

        if (collisionCheckNumber % 2 == 0)
        {
            // Check cardinally neighboring cells every other frame
            CheckCell(1, 0);
            CheckCell(0, 1);
            CheckCell(-1, 0);
            CheckCell(0, -1);
        }

        if (collisionCheckNumber % 3 == 0)
        {
            // Check diagonally neighboring cells every third frame
            CheckCell(1, 1);
            CheckCell(-1, 1);
            CheckCell(1, -1);
            CheckCell(-1, -1);
        }

        void CheckCell(int xOffset, int yOffset)
        {
            int xx = cell.x + xOffset;
            int yy = cell.y + yOffset;

            if (BattleBoardManager.Instance.CellIsOutsideGrid(new Vector2Int(xx, yy))) return;

            IPartitionedInstance currentPotentialCollider = BattleBoardManager.Instance.grid[yy, xx];

            for (; currentPotentialCollider != null; currentPotentialCollider = currentPotentialCollider.Next)
            {
                collisionChecks++;
                if (!BattleBoardManager.Instance.RectsAreColliding(
                    transform.position, new Vector2(RADIUS, RADIUS),
                    currentPotentialCollider.Position, currentPotentialCollider.Size))
                {
                    continue;
                }

                if (!BattleBoardManager.Instance.CirclesAreColliding(
                    transform.position, RADIUS,
                    currentPotentialCollider.Position, currentPotentialCollider.Radius))
                {
                    continue;
                }

                currentPotentialCollider.OnDestroyed();
            }
        }

        collisionCheckNumber = (collisionCheckNumber + 1) % 6;

        DebugText.Instance.Print("Collision checks this frame :: " + collisionChecks);
    }

    private void PrintDebug()
    {

    }

}
