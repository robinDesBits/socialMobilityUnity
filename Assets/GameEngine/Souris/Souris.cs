using UnityEngine;
using System.Collections;

public class Souris : MonoBehaviour {

	public Transform carte;
	private bool defilement;
	Vector3 posSourisPrec;
	void Start()
	{
		defilement = false;
		posSourisPrec= Camera.main.ScreenToViewportPoint (Input.mousePosition);

	}
	// Update is called once per frame
	void Update () {

		if (Input.GetAxis ("Mouse ScrollWheel") != 0) {
			carte.localScale += Input.GetAxis ("Mouse ScrollWheel") * new Vector3(1,1,1);
			
		}
		if (Input.GetMouseButtonDown (0)) {
			posSourisPrec= Camera.main.ScreenToViewportPoint (Input.mousePosition);

			defilement=true;
		}
		if (Input.GetMouseButtonUp (0)) {
			defilement=false;
		}
		if (!defilement) {
			return;
		}
		Vector3 posSouris=Camera.main.ScreenToViewportPoint (Input.mousePosition)-posSourisPrec;
		posSourisPrec= Camera.main.ScreenToViewportPoint (Input.mousePosition);
		if (posSouris.x > 0) {
			Vector3 posTemp;
			posTemp = carte.position;
			posTemp.x += posSouris.x*carte.localScale.x;
			carte.position = posTemp;
		}
		if (posSouris.y > 0) {
			Vector3 posTemp;
			posTemp = carte.position;
			posTemp.y += posSouris.y*carte.localScale.x;
			carte.position = posTemp;
		}
		if (posSouris.x < 0) {
			Vector3 posTemp;
			posTemp = carte.position;
			posTemp.x += posSouris.x*carte.localScale.x;
			carte.position = posTemp;
		}
		if (posSouris.y < 0) {
			Vector3 posTemp;
			posTemp = carte.position;
			posTemp.y += posSouris.y*carte.localScale.x;
			carte.position = posTemp;
		}

	}
}
