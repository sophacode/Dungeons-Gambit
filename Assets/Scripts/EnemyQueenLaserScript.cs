using System;
using System.Collections;
using UnityEngine;

public class EnemyQueenLaserScript : MonoBehaviour
{
    public Vector2 targetDirection;
    private bool canKill;

    public Transform anchor;

    private void Start()
    {
        canKill = true;
        StartCoroutine(Attack());
    }

    private IEnumerator Attack() {
        float a = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg + 90f;
        float b = a + 180f;
        for (int i = 0; i < 60; i++) {
            transform.position = anchor.position;
            float angle = a + (360f/60*i);
            transform.rotation = Quaternion.Euler(new Vector3(1, 1, angle));
            yield return null;
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        string tag = other.gameObject.tag;
        if (tag != "Bullet" && tag != "Enemy") {
            if (tag == "Player" && canKill) {
                other.gameObject.GetComponent<PlayerScript>().TakeDamage(2f, targetDirection);
                canKill = false;
            }
        }
    }
}
