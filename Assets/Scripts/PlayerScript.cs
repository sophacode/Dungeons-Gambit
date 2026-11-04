using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    private Vector2 movement;
    private Vector2 movementNonZero;
    public int speed;
    public int dashDistance;
    private float dashDelay;

    private int attack;
    public GameObject bullet;
    public float bulletShootDelay;
    private bool shooting;
    public GameObject knightMeelee;
    public GameObject laser;

    private float energy;
    public TextMeshProUGUI energyText;

    public float health;
    private float maxHealth;
    private bool canTakeDamage;

    private SpriteRenderer sr;
    public Sprite[] pieceSprites;
    public GameObject[] pieceUIBacks;
    public GameObject dashBlur;
    private SpriteRenderer dashBlurSr;

    private Animator anim;
    public UIswitching uilogic;

    private float promotionTime;

    [SerializeField] GameObject pauseUIPanel;
    [SerializeField] GameObject healthBar;

    private void Awake()
    {
        movementNonZero = new(0, 1);
        dashDelay = 0f;

        bulletShootDelay = 0f;
        shooting = false;
        attack = 0;

        energy = 20f;

        maxHealth = 10f;
        health = maxHealth;
        canTakeDamage = true;

        promotionTime = 0f;

        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        dashBlurSr = dashBlur.GetComponent<SpriteRenderer>();
        dashBlur.SetActive(false);
    }

    private void SetPieceBack(int i) {
        foreach (GameObject g in pieceUIBacks) {
            g.SetActive(false);
        }
        pieceUIBacks[i].SetActive(true);
    }

    public void AddEnergy(float value) {
        energy += value;
        energyText.SetText("Energy: " + energy);
        uilogic.updatePieceFills(energy);
    }

    public void TakeDamage(float damage) {
        if (canTakeDamage) {
            health -= damage;
            healthBar.transform.localScale = new Vector3(health/maxHealth, 1f, 1f);
            StartCoroutine(Flicker());
            if (health <= 0f) {
                health = 0f;
                Die();
            }
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

    private void Die() {
        Debug.Log("DEAD");
        SceneManager.LoadScene(2);
    }

    public void OnPause() {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>().OnPause();
        GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
        pauseUIPanel.SetActive(true);
    }

    public void OnPlay() {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>().OnPlay();
        GetComponent<PlayerInput>().SwitchCurrentActionMap("Movement");
        pauseUIPanel.SetActive(false);
    }

    public void OnMove(InputValue inputValue) {
        movement = inputValue.Get<Vector2>();
        if (movement != Vector2.zero ) {
            movementNonZero = movement;
            anim.SetBool("Walking", true);
        }
        else {
            anim.SetBool("Walking", false);
        }
    }

    public void OnDash() {
        if (dashDelay <= 0f) {  
            StartCoroutine(Dash());
            dashDelay = 0.5f;
        }
    }

    private IEnumerator Dash() {
        canTakeDamage = false;
        for (int i = 0; i < 10; i++) {
            DashFX();
            transform.Translate(movementNonZero * Time.deltaTime * dashDistance/10);
            yield return null;
        }
        canTakeDamage = true;
    }

    private void DashFX() { // hopefully spawns dash sprite behind player character
        Vector2 dir = movementNonZero.normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        dashBlur.transform.position = transform.position - (Vector3)(dir * 0.5f);
        dashBlur.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        dashBlurSr.flipY = dir.x < 0;
        dashBlur.SetActive(true);
        CancelInvoke(nameof(HideDashFX));
        Invoke(nameof(HideDashFX), 0.2f);
    }

    private void HideDashFX() {
        dashBlur.SetActive(false);
    }

    public void OnAttack() {
        shooting = !shooting;
    }

    private void UnPromote() {
        Debug.Log("Unpromote");
        attack = 0;
        anim.SetBool("Walking", false);
        anim.SetTrigger("Unpromote");
        sr.sprite = pieceSprites[0];
    }

    public void OnPromoteBishop() {
        Debug.Log("Attempting to promote to bishop");
        if (energy >= 3f) {
            Debug.Log("Promote");
            attack = 1;
            AddEnergy(-3);
            anim.SetTrigger("PromoteBishop");
            promotionTime = 10f;
        }
    }

    public void OnPromoteKnight() {
        Debug.Log("Attempting to promote to knight");
        if (energy >= 5f) {
            Debug.Log("Promote Knight");
            attack = 2;
            AddEnergy(-5);
            anim.SetTrigger("PromoteKnight");
            promotionTime = 10f;
        }
    }

    public void OnPromoteRook() {
        Debug.Log("Attempting to promote to rook");
        if (energy >= 10f) {
            Debug.Log("Promote Rook");
            attack = 3;
            AddEnergy(-10);
            anim.SetTrigger("PromoteRook");
            promotionTime = 10f;
        }
    }

    public void OnPromoteQueen() {
        Debug.Log("Attempting to promote to queen");
        if (energy >= 20f) {
            Debug.Log("Promote Queen");
            attack = 4;
            AddEnergy(-20);
            anim.SetTrigger("PromoteQueen");
            promotionTime = 10f;
        }
    }


    private void Update()
    {
        transform.Translate(movement * Time.deltaTime * speed);
        bulletShootDelay -= Time.deltaTime;
        dashDelay -= Time.deltaTime;

        promotionTime -= Time.deltaTime;
        if (attack != 0 && promotionTime <= 0f) {
            UnPromote();
        }

        if(shooting && bulletShootDelay < 0f) {
            Camera camera = Camera.main;
            Vector2 mousePos = camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 direction = (mousePos - (Vector2)transform.position).normalized;
            if (attack == 0) {
                //Pawn attack
                GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                bulletScript bulletS = newBullet.GetComponent<bulletScript>();
                bulletS.direction = direction;
                bulletS.bulletSpeed = 5f;
                bulletS.damage = 1f;
                bulletShootDelay = 0.5f;
            }
            else if (attack == 1) {
                //Bishop attack
                Vector2[] v = new Vector2[] {new(1, 1), new(1, -1), new(-1, 1), new(-1, -1)};
                foreach (Vector2 dir in v) {
                    GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                    bulletScript bulletS = newBullet.GetComponent<bulletScript>();
                    bulletS.direction = dir;
                    bulletS.bulletSpeed = 10f;
                    bulletS.damage = 1f;
                }
                bulletShootDelay = 0.3f;
            }
            else if (attack == 2) {
                //Kinght attack
                GameObject melee = Instantiate(knightMeelee, transform.position, Quaternion.Euler(0,0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg));
                bulletShootDelay = 1f;
            }
            else if (attack == 3) {
                //Rook attack
                GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                newBullet.transform.localScale = new Vector3(2f, 2f, 0f);
                bulletScript bulletS = newBullet.GetComponent<bulletScript>();
                bulletS.direction = direction;
                bulletS.bulletSpeed = 3f;
                bulletS.damage = 3f;
                bulletShootDelay = 1f;
            }
            else {
                Vector2[] v = new Vector2[] {new(1, 0), new(0,-1), new(-1, 0), new(0, 1), new(1, 1), new(1, -1), new(-1, 1), new(-1, -1)};
                foreach (Vector2 dir in v) {
                    Debug.Log(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
                    GameObject newLaser = Instantiate(laser, transform.position, Quaternion.Euler(0,0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg));
                    bulletShootDelay = 1f;
                }
            }
        }
    }
}
