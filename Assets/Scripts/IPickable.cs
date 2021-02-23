using UnityEngine;

public interface IPickable:IInteractable
{
    bool Heavy { get; }
    Transform transform { get; }
    void Drop();
}