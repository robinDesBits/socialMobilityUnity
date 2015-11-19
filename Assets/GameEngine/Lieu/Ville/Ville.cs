using UnityEngine;
using System.Collections;

public class Ville : MonoBehaviour {

	public int nombreBucheron;
	public int nombreMarchand;

	public GameObject bucheron;
	public GameObject marchand;
	
	
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
			b.name="bucheron"+i;
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
			b.name="Marchant"+i;
			b.transform.Rotate(0,0,rotationZ);
			b.transform.parent=this.transform.parent;
			b.transform.localScale=new Vector3(0.25f,0.25f,1f);
			yield return null;
		}
	}

}
