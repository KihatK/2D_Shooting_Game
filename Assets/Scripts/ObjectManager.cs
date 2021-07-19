using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameObject enemyBPrefabs;
    public GameObject enemyLPrefabs;
    public GameObject enemyMPrefabs;
    public GameObject enemySPrefabs;
    public GameObject itemCoinPrefabs;
    public GameObject itemPowerPrefabs;
    public GameObject itemBoomPrefabs;
    public GameObject bulletPlayerAPrefabs;
    public GameObject bulletPlayerBPrefabs;
    public GameObject bulletFollowerPrefabs;
    public GameObject bulletEnemyAPrefabs;
    public GameObject bulletEnemyBPrefabs;
    public GameObject bulletBossAPrefabs;
    public GameObject bulletBossBPrefabs;
    public GameObject explosionPrefabs;

    GameObject[] enemyB;
    GameObject[] enemyL;
    GameObject[] enemyM;
    GameObject[] enemyS;

    GameObject[] itemCoin;
    GameObject[] itemPower;
    GameObject[] itemBoom;

    GameObject[] bulletPlayerA;
    GameObject[] bulletPlayerB;
    GameObject[] bulletFollower;
    GameObject[] bulletEnemyA;
    GameObject[] bulletEnemyB;
    GameObject[] bulletBossA;
    GameObject[] bulletBossB;
    GameObject[] explosion;

    GameObject[] targetPool;

    private void Awake() {
        enemyB = new GameObject[1];
        enemyL = new GameObject[10];
        enemyM = new GameObject[10];
        enemyS = new GameObject[20];

        itemCoin = new GameObject[20];
        itemPower = new GameObject[10];
        itemBoom = new GameObject[10];

        bulletPlayerA = new GameObject[200];
        bulletPlayerB = new GameObject[200];
        bulletFollower = new GameObject[100];
        bulletEnemyA = new GameObject[100];
        bulletEnemyB = new GameObject[100];
        bulletBossA = new GameObject[50];
        bulletBossB = new GameObject[1000];
        explosion = new GameObject[100];

        Generate();
    }
    
    void Generate() {
        for (int i = 0; i < enemyB.Length; i++) {
            enemyB[i] = Instantiate(enemyBPrefabs);
            enemyB[i].SetActive(false);
        }
        for (int i = 0; i < enemyL.Length; i++) {
            enemyL[i] = Instantiate(enemyLPrefabs);
            enemyL[i].SetActive(false);
        }
        for (int i = 0; i < enemyM.Length; i++) {
            enemyM[i] = Instantiate(enemyMPrefabs);
            enemyM[i].SetActive(false);
        }
        for (int i = 0; i < enemyS.Length; i++) {
            enemyS[i] = Instantiate(enemySPrefabs);
            enemyS[i].SetActive(false);
        }

        for (int i = 0; i < itemCoin.Length; i++) {
            itemCoin[i] = Instantiate(itemCoinPrefabs);
            itemCoin[i].SetActive(false);
        }
        for (int i = 0; i < itemPower.Length; i++) {
            itemPower[i] = Instantiate(itemPowerPrefabs);
            itemPower[i].SetActive(false);
        }
        for (int i = 0; i < itemBoom.Length; i++) {
            itemBoom[i] = Instantiate(itemBoomPrefabs);
            itemBoom[i].SetActive(false);
        }

        for (int i = 0; i < bulletPlayerA.Length; i++) {
            bulletPlayerA[i] = Instantiate(bulletPlayerAPrefabs);
            bulletPlayerA[i].SetActive(false);
        }
        for (int i = 0; i < bulletPlayerB.Length; i++) {
            bulletPlayerB[i] = Instantiate(bulletPlayerBPrefabs);
            bulletPlayerB[i].SetActive(false);
        }

        for (int i = 0; i < bulletFollower.Length; i++) {
            bulletFollower[i] = Instantiate(bulletFollowerPrefabs);
            bulletFollower[i].SetActive(false);
        }

        for (int i = 0; i < bulletEnemyA.Length; i++) {
            bulletEnemyA[i] = Instantiate(bulletEnemyAPrefabs);
            bulletEnemyA[i].SetActive(false);
        }
        for (int i = 0; i < bulletEnemyB.Length; i++) {
            bulletEnemyB[i] = Instantiate(bulletEnemyBPrefabs);
            bulletEnemyB[i].SetActive(false);
        }
        for (int i = 0; i < bulletBossA.Length; i++) {
            bulletBossA[i] = Instantiate(bulletBossAPrefabs);
            bulletBossA[i].SetActive(false);
        }
        for (int i = 0; i < bulletBossB.Length; i++) {
            bulletBossB[i] = Instantiate(bulletBossBPrefabs);
            bulletBossB[i].SetActive(false);
        }

        for (int i = 0; i < explosion.Length; i++) {
            explosion[i] = Instantiate(explosionPrefabs);
            explosion[i].SetActive(false);
        }
    }

    public GameObject MakeObj(string type) {
        switch (type) {
            case "EnemyB":
                targetPool = enemyB;
                break;
            case "EnemyL":
                targetPool = enemyL;
                break;
            case "EnemyM":
                targetPool = enemyM;
                break;
            case "EnemyS":
                targetPool = enemyS;
                break;
            case "ItemCoin":
                targetPool = itemCoin;
                break;
            case "ItemPower":
                targetPool = itemPower;
                break;
            case "ItemBoom":
                targetPool = itemBoom;
                break;
            case "BulletPlayerA":
                targetPool = bulletPlayerA;
                break;
            case "BulletPlayerB":
                targetPool = bulletPlayerB;
                break;
            case "BulletFollower":
                targetPool = bulletFollower;
                break;
            case "BulletEnemyA":
                targetPool = bulletEnemyA;
                break;
            case "BulletEnemyB":
                targetPool = bulletEnemyB;
                break;
            case "BulletBossA":
                targetPool = bulletBossA;
                break;
            case "BulletBossB":
                targetPool = bulletBossB;
                break;
            case "Explosion":
                targetPool = explosion;
                break;
        }

        for (int i = 0; i < targetPool.Length; i++) {
            if (!targetPool[i].activeSelf) {
                targetPool[i].SetActive(true);
                return targetPool[i];
            }
        }

        return null;
    }

    public GameObject[] GetPool(string type) {
        switch (type) {
            case "EnemyB":
                targetPool = enemyB;
                break;
            case "EnemyL":
                targetPool = enemyL;
                break;
            case "EnemyM":
                targetPool = enemyM;
                break;
            case "EnemyS":
                targetPool = enemyS;
                break;
            case "ItemCoin":
                targetPool = itemCoin;
                break;
            case "ItemPower":
                targetPool = itemPower;
                break;
            case "ItemBoom":
                targetPool = itemBoom;
                break;
            case "BulletPlayerA":
                targetPool = bulletPlayerA;
                break;
            case "BulletPlayerB":
                targetPool = bulletPlayerB;
                break;
            case "BulletFollower":
                targetPool = bulletFollower;
                break;
            case "BulletEnemyA":
                targetPool = bulletEnemyA;
                break;
            case "BulletEnemyB":
                targetPool = bulletEnemyB;
                break;
            case "BulletBossA":
                targetPool = bulletBossA;
                break;
            case "BulletBossB":
                targetPool = bulletBossB;
                break;
            case "Explosion":
                targetPool = explosion;
                break;
        }

        return targetPool;
    }
}
