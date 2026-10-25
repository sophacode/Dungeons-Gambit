using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private void Awake()
    {
        movementNonZero = new(0, 1);
        dashDelay = 0f;
        bulletShootDelay = 0f;
        shooting = false;
        energy = 0f;
        attack = 0;
    }

    public void AddEnergy(float value) {
        energy += value;
        Debug.Log(energy);
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
