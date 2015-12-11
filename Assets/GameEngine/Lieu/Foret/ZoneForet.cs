using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZoneForet : Zone {

	public int nombreArbreInitial;
	private int nombreArbreActuel;
	public GameObject arbre;
	private Bounds zoneForetRect;
	private GameObject arbreInstancie;

	
	void Start () {
		zoneForetRect=this.GetComponent<BoxCollider2D>().bounds;
		nombreArbreActuel=nombreArbreInitial;
		for(int i=0;i<nombreArbreInitial;i++)
		{			
			Vector2 positionArbre=new Vector2(Random.Range(zoneForetRect.min.x,zoneForetRect.max.x),Random.Range(zoneForetRect.min.y,zoneForetRect.max.y));
			
			arbreInstancie=Instantiate(arbre,positionArbre,Quaternion.identity) as GameObject;
			arbreInstancie.transform.parent=this.transform;
			arbreInstancie.transform.localScale=new Vector3(0.1f,0.1f,1);
			
		}
		StartCoroutine("NouveauArbre");
	
	}
	
	IEnumerator NouveauArbre() 
	{
		yield return new WaitForSeconds(120f); 

		for(int i=0;i<nombreArbreActuel/10;i++)
		{			
			Vector2 positionArbre=Vector2.zero;
			positionArbre=new Vector2(Random.Range(zoneForetRect.min.x,zoneForetRect.max.x),Random.Range(zoneForetRect.min.y,zoneForetRect.max.y));
			
			arbreInstancie=Instantiate(arbre,positionArbre,Quaternion.identity) as GameObject;
			arbreInstancie.transform.parent=this.transform;
			arbreInstancie.transform.localScale=new Vector3(0.1f,0.1f,1);
		}
		StartCoroutine("NouveauArbre");
	}
	
	
	
	
	
	
	
	
}
