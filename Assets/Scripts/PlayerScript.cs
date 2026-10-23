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
    public GameObject bullet;
    public float bulletShootDelay;
    private bool shooting;

    private void Awake()
    {
        movementNonZero = new(0, 1);
        dashDelay = 0f;
        bulletShootDelay = 0f;
        shooting = false;
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

    private void Update()
    {
        transform.Translate(movement * Time.deltaTime * speed);
        bulletShootDelay -= Time.deltaTime;
        dashDelay -= Time.deltaTime;

        if(shooting && bulletShootDelay < 0f) {
            Camera camera = Camera.main;
            Vector2 mousePos = camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 direction = (mousePos - (Vector2)transform.position).normalized;

            GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            newBullet.GetComponent<bulletScript>().direction = direction;
            bulletShootDelay = 0.5f;
        }
    }
}
