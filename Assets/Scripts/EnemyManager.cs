using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    OneHand, TwoHand, Archer
  
}

public enum PatrolType
{
    Linear,
    Random,
    Loop
}

public class EnemyManager : MonoBehaviour
{
    public Transform[] spawnPoints;
    public string[] enemyNames;
    public GameObject[] enemyTypes;

    public List<GameObject> enemies;
    //Kill enemies with Two in their name
    public string killCondition = "Two";

    private void Start()
    {

        //SpawnEnemies();

        SpawnAtRandom();

        StartCoroutine(SpawnEnemyAtRandom());


    }
    /// <summary>
    /// Spawns an enemy at every spawn point
    /// </summary>
   
    public void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.I))
            SpawnAtRandom();

        //Kill the first enemy in our list
        if (Input.GetKeyDown(KeyCode.K))
            KillAllEnemies(enemies[0]);

        if (Input.GetKeyDown(KeyCode.A))
        {
            KillSpecificEnemies(killCondition);
        }
    }

    public void SpawnEnemies()
    {
           
        //Looping for all spawn points        
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            //Spawn random enemies from all spawn points every time game starts 
            int rnd = Random.Range(0, enemyTypes.Length);
            GameObject enemy = Instantiate(enemyTypes[rnd], spawnPoints[i].position, spawnPoints[i].rotation);
        }



    }

    /// <summary>
    /// Spawns a random enemy at a random spawn point
    /// </summary>
    void SpawnAtRandom()
    {

        int rndEnemy = Random.Range(0, enemyTypes.Length);
        int rndSpawn = Random.Range(0, spawnPoints.Length);
        GameObject enemy = Instantiate(enemyTypes[rndEnemy], spawnPoints[rndSpawn].position, spawnPoints[rndSpawn].rotation);
        //Enemies is our list
        enemies.Add(enemy);
        ShowEnemyCount();

    }
    /// <summary>
    /// Spawns an enemy randomly with a 2 second delay
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnEnemyAtRandom()
    {
        //Coroutine loop
        for (int i = 0; i < enemyTypes.Length; i++)
        {
            int rndEnemy = Random.Range(0, enemyTypes.Length);
            int rndSpawn = Random.Range(0, spawnPoints.Length);
            GameObject enemy = Instantiate(enemyTypes[rndEnemy], spawnPoints[rndSpawn].position, spawnPoints[rndSpawn].rotation);

            yield return new WaitForSeconds(2);
        }
    }

    /// <summary>
    /// Shows the amount of enemies in the stage
    /// </summary>
    void ShowEnemyCount()
    {
        print("Number of enemies: " + enemies.Count);

    }

    /// <summary>
    /// Kills a specific enemy
    /// </summary>
    /// <param name="_enemy">The enemy we wan to kill</param>
    void KillEnemy(GameObject _enemy)
    {
        if (enemies.Count == 0)
            return;

        Destroy(_enemy);
        enemies.Remove(_enemy);

        ShowEnemyCount();
    }

    /// <summary>
    /// Kills all enemies in our stage
    /// </summary>
    void KillAllEnemies(GameObject _emeny)
    {
        if (enemies.Count == 0)
            return;

        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            KillEnemy(enemies[i]);
        }
        

    }

    /// <summary>
    /// Kills specific enemies
    /// </summary>
    /// <param name="_condition">The condition of the enemy we want to kill</param>
    void KillSpecificEnemies(string _condition)
    {
        //Loop through all the enemies we have and kill the enmey with the kill condition Two in its name
        for (int i = 0; i <= enemies.Count; i++)
        {
            if (enemies[i].name.Contains(_condition))
                KillEnemy(enemies[i]);
        }
    }

    /// <summary>
    /// Get a random sapwn point
    /// </summary>
    /// <returns>A random spawn point</returns>
    public Transform GetRandomSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)];
    }

    void Examples()
    {
        //Loop if i is less than 100, it will add 1 and print
        int numberRepititions = 2000;
        for (int i = 0; i <= numberRepititions; i++)
        {
            //This will run until i=100
            print(i);
        }

        enemyNames[2] = "A new name";
        print(enemyNames.Length);
        print(enemyNames[2]);        
        print(enemyNames[enemyNames.Length - 1]);

        //Create a loop within a loop for a wall
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);

        for (int i = 0; i < 50; i++)
        {
            for (int j = 0; j < 50; j++)
            {
                Instantiate(wall, new Vector3(i, j, 0), transform.rotation);
            }
        }
    }
}
