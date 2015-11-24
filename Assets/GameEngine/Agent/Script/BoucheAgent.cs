using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoucheAgent: MonoBehaviour{

	public bool discutionEnCours;
	public List<Transform> personneAborde;
	public Discution sujet;

	public enum Discution{FORET,BOIS};

	protected Transform elocuteur;

	public void Start()
	{
		personneAborde = new List<Transform> ();
		discutionEnCours = false;

	}
	public void Aborder(Transform t, Discution d)
	{
		personneAborde.Add (t);
		elocuteur = t;
		sujet = d;
		discutionEnCours = true;
		StartCoroutine ("AborderCoroutine");
	}
	private IEnumerator AborderCoroutine()
	{
		yield return null;
		print ("Bonjour toi");
		elocuteur.GetComponent<Personnage>().dialogue.SeFaireAborder(this.transform);

	}
	public IEnumerator OublierPersonne()
	{
		yield return new WaitForSeconds (15.0f);
		personneAborde.RemoveAt (0);
	}
	public void SeFaireAborder(Transform t)
	{
		elocuteur = t;
		discutionEnCours = true;
		StartCoroutine ("SeFaireAborderCoroutine");
	}
	private IEnumerator SeFaireAborderCoroutine()
	{
		yield return null;
		print ("Bien le bonjour");
		elocuteur.GetComponent<Personnage>().dialogue.DebuterDiscution();
	}
	public void DebuterDiscution ()
	{
		StartCoroutine ("DebuterDiscutionCoroutine");
	}
	protected IEnumerator DebuterDiscutionCoroutine()
	{
		yield return null;
		print (sujet);
		if (elocuteur.GetComponent<Personnage> ().dialogue.Demander (sujet)) {
			elocuteur.GetComponent<Personnage> ().dialogue.CommencerNegociation();
		} else {
			print("ok crawoud");
			FinirDiscution();
		}
	}
	public bool Demander(Discution s)
	{
		if(s==Discution.BOIS)
		{
			if(this.transform.GetComponent<Personnage>().inventaire.AObjet("Bois")!=null)
			{
				print("J'ai");
				discutionEnCours=false;
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
		StartCoroutine ("OublierPersonne");

	}
	public void CommencerNegociation()
	{
		int nbrObjetsMarchande = 0;
		int prix = 5;
		print ("Jte propose "+prix + "€");
		int nbrObjetPropose=this.transform.GetComponent<Personnage>().inventaire.CombienObjet("Bois");
		if ((nbrObjetsMarchande = elocuteur.GetComponent<Personnage> ().dialogue.ProposerPrix (prix,nbrObjetPropose)) > 0) {
			print("bah tient prend en" + nbrObjetsMarchande);
			this.transform.GetComponent<Personnage>().inventaire.echanger("Bois",nbrObjetsMarchande,prix, elocuteur);
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








