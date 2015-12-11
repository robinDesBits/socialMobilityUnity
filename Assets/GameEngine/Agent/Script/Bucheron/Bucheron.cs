using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bucheron : Personnage {

	protected List<ZoneForet> forets;
	protected bool plusArbre;
	

	protected override void Start()
	{	
		base.Start();
		forets=new List<ZoneForet>();
		plusArbre=true;
		intention=Intention.RAMENERBOIS;
		intentionEtape=IntentionEtape.CHERCHERARBRE;
	}
	protected override void ChoisirAction()
	{
		if (intention == Intention.RAMENERBOIS) {
			switch (intentionEtape) {
			case IntentionEtape.CHERCHERARBRE:
				if (champVision.Exists (x => ArbreLibre (x))) {
					plusArbre = false;
					cible = champVision.Find (x => ArbreLibre (x));
					action = Action.SEDIRIGERVERS;
					if (Vector3.Distance (cible.transform.position, this.transform.position) <= 0.2f) {
						intentionEtape = IntentionEtape.COUPERARBRE;
						action = Action.COUPERARBRE;
					}
				} else {
					if (forets.Count > 0) {
						if (champVision.Exists (x => ArbreLibre (x))) {
							cible = champVision.Find (x => ArbreLibre (x));
						} else {
							//à remplacer par la plus proche foret
							cible = forets [0].transform;
						}
						action = Action.SEDIRIGERVERS;
					} else {
						action = Action.CHERCHER;
					}
				}
				break;
			case IntentionEtape.COUPERARBRE:
				if (inventaire.InventairePlein()) {
					intention = Intention.SEPROMENER;
					action = Action.SEPROMENERDANSZONE;
				} else if (plusArbre == true) {
					intentionEtape = IntentionEtape.CHERCHERARBRE;
					action = Action.SEPROMENERDANSZONE;
				}
				break;
			}
		} else if (intention == Intention.SEPROMENER) {

			if (!inventaire.InventairePlein()) {
				intention = Intention.RAMENERBOIS;
				intentionEtape=IntentionEtape.CHERCHERARBRE;

			}
		}
	}
	protected override void FaireAction()
	{
		if (dialogue.discutionEnCours) {
			return;
		}
		switch(action)
		{
			case Action.CHERCHER:
				SeBaladerEnCherchant();
			break;
			case Action.SEDIRIGERVERS:
				SeDirigerVers();
			break;
			case Action.COUPERARBRE:
				if(!actionEnCours)
				{
					StartCoroutine("CouperArbre");
				}
			break;
			case Action.SEPROMENERDANSZONE:
			SePromenerDansZone(forets[0]);
			break;
		}
	}

	
	protected IEnumerator CouperArbre()
	{
		Arbre monArbre=cible.GetComponent<Arbre>();
		actionEnCours=true;
		if(monArbre.estLibre)
		{
			monArbre.estLibre=false;
			yield return new WaitForSeconds(4f*DeroulementJournee.multiplicateurVitesse);
			if(!DeroulementJournee.nuit)
			{
				plusArbre=monArbre.CouperArbre();
				Objet o=new Objet("Bois",2);
				inventaire.AjouterObjet(o);
				monArbre.estLibre=true;
				actionEnCours=false;
			}
			else
			{
				monArbre.estLibre=true;
				plusArbre=true;
				actionEnCours=false;
			}
		}
		else
		{
			action=Action.CHERCHER;
		}
	}
	
	protected override void OnTriggerEnter2D(Collider2D other)
	{
		base.OnTriggerEnter2D(other);
		if(other.gameObject.CompareTag("Arbre"))
		{
			if(!forets.Contains(other.transform.parent.GetComponent<ZoneForet>()))
			{
				forets.Add(other.transform.parent.GetComponent<ZoneForet>());
			}
		}
	}

	protected bool ArbreLibre(Transform t)
	{
		return t.CompareTag("Arbre")&& t.GetComponent<Arbre>().estLibre;
	}


}
