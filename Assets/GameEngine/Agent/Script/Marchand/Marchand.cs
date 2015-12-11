using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Marchand : Personnage {

	private List<Transform> marches;
	
	protected override void Start()
	{	
		base.Start();
		marches = new List<Transform> ();
		dialogue.estMarchand = true;
		dialogue.prix *= 2;
		intention = Intention.ACHETERRESSOURCE;
		intentionEtape = IntentionEtape.CHERCHERVENDEUR;
		action = Action.CHERCHER;
	}
	
	protected override void ChoisirAction()
	{
		if (intention == Intention.ACHETERRESSOURCE) {
			switch (intentionEtape) {
			case IntentionEtape.CHERCHERVENDEUR:
				if (inventaire.InventairePlein ()) {
					intention = Intention.VENDRERESSOURCE;
					intentionEtape = IntentionEtape.ALLERMARCHE;
				}
				else if (champVision.Exists (x => AgentLibre (x))) {
					cible = champVision.Find (x => AgentLibre (x));
					action = Action.SEDIRIGERVERS;
					intentionEtape = IntentionEtape.ABORDER;
					action = Action.ABORDER;
				}
				else
				{

					action=Action.CHERCHER;
				}
				break;
			case IntentionEtape.ABORDER:
				if (inventaire.InventairePlein ()) {
					intention = Intention.VENDRERESSOURCE;
					intentionEtape = IntentionEtape.ALLERMARCHE;
				} else if (champVision.Exists (x => AgentLibre (x))) {
						
					cible = champVision.Find (x => AgentLibre (x));
					intentionEtape = IntentionEtape.ABORDER;
					action = Action.ABORDER;
				} else {
					intentionEtape = IntentionEtape.CHERCHERVENDEUR;
					action = Action.CHERCHER;
				}
				break;

			}
		} else if (intention == Intention.VENDRERESSOURCE) {
			switch (intentionEtape)
			{
				case IntentionEtape.ALLERMARCHE:
				if(!inventaire.InventairePlein())
				{
					intention=Intention.ACHETERRESSOURCE;
					intentionEtape=IntentionEtape.CHERCHERVENDEUR;
				}
				else if (marches.Count > 0) {

						cible = marches [0];
						action = Action.SEDIRIGERVERS;
						if(ville.EstDansZone(this.transform))
						{
							action = Action.SEPROMENERDANSZONE;
						}
				}
				else {
					if(ville.EstDansZone(this.transform))
					{
						action = Action.SEPROMENERDANSZONE;

					}
					else 
					{
						cible = ville.transform;
						action = Action.SEDIRIGERVERS;
					}
				}
				break;
			}

		}


	}
	protected override void FaireAction()
	{
		if (dialogue.discutionEnCours) {
			return;
		}
		if (action == Action.CHERCHER) {
			SeBaladerEnCherchant ();
		} else if (action == Action.ABORDER) {
			dialogue.Aborder (cible, BoucheAgent.Discution.BOIS);
		} else if (action == Action.SEDIRIGERVERS) {
			SeDirigerVers ();
		} else if (action == Action.SEPROMENERDANSZONE) {
			if(cible==marches[0])
			{
				SePromenerDansZone(marches[0].GetComponent<Zone>());
			}
			else if(cible==ville.transform)
			{
				SePromenerDansZone(ville);
			}
		}
	}
	protected bool AgentLibre(Transform t)
	{
		return t.CompareTag ("Personnage") && t.gameObject.name.StartsWith("Bucheron") && !dialogue.personneAborde.Contains (t);
	}
	protected override void OnTriggerEnter2D(Collider2D other)
	{
		base.OnTriggerEnter2D(other);
		if(other.gameObject.CompareTag("Marché"))
		{
			if(!marches.Contains(other.transform))
			{
				marches.Add(other.transform);
			}
		}
	}
}
