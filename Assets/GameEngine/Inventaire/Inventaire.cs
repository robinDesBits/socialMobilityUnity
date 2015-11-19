using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventaire{


	protected int capaciteMax;
	protected int porteActuel;
	protected List<Objet> contenu;
	
	public Inventaire()
	{
		capaciteMax=10;
		porteActuel=0;
		contenu=new List<Objet>();
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
