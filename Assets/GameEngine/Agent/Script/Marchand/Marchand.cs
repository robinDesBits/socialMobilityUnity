using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Marchand : Personnage {

	private List<Transform> marches;
	
	protected override void Start()
	{	
		base.Start();
		marches = new List<Transform> ();
		intention = Intention.ACHETERRESSOURCE;
		intentionEtape = IntentionEtape.CHERCHERVENDEUR;
		action = Action.CHERCHER;
	}
	
	protected override void ChoisirAction()
	{
		if (intention == Intention.ACHETERRESSOURCE) 
		{
			switch(intentionEtape)
			{
				case IntentionEtape.CHERCHERVENDEUR:
				if(champVision.Exists(x=>AgentLibre(x)))
				{

					cible=champVision.Find(x=>AgentLibre(x));
					action=Action.SEDIRIGERVERS;
					intentionEtape=IntentionEtape.ABORDER;
					action=Action.ABORDER;
				}
				break;
				case IntentionEtape.ABORDER:
				if(champVision.Exists(x=>AgentLibre(x)))
				{
					
					cible=champVision.Find(x=>AgentLibre(x));
					action=Action.SEDIRIGERVERS;
					intentionEtape=IntentionEtape.ABORDER;
					action=Action.ABORDER;
				}
				else 
				{
					intentionEtape = IntentionEtape.CHERCHERVENDEUR;
					action=Action.CHERCHER;
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
			dialogue.Aborder (cible);
		} else if (action == Action.SEDIRIGERVERS) {
			SeDirigerVers ();
		}
	}
	protected bool AgentLibre(Transform t)
	{
		return t.CompareTag ("Personnage") && !dialogue.personneAborde.Contains (t);
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
