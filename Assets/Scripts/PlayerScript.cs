using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    private Vector2 movement;
    public int speed;
    public int dashDistance;

    public void OnMove(InputValue inputValue) {
        movement = inputValue.Get<Vector2>();
    }

    public void OnDash() {
        Debug.Log(movement);

        Vector3 movementV3 = new(movement.x, movement.y, 0);
        transform.Translate(movementV3 * Time.deltaTime * dashDistance);
    }

    private void Update()
    {
        Vector3 movementV3 = new(movement.x, movement.y, 0);
        transform.Translate(movementV3 * Time.deltaTime * speed);
    }
}
