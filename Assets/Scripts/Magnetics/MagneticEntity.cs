using System.Collections.Generic;
using UnityEngine;

// A generic entity that can react to magnetic forces and provides an anchor point for tethering

public abstract class MagneticEntity : MonoBehaviour
{
	public abstract Anchor GetAnchor(Vector3 targetPosition);

	protected List<Tether> tethers;

	protected void Update()
	{
		UpdateAnchorage();
		RefreshTethers();
		ReadTethers();
	}

	// Updates self anchor(s)
	protected abstract void UpdateAnchorage();

	// Refreshes list of tethers
	protected abstract void RefreshTethers();

	// Applies force to self based on attached tethers
	protected abstract void ReadTethers();
}