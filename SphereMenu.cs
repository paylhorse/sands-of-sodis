using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class SphereMenu : MonoBehaviour
{
    // Battle Manager
    public BattleManager battleManager;

    // Target Select
    public TargetSelectController targetSelect;

    // UI Manager
    public UIManager UI;

    public GameObject childSphere;
    public float rotationAngle = 91.5f;
    public float flipAngle = 60.0f;
    public float rotationDuration = 0.5f;
    public KeyCode leftKey = KeyCode.LeftArrow;
    public KeyCode rightKey = KeyCode.RightArrow;
    public KeyCode upKey = KeyCode.UpArrow;
    public KeyCode downKey = KeyCode.DownArrow;
    public KeyCode confirmKey = KeyCode.Return;

    private bool isRotating;
    private bool rotationEnabled = true;
    private int gridX = 1;
    private int gridY = 0;

    // Label
    [SerializeField] private string[,] menuTexts = new string[4, 2]
    {
        { "INHERENT SK.", "ITEMS." },
        { "COMBO.", "EQUIPMENT SK." },
        { "GUARD.", "SPELLS." },
        { "EVADE.", "SWITCH." } // New column
    };

    // Directional Hands
    public Transform upHand;
    public Transform downHand;
    public Transform leftHand;
    public Transform rightHand;
    public float handBounceDistance = 0.2f;
    public float handBounceSpeed = 3.0f;

    private Vector3 upHandInitialPos;
    private Vector3 downHandInitialPos;
    private Vector3 leftHandInitialPos;
    private Vector3 rightHandInitialPos;

    // Canvas Element
    public GameObject sphereMenuHolder;

    [Header("Party Member Info")]

    public CharacterState character;

    // Evasion
    public PointerController pointerController;
    public EvasionController evasionController;

    // Floating Label
    [SerializeField] private float floatAmplitude = 5f;
    [SerializeField] private float floatFrequency = 1f;
    private Vector3 initialTextPosition;

    [SerializeField] private TextMeshProUGUI menuText;

    // Commands
    public delegate void SphereCommand();

    private SphereCommand[] commands = new SphereCommand[8];

    // Processing
    private bool isActive;

    // Add a reference to the BattleCamera script
    private BattleCamera battleCamera;

    // Preset Grid Menu
    private Vector3[,] childSphereRotations = new Vector3[4, 2]
    {
        {new Vector3(0, -118.346f, 0), new Vector3(0, -118.346f, 0)},
        {new Vector3(0, -26.846f, 0), new Vector3(0, -26.846f, 0)},
        {new Vector3(0, 64.654f, 0), new Vector3(0, 64.654f, 0)},
        {new Vector3(0, 156.154f, 0), new Vector3(0, 156.154f, 0)}
    };

    private Vector3[,] objectRotations = new Vector3[4, 2]
    {
        {new Vector3(0, 0, 0), new Vector3(42.6f, 0, 0)},
        {new Vector3(0, 0, 0), new Vector3(42.6f, 0, 0)},
        {new Vector3(0, 0, 0), new Vector3(42.6f, 0, 0)},
        {new Vector3(0, 0, 0), new Vector3(42.6f, 0, 0)}
    };

    // Skill Menus
    [SerializeField] private GameObject inherentSkillMenu;
    [SerializeField] private GameObject spellBookMenu;
    [SerializeField] private GameObject equipSkillMenu;
    [SerializeField] private GameObject itemMenu;

    // Audio
    public SoundManager UISoundManager;

    private string sphereMove = "SphereMove";
    private string sphereConfirm = "Pon";

    public void ShowInherentSkillMenu()
    {
        UI.ActivateFirstLayer();
        inherentSkillMenu.SetActive(true);
    }

    public void ShowSpellBookMenu()
    {
        UI.ActivateFirstLayer();
        spellBookMenu.SetActive(true);
    }

    public void ShowEquipSkillMenu()
    {
        UI.ActivateFirstLayer();
        equipSkillMenu.SetActive(true);
    }

    public void ShowItemMenu()
    {
        UI.ActivateFirstLayer();
        itemMenu.SetActive(true);
    }

    public void HideAllMenus()
    {
        UI.DeactivateFirstLayer();
        inherentSkillMenu.SetActive(false);
        spellBookMenu.SetActive(false);
        equipSkillMenu.SetActive(false);
        itemMenu.SetActive(false);
        SetRotationEnabled(true);
    }


    public void AssignCommandToGridBox(int gridBoxIndex, SphereCommand command)
    {
        if (gridBoxIndex >= 0 && gridBoxIndex < commands.Length)
        {
            commands[gridBoxIndex] = command;
        }
    }

    private void Start()
    {
        initialTextPosition = menuText.transform.localPosition;

        // Set the SphereMenu as inactive by default.
        isActive = false;
        SetRotationEnabled(false);

        upHandInitialPos = upHand.position;
        downHandInitialPos = downHand.position;
        leftHandInitialPos = leftHand.position;
        rightHandInitialPos = rightHand.position;

        battleCamera = FindObjectOfType<BattleCamera>();
    }

    void AnimateHands()
    {
        float bounce = Mathf.PingPong(Time.time * handBounceSpeed, handBounceDistance);

        if (gridY > 0)
        {
            upHand.position = upHandInitialPos + Vector3.up * bounce;
            upHand.gameObject.SetActive(true);
        }
        else
        {
            upHand.gameObject.SetActive(false);
        }

        if (gridY < 1)
        {
            downHand.position = downHandInitialPos + Vector3.down * bounce;
            downHand.gameObject.SetActive(true);
        }
        else
        {
            downHand.gameObject.SetActive(false);
        }

        if (gridX > 0)
        {
            leftHand.position = leftHandInitialPos + Vector3.left * bounce;
            leftHand.gameObject.SetActive(true);
        }
        else
        {
            leftHand.gameObject.SetActive(false);
        }

        if (gridX < 2)
        {
            rightHand.position = rightHandInitialPos + Vector3.right * bounce;
            rightHand.gameObject.SetActive(true);
        }
        else
        {
            rightHand.gameObject.SetActive(false);
        }
    }


    public void Activate()
    {
        sphereMenuHolder.SetActive(true);
        isActive = true;
        SetRotationEnabled(true);

        battleCamera.MoveToSelectCam();

    }

    public void Deactivate()
    {
        battleCamera.ReturnToMain();
        sphereMenuHolder.SetActive(false);
        SetRotationEnabled(false);
    }

    // Check if the SphereMenu is active.
    public bool IsActive()
    {
        return isActive;
    }

    private void Update()
    {
        if (!isRotating && rotationEnabled)
        {
            if (Input.GetKeyDown(leftKey) && gridX > 0)
            {
                MoveToButton(gridX - 1, gridY);
                UISoundManager.PlaySound(sphereMove);
            }
            else if (Input.GetKeyDown(rightKey) && gridX < 3)
            {
                MoveToButton(gridX + 1, gridY);
                UISoundManager.PlaySound(sphereMove);
            }
            else if (Input.GetKeyDown(upKey) && gridY > 0)
            {
                MoveToButton(gridX, gridY - 1);
                UISoundManager.PlaySound(sphereMove);
            }
            else if (Input.GetKeyDown(downKey) && gridY < 1)
            {
                MoveToButton(gridX, gridY + 1);
                UISoundManager.PlaySound(sphereMove);
            }

            UpdateMenuText();
            FloatText();

            if (Input.GetKeyDown(confirmKey))
            {
                ExecuteCommandForCurrentGridBox();
                UISoundManager.PlaySound(sphereConfirm);
            }
        }

        AnimateHands();
    }

    private IEnumerator RotateObject(GameObject targetObject, Vector3 targetRotation)
    {
        isRotating = true;
        float elapsedTime = 0;
        Quaternion startLocalRotation = targetObject.transform.localRotation;
        Quaternion targetLocalRotation = Quaternion.Euler(targetRotation);

        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp(elapsedTime / rotationDuration, 0, 1);
            targetObject.transform.localRotation = Quaternion.Slerp(startLocalRotation, targetLocalRotation, t);
            yield return null;
        }

        targetObject.transform.localRotation = targetLocalRotation;
        isRotating = false;
    }



    public void ResetSphereRotation()
    {
        childSphere.transform.localRotation = Quaternion.identity;
        gameObject.transform.rotation = Quaternion.identity;
        gridX = 1;
        gridY = 0;
    }

    public void SetRotationEnabled(bool enabled)
    {
        rotationEnabled = enabled;
    }

    private void UpdateMenuText()
    {
        menuText.text = menuTexts[gridX, gridY];
    }

    private void FloatText()
    {
        float newY = initialTextPosition.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        menuText.transform.localPosition = new Vector3(initialTextPosition.x, newY, initialTextPosition.z);
    }

    private void ExecuteCommandForCurrentGridBox()
    {
        int currentGridBoxIndex = gridY * 3 + gridX;
        if (commands[currentGridBoxIndex] != null)
        {
            commands[currentGridBoxIndex]();
        }
        if (menuTexts[gridX, gridY] == "COMBO.")
        {
            Debug.Log("COMBO Selected!");
            //battleCamera.MoveToTargetCam();
            character.selectedCommand = "COMBO";
            targetSelect.enabled = true;
            Deactivate();
        }
        if (menuTexts[gridX, gridY] == "INHERENT SK.")
        {
            ShowInherentSkillMenu();
            SetRotationEnabled(false);
        }
        if (menuTexts[gridX, gridY] == "SPELLS.")
        {
            ShowSpellBookMenu();
            SetRotationEnabled(false);
        }
        if (menuTexts[gridX, gridY] == "EQUIPMENT SK.")
        {
            ShowEquipSkillMenu();
            SetRotationEnabled(false);
        }
        if (menuTexts[gridX, gridY] == "ITEMS.")
        {
            ShowItemMenu();
            SetRotationEnabled(false);
        }
        if (menuTexts[gridX, gridY] == "EVADE.")
        {
            // Enter Evasion
            evasionController.ShowEvasionCircle();
            pointerController.gameObject.SetActive(true);
            battleCamera.MoveToEvadeCam();
            Deactivate();
        }
    }

    public void MoveToButton(int gridX, int gridY)
    {
        // Check if indices are within bounds
        if (gridX >= 0 && gridX < 4 && gridY >= 0 && gridY < 2)
        {
            StartCoroutine(RotateObject(childSphere, childSphereRotations[gridX, gridY]));
            StartCoroutine(RotateObject(gameObject, objectRotations[gridX, gridY]));

            // Update the grid indices
            this.gridX = gridX;
            this.gridY = gridY;

            // Update the menu text
            UpdateMenuText();
        }
    }
}









