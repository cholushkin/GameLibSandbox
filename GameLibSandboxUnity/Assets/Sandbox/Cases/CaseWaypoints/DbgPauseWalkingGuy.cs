using GameLib;

public class DbgPauseWalkingGuy : Pane
{
    public WayPointMover Mover;

    public override void InitializeState()
    {
        base.InitializeState();
        SetText($"{Mover.name}\nplay/pause");
    }

    public override void OnClick()
    { 
        base.OnClick();
        Mover.Pause(WayPointMover.PauseMode.OnWaypoint);
    }
}
