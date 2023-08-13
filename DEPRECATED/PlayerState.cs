using UnityEngine;

// **** Player Class

public class PlayerState : CharacterState
{
    // Add player-specific properties and methods here.
    public int currentDet = 0;
    public int maxDet = 60;
    public int currentFey = 0;
    public int maxFey = 20;

    public VitBar detBar;
    public VitBar feyBar;

    protected override void Start()
    {
        base.Start();
        // Any additional player-specific initialization
    }

    public void Update()
    {
        base.Update();

        detBar.UpdateVitBar(currentDet, maxDet);
        feyBar.UpdateVitBar(currentFey, maxFey);
    }
}

