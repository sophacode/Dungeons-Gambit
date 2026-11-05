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
    public BoxCollider2D collider;
    public float knockbackStrength;

    private int attack;
    public GameObject bullet;
    public float bulletShootDelay;
    public bool shooting;
    public GameObject knightMeelee;
    public GameObject laser;

    private float energy;
    public TextMeshProUGUI energyText;

    public float health;
    private float maxHealth;
    private bool canTakeDamage;

    private SpriteRenderer sr;
    public Sprite[] pieceSprites;
    public GameObject dashBlur;
    private SpriteRenderer dashBlurSr;

    private Animator anim;
    public UIswitching uilogic;

    private float promotionTime;
    public GameObject promotionBar;
    public GameObject promotionSprite;

    private TutorialManagerScript king;
    public bool canDash;
    private bool firstDashDone;
    public bool canShoot;
    public bool canPromote;
    public bool canPromoteRook;
    private bool firstPromotion;

    [SerializeField] GameObject pauseUIPanel;
    [SerializeField] GameObject healthBar;

    private void Awake()
    {
        movementNonZero = new(0, 1);
        dashDelay = 0f;

        knockbackStrength = 7.5f;

        bulletShootDelay = 0f;
        shooting = false;
        attack = 0;

        energy = 0f;

        maxHealth = 10f;
        health = maxHealth;
        canTakeDamage = false;

        promotionTime = 0f;

        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
        dashBlurSr = dashBlur.GetComponent<SpriteRenderer>();
        dashBlur.SetActive(false);

        king = GameObject.FindGameObjectWithTag("Tutorial").GetComponent<TutorialManagerScript>();
        canDash = false;
        firstDashDone = false;
        canShoot = false;
        canPromote = false;
        canPromoteRook = false;
        firstPromotion = false;
    }

    public void FromBoss() {
        transform.position = new Vector3(0, 160, 0);
        canDash = true;
        firstDashDone = true;
        canShoot = true;
        canPromote = true;
        canPromoteRook = true;
        firstPromotion = true;
        canTakeDamage = true;
        AddEnergy(20);

    }

    public void Speedrun() {
        canDash = true;
        firstDashDone = true;
        canShoot = true;
        canPromote = true;
        canPromoteRook = true;
        firstPromotion = true;
        canTakeDamage = true;
    }

    public void AddEnergy(float value) {
        energy += value;
        energyText.SetText("Energy: " + energy);
        uilogic.updatePieceFills(energy);
    }

    public void TakeDamage(float damage, Vector2 direction) {
        if (canTakeDamage) {
            health -= damage;
            AudioManager.Play(SoundEffect.PlayerHit);
            healthBar.transform.localScale = new Vector3(health/maxHealth, 1f, 1f);
            StartCoroutine(Flicker());
            StartCoroutine(Knockback(direction));
            if (health <= 0f) {
                health = 0f;
                AudioManager.Play(SoundEffect.PlayerDeath);
                Die();
            } else if (health == 3f) {
                AudioManager.Play(SoundEffect.PlayerLowHealth);
            }
        }
    }

    private IEnumerator Knockback(Vector2 dir) {
        for (int i = 0; i < 5; i++) {
            transform.Translate(dir * Time.deltaTime * knockbackStrength);
            yield return null;
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
        AudioManager.Play(SoundEffect.PlayerDeath);
        if (!GlobalInfoScript.speedrun && GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>().roomCounter == 6) {
            SceneManager.LoadScene(3);
        }
        else {
            SceneManager.LoadScene(2);
        }
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

    public void OnEnter() {
        king.HideKing();
    }

    public void OnFirstPromote() {
        if (canPromote && !firstPromotion) {
            firstPromotion = true;
            OnPlay();
            OnPromoteBishop();
            king.FirstPromotion();
            canPromote = false;
            king.hintText.SetActive(false);
        }
    }

    public void OnSecondPromote() {
        if (canPromote && firstPromotion) {
            OnPlay();
            OnPromoteKnight();
            king.waiting = false;
            king.hintText.SetActive(false);
        }
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
        if (dashDelay <= 0f && canDash) {  
            if (!firstDashDone) {
                firstDashDone = true;
                king.FirstDash();
            }
            StartCoroutine(Dash());
            dashDelay = 0.5f;
        }
    }

    private IEnumerator Dash() {
        canTakeDamage = false;
        ChangeEnemyColliders(false);
        for (int i = 0; i < 10; i++) {
            DashFX();
            transform.Translate(movementNonZero * Time.deltaTime * dashDistance/10);
            yield return null;
        }
        ChangeEnemyColliders(true);
        canTakeDamage = true;
    }

    private void ChangeEnemyColliders(bool b) {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) {
            enemy.GetComponent<BoxCollider2D>().enabled = b;
        }
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
        if (canShoot) {
            shooting = !shooting;
        }
    }

    private void UnPromote() {
        attack = 0;
        anim.SetBool("Walking", false);
        anim.SetTrigger("Unpromote");
        sr.sprite = pieceSprites[0];
        promotionSprite.GetComponent<Image>().sprite = pieceSprites[0];
        promotionBar.transform.localScale = new(1, 1, 1);
    }

    public void OnPromoteBishop() {
        if (energy >= 3f && canPromote) {
            attack = 1;
            AddEnergy(-3);
            anim.SetTrigger("PromoteBishop");
            promotionTime = 10f;
            promotionSprite.GetComponent<Image>().sprite = pieceSprites[1];
            AudioManager.Play(SoundEffect.PromotePiece);
        }
    }

    public void OnPromoteKnight() {
        if (energy >= 5f && canPromote) {
            attack = 2;
            AddEnergy(-5);
            anim.SetTrigger("PromoteKnight");
            promotionTime = 10f;
            promotionSprite.GetComponent<Image>().sprite = pieceSprites[2];
            AudioManager.Play(SoundEffect.PromotePiece);
        }
    }

    public void OnPromoteRook() {
        if (energy >= 10f && canPromote && canPromoteRook) {
            attack = 3;
            AddEnergy(-10);
            anim.SetTrigger("PromoteRook");
            promotionTime = 10f;
            promotionSprite.GetComponent<Image>().sprite = pieceSprites[3];
            AudioManager.Play(SoundEffect.PromotePiece);
        }
    }

    public void OnPromoteQueen() {
        if (energy >= 20f && canPromote) {
            attack = 4;
            AddEnergy(-20);
            anim.SetTrigger("PromoteQueen");
            promotionTime = 10f;
            promotionSprite.GetComponent<Image>().sprite = pieceSprites[4];
            AudioManager.Play(SoundEffect.PromotePiece);
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
        else if (promotionTime >= 0) {
            promotionBar.transform.localScale = new(promotionTime/10f, 1, 1);
        }

        if(shooting && bulletShootDelay < 0f) {
            Camera camera = Camera.main;
            Vector2 mousePos = camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 direction = (mousePos - (Vector2)transform.position).normalized;
            if (attack == 0) {
                //Pawn attack
                GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                AudioManager.Play(SoundEffect.BulletFire);
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
                    AudioManager.Play(SoundEffect.BulletFire);
                    bulletS.direction = dir;
                    bulletS.bulletSpeed = 10f;
                    bulletS.damage = 1f;
                }
                bulletShootDelay = 0.3f;
            }
            else if (attack == 2) {
                //Kinght attack
                GameObject melee = Instantiate(knightMeelee, transform.position, Quaternion.Euler(0,0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg));
                AudioManager.Play(SoundEffect.MeleeAttack);
                bulletShootDelay = 1f;
            }
            else if (attack == 3) {
                //Rook attack
                GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                AudioManager.Play(SoundEffect.BulletFire);
                newBullet.transform.localScale = new Vector3(2f, 2f, 0f);
                bulletScript bulletS = newBullet.GetComponent<bulletScript>();
                bulletS.direction = direction;
                bulletS.bulletSpeed = 3f;
                bulletS.damage = 3f;
                bulletShootDelay = 1f;
            }
            else {
                // queen attack
                AudioManager.Play(SoundEffect.QueenLaser);
                Vector2[] v = new Vector2[] {new(1, 0), new(0,-1), new(-1, 0), new(0, 1), new(1, 1), new(1, -1), new(-1, 1), new(-1, -1)};
                foreach (Vector2 dir in v) {
                    GameObject newLaser = Instantiate(laser, transform.position, Quaternion.Euler(0,0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg));
                    bulletShootDelay = 1f;
                }
            }
        }
    }
}
