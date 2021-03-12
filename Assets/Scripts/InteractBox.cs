using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractBox : MonoBehaviour
{
    [SerializeField]
    float offset;
    [SerializeField]
    Vector3 size;
    [SerializeField]
    LayerMask whatCanWeInteractWith;

    public IInteractable Interactable { get; private set; }
    public float Offset => offset;
    public Vector3 Size => size;

    public void LookForInteractables()
    {       
        var targets = Physics.OverlapBox(transform.position+ transform.forward*offset, size/2f, transform.rotation, whatCanWeInteractWith);
        if (targets.Length > 0)
        {
            foreach (var target in targets)
            {
                var box = target.GetComponent<InteractBox>();
                if(box !=null && box==this)
                {
                    continue;
                }
                var inter = target.GetComponent<IInteractable>();
                if (inter != null)
                    Interactable = inter;
            }
        }
        else
        {
            Interactable = null;
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position + transform.forward*offset, size);
    }
}
