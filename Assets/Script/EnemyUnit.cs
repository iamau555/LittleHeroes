/**
 * Created by Viroj Loharatanavisit
 * This game is created for the Infinity Levels nscreening test
 *
 */

using UnityEngine;
using System.Collections;

public class EnemyUnit : UnitProfile
{

	public Animator anim;
	public bool isAttacked = false;

	// Use this for initialization
	void Awake ()
	{
		base.Awake ();
		Debug.Log ("enemy hp" + hp + " sword" + sword + " sheild" + shield);
	}
	
	// Update is called once per frame
	void Update ()
	{
		flip (base.getDirection ());

		if (Core.getInstance ().getCurrentEnemy () != null &&
			this.GetInstanceID () == Core.getInstance ().getCurrentEnemy ().GetInstanceID ()) {
			anim.SetInteger ("status", Core.getInstance ().getEnemyStatus ());
			if (Core.getInstance ().getEnemyStatus () == Core.ON_DEATH)
				StartCoroutine (KillOnAnimationEnd ());
		}
	}

	private IEnumerator KillOnAnimationEnd ()
	{
		yield return new WaitForSeconds (0.5f);
		Destroy (gameObject);
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if ((other.CompareTag ("Hero") || other.CompareTag ("Enemy")) && !other.GetComponent<HeroUnit> ().isHead && !isDead ()) {
			RectTransform rt = GetComponent<RectTransform> ();
			transform.position = new Vector2 (((int)Random.Range (-14, 15)) * rt.rect.width * rt.localScale.x, ((int)Random.Range (-10, 11)) * rt.rect.height * rt.localScale.y);
		}
	}

}
