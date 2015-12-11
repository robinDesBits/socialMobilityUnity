using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventaire{
	
	protected int capaciteMax;
	protected int porteActuel;

	public int argent;

	public List<Objet> contenu;


	public Inventaire()
	{
		capaciteMax=10;
		porteActuel=0;
		argent = 15;
		contenu=new List<Objet>();
	}
	public override string ToString()
	{
		string toString = "Argent: " + argent;
		for (int i=0; i< contenu.Count; i++) {
			toString+="-"+ contenu[i].nomObjet;
		}
		toString += " est plein: " + InventairePlein ();
		return toString;
	}
	public void echanger(string name,int nbrObjetDonner, int prix, Transform destinataire)
	{
		for (int i=0; i<nbrObjetDonner; i++) {
			Objet o=contenu.Find (x => x.nomObjet.Equals(name));
			argent+=prix;
			contenu.Remove(o);
			porteActuel-=o.poidObjet;
			destinataire.GetComponent<Personnage>().inventaire.AjouterObjet(o);
			destinataire.GetComponent<Personnage>().inventaire.argent-=prix;
			Debug.Log(o.nomObjet + " donné");
		}
	}
	public void EnleverObjet(Objet o)
	{
			contenu.Remove(o);
			porteActuel-=o.poidObjet;
	}
	public List<Objet> AObjet(string name)
	{
		if (contenu.Exists (o => o.nomObjet.Equals(name))) {
			return contenu.FindAll (o => o.nomObjet.Equals(name));
		} else
			return null;
	}
	public int CombienObjet(string name)
	{
		if (contenu.Exists (o => o.nomObjet.Equals(name))) {
			return contenu.FindAll (o => o.nomObjet.Equals(name)).Count;
		} else
			return 0;
	}
	public Objet AjouterObjet(Objet o)
	{
		if(capaciteMax>=porteActuel+o.poidObjet)
		{
			porteActuel+=o.poidObjet;
			contenu.Add(o);
			return null;
		}
		else 
		{
			return o;
		}
	}
	public bool InventairePlein()
	{
		return capaciteMax<=porteActuel;
	}
	
}
