using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public abstract class Personnage : MonoBehaviour {

	public float vitesse;
	public float vitesseRotation;
	static int nbAgent;
	private int idAgent;
	protected Intention intention;
	protected IntentionEtape intentionEtape;
	protected Action action;
	
	protected enum Action{ABORDER, SEDIRIGERVERS,CHERCHER, SEPROMENERDANSZONE,/*bucheron*/COUPERARBRE, /*architecte*/ CONSTRUIRE,POSERCHANTIER};
	protected enum Intention{SEPROMENER,/*bucheron*/RAMENERBOIS,/*marchand*/ACHETERRESSOURCE, VENDRERESSOURCE, /*architecte*/ CONSTRUIRE};
	protected enum IntentionEtape{ABORDER,/*bucheron*/CHERCHERARBRE,COUPERARBRE,/*marchand*/CHERCHERVENDEUR, ALLERMARCHE, /*architecte*/POSERCHANTIER, CHERCHERMARCHAND, ALLERCHANTIER,CONSTRUIRE};
	
	protected bool actionEnCours;

	protected Transform cible;
	protected Transform cibleNuit;

	public int energie;
	public bool faitDodo;

	protected Zone ville;
	protected Batiment hotelFavori;
	
	protected List<Transform> champVision;
	protected List<Transform> contact;

	protected Transform centreCarte;

	protected List<Zone> murs;
	public Inventaire inventaire;
	public BoucheAgent dialogue;

	private int coteEvitementObstacle=0;

	protected virtual void Start()
	{
		champVision= new List<Transform>();
		contact=new List<Transform>();
		inventaire=new Inventaire();
		energie = 100;
		dialogue = gameObject.AddComponent <BoucheAgent>() as BoucheAgent;
		actionEnCours=false;
		idAgent = nbAgent++;
		faitDodo = false;
		centreCarte = GameObject.FindGameObjectWithTag ("Plans").transform;
		ville = GameObject.FindGameObjectWithTag("Ville").GetComponent<Ville>();
		murs = new List<Zone> ();
		foreach(GameObject m in GameObject.FindGameObjectsWithTag("Mur"))
		{
			murs.Add(m.GetComponent<Zone>());
		}
		StartCoroutine ("DiminutionEnergie");
	}
	protected virtual void Update()
	{
		//VerifierPosition ();
		if (DeroulementJournee.nuit) {
			ComportementNuit();
		} else {
			ChoisirAction ();
			FaireAction ();
			if(faitDodo)
			{
				faitDodo = false;
			}
		}
	}
	protected void VerifierPosition()
	{
		if (this.transform.localPosition.x < 0 || this.transform.localPosition.y > 0 ||this.transform.localPosition.x < 5 || this.transform.localPosition.y < -3) {
			Vector3 tempPosition= new Vector3(0.1f,-0.1f,0);
			this.transform.position=tempPosition;
		}

	}
	protected void ComportementNuit()
	{
		if (faitDodo) {
			cibleNuit = hotelFavori.transform;
			return;
		} else {
			if (hotelFavori != null) {
				if (hotelFavori.EstDansZone (this.transform)) {
					if (hotelFavori.Dormir (this)) {
						faitDodo = true;
					}
					else
					{
						SePromenerDansZone (ville);
					}
				} else {
					cibleNuit = hotelFavori.transform;
					SeDirigerVers ();
				}
			} else {
				cibleNuit = ville.transform;
				if (ville.EstDansZone (this.transform)) {
					SePromenerDansZone (ville);
				} else {
					SeDirigerVers ();
				}
			}
		}
	}
	protected void Avancer()
	{
		transform.Translate(0,-vitesse/DeroulementJournee.multiplicateurVitesse,0);
	}
	protected void Reculer()
	{
		transform.Translate(0,vitesse/DeroulementJournee.multiplicateurVitesse,0);
	}
	protected void Tourner(int direction=1)
	{
		transform.Rotate(0,0,direction * vitesseRotation/DeroulementJournee.multiplicateurVitesse);
	}
		
	protected void SeDirigerVers()
	{
		if (DeroulementJournee.nuit) {
			LookAt2D(cibleNuit);
		} else {
				LookAt2D(cible);
		}

		Avancer();
	}
	protected void SeBaladerAuHasard()
	{
		if(champVision.Exists(x=>x.CompareTag("Mur")))
		{
			EviterObstacle();
			Avancer();
		}
		else if (contact.Count>0)
		{
			seDegagerObstacle();
		}
		else if(contact.Count==0)
		{
			Avancer();
		}
	}

	protected void SePromenerDansZone(Zone z)
	{
		if (z.EstDansZone (this.transform)) {
			Avancer ();
		} else {
			LookAt2D(z.transform);
			this.transform.Rotate(0,0,Random.Range(-89,89));
			Avancer ();
		}
	}
	public IEnumerator Dormir()
	{
		yield return new WaitForSeconds (1*DeroulementJournee.multiplicateurVitesse);
		if (DeroulementJournee.nuit) {
			StartCoroutine("Dormir");
		} else {
			energie=100;
		}
	}
    int etape=0;	
	protected void SeBaladerEnCherchant()
	{
		if(champVision.Exists(x=>x.CompareTag("Mur")))
		{
			EviterObstacle();
			Avancer();
		}
		else if (contact.Count>0)
		{
			seDegagerObstacle();
		}
		else if(contact.Count==0)
		{
			etape++;
			if(etape==120)
			{
				transform.Rotate(0,0,20);
			}
			else if(etape==200)
			{
				transform.Rotate(0,0,-40);
			}
			else if(etape==280)
			{
				transform.Rotate(0,0,20);
				etape++;
			}
			else if(etape==281)
			{
				transform.Rotate(0,0,90);
				etape++;
			}
			else if(etape==282)
			{
				transform.Rotate(0,0,-90);			
				etape++;
			}
			else if(etape==283)
			{
				transform.Rotate(0,0,90);
				etape=0;
			}
			
				Avancer();
		}
	}
	protected IEnumerator DiminutionEnergie()
	{
		yield return new WaitForSeconds (10*DeroulementJournee.multiplicateurVitesse);
		energie--;
		if (energie == 0) {
			print ("EST MORT!!!!");
		} else {
			StartCoroutine("DiminutionEnergie");
		}
	}
	protected void EviterObstacle()
	{
		if(coteEvitementObstacle==0)
		{
			Transform obstacle=null;
			if(champVision.Exists(x=>x.CompareTag("Mur")))
			{
				obstacle=champVision.Find(x=>x.CompareTag("Mur"));
			}
			Vector3 crossProduct=Vector3.Cross(this.transform.up, obstacle.position);
			if(crossProduct.z>=0)
			{
				coteEvitementObstacle=1;
			}
			else
			{
				coteEvitementObstacle=-1;
			}
		}
		Tourner(coteEvitementObstacle);
	}
	//Pour éviter interbloquage agents
	protected void seDegagerObstacle()
	{
		if(contact.Exists(x=>x.CompareTag("Personnage")))
		{
			Transform obstacle=contact.Find(x=>x.CompareTag("Personnage"));
			if(Vector3.Dot(obstacle.up,this.transform.up)<=0)
			{
				Reculer();
				Tourner(-1);
			}
			else
			{
				Avancer();
				Tourner(1);
			}
		}
	}
	protected virtual void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag ("Mur") || other.gameObject.CompareTag ("Arbre")) {
			if (!champVision.Contains (other.transform)) {
				champVision.Add (other.transform);
			}
		} else if (other.gameObject.CompareTag ("Personnage")) {
			if (!other.isTrigger) {
				if (!champVision.Contains (other.transform)) {
					champVision.Add (other.transform);
				}
			}
		} else if (other.gameObject.CompareTag ("Hotel")) {
			if(!other.GetComponent<Batiment>().enChantier)
			{
				hotelFavori=other.GetComponent<Batiment>();
			}
		}
	}
	protected virtual void OnTriggerExit2D(Collider2D other)
	{
		if(other.gameObject.CompareTag("Mur")||other.gameObject.CompareTag("Arbre")|| other.gameObject.CompareTag ("Personnage"))
		{
			if(champVision.Contains(other.transform))
			{
				champVision.Remove(other.transform);
			}
		}
	}
	protected virtual void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag ("Arbre") || other.gameObject.CompareTag ("Personnage")) {
			if (!contact.Contains (other.transform)) {
				contact.Add (other.transform);
			}
		} else if (other.gameObject.CompareTag ("Mur")) {
			LookAt2D(centreCarte);
			this.transform.Rotate(new Vector3(0,0,Random.Range(-89,89)));
		}

	}
	protected virtual void OnCollisionExit2D(Collision2D other)
	{
			if(contact.Contains(other.transform))
			{
				contact.Remove(other.transform);
			}
	}
	protected void LookAt2D(Transform t)
	{
		Vector3 diff = t.position - transform.position;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0f, 0f, rot_z + 90);
	}
	protected void OnMouseDown()
	{
		print (this.gameObject.name + "\n Inventaire: "+inventaire.ToString());
		print (intention);
		print (intentionEtape);
		print (action);
		print ("Dialogue ? " + dialogue.discutionEnCours + ", nbrPersonne mémorisé: " + dialogue.personneAborde.Count);
	}
	protected abstract void ChoisirAction();
	protected abstract void FaireAction();


}
