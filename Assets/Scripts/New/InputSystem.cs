using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystem: MonoBehaviour
{
    public float moveSpeed = 5f; // Скорость движения персонажа
    public float jumpForce = 7f; // Сила прыжка

    private Rigidbody rb;

    // Векторы для перемещения
    private Vector2 moveInput = Vector2.zero;
    private bool jumpPressed = false;

    void Start()
    {
        // Получаем компонент Rigidbody, чтобы управлять физикой объекта
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Получение ввода перемещения в горизонтальной и вертикальной плоскости
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
        {
            moveInput.y = 1f;
        }
        else if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
        {
            moveInput.y = -1f;
        }
        else
        {
            moveInput.y = 0f;
        }

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
        {
            moveInput.x = -1f;
        }
        else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            moveInput.x = 1f;
        }
        else
        {
            moveInput.x = 0f;
        }

        // Проверка нажатия клавиши прыжка
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            jumpPressed = true;
        }
    }

    void FixedUpdate()
    {
        // Выполнение перемещения с помощью Rigidbody
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

        // Выполнение прыжка, если кнопка была нажата
        if (jumpPressed)
        {
            if (Mathf.Abs(rb.velocity.y) < 0.01f) // Проверка, что персонаж на земле
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
            jumpPressed = false;
        }
    }
}
