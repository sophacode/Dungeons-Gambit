using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth;
    public float health;

    private SpriteRenderer sr;

    public void TakeDamage(float damage) {
        health -= damage;
        StartCoroutine(Flicker());
        if (health <= 0f) {
            Destroy(gameObject);
        }
    }

    private IEnumerator Flicker() {
        Color c = sr.color;
        for (int i = 0; i < 2; i++) {
            for (int j = 0; j < 5; j++){
                float alpha = Mathf.Lerp(1f, 0.5f, (j + 1) / 5f);
                sr.color = new Color(c.r, c.g, c.b, alpha);
                yield return null;
            }
            Debug.Log(sr.color);
            for (int j = 0; j < 5; j++){
                float alpha = Mathf.Lerp(0.5f, 1f, (j + 1) / 5f);
                sr.color = new Color(c.r, c.g, c.b, alpha);
                yield return null;
            }
            sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, 1f);
            yield return null;
            Debug.Log(sr.color);
        }
    }

    private void Awake()
    {
        health = maxHealth;
        sr = GetComponent<SpriteRenderer>();
    }

}
