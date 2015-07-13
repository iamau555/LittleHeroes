/**
 * Created by Viroj Loharatanavisit
 * This game is created for the Infinity Levels nscreening test
 *
 */

using UnityEngine;
using System.Collections;

public class Core
{
	
	public static Core instance = null;
	public static string enemyDirection = "none";
	public static string tempDirection = "left";
	public static bool isActive = false;
	public static bool heroCrash = false;
	public const int ON_PASSIVE = 0;
	public const int LEFT_ACTIVE = 1;
	public const int RIGHT_ACTIVE = 2;
	public const int UP_ACTIVE = 3;
	public const int DOWN_ACTIVE = 4;
	public const int ON_DEATH = -1;

	public static ArrayList heroList = new ArrayList ();
	public static Vector3 tempPostion = new Vector3 ();

	public static HeroUnit currentHead;
	public static EnemyUnit currentEnemy;

	public static EnemyUnit currentLeftEnemy;
	public static EnemyUnit currentRightEnemy;
	public static EnemyUnit currentUpEnemy;
	public static EnemyUnit currentDownEnemy;

	public static int totalScore = 0;
	public static bool gameStart = false;
	public static bool gameEnd = false;
	public static bool gameRestart = true;
	public static bool anotherRound = false;

	public static Core getInstance ()
	{
		if (instance == null) {
			instance = new Core ();
		}
		return instance;
	}

	public ArrayList heroLine ()
	{
		return heroList;
	}

	public void setTempPosition (Vector3 tmp)
	{
		tempPostion = tmp;
	}

	public Vector3 getTempPosition ()
	{
		return tempPostion;	
	}

	public void setTempDirection (string tmp)
	{
		if (tmp.Equals ("left"))
			tempDirection = "right";
		else if (tmp.Equals ("right"))
			tempDirection = "left";
		else
			tempDirection = tmp;
	}
	
	public string getTempDirection ()
	{
		return tempDirection;	
	}


	public Core ()
	{

	}

	public void addScore (int score)
	{
		totalScore += score;
	}

	public void resetScore ()
	{
		totalScore = 0;
	}

	public int getScore ()
	{
		return totalScore;
	}

	public bool getToEnemy (string dir)
	{

		if (checkEnemy (dir))
			return true;
		else
			return false;
	}

	public bool checkEnemy (string dir)
	{
		if ((dir.Equals ("left") && currentLeftEnemy != null) || 
			(dir.Equals ("right") && currentRightEnemy != null) || 
			(dir.Equals ("up") && currentUpEnemy != null) || 
			(dir.Equals ("down") && currentDownEnemy != null))
			return true;
		else
			return false;
	}

	public void setActive (bool c)
	{
		isActive = c;
	}

	public void removeCurrentEnemy (string dir)
	{
		if (dir.Equals ("left") && currentLeftEnemy != null)
			setLeftEnemy (null);
		else if (dir.Equals ("right") && currentRightEnemy != null)
			setRightEnemy (null);
		else if (dir.Equals ("up") && currentUpEnemy != null)
			setUpEnemy (null);
		else if (dir.Equals ("down") && currentDownEnemy != null)
			setDownEnemy (null);
	}

	private EnemyUnit getCurrentEnemy (string dir)
	{
		if (dir.Equals ("left") && currentLeftEnemy != null)
			return currentLeftEnemy;
		else if (dir.Equals ("right") && currentRightEnemy != null)
			return currentRightEnemy;
		else if (dir.Equals ("up") && currentUpEnemy != null)
			return currentUpEnemy;
		else if (dir.Equals ("down") && currentDownEnemy != null)
			return currentDownEnemy;
		else
			return null;
	}

	public EnemyUnit getCurrentEnemy ()
	{
		return currentEnemy;
	}

	public HeroUnit getCurrentHead ()
	{
		return currentHead;
	}

	public void setHeroDirection (string dir)
	{
		//currentEnemy = getCurrentEnemy(dir);
		currentHead.setDirection (dir);
		currentEnemy = getCurrentEnemy (dir);
	}

	public void setCurrentHeadHero (HeroUnit hero)
	{
		currentHead = hero;
	}

	public void setLeftEnemy (EnemyUnit enemy)
	{
		currentLeftEnemy = enemy;
		if (currentLeftEnemy != null) {
			currentLeftEnemy.setDirection ("right");
			currentLeftEnemy.setTempEnemyStatus ("right");
		}
	}

	public void setRightEnemy (EnemyUnit enemy)
	{
		currentRightEnemy = enemy;
		if (currentRightEnemy != null) {
			currentRightEnemy.setDirection ("left");
			currentRightEnemy.setTempEnemyStatus ("left");
		}
	}

	public void setUpEnemy (EnemyUnit enemy)
	{
		currentUpEnemy = enemy;
		if (currentUpEnemy != null) {
			currentUpEnemy.setDirection ("down");
			currentUpEnemy.setTempEnemyStatus ("down");
		}
	}

	public void setDownEnemy (EnemyUnit enemy)
	{
		currentDownEnemy = enemy;
		if (currentDownEnemy != null) {
			currentDownEnemy.setDirection ("up");
			currentDownEnemy.setTempEnemyStatus ("up");
		}
	}

	public int getHeroStatus ()
	{
		if (currentHead.getDirection ().Equals ("left") && isActive)
			return LEFT_ACTIVE;
		else if (currentHead.getDirection ().Equals ("right") && isActive)
			return RIGHT_ACTIVE;
		else if (currentHead.getDirection ().Equals ("up") && isActive)
			return UP_ACTIVE;
		else if (currentHead.getDirection ().Equals ("down") && isActive)
			return DOWN_ACTIVE;
		else if (currentHead.getDirection ().Equals ("death"))
			return ON_DEATH;
		else
			return ON_PASSIVE;
	}

	public int getEnemyStatus ()
	{
		if (currentEnemy != null) {
			if (currentEnemy.getDirection ().Equals ("left") && isActive)
				return LEFT_ACTIVE;
			else if (currentEnemy.getDirection ().Equals ("right") && isActive)
				return RIGHT_ACTIVE;
			else if (currentEnemy.getDirection ().Equals ("up") && isActive)
				return UP_ACTIVE;
			else if (currentEnemy.getDirection ().Equals ("down") && isActive)
				return DOWN_ACTIVE;
			else if (currentEnemy.getDirection ().Equals ("death"))
				return ON_DEATH;
			else
				return ON_PASSIVE;
		} else {
			return ON_PASSIVE;
		}
	}

	public void getFight ()
	{
		currentHead.getDamaged (currentEnemy.getSword (), currentEnemy.getType (), false);
		currentEnemy.getDamaged (currentHead.getSword (), currentHead.getType (), true);
	}

	public bool isFight ()
	{
		return isActive;
	}

	public bool heroWin ()
	{
		if (currentEnemy != null && currentEnemy.isDead ()) {
			return true;
		} else
			return false;
	}

	public bool enemyWin ()
	{
		if (currentHead.isDead ()) {
			return true;
		} else
			return false;
	}



}
