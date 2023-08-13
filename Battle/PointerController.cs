using UnityEngine;

// **** Controls Player Ghost during EVADE

public class PointerController : MonoBehaviour
{
    public PlayerState player;
    public EvasionController evasionController;
    public BattleCamera battleCamera;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            player.MoveInEvasion(transform.position);
            evasionController.HideEvasionCircle();
            battleCamera.ReturnToMain();
            gameObject.SetActive(false);
        }
    }
}
