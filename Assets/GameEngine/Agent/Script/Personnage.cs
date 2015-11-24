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
	
	protected enum Action{ABORDER, SEDIRIGERVERS,CHERCHER, SEPROMENER,/*bucheron*/COUPERARBRE, /*architecte*/ CONSTRUIRE,POSERCHANTIER};
	protected enum Intention{SEPROMENER,/*bucheron*/RAMENERBOIS,/*marchand*/ACHETERRESSOURCE, /*architecte*/ CONSTRUIRE};
	protected enum IntentionEtape{ABORDER,/*bucheron*/CHERCHERARBRE,COUPERARBRE,/*marchand*/CHERCHERVENDEUR, /*architecte*/POSERCHANTIER, CHERCHERMARCHAND, ALLERCHANTIER,CONSTRUIRE};
	
	protected bool actionEnCours;

	protected Transform cible;
	
	protected List<Transform> champVision;
	protected List<Transform> contact;

	public Inventaire inventaire;
	public BoucheAgent dialogue;
	protected bool inventairePlein;
	
	private int coteEvitementObstacle=0;

	protected virtual void Start()
	{
		champVision= new List<Transform>();
		contact=new List<Transform>();
		inventaire=new Inventaire();
		dialogue = gameObject.AddComponent <BoucheAgent>() as BoucheAgent;
		actionEnCours=false;
		idAgent = nbAgent++;
	}
	protected virtual void Update()
	{
		ChoisirAction();
		FaireAction();
	}
	protected void Avancer()
	{
		transform.Translate(0,-vitesse,0);
	}
	protected void Reculer()
	{
		transform.Translate(0,vitesse,0);
	}
	protected void Tourner(int direction=1)
	{
		transform.Rotate(0,0,direction * vitesseRotation);
	}
		
	protected void SeDirigerVers()
	{
		LookAt2D(cible);
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
			if(!other.isTrigger)
			{
				if (!champVision.Contains (other.transform)) {
					champVision.Add (other.transform);
				}
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

		if(other.gameObject.CompareTag("Arbre")||other.gameObject.CompareTag("Mur")||other.gameObject.CompareTag("Personnage"))
		{
			if(!contact.Contains(other.transform))
			{
				contact.Add(other.transform);
			}
		}
	}
	protected virtual void OnCollisionExit2D(Collision2D other)
	{
		if(other.gameObject.CompareTag("Arbre")||other.gameObject.CompareTag("Mur")||other.gameObject.CompareTag("Personnage"))
		{
			if(contact.Contains(other.transform))
			{
				contact.Remove(other.transform);
			}
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
		print ("agent n°"+idAgent + "\n Inventaire: "+inventaire.ToString());
	}
	protected abstract void ChoisirAction();
	protected abstract void FaireAction();


}
