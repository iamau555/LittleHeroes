/**
 * Created by Viroj Loharatanavisit
 * This game is created for the Infinity Levels nscreening test
 *
 */

using UnityEngine;
using System.Collections;

public class HeroUnit : UnitProfile
{

	public Animator anim;
	public bool isHead = false;
	public bool isGet = false;
	public GameObject leftSpector;
	public GameObject rightSpector;
	public GameObject upSpector;
	public GameObject downSpector;

	public AudioClip getSound;
	private AudioSource source;
	//private Sprite[] sprites = Resources.LoadAll <Sprite> ("Sprites/HeroSprite");  

	// Use this for initialization
	void Awake ()
	{
		base.Awake ();
		source = GetComponent<AudioSource> ();
		Debug.Log ("hero hp" + hp + " sword" + sword + " sheild" + shield);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (isHead) {
			leftSpector.SetActive (true);
			rightSpector.SetActive (true);
			upSpector.SetActive (true);
			downSpector.SetActive (true);
			anim.SetInteger ("status", Core.getInstance ().getHeroStatus ());
			if (base.hp <= 0) {
				anim.SetInteger ("status", Core.ON_DEATH);
				StartCoroutine (KillOnAnimationEnd ());
			}

		} else {
			leftSpector.SetActive (false);
			rightSpector.SetActive (false);
			upSpector.SetActive (false);
			downSpector.SetActive (false);
		}
	}

	private IEnumerator KillOnAnimationEnd ()
	{
		yield return new WaitForSeconds (0.5f);
		Destroy (gameObject);
	}

	public void setHead (bool c)
	{
		isHead = c;
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (isHead && other.CompareTag ("Hero") && !other.GetComponent<HeroUnit> ().isGet) {
			this.isGet = true;
			Debug.Log ("get hero");
			source.PlayOneShot (getSound);
			Core.getInstance ().heroLine ().Add (other.GetComponent<HeroUnit> ());
			other.GetComponent<HeroUnit> ().transform.position = Core.getInstance ().getTempPosition ();
			other.GetComponent<HeroUnit> ().flip (Core.getInstance ().getTempDirection ());
			other.GetComponent<HeroUnit> ().isGet = true;
		} else if (isHead && other.CompareTag ("Hero") && other.GetComponent<HeroUnit> ().isGet && !Core.isActive && !base.isDead () && !other.GetComponent<HeroUnit> ().isDead ()) {
			Core.heroCrash = true;
			//base.addHp (-100);
		} else if (!isGet && !isHead && (other.CompareTag ("Hero") || other.CompareTag ("Enemy")) && !other.GetComponent<HeroUnit> ().isHead && !isDead ()) {
			RectTransform rt = GetComponent<RectTransform> ();
			transform.position = new Vector2 (((int)Random.Range (-14, 15)) * rt.rect.width * rt.localScale.x, ((int)Random.Range (-10, 11)) * rt.rect.height * rt.localScale.y);
		}
	}

	void OnTriggerExit2D (Collider2D other)
	{
	
	}
}
