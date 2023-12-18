using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPartitionedInstance
{

	/// <summary>
	/// The GameObject this interface is attached to.
	/// </summary>
	public GameObject GameObject { get; }

	/// <summary>
	/// Real position.
	/// </summary>
	public Vector2 Position { get; }

	/// <summary>
	/// Rect for approximate distance checking.
	/// </summary>
	public Vector2 Size { get; }

	/// <summary>
	/// Radius for precise distance checking. Works best for circular instances.
	/// </summary>
	public float Radius { get; }

	/// <summary>
	/// Previous linked instance.
	/// </summary>
	public IPartitionedInstance Prev { get; set; }

	/// <summary>
	/// Next linked instance.
	/// </summary>
	public IPartitionedInstance Next { get; set; }

	/// <summary>
	/// Call this whenever the instance moves to update its spatial partition.
	/// </summary>
	public void OnMove(Vector2 oldPos);

	/// <summary>
	/// Call this when the instance is destroyed to update the spatial partitioning.
	/// </summary>
	public void OnDestroyed();
	
}
