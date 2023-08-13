using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public Texture2D defaultCursorTexture;
    public Texture2D clickCursorTexture;
    public Texture2D overEnemyCursorTexture;
    public Vector2 hotSpot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

    void Start()
    {
        SetCustomCursor(defaultCursorTexture);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetCustomCursor(clickCursorTexture);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            SetCustomCursor(defaultCursorTexture);
        }
    }

    public void SetCustomCursor(Texture2D cursorTexture)
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    public void SetCursorOverEnemy()
    {
        SetCustomCursor(overEnemyCursorTexture);
    }
}
