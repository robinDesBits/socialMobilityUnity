using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BarreVie : MonoBehaviour {


	public List<Sprite> etatsBarre;


	void Start () {
		StartCoroutine ("CheckEtat");
	}
	
	public IEnumerator CheckEtat()
	{
		int energie = this.transform.parent.GetComponent<Personnage> ().energie;
		yield return new WaitForSeconds (10);
		if (energie <= 25) {
			this.transform.GetComponent<SpriteRenderer>().sprite=etatsBarre[0];
		} else if (energie <= 50) {
			this.transform.GetComponent<SpriteRenderer>().sprite=etatsBarre[1];
		} else if (energie <= 75) {
			this.transform.GetComponent<SpriteRenderer>().sprite=etatsBarre[2];
		} else {
			this.transform.GetComponent<SpriteRenderer>().sprite=etatsBarre[3];
		}

		if (energie == 0) {
			this.gameObject.SetActive(false);
		} else {
			StartCoroutine ("CheckEtat");
		}
	}
}
