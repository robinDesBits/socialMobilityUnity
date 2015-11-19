using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoucheAgent: MonoBehaviour{

	public bool discutionEnCours;
	public List<Transform> personneAborde;

	public void Start()
	{
		personneAborde = new List<Transform> ();
		discutionEnCours = false;

	}
	public void Aborder(Transform t)
	{
		print ("Bonjour toi");
		personneAborde.Add (t);
		discutionEnCours = true;
		t.GetComponent<Personnage>().dialogue.SeFaireAborder();
		FinirDiscution ();
	}
	public IEnumerator OublierPersonne()
	{
		yield return new WaitForSeconds (15.0f);
		personneAborde.RemoveAt (0);
	}
	public void SeFaireAborder()
	{
		print (" Bien le bonjour");
		discutionEnCours = true;
		discutionEnCours = false;
		//StartCoroutine ("Discuter");

	}
	/*public IEnumerator Discuter()
	{
	}
	*/
	public void FinirDiscution()
	{
		discutionEnCours = false;
		StartCoroutine ("OublierPersonne");

	}

}
