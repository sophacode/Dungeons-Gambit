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

    private float energy;
    public TextMeshProUGUI energyText;

    public float health;
    private float maxHealth;

    private SpriteRenderer sr;
    public Sprite[] pieceSprites;
    public GameObject[] pieceUIBacks;

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

        sr = GetComponent<SpriteRenderer>();
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

    public void OnPromoteBishop() {
        Debug.Log("Attempting to promote to bishop");
        if (energy >= 3f) {
            Debug.Log("Promote");
            attack = 1;
            energy -= 3f;
            energyText.SetText("Energy: " + energy);
            sr.sprite = pieceSprites[1];
            SetPieceBack(1);
        }
    }

    private void Update()
    {
        transform.Translate(movement * Time.deltaTime * speed);
        bulletShootDelay -= Time.deltaTime;
        dashDelay -= Time.deltaTime;

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
                }
                bulletShootDelay = 0.3f;
            }
        }
    }
}
