using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoucheAgent: MonoBehaviour{

	public bool discutionEnCours;
	public List<Transform> personneAborde;

	public bool estMarchand;
	public int prix;

	public Discution sujet;



	public enum Discution{FORET,BOIS};

	protected Transform elocuteur;

	public void Start()
	{
		personneAborde = new List<Transform> ();
		discutionEnCours = false;
		prix = 2;
		StartCoroutine ("finDialogueCoroutine");

	}
	public IEnumerator finDialogueCoroutine()
	{
		yield return new WaitForSeconds (10);
		discutionEnCours = false;
		StartCoroutine("finDialogueCoroutine");
	}
	public void Aborder(Transform t, Discution d)
	{
		if (discutionEnCours) {
			return;
		}
		else {
			personneAborde.Add (t);
			elocuteur = t;
			sujet = d;
			discutionEnCours = true;
			StartCoroutine ("OublierPersonne");
			StartCoroutine ("AborderCoroutine");
		}
	}
	private IEnumerator AborderCoroutine()
	{
		yield return new WaitForSeconds(0.5f*DeroulementJournee.multiplicateurVitesse);
		print ("Bonjour toi");
		elocuteur.GetComponent<Personnage>().dialogue.SeFaireAborder(this.transform);

	}
	public IEnumerator OublierPersonne()
	{
		yield return new WaitForSeconds (5.0f*DeroulementJournee.multiplicateurVitesse);
		if (personneAborde.Count > 0) {
			personneAborde.RemoveAt (0);
		}
		discutionEnCours = false;
	}
	public void SeFaireAborder(Transform t)
	{
		elocuteur = t;
		discutionEnCours = true;
		StartCoroutine ("SeFaireAborderCoroutine");
	}
	private IEnumerator SeFaireAborderCoroutine()
	{
		yield return new WaitForSeconds(0.5f*DeroulementJournee.multiplicateurVitesse);
		print ("Bien le bonjour");
		elocuteur.GetComponent<Personnage>().dialogue.DebuterDiscution();
	}
	public void DebuterDiscution ()
	{
		StartCoroutine ("DebuterDiscutionCoroutine");
	}
	protected IEnumerator DebuterDiscutionCoroutine()
	{
		yield return new WaitForSeconds(0.5f*DeroulementJournee.multiplicateurVitesse);
		print (sujet);
		if (elocuteur.GetComponent<Personnage> ().dialogue.Demander (sujet)) {
			elocuteur.GetComponent<Personnage> ().dialogue.CommencerNegociation();
		} else {
			print("ok crawoud");
			FinirDiscution();
			elocuteur.GetComponent<Personnage> ().dialogue.discutionEnCours=false;

		}
	}
	public bool Demander(Discution s)
	{
		if(s==Discution.BOIS)
		{
			if(this.transform.GetComponent<Personnage>().inventaire.AObjet("Bois")!=null)
			{
				print("J'ai");
				return true;
			}
			else 
			{
				print("J'ai pas");
				discutionEnCours=false;
				return false;
			}
		}
		return false;
	}
	public void FinirDiscution()
	{
		discutionEnCours = false;


	}
	public void CommencerNegociation()
	{
		int nbrObjetsMarchande = 0;
		print ("Jte propose "+prix + "€");
		int nbrObjetPropose=this.transform.GetComponent<Personnage>().inventaire.CombienObjet("Bois");
		if ((nbrObjetsMarchande = elocuteur.GetComponent<Personnage> ().dialogue.ProposerPrix (prix, nbrObjetPropose)) > 0) {
			print ("bah tient prend en" + nbrObjetsMarchande);
			this.transform.GetComponent<Personnage> ().inventaire.echanger ("Bois", nbrObjetsMarchande, prix, elocuteur);
			FinirDiscution ();
			discutionEnCours = false;
			elocuteur.GetComponent<Personnage> ().dialogue.discutionEnCours = false;
		} else {
			FinirDiscution();
			discutionEnCours = false;
			elocuteur.GetComponent<Personnage> ().dialogue.discutionEnCours = false;
		}

	}
	public int ProposerPrix(int prix,int nbrObjetPropose)
	{
		int nbrObjetPris = min (this.transform.GetComponent<Personnage> ().inventaire.argent / prix, nbrObjetPropose);
		print ("ok j'ten prend "+ nbrObjetPris);
		return nbrObjetPris;
	}
	private int min(int a, int b)
	{
		if (a <= b) {
			return a;
		}else
			return b;

	}
}








