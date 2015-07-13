/**
 * Created by Viroj Loharatanavisit
 * This game is created for the Infinity Levels nscreening test
 *
 */

using UnityEngine;
using System.Collections;

public class UnitProfile : MonoBehaviour
{

	Animator anim;

	public SpriteRenderer mSprite;
	public string nextDirection = "left";

	// Unit main stats are start at 10 
	private readonly int START_STATS = 10;
	private readonly int TYPE_RED = 0;
	private readonly int TYPE_GREEN = 1;
	private readonly int TYPE_BLUE = 2;

	public int hp;
	public int sword;
	public int shield;
	private string direction = "none";
	private int currentDamage = 0;

	// Unit bonus stat that will randonmly seperate to each stat is also 10
	private int bonusStat = 10;

	private int type;

	public void flip (string direction)
	{
		if (direction.Equals (nextDirection)) {
			if (nextDirection.Equals ("right"))
				nextDirection = "left";
			else
				nextDirection = "right";
			Vector3 scale = mSprite.transform.localScale;
			scale.x *= - 1;
			mSprite.transform.localScale = scale;
		}
	}

	public string getFlipDirection ()
	{
		return nextDirection;
	}

	// Use this for initialization
	public void Awake ()
	{

		// Random type from 0-2
		// red = 0, blue = 1, green = 2;
		switch ((int)Random.Range (0, 3)) {
		case 0:
			type = TYPE_RED;
			break;
		case 1:
			type = TYPE_GREEN;
			break;
		default :
			type = TYPE_BLUE;
			break;
		}

		anim = GetComponent<Animator> ();
		anim.SetInteger ("Type", type);

		// randomly seperate random stat to each stat plus START_STATS = 10
		hp = START_STATS + (int)Random.Range (0, bonusStat);
		bonusStat -= (hp - START_STATS);
		sword = START_STATS + (int)Random.Range (0, bonusStat);
		bonusStat -= (sword - START_STATS);
		shield = START_STATS + bonusStat;
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	string tmpStatus;

	public Sprite getSprite ()
	{
		return mSprite.sprite;
	}
	
	public void setTempEnemyStatus (string tmp)
	{
		tmpStatus = tmp;
	}

	public string getTempEnemyStatus ()
	{
		return tmpStatus;
	}

	public void setDirection (string dir)
	{
		direction = dir;
	}

	public string getDirection ()
	{
		return direction;
	}

	public int getHp ()
	{
		return hp;
	}

	public int getSword ()
	{
		return sword;	
	}

	public int getShield ()
	{
		return shield;
	}

	public int getType ()
	{
		return type;	
	}

	public int getDamageNumber ()
	{
		return currentDamage;
	}

	public void getDamaged (int foeSword, int foeType, bool isHero)
	{

		currentDamage = (foeSword <= shield) ? 1 : (foeSword - shield);

		if (isHero && (type == foeType)) {
			currentDamage *= 2;
		}

		hp -= currentDamage;

		Debug.Log (hp + " " + (isHero ? "hero" : "enemy"));

	}

	public void addHp (int extraHp)
	{
		hp += extraHp;
		Debug.Log ("add");
	}

	public bool isDead ()
	{
		if (hp <= 0) {
			return true;
		} else 
			return false;
	}

	void OnTriggerEnter2D (Collider2D other)
	{

	}

}
