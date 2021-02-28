using UnityEngine.UI;

public class BackButton : Button
{
    private static BackButton _instance;
    public static bool Pressed => _instance != null && _instance.IsPressed();

    protected override void OnEnable()
    {
        _instance = this;
        base.OnEnable();
    }
}
