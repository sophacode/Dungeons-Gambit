using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    private Vector2 movement;
    private Vector2 movementNonZero;
    public int speed;
    public int dashDistance;
    public GameObject bullet;
    public float bulletShootDelay;

    private void Awake()
    {
        movementNonZero = new(0, 1);
        bulletShootDelay = 0f;
    }

    public void OnMove(InputValue inputValue) {
        movement = inputValue.Get<Vector2>();
        if (movement != Vector2.zero ) {
            movementNonZero = movement;
        }
    }

    public void OnDash() {
        transform.Translate(movementNonZero * Time.deltaTime * dashDistance);
    }

    public void OnAttack() {
        if(bulletShootDelay < 0f) {
            Camera camera = Camera.main;
            Vector2 mousePos = camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 direction = (mousePos - (Vector2)transform.position).normalized;

            GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            newBullet.GetComponent<bulletScript>().direction = direction;
            bulletShootDelay = 0.5f;
        }

    }

    private void Update()
    {
        transform.Translate(movement * Time.deltaTime * speed);
        bulletShootDelay -= Time.deltaTime;
    }
}
