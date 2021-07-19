using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    public ObjectManager objectManager;
    public GameObject player;
    public GameObject gameOverSet;
    public Transform[] spawnPoints;
    public Image[] lifeImage;
    public Image[] boomImage;
    public Text scoreText;
    public float nextSpawnDelay;
    public float curSpawnDelay;
    public int stage;

    public Animator stageAnim;
    public Animator clearAnim;
    public Animator fadeAnim;
    public Transform playerPos;

    //Read Spawn Information
    public List<Spawn> spawnList;
    public int spawnIndex;
    public bool spawnEnd;

    string[] enemyObjs;

    private void Awake() {
        spawnList = new List<Spawn>();
        enemyObjs = new string[] { "EnemyS", "EnemyM", "EnemyL", "EnemyB" };

        StageStart();
    }

    public void StageStart() {
        //#.Stage Ui Load
        stageAnim.SetTrigger("On");
        stageAnim.GetComponent<Text>().text = "STAGE " + stage + "\nSTART";
        clearAnim.GetComponent<Text>().text = "STAGE " + stage + "\nCLEAR!!";

        //#.Enemy Spawn File Read
        ReadSpawnFile();

        //#.Fade In
        fadeAnim.SetTrigger("In");
    }

    public void StageEnd() {
        //#.Stage Ui Load
        clearAnim.SetTrigger("On");

        //#.Fade Out
        fadeAnim.SetTrigger("Out");

        //#.Player Repos
        player.transform.position = playerPos.position;

        //#.Stage Increment
        stage++;
        if (stage > 2) {
            Invoke("GameOver", 6);
        }
        else {
            Invoke("StageStart", 5);
        }
    }

    void ReadSpawnFile() {
        //variable initialization
        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;

        TextAsset textFile = Resources.Load("Stage " + stage) as TextAsset;
        StringReader stringReader = new StringReader(textFile.text);

        while (stringReader != null) {
            string line = stringReader.ReadLine();
            Debug.Log(line);

            if (line == null) {
                break;
            }

            Spawn spawnData = new Spawn();
            spawnData.delay = float.Parse(line.Split(',')[0]);
            spawnData.type = line.Split(',')[1];
            spawnData.point = int.Parse(line.Split(',')[2]);

            spawnList.Add(spawnData);
        }

        //텍스트파일 닫기
        stringReader.Close();

        nextSpawnDelay = spawnList[0].delay;
    }

    private void Update() {
        curSpawnDelay += Time.deltaTime;

        if (curSpawnDelay > nextSpawnDelay && !spawnEnd) {
            SpawnEnemy();

            curSpawnDelay = 0;
        }

        Player playerLogic = player.GetComponent<Player>();
        scoreText.text = string.Format("{0:n0}", playerLogic.score);
    }

    void SpawnEnemy() {
        int enemyIndex = 0;
        
        switch (spawnList[spawnIndex].type) {
            case "S":
                enemyIndex = 0;
                break;
            case "M":
                enemyIndex = 1;
                break;
            case "L":
                enemyIndex = 2;
                break;
            case "B":
                enemyIndex = 3;
                break;
        }
        int enemyPoint = spawnList[spawnIndex].point;

        GameObject enemy = objectManager.MakeObj(enemyObjs[enemyIndex]);
        enemy.transform.position = spawnPoints[enemyPoint].position;

        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        enemyLogic.gameManager = this;
        enemyLogic.objectManager = objectManager;
        enemyLogic.player = player;

        if (enemyPoint == 5 || enemyPoint == 6) {
            enemy.transform.Rotate(Vector3.back * 90);
            rigid.velocity = new Vector2((-1) * enemyLogic.speed, -1);
        }
        else if (enemyPoint == 7 || enemyPoint == 8) {
            enemy.transform.Rotate(Vector3.forward * 90);
            rigid.velocity = new Vector2(enemyLogic.speed, -1);
        }
        else {
            rigid.velocity = Vector2.down * enemyLogic.speed;
        }

        //Respawn Index increase
        spawnIndex++;
        if (spawnIndex == spawnList.Count) {
            spawnEnd = true;
            return;
        }

        nextSpawnDelay = spawnList[spawnIndex].delay;
    }

    public void UpdateLifeIcon(int life) {
        //Life Icon 초기화
        for (int i = 0; i < 3; i++) {
            lifeImage[i].color = new Color(1, 1, 1, 0);
        }

        //Life Icon Active
        for (int i = 0; i < life; i++) {
            lifeImage[i].color = new Color(1, 1, 1, 1);
        }
    }

    public void UpdateBoomIcon(int boom) {
        //초기화
        for (int i = 0; i < 3; i++) {
            boomImage[i].color = new Color(1, 1, 1, 0);
        }

        for (int i = 0; i < boom; i++) {
            boomImage[i].color = new Color(1, 1, 1, 1);
        }
    }

    public void CallExplosion(Vector3 pos, string type) {
        GameObject explosion = objectManager.MakeObj("Explosion");
        Explosion explosionLogic = explosion.GetComponent<Explosion>();

        explosion.transform.position = pos;
        explosionLogic.StartExplosion(type);
    }

    public void RespawnPlayer() {
        Invoke("RespawnPlayerExe", 2f);
    }

    void RespawnPlayerExe() {
        player.transform.position = Vector3.down * 4;
        player.SetActive(true);

        Player playerLogic = player.GetComponent<Player>();
        playerLogic.isHit = false;
    }

    public void GameOver() {
        gameOverSet.SetActive(true);
    }

    public void GameRetry() {
        SceneManager.LoadScene(0);
    }
}
