using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Batiment : Zone {

	protected ListeBatiment plan;

	protected List<string> ressourcesNecessaires;
	protected int nbrBoisNecessaire;

	public Personnage proprietaire;

	public int numBatiment;

	protected List<string> ressources;
	
	public bool enChantier;

	public bool manqueRessource;

	private float etapeConstruction;

	public virtual void Start()
	{
		plan = GameObject.FindGameObjectWithTag ("Plans").GetComponent<ListeBatiment> ();
		ressources = new List<string> ();
		ressourcesNecessaires = new List<string> ();
		enChantier = true;
		manqueRessource = true;
		etapeConstruction = 0;
	}

	public void CommencerChantier(int num)
	{
		StartCoroutine ("CommencerChantierCoroutine", num);

	}

	private IEnumerator CommencerChantierCoroutine(int num)
	{
		yield return null;
		numBatiment = num;
		nbrBoisNecessaire=plan.nbrBoisNecessaire[numBatiment];
		for (int i=0; i<plan.nbrBoisNecessaire[numBatiment]; i++) {
			ressourcesNecessaires.Add("Bois");
		}
	}

	public void AjouterRessource(Objet o)
	{
		if (ManqueRessource (o)) {
			ressources.Add (o.nomObjet);
			if(o.nomObjet.Equals("Bois"))
			{
				nbrBoisNecessaire--;
			}
			if(ressources.Count==ressourcesNecessaires.Count)
			{
				manqueRessource=false;
			}
		}
	}
	public bool Dormir(Personnage p)
	{
		if (p.inventaire.argent >= 5) {
			p.inventaire.argent -= 5;
			proprietaire.inventaire.argent+=5;
			p.StartCoroutine ("Dormir");
			return true;
		} else {
			return false;
		}
	}
	public bool ManqueRessource(Objet o)
	{
		if(ressourcesNecessaires.FindAll(x=>x.Equals(o.nomObjet)).Count==ressources.FindAll(x=>x.Equals(o.nomObjet)).Count)
		{
			return false;
		}
		return true;
	}
	public bool Construire()
	{
		etapeConstruction++;
		if (etapeConstruction >= 1000*DeroulementJournee.multiplicateurVitesse) {
			BatimentConstruit();
			return true;
		} else
			return false;
	}
	public void BatimentConstruit()
	{
		this.transform.GetComponent<SpriteRenderer> ().sprite = plan.spriteBatiment[numBatiment];
		enChantier = false;
		StartCoroutine ("BatimentVie");
	}
	public IEnumerator BatimentVie()
	{
		yield return new WaitForSeconds (500*DeroulementJournee.multiplicateurVitesse);
		if (DeroulementJournee.nuit) {
			StartCoroutine("AttendreJourPourDetruire");
		} else {
			GameObject.Destroy (this.gameObject);

		}
	}
	private IEnumerator AttendreJourPourDetruire()
	{
		yield return new WaitForSeconds (5*DeroulementJournee.multiplicateurVitesse);
		if (DeroulementJournee.nuit) {
			StartCoroutine ("AttendreJourPourDetruire");
		} else {
			GameObject.Destroy (this.gameObject);
		}

	}
	public void OnMouseDown()
	{
		print ("batiment: " + ressources.Count + ";"+ressourcesNecessaires.Count);

	}
}
