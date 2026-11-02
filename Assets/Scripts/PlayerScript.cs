using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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

    private float energy;
    public TextMeshProUGUI energyText;

    public float health;
    private float maxHealth;

    private SpriteRenderer sr;
    public Sprite[] pieceSprites;
    public GameObject[] pieceUIBacks;

    private Animator anim;

    private float promotionTime;

    private void Awake()
    {
        movementNonZero = new(0, 1);
        dashDelay = 0f;

        bulletShootDelay = 0f;
        shooting = false;
        attack = 0;

        energy = 0f;

        maxHealth = 10f;
        health = maxHealth;

        promotionTime = 0f;

        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void SetPieceBack(int i) {
        foreach (GameObject g in pieceUIBacks) {
            g.SetActive(false);
        }
        pieceUIBacks[i].SetActive(true);
    }

    public void AddEnergy(float value) {
        energy += value;
        //energyText.text = "Energy: " + (int)energy;
        energyText.SetText("Energy: " + energy);
    }

    public void TakeDamage(float damage) {
        health -= damage;
        if (health <= 0f) {
            health = 0f;
            Die();
        }
        //Healthbar update
    }

    private void Die() {

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
            transform.Translate(movementNonZero * Time.deltaTime * dashDistance);
            dashDelay = 0.25f;
        }
    }

    public void OnAttack() {
        shooting = !shooting;
    }

    private void UnPromote() {
        Debug.Log("Unpromote");
        attack = 0;
        anim.SetBool("Walking", false);
        anim.SetTrigger("UnPromote");
        sr.sprite = pieceSprites[0];
    }

    public void OnPromoteBishop() {
        Debug.Log("Attempting to promote to bishop");
        if (energy >= 3f) {
            Debug.Log("Promote");
            attack = 1;
            energy -= 3f;
            energyText.SetText("Energy: " + energy);
            anim.SetBool("Walking", false);
            anim.SetTrigger("PromoteBishop");
            sr.sprite = pieceSprites[1];
            promotionTime = 10f;
        }
    }

    public void OnPromoteKnight() {
        Debug.Log("Attempting to promote to knight");
        if (energy >= 5f) {
            Debug.Log("Promote Knight");
            attack = 2;
            energy -= 5f;
            energyText.SetText("Energy: " + energy);
            anim.SetBool("Walking", false);
            anim.SetTrigger("PromoteKnight");
            sr.sprite = pieceSprites[2];
            promotionTime = 10f;
        }
    }

    public void OnPromoteRook() {
        Debug.Log("Attempting to promote to rook");
        if (energy >= 10f) {
            Debug.Log("Promote Rook");
            attack = 3;
            energy -= 10f;
            energyText.SetText("Energy: " + energy);
            anim.SetBool("Walking", false);
            anim.SetTrigger("PromoteRook");
            sr.sprite = pieceSprites[3];
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
                Vector2[] v = new Vector2[] {new(0.5f, 0.5f), new(0.5f, -0.5f), new(-0.5f, 0.5f), new(-0.5f, -0.5f)};
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
                GameObject melee = Instantiate(knightMeelee, transform.position, Quaternion.Euler(0,0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f));
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
        }
    }
}
