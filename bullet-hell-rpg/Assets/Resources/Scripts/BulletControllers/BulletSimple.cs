using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSimple : MonoBehaviour, IPartitionedInstance
{

    public GameObject GameObject { get; private set; }
    public Vector2 Position { get; private set; }
    public Vector2 Size { get; private set; } = new Vector2(1, 1);
    public float Radius { get; private set; } = 1;

    public IPartitionedInstance Prev { get; set; }
    public IPartitionedInstance Next { get; set; }

    private Vector2 speed;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        GameObject = gameObject;

        spriteRenderer = transform.Find("Image").GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        HandleMovement();
    }

    // Publics

    public void SetUp(Vector2 speed)
    {
        this.speed = speed;
    }

    // Privates

    private void HandleMovement()
    {
        Vector2 oldPos = Position;
        Vector2 newPos = Position + speed * Time.deltaTime;        

        if (newPos.x > BattleBoardManager.Instance.boardRight ||
            newPos.x < BattleBoardManager.Instance.boardLeft)
        {
            speed.x *= -1;
        }

        if (newPos.y > BattleBoardManager.Instance.boardTop ||
            newPos.y < BattleBoardManager.Instance.boardBottom)
        {
            speed.y *= -1;
        }

        newPos.x = Mathf.Clamp(newPos.x, BattleBoardManager.Instance.boardLeft, BattleBoardManager.Instance.boardRight);
        newPos.y = Mathf.Clamp(newPos.y, BattleBoardManager.Instance.boardBottom, BattleBoardManager.Instance.boardTop);

        Position = newPos;
        transform.position = Position;

        OnMove(oldPos);
    }

    // Interface methods

    public void OnMove(Vector2 oldPos)
    {
        BattleBoardManager.Instance.HandleMovedInstance(this, oldPos);

        Vector2Int cell = BattleBoardManager.Instance.GetCellPosition(Position);
        spriteRenderer.color = new Color(
            cell.x / (float)BattleBoardManager.PARTITION_HOR_COUNT,
            cell.y / (float)BattleBoardManager.PARTITION_VER_COUNT,
            0f);
    }

    public void OnDestroyed()
    {
        BattleBoardManager.Instance.HandleDestroyedInstance(this);
    }

}
