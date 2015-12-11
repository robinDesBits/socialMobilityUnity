using UnityEngine;
using System.Collections;

public class Ville : Zone {

	public int nombreBucheron;
	public int nombreMarchand;
	public int nombreArchitecte;

	public GameObject bucheron;
	public GameObject marchand;
	public GameObject architecte;
	
	
	private Bounds zoneVilleRect;
	
	void Start () 
	{
		zoneVilleRect=this.GetComponent<BoxCollider2D>().bounds;
		StartCoroutine("InstancierPopulation");
	}
	
	IEnumerator InstancierPopulation()
	{
		yield return null;
		for(int i=0;i<nombreBucheron;i++)
		{
			Vector2 positionAgent=new Vector2(Random.Range(zoneVilleRect.min.x,zoneVilleRect.max.x),Random.Range(zoneVilleRect.min.y,zoneVilleRect.max.y));
			int rotationZ=Random.Range(1,359);
			GameObject b=Instantiate(bucheron,positionAgent,Quaternion.identity) as GameObject;
			b.name="Bucheron"+i;
			b.transform.Rotate(0,0,rotationZ);
			b.transform.parent=this.transform.parent;
			b.transform.localScale=new Vector3(0.25f,0.25f,1f);
			yield return null;
		}
		for(int i=0;i<nombreMarchand;i++)
		{
			Vector2 positionAgent=new Vector2(Random.Range(zoneVilleRect.min.x,zoneVilleRect.max.x),Random.Range(zoneVilleRect.min.y,zoneVilleRect.max.y));
			int rotationZ=Random.Range(1,359);
			GameObject b=Instantiate(marchand,positionAgent,Quaternion.identity) as GameObject;
			b.name="Marchand"+i;
			b.transform.Rotate(0,0,rotationZ);
			b.transform.parent=this.transform.parent;
			b.transform.localScale=new Vector3(0.25f,0.25f,1f);
			yield return null;
		}
		for(int i=0;i<nombreArchitecte;i++)
		{
			Vector2 positionAgent=new Vector2(Random.Range(zoneVilleRect.min.x,zoneVilleRect.max.x),Random.Range(zoneVilleRect.min.y,zoneVilleRect.max.y));
			int rotationZ=Random.Range(1,359);
			GameObject b=Instantiate(architecte,positionAgent,Quaternion.identity) as GameObject;
			b.name="Architecte"+i;
			b.transform.Rotate(0,0,rotationZ);
			b.transform.parent=this.transform.parent;
			b.transform.localScale=new Vector3(0.25f,0.25f,1f);
			yield return null;
		}
	}

}
