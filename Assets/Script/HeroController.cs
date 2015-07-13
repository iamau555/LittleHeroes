/**
 * Created by Viroj Loharatanavisit
 * This game is created for the Infinity Levels nscreening test
 *
 */

using UnityEngine;
using System.Collections;

public class HeroController : MonoBehaviour
{

	private HeroUnit frontHero;
	public GameObject heroObject;
	public GameObject enemyObject;

	private RectTransform heroSize;

	private float nextUsage;
	private float nextAttack;
	private float nextChange;

	private float moveX = 0;
	private float moveY = 0;
	private float tmpX = 0;
	private float tmpY = 0;
	private bool isClicked = false;
	private bool isNearbyWallUp = false;
	private bool isNearbyWallDown = false;
	private bool isNearbyWallLeft = false;
	private bool isNearbyWallRight = false;
	public float delay = 0.3f;
	public float delayAttack = 0.5f;
	public float delayChange = 0.1f;
	
	private bool onUp = false;
	private bool onDown = false;
	private bool onLeft = false;
	private bool onRight = false;

	private AudioSource source;

	public AudioClip atkSound;
	public AudioClip swapSound;
	public AudioClip winSound;
	public AudioClip loseSound;
	public AudioClip walkSound;

	private string currentDirection;
	
	// Use this for initialization
	void Awake ()
	{
		source = GetComponent<AudioSource> ();

		GameObject hero = (GameObject)Instantiate (heroObject, new Vector2 (0, 0), Quaternion.identity);
		hero.transform.SetParent (transform);
		frontHero = hero.GetComponent<HeroUnit> ();
		Core.getInstance ().heroLine ().Add (frontHero);
		Core.getInstance ().setCurrentHeadHero (frontHero);
		Core.getInstance ().getCurrentHead ().setHead (true);
		Core.getInstance ().getCurrentHead ().isGet = true;
		Core.getInstance ().getCurrentHead ().addHp (20);
		delay = 0.3f;

	}
	// Update is called once per frame
	void FixedUpdate ()
	{

		if (Core.gameStart && !Core.gameEnd) {

			if (Core.gameRestart) {
				if (Core.anotherRound) {
					for (int i =0; i < GetComponentsInChildren<UnitProfile>().Length; i++) 
						Destroy (GetComponentsInChildren <UnitProfile> () [i].gameObject);
					Awake ();
				}

				Core.gameRestart = false;
				nextUsage = Time.time + delay;
				heroSize = frontHero.GetComponent<RectTransform> ();
				nextAttack = 0;
				nextChange = 0;
				moveX = 0;
				moveY = - heroSize.rect.height * heroSize.localScale.y;
				currentDirection = "down";
				setMove (currentDirection);
				for (int i = 0; i < 2; i++) {
					spawnUnits ();
				}

				for (int i =0; i < GetComponentsInChildren<UnitProfile>().Length; i++) {
					Debug.Log (GetComponentsInChildren <UnitProfile> () [i].name);
				}

			}

			// change character from button z or x
			if (!Core.getInstance ().isFight () && Time.time > nextChange) {
				// switching from front to endline
				if (Input.GetKeyUp ("x")) {

					source.PlayOneShot (swapSound);

					nextChange = Time.time + delayChange;

					Debug.Log (((HeroUnit)Core.getInstance ().heroLine () [0]).name);

					// first, set tmp direction and position
					Core.getInstance ().setTempPosition (((HeroUnit)Core.getInstance ().heroLine () [0]).transform.position);
					Core.getInstance ().setTempDirection (((HeroUnit)Core.getInstance ().heroLine () [0]).getFlipDirection ());

					// set the current head to false
					frontHero.setHead (false);
					frontHero.enabled = false;

					// and move up the line from character 2 up ahead
					for (int j = 1; j < Core.getInstance().heroLine().Count; j++) {
						Vector3 otherTemp = ((HeroUnit)Core.getInstance ().heroLine () [j]).transform.position;
						string dirTemp = ((HeroUnit)Core.getInstance ().heroLine () [j]).getFlipDirection ();
						((HeroUnit)Core.getInstance ().heroLine () [j]).transform.position = Core.getInstance ().getTempPosition ();	
						((HeroUnit)Core.getInstance ().heroLine () [j]).flip (Core.getInstance ().getTempDirection ());
						Core.getInstance ().setTempPosition (otherTemp);
						Core.getInstance ().setTempDirection (dirTemp);
					}
					// then move the first hero to the last of the line
					((HeroUnit)Core.getInstance ().heroLine () [0]).transform.position = Core.getInstance ().getTempPosition ();
					((HeroUnit)Core.getInstance ().heroLine () [0]).flip (Core.getInstance ().getTempDirection ());

					// now swap the array and set the rest
					Core.getInstance ().heroLine ().RemoveAt (0);
					Core.getInstance ().heroLine ().Add (frontHero);
					frontHero.enabled = true;
					frontHero = ((HeroUnit)Core.getInstance ().heroLine () [0]);

					Debug.Log (((HeroUnit)Core.getInstance ().heroLine () [0]).name);

					heroSize = frontHero.GetComponent<RectTransform> ();
					Core.getInstance ().setCurrentHeadHero (frontHero);
					Core.getInstance ().getCurrentHead ().setHead (true);
					Core.getInstance ().getCurrentHead ().setDirection (currentDirection);

					// switching from end to frontline
				} else if (Input.GetKeyUp ("z")) {

					source.PlayOneShot (swapSound);

					nextChange = Time.time + delayChange;

					// first, set tmp direction and position
					Core.getInstance ().setTempPosition (((HeroUnit)Core.getInstance ().heroLine () [Core.getInstance ().heroLine ().Count - 1]).transform.position);
					Core.getInstance ().setTempDirection (((HeroUnit)Core.getInstance ().heroLine () [Core.getInstance ().heroLine ().Count - 1]).getFlipDirection ());
			
					// set the current head to false
					frontHero.setHead (false);
					frontHero = (HeroUnit)Core.getInstance ().heroLine () [Core.getInstance ().heroLine ().Count - 1];
					frontHero.enabled = false;
					Vector3 otherTemp;
					string dirTemp;

					// and move back the line from hero before last to the last
					for (int j = Core.getInstance().heroLine().Count-2; j >= 0; j--) {
						otherTemp = ((HeroUnit)Core.getInstance ().heroLine () [j]).transform.position;
						dirTemp = ((HeroUnit)Core.getInstance ().heroLine () [j]).getFlipDirection ();
						((HeroUnit)Core.getInstance ().heroLine () [j]).transform.position = Core.getInstance ().getTempPosition ();	
						((HeroUnit)Core.getInstance ().heroLine () [j]).flip (Core.getInstance ().getTempDirection ());
						Core.getInstance ().setTempPosition (otherTemp);
						Core.getInstance ().setTempDirection (dirTemp);
					}
					// then move the last hero to the front of the line
					otherTemp = ((HeroUnit)Core.getInstance ().heroLine () [Core.getInstance ().heroLine ().Count - 1]).transform.position;
					dirTemp = ((HeroUnit)Core.getInstance ().heroLine () [Core.getInstance ().heroLine ().Count - 1]).getFlipDirection ();
					((HeroUnit)Core.getInstance ().heroLine () [Core.getInstance ().heroLine ().Count - 1]).transform.position = Core.getInstance ().getTempPosition ();
					((HeroUnit)Core.getInstance ().heroLine () [Core.getInstance ().heroLine ().Count - 1]).flip (Core.getInstance ().getTempDirection ());
					Core.getInstance ().setTempPosition (otherTemp);
					Core.getInstance ().setTempDirection (dirTemp);
			
					// now swap the array
					Core.getInstance ().heroLine ().RemoveAt (Core.getInstance ().heroLine ().Count - 1);
					Core.getInstance ().heroLine ().Insert (0, frontHero);
					frontHero.enabled = true;
					heroSize = frontHero.GetComponent<RectTransform> ();
					Core.getInstance ().setCurrentHeadHero (frontHero);
					Core.getInstance ().getCurrentHead ().setHead (true);
					Core.getInstance ().getCurrentHead ().setDirection (currentDirection);
				}
			}



			if (!isClicked) {
				if (Input.GetKey ("up") && !onDown) {
					moveX = 0;
					moveY = heroSize.rect.height * heroSize.localScale.y;
					isClicked = true;
					if ((onRight || onLeft) && isNearbyWallUp)
						setMove (currentDirection);
					else {
						currentDirection = "up";
						setMove (currentDirection);
					}
				} else if (Input.GetKey ("down") && !onUp) {
					moveX = 0;
					moveY = - heroSize.rect.height * heroSize.localScale.y;
					isClicked = true;
					if ((onRight || onLeft) && isNearbyWallDown)
						setMove (currentDirection);
					else {
						currentDirection = "down";
						setMove (currentDirection);
					}
				} else if (Input.GetKey ("left") && !onRight) {
					moveX = - heroSize.rect.width * heroSize.localScale.x;
					moveY = 0;
					isClicked = true;
					if ((onUp || onDown) && isNearbyWallLeft)
						setMove (currentDirection);
					else {
						currentDirection = "left";
						setMove (currentDirection);
					}
				} else if (Input.GetKey ("right") && !onLeft) {

					moveX = heroSize.rect.width * heroSize.localScale.x;
					moveY = 0;
					isClicked = true;
					if ((onUp || onDown) && isNearbyWallRight)
						setMove (currentDirection);
					else {
						currentDirection = "right";
						setMove (currentDirection);
					}
				}

			}

			if (Core.heroCrash) {
				losingHero ();
				nextUsage = Time.time + delay;
			}

			// check if is battle
			if (Core.getInstance ().isFight () && Time.time > nextAttack) {

				//check if still on fight
				if (Core.getInstance ().getCurrentHead () != null && 
					!Core.getInstance ().heroWin () 
					&& !Core.getInstance ().enemyWin ()) {
					source.PlayOneShot (atkSound);
					nextAttack = Time.time + delayAttack;
					Core.getInstance ().getFight ();
					Debug.Log ("fight");
				}

				//check if hero death in each time
				if (Core.getInstance ().enemyWin ()) {

					source.PlayOneShot (loseSound);
					Core.getInstance ().getCurrentEnemy ().setDirection ("none");
					Core.getInstance ().setTempPosition (((HeroUnit)Core.getInstance ().heroLine () [0]).transform.position);
					Core.getInstance ().setTempDirection (((HeroUnit)Core.getInstance ().heroLine () [0]).getFlipDirection ());
					Core.getInstance ().heroLine ().RemoveAt (0);

					if (Core.getInstance ().heroLine ().Count == 0) {
						Core.gameEnd = true;
						Core.anotherRound = true;
					} else {
						frontHero = ((HeroUnit)Core.getInstance ().heroLine () [0]);
						heroSize = frontHero.GetComponent<RectTransform> ();
						Debug.Log (frontHero.name);
						Core.getInstance ().setCurrentHeadHero (frontHero);
						for (int j = 0; j < Core.getInstance().heroLine().Count; j++) {
							Debug.Log ("count" + Core.getInstance ().heroLine ().Count);
							Vector3 otherTemp = ((HeroUnit)Core.getInstance ().heroLine () [j]).transform.position;
							string dirTemp = ((HeroUnit)Core.getInstance ().heroLine () [j]).getFlipDirection ();
							((HeroUnit)Core.getInstance ().heroLine () [j]).transform.position = Core.getInstance ().getTempPosition ();	
							((HeroUnit)Core.getInstance ().heroLine () [j]).flip (Core.getInstance ().getTempDirection ());
							Core.getInstance ().setTempPosition (otherTemp);
							Core.getInstance ().setTempDirection (dirTemp);
						}
						Core.getInstance ().getCurrentHead ().setHead (true);
						Core.getInstance ().getCurrentHead ().setDirection (currentDirection);
						//setMove (currentDirection);
						//Core.getInstance ().setHeroDirection (currentDirection);
						Core.getInstance ().getCurrentEnemy ().setDirection (Core.getInstance ().getCurrentEnemy ().getTempEnemyStatus ());
					}
				
					Debug.Log ("enemy win");
				} else if (Core.getInstance ().heroWin ()) {
					source.PlayOneShot (winSound);
					Core.getInstance ().setActive (false);
					Core.getInstance ().getCurrentEnemy ().setDirection ("death");
					Core.getInstance ().removeCurrentEnemy (currentDirection);
					moveX = tmpX;
					moveY = tmpY;
					Debug.Log ("hero win");

					for (int i = 0; i < Core.getInstance().heroLine().Count; i++) {
						Core.getInstance ().addScore (((HeroUnit)Core.getInstance ().heroLine () [i]).getHp ());
					}

					spawnUnits ();

					if (delay > 0.05)
						delay -= 0.01f;
				}


			}

			// While walking
			if (!Core.getInstance ().isFight () && Time.time > nextUsage) {

				source.PlayOneShot (walkSound);

				for (int i = 0; i < Core.getInstance().heroLine().Count; i++) {
					//if on first hero
					if (i == 0) {

						Core.getInstance ().getCurrentHead ().flip (currentDirection);
						Core.getInstance ().setTempPosition (((HeroUnit)Core.getInstance ().heroLine () [i]).transform.position);
						Core.getInstance ().setTempDirection (((HeroUnit)Core.getInstance ().heroLine () [i]).getFlipDirection ());

						if (Core.getInstance ().getToEnemy (currentDirection)) {
							tmpX = moveX;
							tmpY = moveY;
							moveX = 0;
							moveY = 0;
							Core.getInstance ().setHeroDirection (currentDirection);
							Core.getInstance ().setActive (true);
							break;
						} else {
							isClicked = false;
						}

						frontHero.transform.position = new Vector3 (
				Mathf.Clamp (frontHero.transform.position.x + moveX, -5.04f, 5.04f),
				Mathf.Clamp (frontHero.transform.position.y + moveY, -3.6f, 3.6f),
				frontHero.transform.position.z);
						nextUsage = Time.time + delay;

						if (isCrash ()) {
							losingHero ();
							break;
							//Core.getInstance ().heroLine ().re
						}

					} else {
						Vector3 otherTemp = ((HeroUnit)Core.getInstance ().heroLine () [i]).transform.position;
						string dirTemp = ((HeroUnit)Core.getInstance ().heroLine () [i]).getFlipDirection ();
						((HeroUnit)Core.getInstance ().heroLine () [i]).transform.position = Core.getInstance ().getTempPosition ();	
						((HeroUnit)Core.getInstance ().heroLine () [i]).flip (Core.getInstance ().getTempDirection ());
						Core.getInstance ().setTempPosition (otherTemp);
						Core.getInstance ().setTempDirection (dirTemp);
					}


				}
			}

			//Debug.Log("x : "+ (frontHero.transform.position.x + moveX) + " y : "+ (frontHero.transform.position.y + moveY));
		}
	}

	// Spawn both hero and enemy
	private void spawnUnits ()
	{
		GameObject hero = (GameObject)Instantiate (heroObject, new Vector2 (((int)Random.Range (-14, 15)) * heroSize.rect.width * heroSize.localScale.x, ((int)Random.Range (-10, 11)) * heroSize.rect.height * heroSize.localScale.y), heroObject.transform.rotation);
		GameObject enemy = (GameObject)Instantiate (enemyObject, new Vector2 (((int)Random.Range (-14, 15)) * heroSize.rect.width * heroSize.localScale.x, ((int)Random.Range (-10, 11)) * heroSize.rect.height * heroSize.localScale.y), heroObject.transform.rotation);
		hero.transform.SetParent (transform);
		enemy.transform.SetParent (transform);
	
	
	}

	private void setMove (string direction)
	{
		//frontHero.setDirection (direction);
		onUp = direction.Equals ("up") ? true : false;
		onDown = direction.Equals ("down") ? true : false;
		onLeft = direction.Equals ("left") ? true : false;
		onRight = direction.Equals ("right") ? true : false;
	}

	void OnTriggerEnter2D (Collider2D other)
	{

	}

	// When head hero die, rearrange the line 
	private void losingHero ()
	{
		source.PlayOneShot (loseSound);
		Core.getInstance ().setTempPosition (((HeroUnit)Core.getInstance ().heroLine () [0]).transform.position);
		Core.getInstance ().setTempDirection (((HeroUnit)Core.getInstance ().heroLine () [0]).getFlipDirection ());
		Debug.Log ("crash");
		Core.getInstance ().getCurrentHead ().addHp (-100);
		Core.getInstance ().heroLine ().RemoveAt (0);

		if (Core.getInstance ().heroLine ().Count == 0) {
			Core.gameEnd = true;
			Core.anotherRound = true;
		} else {
		
			frontHero = ((HeroUnit)Core.getInstance ().heroLine () [0]);
			heroSize = frontHero.GetComponent<RectTransform> ();
			Debug.Log (frontHero.name);
			Core.getInstance ().setCurrentHeadHero (frontHero);
			Core.getInstance ().getCurrentHead ().setHead (true);
			Core.getInstance ().getCurrentHead ().setDirection (currentDirection);
			//setMove (currentDirection);
			if (Core.heroCrash)
				Core.heroCrash = false;
			else {
				for (int j = 0; j < Core.getInstance().heroLine().Count; j++) {
					Vector3 otherTemp = ((HeroUnit)Core.getInstance ().heroLine () [j]).transform.position;
					string dirTemp = ((HeroUnit)Core.getInstance ().heroLine () [j]).getFlipDirection ();
					((HeroUnit)Core.getInstance ().heroLine () [j]).transform.position = Core.getInstance ().getTempPosition ();	
					((HeroUnit)Core.getInstance ().heroLine () [j]).flip (Core.getInstance ().getTempDirection ());
					Core.getInstance ().setTempPosition (otherTemp);
					Core.getInstance ().setTempDirection (dirTemp);
				}
			}
		}
	}

	private bool isCrash ()
	{

		//When hit walll directly in vertical
		//First time check is nearby wall, then take a crash if hit again
		if (frontHero.transform.position.x + moveX < -5.1f ||
			frontHero.transform.position.x + moveX > 5.1f && moveY == 0) {

			if (isNearbyWallLeft || isNearbyWallRight) {
				return true;
			}

			if (frontHero.transform.position.x + moveX < -5.1f)
				isNearbyWallLeft = true;
			else
				isNearbyWallRight = true;

			return false;

			//When hit walll directly in horizontal
			//First time check is nearby wall, then take a crash if hit again
		} else if ((frontHero.transform.position.y + moveY < -3.7f ||
			frontHero.transform.position.y + moveY > 3.7f && moveX == 0)) {
	
			if (isNearbyWallUp || isNearbyWallDown) {
				return true;
			}

			if (frontHero.transform.position.y + moveY < -3.7f)
				isNearbyWallDown = true;
			else
				isNearbyWallUp = true;

			return false;

			// When hit wall and move along vertical wall 
		} else if ((frontHero.transform.position.x < -5.03f ||
			frontHero.transform.position.x > 5.03f && moveX == 0)) {

			if (frontHero.transform.position.x < -5.03f)
				isNearbyWallLeft = true;
			else
				isNearbyWallRight = true;

			isNearbyWallUp = false;
			isNearbyWallDown = false;

			return false;
		}

		// When hit wall and move along horizontal wall
		 else if ((frontHero.transform.position.y < -3.5f ||
			frontHero.transform.position.y > 3.5f && moveY == 0)) {

			if (frontHero.transform.position.y < -3.5f)
				isNearbyWallDown = true;
			else
				isNearbyWallUp = true;

			isNearbyWallLeft = false;
			isNearbyWallRight = false;
			return false;

		} else {
			isNearbyWallLeft = false;
			isNearbyWallRight = false;
			isNearbyWallUp = false;
			isNearbyWallDown = false;
			return false;

		}
	}
}
