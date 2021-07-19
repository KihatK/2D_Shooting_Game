using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public ObjectManager objectManager;
    public Player player;
    public float maxShotDelay;
    public float curShotDelay;

    public Vector3 followPos;
    public int followDelay;
    public Transform parent;
    public Queue<Vector3> parentPos;

    private void Awake() {
        parentPos = new Queue<Vector3>();
    }

    private void OnEnable() {
        transform.position = parent.position;
    }

    private void Update() {
        Watch();
        Follow();
        Fire();
        Reload();
    }

    void Watch() {
        if (!parentPos.Contains(parent.position)) {
            parentPos.Enqueue(parent.position);
        }

        if (parentPos.Count > followDelay) {
            followPos = parentPos.Dequeue();
        }
        else if (parentPos.Count < followDelay) {
            followPos = parent.position;
        }
    }

    void Follow() {
        transform.position = followPos;
    }

    void Fire() {
        //if (!Input.GetButton("Fire1")) {
        //    return;
        //}

        if (!player.isButtonA) {
            return;
        }

        if (curShotDelay < maxShotDelay) {
            return;
        }

        GameObject bullet = objectManager.MakeObj("BulletFollower");
        bullet.transform.position = transform.position;

        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

        curShotDelay = 0;
    }

    void Reload() {
        curShotDelay += Time.deltaTime;
    }
}
