using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour
{
	public GameObject currentHero;
	public GameObject currentEnemy;
	public GameObject previousHero;
	public GameObject nextHero;
	public TextMesh heroHp;
	public TextMesh heroStatus;
	public TextMesh enemyHp;
	public TextMesh enemyStatus;
	public TextMesh score;
	public TextMesh title;
	public TextMesh instruction;
	Animator heroAnim;
	Animator enemyAnim;
	Animator preAnim;
	Animator nextAnim;

	private AudioSource source;
	public AudioClip countSound;

	private bool countStart = false;

	// Use this for initialization
	void Start ()
	{
		source = GetComponent<AudioSource> ();
		heroAnim = currentHero.GetComponent<Animator> ();
		enemyAnim = currentEnemy.GetComponent<Animator> ();
		preAnim = previousHero.GetComponent<Animator> ();
		nextAnim = nextHero.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (Core.gameEnd) {
			title.characterSize = 3.0f;
			title.text = "YOUR TOTAL\n" + "SCORE IS : \n\n" + Core.getInstance ().getScore ();
			instruction.text = "PRESS [ ENTER ]\nTO RESTRART";
			Core.gameStart = false;
			Core.gameEnd = false;
			Core.gameRestart = true;
		}


		if (Input.GetKeyUp (KeyCode.Return) && !Core.gameStart && !countStart) {
			countStart = true;
			StartCoroutine (startGame ());
		}

		score.text = Core.getInstance ().getScore () + "";

		if (Core.getInstance ().getCurrentHead () != null && !Core.getInstance ().getCurrentHead ().isDead ()) {
			currentHero.GetComponentsInChildren <SpriteRenderer> () [1].sprite = Core.getInstance ().getCurrentHead ().getSprite ();
			heroHp.text = "HP : " + Core.getInstance ().getCurrentHead ().getHp ();
			heroStatus.text = "ATK : " + Core.getInstance ().getCurrentHead ().getSword () + " DEF : " + Core.getInstance ().getCurrentHead ().getShield ();
			heroAnim.SetInteger ("Type", Core.getInstance ().getCurrentHead ().getType ());
		} else {
			currentHero.GetComponentsInChildren <SpriteRenderer> () [1].sprite = null;
			heroHp.text = "";
			heroStatus.text = "";
		}

		if (Core.getInstance ().getCurrentEnemy () != null && !Core.getInstance ().getCurrentEnemy ().isDead ()) {
			currentEnemy.GetComponentsInChildren <SpriteRenderer> () [1].sprite = Core.getInstance ().getCurrentEnemy ().getSprite ();
			enemyHp.text = "HP : " + Core.getInstance ().getCurrentEnemy ().getHp ();
			enemyStatus.text = "ATK : " + Core.getInstance ().getCurrentEnemy ().getSword () + " DEF : " + Core.getInstance ().getCurrentHead ().getShield (); 
			enemyAnim.SetInteger ("Type", Core.getInstance ().getCurrentEnemy ().getType ());
		} else {
			currentEnemy.GetComponentsInChildren <SpriteRenderer> () [1].sprite = null;
			enemyHp.text = "";
			enemyStatus.text = "";
		}

		if (Core.getInstance ().heroLine ().Count > 1) {
			previousHero.GetComponentsInChildren <SpriteRenderer> () [1].sprite = ((HeroUnit)Core.getInstance ().heroLine () [Core.getInstance ().heroLine ().Count - 1]).getSprite ();
			preAnim.SetInteger ("Type", ((HeroUnit)Core.getInstance ().heroLine () [Core.getInstance ().heroLine ().Count - 1]).getType ());
			nextHero.GetComponentsInChildren <SpriteRenderer> () [1].sprite = ((HeroUnit)Core.getInstance ().heroLine () [1]).getSprite ();
			nextAnim.SetInteger ("Type", ((HeroUnit)Core.getInstance ().heroLine () [1]).getType ());
		} else {
			previousHero.GetComponentsInChildren <SpriteRenderer> () [1].sprite = null;
			nextHero.GetComponentsInChildren <SpriteRenderer> () [1].sprite = null;
		}
	
	}

	private IEnumerator startGame ()
	{
		title.text = "";
		source.PlayOneShot (countSound);
		instruction.text = "3..";
		yield return new WaitForSeconds (1f);
		source.PlayOneShot (countSound);
		instruction.text = ".2.";
		yield return new WaitForSeconds (1f);
		source.PlayOneShot (countSound);
		instruction.text = "..1";
		yield return new WaitForSeconds (1f);
		instruction.text = "";
		Core.gameStart = true;
		countStart = false;
	}

}
