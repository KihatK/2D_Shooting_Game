using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager gameManager;
    public ObjectManager objectManager;
    public GameObject[] followers;
    public GameObject boomEffect;
    public float speed;
    public float maxShotDelay;
    public float curShotDelay;
    public int life;
    public int maxPower;
    public int power;
    public int maxBoom;
    public int boom;
    public int score;
    public bool isHit;
    public bool isRespawnTime;
    public bool isBoomTime;

    public bool[] joyControl;
    public bool isControl;
    public bool isButtonA;
    public bool isButtonB;

    SpriteRenderer spriteRenderer;
    Animator anim;
    bool isTouchTop;
    bool isTouchBottom;
    bool isTouchRight;
    bool isTouchLeft;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable() {
        Unbeatable();

        Invoke("Unbeatable", 3f);
    }

    void Unbeatable() {
        isRespawnTime = !isRespawnTime;

        if (isRespawnTime) {
            //Immortal mode for 3 seconds
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);
        }
        else {
            //Get back to normal mode
            spriteRenderer.color = new Color(1, 1, 1, 1);
        }
    }

    void Update()
    {
        Move();
        Fire();
        Boom();
        Reload();
    }

    public void JoyPanel(int type) {
        for (int i = 0; i < 9; i++) {
            joyControl[i] = i == type;
        }
    }

    public void JoyDown() {
        isControl = true;
    }

    public void JoyUp() {
        isControl = false;
    }

    void Move() {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        //Joy Control Value
        if (joyControl[0]) { h = -1; v = 1; }
        else if (joyControl[1]) { h = 0; v = 1; }
        else if (joyControl[2]) { h = 1; v = 1; }
        else if (joyControl[3]) { h = -1; v = 0; }
        else if (joyControl[4]) { h = 0; v = 0; }
        else if (joyControl[5]) { h = 1; v = 0; }
        else if (joyControl[6]) { h = -1; v = -1; }
        else if (joyControl[7]) { h = 0; v = -1; }
        else if (joyControl[8]) { h = 1; v = -1; }

        if ((isTouchLeft && h == -1) || (isTouchRight && h == 1) || !isControl) {
            h = 0;
        }
        if ((isTouchTop && v == 1) || (isTouchBottom && v == -1) || !isControl) {
            v = 0;
        }

        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime;

        transform.position = curPos + nextPos;

        //Animation
        if (Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Horizontal")) {
            anim.SetInteger("Input", (int)h);
        }
    }

    public void ButtonADown() {
        isButtonA = true;
    }

    public void ButtonAUp() {
        isButtonA = false;
    }

    public void ButtonBDown() {
        isButtonB = true;
    }

    void Fire() {
        //if (!Input.GetButton("Fire1")) {
        //    return;
        //}

        if (!isButtonA) {
            return;
        }

        if (curShotDelay < maxShotDelay) {
            return;
        }

        //Firing Process
        switch (power) {
            case 1:
                GameObject bullet = objectManager.MakeObj("BulletPlayerA");
                bullet.transform.position = transform.position;

                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 2:
                GameObject bulletR = objectManager.MakeObj("BulletPlayerA");
                bulletR.transform.position = transform.position + Vector3.right * 0.1f;
                GameObject bulletL = objectManager.MakeObj("BulletPlayerA");
                bulletL.transform.position = transform.position + Vector3.left * 0.1f;

                Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
                rigidR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            default:
                GameObject bulletRR = objectManager.MakeObj("BulletPlayerA");
                bulletRR.transform.position = transform.position + Vector3.right * 0.35f;
                GameObject bulletCC = objectManager.MakeObj("BulletPlayerB");
                bulletCC.transform.position = transform.position;
                GameObject bulletLL = objectManager.MakeObj("BulletPlayerA");
                bulletLL.transform.position = transform.position + Vector3.left * 0.35f;

                Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidCC = bulletCC.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();
                rigidRR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidCC.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidLL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
        }

        curShotDelay = 0;
    }

    void Reload() {
        curShotDelay += Time.deltaTime;
    }

    void Boom() {
        //if (!Input.GetButton("Fire2")) {
        //    return;
        //}

        if (!isButtonB) {
            return;
        }

        if (isBoomTime) {
            return;
        }

        if (boom == 0) {
            return;
        }

        boom--;
        isBoomTime = true;
        gameManager.UpdateBoomIcon(boom);

        //Boom Animation
        boomEffect.SetActive(true);
        Invoke("OffBoomEffect", 4f);

        GameObject[] enemyL = objectManager.GetPool("EnemyL");
        GameObject[] enemyM = objectManager.GetPool("EnemyM");
        GameObject[] enemyS = objectManager.GetPool("EnemyS");

        for (int i = 0; i < enemyL.Length; i++) {
            if (enemyL[i].activeSelf) {
                Enemy enemyLogic = enemyL[i].GetComponent<Enemy>();

                enemyLogic.OnHit(1000);
            }
        }
        for (int i = 0; i < enemyM.Length; i++) {
            if (enemyM[i].activeSelf) {
                Enemy enemyLogic = enemyM[i].GetComponent<Enemy>();

                enemyLogic.OnHit(1000);
            }
        }
        for (int i = 0; i < enemyS.Length; i++) {
            if (enemyS[i].activeSelf) {
                Enemy enemyLogic = enemyS[i].GetComponent<Enemy>();

                enemyLogic.OnHit(1000);
            }
        }

        GameObject[] bulletsA = objectManager.GetPool("BulletEnemyA");
        GameObject[] bulletsB = objectManager.GetPool("BulletEnemyB");
        GameObject[] bulletsBossA = objectManager.GetPool("BulletBossA");
        GameObject[] bulletsBossB = objectManager.GetPool("BulletBossB");

        for (int i = 0; i < bulletsA.Length; i++) {
            if (bulletsA[i].activeSelf) {
                bulletsA[i].SetActive(false);
            }
        }
        for (int i = 0; i < bulletsB.Length; i++) {
            if (bulletsB[i].activeSelf) {
                bulletsB[i].SetActive(false);
            }
        }
        for (int i = 0; i < bulletsBossA.Length; i++) {
            if (bulletsBossA[i].activeSelf) {
                bulletsBossA[i].SetActive(false);
            }
        }
        for (int i = 0; i < bulletsBossB.Length; i++) {
            if (bulletsBossB[i].activeSelf) {
                bulletsBossB[i].SetActive(false);
            }
        }
    }

    void OffBoomEffect() {
        boomEffect.SetActive(false);

        isButtonB = false;
        isBoomTime = false;
    }

    void AddFollower() {
        if (power == 4) {
            followers[0].SetActive(true);
        }
        else if (power == 5) {
            followers[1].SetActive(true);
        }
        else if (power == 6) {
            followers[2].SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Border") {
            switch (collision.gameObject.name) {
                case "Top":
                    isTouchTop = true;
                    break;
                case "Bottom":
                    isTouchBottom = true;
                    break;
                case "Right":
                    isTouchRight = true;
                    break;
                case "Left":
                    isTouchLeft = true;
                    break;
            }
        }
        else if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBullet") {
            if (isRespawnTime) {
                return;
            }

            if (isHit) {
                return;
            }

            isHit = true;
            life--;

            //Die Animation
            gameManager.UpdateLifeIcon(life);
            gameManager.CallExplosion(transform.position, "P");

            if (life == 0) {
                //Game Over
                gameManager.GameOver();
            }
            else {
                //Player Respawn
                gameManager.RespawnPlayer();
            }

            //Player and hit bullet disappear
            gameObject.SetActive(false);
            collision.gameObject.SetActive(false);
            
            if (collision.gameObject.tag == "Enemy") {
                //if collision is enemy rotation normalize
                collision.transform.rotation = Quaternion.identity;
            }
        }
        else if (collision.gameObject.tag == "Item") {
            Item item = collision.GetComponent<Item>();

            switch (item.type) {
                case "Coin":
                    score += 1000;
                    break;
                case "Power":
                    if (power == maxPower) {
                        score += 500;
                    }
                    else {
                        power++;
                        AddFollower();
                    }
                    break;
                case "Boom":
                    if (boom == maxBoom) {
                        score += 500;
                    }
                    else {
                        boom++;
                        gameManager.UpdateBoomIcon(boom);
                    }
                    break;
            }
            collision.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Border") {
            switch (collision.gameObject.name) {
                case "Top":
                    isTouchTop = false;
                    break;
                case "Bottom":
                    isTouchBottom = false;
                    break;
                case "Right":
                    isTouchRight = false;
                    break;
                case "Left":
                    isTouchLeft = false;
                    break;
            }
        }
    }
}
