using UnityEngine;

public interface IPickUp
{
    public void PickUp();

    public void DropPickUp();
    public void AttachTo(Transform parent);

    public void Detach(Vector3 dropPosition);

    public Vector3 GetHalfExtends();
}
