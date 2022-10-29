using System.Collections.Generic;
using UnityEngine;

// Creates and manages a tether

public class Gun : MonoBehaviour
{
	public Tether ActiveTether { get; private set; }

	protected MagneticEntity magEntity;

	[HideInInspector]
	public float Strength = 80f;
	[HideInInspector]
	public float DetectionRadius = 5f;

	private void Awake()
	{
		magEntity = GetComponent<MagneticEntity>();
	}

	public MagneticEntity[] GetTargets(Vector3 targetPos)
	{
		MagneticEntity[] magneticEntities = FindAvailableTargetsInRadius(targetPos);

		return magneticEntities;
	}

	// Find an anchor and create a tether
	public Anchor Fire(Vector3 targetPos, bool pull)
	{
		//print("Firing Gun");

		Anchor target = FindClosestAnchorInRadius(targetPos);

		if (target == null)
		{
			return null;
		}

		Anchor self = magEntity.GetAnchor(transform.position);

		if (ActiveTether != null)
		{
			ActiveTether.Detach();
			ActiveTether = null;
		}

		ActiveTether = Tether.CreateTether(self, target);
		ActiveTether.Strength = Strength * (pull ? 1f : -1f);

		return target;
	}

	public void Detach()
	{
		//print("Detaching Gun");

		if (ActiveTether != null)
		{
			ActiveTether.Detach();
			ActiveTether = null;
		}
	}

	// Finds all magnetic entities within range of the target position
	MagneticEntity[] FindAvailableTargetsInRadius(Vector3 targetPos)
	{
		// First, find all potential targets by checking for physics objects
		Collider[] potentialTargets = Physics.OverlapSphere(targetPos, DetectionRadius);
		if (potentialTargets.Length == 0)
		{
			return null;
		}

		// Trim potential targets down to objects with Anchors and find the closest
		List<MagneticEntity> targets = new List<MagneticEntity>();
		foreach (Collider potentialTarget in potentialTargets)
		{
			// Check if the potential target is a magnetic entity that *isn't* the entity attached to this gun
			MagneticEntity targetEntity = potentialTarget.GetComponent<MagneticEntity>();
			if (targetEntity != null && targetEntity != magEntity)
			{
				targets.Add(targetEntity);
			}
		}

		return targets.ToArray();
	}

	// Finds the closest Anchor to the target position within range
	Anchor FindClosestAnchorInRadius(Vector3 targetPos)
	{
		// First, find all potential targets by checking for physics objects
		Collider[] potentialTargets = Physics.OverlapSphere(targetPos, DetectionRadius);
		if (potentialTargets.Length == 0)
		{
			return null;
		}

		// Trim potential targets down to objects with Anchors and find the closest
		Anchor closestTarget = null;
		float closestDist = Mathf.Infinity;
		foreach (Collider potentialTarget in potentialTargets)
		{
			// Check if the potential target is a magnetic entity that *isn't* the entity attached to this gun
			MagneticEntity targetEntity = potentialTarget.GetComponent<MagneticEntity>();
			if (targetEntity != null && targetEntity != magEntity)
			{
				// Check to see if the target entity's given anchor is the closest entity found so far
				Anchor targetAnchor = targetEntity.GetAnchor(targetPos);
				float dist = Vector3.Distance(targetAnchor.Position, targetPos);
				if (dist < closestDist)
				{
					// Target is closer, store its anchor and distance
					closestTarget = targetAnchor;
					closestDist = dist;
				}
			}
		}

		return closestTarget;
	}
}
