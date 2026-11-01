using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth;
    public float health;
    public bool alive;

    private SpriteRenderer sr;

    public void TakeDamage(float damage) {
        health -= damage;
        StartCoroutine(Flicker());
        if (health <= 0f) {
            alive = false;
            StartCoroutine(Death());
        }
    }

    private IEnumerator Flicker() {
        float duration = 0.1f;
        Color c = sr.color;
        for (int i = 0; i < 2; i++) {
            float elapsed = 0f;
            while (elapsed < duration) {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0.5f, elapsed / duration);
                sr.color = new Color(c.r, c.g, c.b, alpha);
                yield return null;
            }
            elapsed = 0f;
            while (elapsed < duration) {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(0.5f, 1f, elapsed / duration);
                sr.color = new Color(c.r, c.g, c.b, alpha);
                yield return null;
            }
        }
        sr.color = new Color(c.r, c.g, c.b, 1f);
    }

    private IEnumerator Death() {
        float duration = 0.1f;
        Color c = sr.color;
        sr.color = new Color(c.r, 0, 0, c.a);
        float elapsed = 0f;
        while (elapsed < duration) {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0.5f, elapsed / duration);
            sr.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }
        Destroy(gameObject);
    }

    private void Awake()
    {
        health = maxHealth;
        alive = true;
        sr = GetComponent<SpriteRenderer>();
    }

}
