using UnityEngine;
using System.Collections;

public class Zone : MonoBehaviour {

	private BoxCollider2D monCollider;

	private void Awake()
	{
		monCollider = this.transform.GetComponent<BoxCollider2D> ();
	}

	public bool EstDansZone(Transform perso)
	{
		return monCollider.bounds.Contains (perso.position);
	}

	public bool IntersecteZone(BoxCollider2D box)
	{
		return monCollider.bounds.Intersects(box.bounds);
	}
}
