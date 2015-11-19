using UnityEngine;
using System.Collections;

public class Arbre : MonoBehaviour {
	
	private SpriteRenderer apparenceArbre;
	private int etapeArbre;
	public bool estLibre;
	public Sprite[] apparences;
	
	void Start()
	{
		estLibre=true;
		etapeArbre=3;
		apparenceArbre=this.transform.GetComponent<SpriteRenderer>();
	}
	
	public bool CouperArbre()
	{
		etapeArbre--;
		if(etapeArbre==-1)
		{
			this.transform.position=new Vector2(-10,-10);
			StartCoroutine("Detruire");
			return true;
		}
		else
		{
			apparenceArbre.sprite=apparences[etapeArbre];
			return false;
		}
	}
	
	private IEnumerator Detruire()
	{
		yield return null;
		yield return null;
		Destroy(this.gameObject);
	}
}










