/**
 * Created by Viroj Loharatanavisit
 * This game is created for the Infinity Levels nscreening test
 *
 */

using UnityEngine;
using System.Collections;

public class SpriteScript : MonoBehaviour
{
	public UnitProfile unitData;
	public bool isHero;
	public TextMesh textDamage;
	public float textMovePosition = 0f;
	public float textAlpha = 1f;

	private float TEXT_MOVE = 0.1f;
	private float tmpMove = 0;


	// Use this for initialization

	void Start ()
	{
		UnityEngine.Sprite[] sprites = Resources.LoadAll <UnityEngine.Sprite> (isHero ? "Sprite/HeroSprite" : "Sprite/EnemySprite");  
		GetComponent<SpriteRenderer> ().sprite = sprites [(int)Random.Range (0, sprites.Length)];

	}
	
	// Update is called once per frame
	void Update ()
	{
		textDamage.text = "-" + unitData.getDamageNumber ();
		textDamage.color = new Color (textDamage.color.r, textDamage.color.g, textDamage.color.b, textAlpha);
		textDamage.transform.position = new Vector2 (textDamage.transform.position.x, textDamage.transform.position.y + (TEXT_MOVE * textMovePosition) - tmpMove);
		tmpMove = TEXT_MOVE * textMovePosition;
	}
}
