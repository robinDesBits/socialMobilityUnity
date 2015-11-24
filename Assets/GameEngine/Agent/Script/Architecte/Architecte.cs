using UnityEngine;
using System.Collections;

public class Architecte : Personnage {

		
		private Transform lieuChantier;
	private ListeBatiment plans;
		private bool chantierActif;
		private int intentionConstruction;

		public GameObject chantier;

		protected override void Start()
		{	
			base.Start();
			plans = GameObject.FindGameObjectWithTag ("Plans").GetComponent<ListeBatiment> ();
			intentionConstruction = Random.Range(0,plans.nomBatiment.Count-1);

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
					break;
				case IntentionEtape.CHERCHERMARCHAND:
					if(champVision.Exists(x=>AgentLibre(x)))
					{
						
						cible=champVision.Find(x=>AgentLibre(x));
						action=Action.SEDIRIGERVERS;
						intentionEtape=IntentionEtape.ABORDER;
						action=Action.ABORDER;
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
			PoserChantier();
			action=Action.CHERCHER;
			} else if (action == Action.SEDIRIGERVERS) {
			}
		}
		protected bool AgentLibre(Transform t)
		{
			return t.CompareTag ("Personnage") && !dialogue.personneAborde.Contains (t);
		}
		protected override void OnTriggerEnter2D(Collider2D other)
		{
			base.OnTriggerEnter2D(other);
		}
		private bool PeutPoserBatiment(int num)
		{
			return true;
		}

		private void PoserChantier()
		{
			GameObject monChantier;
			monChantier=Instantiate (chantier, this.transform.position, Quaternion.identity) as GameObject;
			monChantier.name = plans.nomBatiment [intentionConstruction];
			monChantier.transform.SetParent(this.transform.parent);
		}
		protected bool AgentLibre(Transform t)
		{
					return t.CompareTag ("Personnage") && (t.gameObject.name.StartsWith("Bucheron") || t.gameObject.name.StartsWith("Marchand")) && !dialogue.personneAborde.Contains (t);
		}

}
