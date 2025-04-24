using UnityEngine;

public class Ladder : Interactable
{
    public Transform tpPos, boatPos;

    void Update()
    {
        transform.position = boatPos.position;
        transform.rotation = boatPos.rotation;
    }

    public override void Interact()
    {
        GameObject.Find("Player").GetComponent<PlayerManager>().DoTeleport(tpPos.position);
    }
}
