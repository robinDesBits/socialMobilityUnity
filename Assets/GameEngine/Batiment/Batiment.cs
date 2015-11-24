using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Batiment : MonoBehaviour {

	protected ListeBatiment plan;

	protected List<string> ressourcesNecessaires;
	protected int nbrBoisNecessaire;

	public int numBatiment;

	protected List<string> ressources;
	
	public bool enChantier;

	public virtual void Start()
	{
		plan = GameObject.FindGameObjectWithTag ("Plans").GetComponent<ListeBatiment> ();
		ressources = new List<string> ();
		ressourcesNecessaires = new List<string> ();
		enChantier = true;
	}

	public void CommencerChantier(int num)
	{
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
			if(o.nomObjet=="Bois")
			{
				nbrBoisNecessaire--;
			}
			if(ressources.Count==ressourcesNecessaires.Count)
			{
				BatimentConstruit();
			}
		}
	}
	
	public bool ManqueRessource(Objet o)
	{
		if(ressourcesNecessaires.FindAll(x=>x.Equals(o.nomObjet)).Count==ressources.FindAll(x=>x.Equals(o.nomObjet)).Count)
		{
			return true;
		}
		return false;
	}

	public void BatimentConstruit()
	{
		this.transform.GetComponent<SpriteRenderer> ().sprite = plan.spriteBatiment[numBatiment];
		enChantier = false;
	}
}
