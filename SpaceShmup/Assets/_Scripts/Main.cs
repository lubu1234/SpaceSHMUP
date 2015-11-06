using UnityEngine; // Required for Unity
using System.Collections; // Required for Arrays & other Collections
using System.Collections.Generic; // Required to use Lists or Dictionaries


public class Main : MonoBehaviour {

	static public Main S;
	static public Dictionary<Weapon.WeaponType, Weapon.WeaponDefinition> W_DEFS;

	public GameObject[] prefabEnemies;
	public float enemySpawnPerSecond = 0.5f; //#Enemies/second
	public float enemySpawnPadding = 1.5f; //Padding for position
	public Weapon.WeaponDefinition[] weaponDefinitions;
	public GameObject prefabPowerUp;
	public Weapon.WeaponType[] powerUpFrequency = new Weapon.WeaponType[]{
		Weapon.WeaponType.blaster,Weapon.WeaponType.blaster,Weapon.WeaponType.spread, Weapon.WeaponType.shield
	};

	public bool __________________________;

	public Weapon.WeaponType[] activeWeaponTypes;

	public float enemySpawnRate; //Delay between Enemy spawns

	void Awake(){

		S = this;
		//Set Utils.camBounds

		Utils.SetCameraBounds (this.camera);

		//0.5 enemies/second = enemySpawnRate of 2

		enemySpawnRate = 1f / enemySpawnPerSecond;

		//Invoke call SpawnEnemy() once after a 2 second delay

		Invoke ("SpawnEnemy", enemySpawnRate);

		//A generic Dictionary with WeaponType as the key
		W_DEFS = new Dictionary<Weapon.WeaponType, Weapon.WeaponDefinition> ();


		foreach (Weapon.WeaponDefinition def in weaponDefinitions) {
			W_DEFS[def.type] = def;
		}

	}

	static public Weapon.WeaponDefinition GetWeaponDefinition(Weapon.WeaponType wt){
		//Check to make sure that the key exists in the Dictionary attempting to retrieve a key that didn't exist, would throw an error
		//so the following if statement is important

		if (W_DEFS.ContainsKey (wt)) {
			return(W_DEFS[wt]);
		}

		//This will return a definition for WeaponType.none which means it has failed to find the WeaponDefinition
		return (new Weapon.WeaponDefinition ());
	}

	void Start(){
		activeWeaponTypes = new Weapon.WeaponType[weaponDefinitions.Length];
		for (int i = 0; i<weaponDefinitions.Length; i++) {
			activeWeaponTypes[i] = weaponDefinitions[i].type;
		}

	}


	public void SpawnEnemy(){

		//Pick a random Enemy prefab to instantiate

		int ndx = Random.Range (0, prefabEnemies.Length);
		GameObject go = Instantiate (prefabEnemies [ndx]) as GameObject;

		//Position the enemy above the screen with a random X position

		Vector3 pos = Vector3.zero;
		float xMin = Utils.camBounds.min.x + enemySpawnPadding;
		float xMax = Utils.camBounds.max.x - enemySpawnPadding;
		pos.x = Random.Range (xMin, xMax);
		pos.y = Utils.camBounds.max.y + enemySpawnPadding;
		go.transform.position = pos;

		//Call SpawnEnemy() again in a couple of seconds
		Invoke ("SpawnEnemy", enemySpawnRate);


	}


	public void ShipDestroyed(Enemy e){
		//Potentially generate a PowerUp
		if (Random.value <= e.powerUpDropChance) {
			//Random.value generates a value between 0 & 1 never 1
			//If the e.powerUpDropChance is 0.5f a PowerUp will be generated 50% of the time. 

			//Choose which PowerUp to pick
			//Pick one from the possibilities in powerUpFrequency
			int ndx = Random.Range(0,powerUpFrequency.Length);
			Weapon.WeaponType puType = powerUpFrequency[ndx];

			//Spawn a PowerUp
			GameObject go = Instantiate(prefabPowerUp) as GameObject;
			PowerUp pu = go.GetComponent<PowerUp>();

			//Set it to the proper WeaponType
			pu.SetType(puType);

			//Set it to the position of the destroyed ship
			pu.transform.position = e.transform.position;
		}
	}


	public void DelayedRestart(float delay){
		//Invoke the Restart() method in delay seconds
		Invoke ("Restart", delay);

	}

	public void Restart(){
		//Reload _Scene_0 to restart the game
		Application.LoadLevel ("_Scene_0");
	}



}
