using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Architecte : Personnage {
		
		private List<Transform> marches;
		private static List<Batiment> chantiers=new List<Batiment>();
		private Transform lieuChantier;
		private ListeBatiment plans;
		private bool chantierActif;
		private int intentionConstruction;
		private GameObject monChantier;
		private int boisNecessaire;
		public GameObject chantier;

		protected override void Start()
		{	
			base.Start();
			plans = GameObject.FindGameObjectWithTag ("Plans").GetComponent<ListeBatiment> ();
			marches = new List<Transform> ();

			intentionConstruction = Random.Range(0,plans.nomBatiment.Count-1);
			boisNecessaire = plans.nbrBoisNecessaire [0];
			intention = Intention.CONSTRUIRE;
			intentionEtape = IntentionEtape.POSERCHANTIER;
			action = Action.CHERCHER;
		}
		
		protected override void ChoisirAction()
		{
			if (intention == Intention.CONSTRUIRE) 
			{
				switch(intentionEtape)
				{
				case IntentionEtape.POSERCHANTIER:
					if(PeutPoserBatiment(0))
					{
						action=Action.POSERCHANTIER;
						intentionEtape=IntentionEtape.CHERCHERMARCHAND;
					}
					else
					{
					action = Action.CHERCHER;
					}
					break;
				case IntentionEtape.CHERCHERMARCHAND:
					if (inventaire.InventairePlein ()) {
						cible=monChantier.transform;
						intentionEtape = IntentionEtape.ALLERCHANTIER;
					}
					else if(champVision.Exists(x=>AgentLibre(x)))
					{
						cible=champVision.Find(x=>AgentLibre(x));
						intentionEtape=IntentionEtape.ABORDER;
						action=Action.ABORDER;
					}
					else
					{
						if (marches.Count > 0) {

							if (marches[0].GetComponent<Zone>().EstDansZone(this.transform))
							{
								action = Action.SEPROMENERDANSZONE;
							}
							else if (champVision.Exists (x => x.CompareTag("Marché"))) 
							{
								cible = champVision.Find (x => x.CompareTag("Marché"));

							action = Action.SEDIRIGERVERS;

							} else
							{
								cible = marches [0].transform;
								action = Action.SEDIRIGERVERS;
							}
						} else {
							if(ville.EstDansZone(this.transform))
						   	{
								action = Action.CHERCHER;
							}
							else
							{
								cible=ville.transform;
								action = Action.SEDIRIGERVERS;

							}
						}
					}
				break;
			case IntentionEtape.ABORDER:
				if (inventaire.InventairePlein ()||inventaire.CombienObjet("Bois")>=boisNecessaire) {
					cible=monChantier.transform;
					intentionEtape = IntentionEtape.ALLERCHANTIER;
				} else if (champVision.Exists (x => AgentLibre (x))) {
					
					cible = champVision.Find (x => AgentLibre (x));
					intentionEtape = IntentionEtape.ABORDER;
					action = Action.ABORDER;
				} else {
					intentionEtape = IntentionEtape.CHERCHERMARCHAND;
					action = Action.CHERCHER;
				}
				break;
			
			case IntentionEtape.ALLERCHANTIER:
				cible=monChantier.transform;
				action=Action.SEDIRIGERVERS;
				if(monChantier.GetComponent<Zone>().EstDansZone(this.transform))
				{
					action=Action.CONSTRUIRE;
					intentionEtape=IntentionEtape.CONSTRUIRE;
				}
				break;
			case IntentionEtape.CONSTRUIRE:
				if(!monChantier.GetComponent<Zone>().EstDansZone(this.transform))
				{
					intentionEtape=IntentionEtape.ALLERCHANTIER;
				}
				else if(!monChantier.GetComponent<Batiment>().enChantier)
				{
					intention = Intention.CONSTRUIRE;
					intentionEtape = IntentionEtape.POSERCHANTIER;
					action = Action.CHERCHER;
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
			} else if (action == Action.POSERCHANTIER) {
				PoserChantier ();
				action = Action.CHERCHER;
			} else if (action == Action.SEDIRIGERVERS) {
				SeDirigerVers ();
			} else if (action == Action.ABORDER) {
				dialogue.Aborder (cible, BoucheAgent.Discution.BOIS);
			} else if (action == Action.SEPROMENERDANSZONE) {
				SePromenerDansZone (marches [0].GetComponent<Zone> ());
			} else if (action == Action.CONSTRUIRE) {
				construire ();
			}
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
		private bool PeutPoserBatiment(int num)
		{
			GameObject chantierTemp;
			chantierTemp = Instantiate (chantier, this.transform.position, Quaternion.identity) as GameObject;
			if (ville.EstDansZone (this.transform)) {

				if(!ville.transform.GetChild(0).GetComponent<Zone>().IntersecteZone(chantierTemp.GetComponent<BoxCollider2D>()))
				{

					if(!murs[0].IntersecteZone(chantierTemp.GetComponent<BoxCollider2D>()) && !murs[1].IntersecteZone(chantierTemp.GetComponent<BoxCollider2D>()) &&  !murs[2].IntersecteZone(chantierTemp.GetComponent<BoxCollider2D>()) && !murs[3].IntersecteZone(chantierTemp.GetComponent<BoxCollider2D>()))
					{
						bool constructOK=true;
						foreach(Batiment b in chantiers)
						{
							if(b==null)
							{	
								chantiers.Remove(b);
								constructOK=false;
								break;
							}
							else if(b.IntersecteZone(chantierTemp.GetComponent<BoxCollider2D>()))
							{
							constructOK=false;
							}
						}
						if(constructOK)
						{
							Destroy (chantierTemp.gameObject);
							return true;
						}
					}
				}
			}
			Destroy (chantierTemp.gameObject);
			return false;
			
		}

		private void PoserChantier()
		{
			monChantier=Instantiate (chantier, this.transform.position, Quaternion.identity) as GameObject;
			monChantier.name = plans.nomBatiment [intentionConstruction];
			monChantier.transform.SetParent(this.transform.parent);
			chantiers.Add (monChantier.GetComponent<Batiment> ());
			monChantier.GetComponent<Batiment> ().CommencerChantier (intentionConstruction);
		monChantier.GetComponent<Batiment> ().proprietaire = this;

		}
		private void construire()
		{
			if (monChantier.GetComponent<Batiment> ().manqueRessource) {
			foreach (Objet o in inventaire.contenu) {
				if (monChantier.GetComponent<Batiment> ().ManqueRessource (o)) {
					monChantier.GetComponent<Batiment> ().AjouterRessource (o);
					inventaire.EnleverObjet (o);
					break;
				}
			}
			} else {
				if(monChantier.GetComponent<Batiment> ().Construire())
				{
				intention=Intention.CONSTRUIRE;
				intentionEtape=IntentionEtape.POSERCHANTIER;
				}
			}
			
		}

		protected bool AgentLibre(Transform t)
		{
			return t.CompareTag ("Personnage") && t.gameObject.name.StartsWith("Marchand") && !dialogue.personneAborde.Contains (t);
		}

}
