/**
 * Created by Viroj Loharatanavisit
 * This game is created for the Infinity Levels nscreening test
 *
 */

using UnityEngine;
using System.Collections;

public class EnemySpector : MonoBehaviour
{

	public string direction;
	Core core = Core.getInstance ();

	// Use this for initialization
	void Start ()
	{
	
	}

	// Update is called once per frame
	void Update ()
	{
	}

	void OnTriggerEnter2D (Collider2D other)
	{

		if (gameObject.tag.Equals ("LeftCheck"))
			core.setLeftEnemy (other.gameObject.GetComponent<EnemyUnit> ());
		else if (gameObject.tag.Equals ("RightCheck"))
			core.setRightEnemy (other.gameObject.GetComponent<EnemyUnit> ());
		else if (gameObject.tag.Equals ("UpCheck"))
			core.setUpEnemy (other.gameObject.GetComponent<EnemyUnit> ());
		else if (gameObject.tag.Equals ("DownCheck"))
			core.setDownEnemy (other.gameObject.GetComponent<EnemyUnit> ());
	}

	void OnTriggerExit2D (Collider2D other)
	{
		if (gameObject.tag.Equals ("LeftCheck"))
			core.setLeftEnemy (null);
		else if (gameObject.tag.Equals ("RightCheck"))
			core.setRightEnemy (null);
		else if (gameObject.tag.Equals ("UpCheck"))
			core.setUpEnemy (null);
		else if (gameObject.tag.Equals ("DownCheck"))
			core.setDownEnemy (null);
	}
}
