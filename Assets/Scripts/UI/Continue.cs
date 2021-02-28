using UnityEngine.UI;

public class Continue : Button
{
    private static Continue _instance;
    public static bool Pressed => _instance != null && _instance.IsPressed();

    protected override void OnEnable()
    {
        _instance = this;
        base.OnEnable();
    }
}
