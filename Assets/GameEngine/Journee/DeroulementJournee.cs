using UnityEngine;
using System.Collections;

public class DeroulementJournee : MonoBehaviour {

	public static bool nuit;
	public GameObject filtreNuit;
	public float multiplicateurVitesseConfig;
	public static float multiplicateurVitesse;
	// Use this for initialization
	void Start () {
		multiplicateurVitesse = multiplicateurVitesseConfig;
		nuit = false;
		StartCoroutine ("Journee");
	}

	void Update()
	{
		multiplicateurVitesse = multiplicateurVitesseConfig;

	}
	public IEnumerator Journee()
	{
		if (nuit) {
			yield return new WaitForSeconds(60*multiplicateurVitesse);

		} else {
			yield return new WaitForSeconds(120*multiplicateurVitesse);
		}
		nuit = !nuit;
		filtreNuit.SetActive(nuit);
		StartCoroutine ("Journee");

	}

}
