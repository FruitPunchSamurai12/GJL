using UnityEngine;
using UnityEngine.UI;

public interface IPickable:IInteractable
{
    Sprite Icon { get; }
    bool Heavy { get; }
    Transform transform { get; }
    void Drop();
}