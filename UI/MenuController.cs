using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

//      +--------------+
//     /|             /|
//    *--+-----------* |
//    | | MENU       | |
//    | | CONTROLLER | |
//    | +------------|-+
//    |/             |/
//    *--------------*
//
// Class for all button-based, list-style Menus

public class MenuController : MonoBehaviour
{
    public List<Button> menuButtons;
    public RectTransform handCursor;
    public float cursorOffset;
    public float navigationSpeed;
    private int currentIndex;

    // Floating
    public float floatingAmplitude;

    // Cooldown
    public float inputCooldown = 0.2f;
    private float nextInputTime;

    // Pom Sound
    public AudioClip cursorMoveSound;
    private AudioSource audioSource;

    void Start()
    {
        currentIndex = 0;
        PositionHandCursor();
        SelectButton(menuButtons[currentIndex]);

        // Initialize the AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // NEW INPUT SYSTEM
    // void Update()
    // {
    //     // New Input System
    //     Gamepad gamepad = Gamepad.current;

    //     if (gamepad != null)
    //     {
    //         // Analog stick
    //         float verticalAxis = gamepad.leftStick.y.ReadValue();

    //         // D-Pad
    //         float dpadVerticalAxis = gamepad.dpad.y.ReadValue();

    //         if (verticalAxis > 0.5f || dpadVerticalAxis > 0.5f)
    //         {
    //             MoveCursor(-1);
    //             return;
    //         }
    //         else if (verticalAxis < -0.5f || dpadVerticalAxis < -0.5f)
    //         {
    //             MoveCursor(1);
    //             return;
    //         }

    //         if (gamepad.buttonSouth.wasPressedThisFrame)
    //         {
    //             menuButtons[currentIndex].onClick.Invoke();
    //         }
    //     }

    //     // Old Input System
    //     float verticalInput = Input.GetAxis("Vertical");

    //     if (verticalInput > 0.5f)
    //     {
    //         MoveCursor(-1);
    //     }
    //     else if (verticalInput < -0.5f)
    //     {
    //         MoveCursor(1);
    //     }

    //     // Keyboard
    //     if (Input.GetKeyDown(KeyCode.UpArrow))
    //     {
    //         MoveCursor(-1);
    //     }
    //     else if (Input.GetKeyDown(KeyCode.DownArrow))
    //     {
    //         MoveCursor(1);
    //     }

    //     if (Input.GetButtonDown("Submit"))
    //     {
    //         menuButtons[currentIndex].onClick.Invoke();
    //     }

    //     FloatHandCursor();
    // }

    void Update()
    {
        // If we're within the cooldown period, we don't check for any input.
        if (Time.time < nextInputTime)
            return;

        float verticalInput = Input.GetAxis("Vertical");

        if (verticalInput > 0.5f)
        {
            MoveCursor(-1);
            nextInputTime = Time.time + inputCooldown;
            return; // We exit early to avoid multiple input checks
        }
        else if (verticalInput < -0.5f)
        {
            MoveCursor(1);
            nextInputTime = Time.time + inputCooldown;
            return; // Same reasoning
        }

        // Keyboard
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveCursor(-1);
            nextInputTime = Time.time + inputCooldown;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveCursor(1);
            nextInputTime = Time.time + inputCooldown;
        }

        if (Input.GetButtonDown("Submit"))
        {
            menuButtons[currentIndex].onClick.Invoke();
        }

        FloatHandCursor();

        // New Input System
        Gamepad gamepad = Gamepad.current;

        if (gamepad != null)
        {
            // Analog stick
            float verticalAxis = gamepad.leftStick.y.ReadValue();

            // D-Pad
            float dpadVerticalAxis = gamepad.dpad.y.ReadValue();

            if (verticalAxis > 0.5f || dpadVerticalAxis > 0.5f)
            {
                MoveCursor(-1);
                return;
            }
            else if (verticalAxis < -0.5f || dpadVerticalAxis < -0.5f)
            {
                MoveCursor(1);
                return;
            }

            if (gamepad.buttonSouth.wasPressedThisFrame)
            {
                menuButtons[currentIndex].onClick.Invoke();
            }
        }
    }

    private void MoveCursor(int direction)
    {
        if (Time.time < nextInputTime)
            return;
        nextInputTime = Time.time + inputCooldown;

        currentIndex += direction;

        if (currentIndex < 0)
        {
            currentIndex = menuButtons.Count - 1;
        }
        else if (currentIndex >= menuButtons.Count)
        {
            currentIndex = 0;
        }

        PositionHandCursor();
        SelectButton(menuButtons[currentIndex]);
    }

    private void PositionHandCursor()
    {
        Vector2 targetPosition = menuButtons[currentIndex].GetComponent<RectTransform>().position;

        targetPosition.x -= cursorOffset;
        handCursor.position = targetPosition;
    }

    private void FloatHandCursor()
    {
        float x =
            handCursor.position.x
            + Mathf.PingPong(Time.time * navigationSpeed, floatingAmplitude)
            - (floatingAmplitude / 2);
        float y = handCursor.position.y;
        Vector2 floatingPosition = new Vector2(x, y);
        handCursor.position = Vector2.Lerp(
            handCursor.position,
            floatingPosition,
            Time.deltaTime * navigationSpeed
        );
    }

    private void SelectButton(Button button)
    {
        button.Select();
        PlayCursorMoveSound();
    }

    private void PlayCursorMoveSound()
    {
        if (cursorMoveSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(cursorMoveSound);
        }
    }

    public void OnButtonHover(Button hoveredButton)
    {
        int buttonIndex = menuButtons.IndexOf(hoveredButton);
        if (buttonIndex != -1)
        {
            currentIndex = buttonIndex;
            PositionHandCursor();
            // PlayCursorMoveSound();
        }
    }
}
