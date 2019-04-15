using UnityEngine;

public class IslandCollider : MonoBehaviour
{
    int _shootable;
    private void Awake()
    {
        _shootable = LayerMask.NameToLayer("Shootable");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("IslandTrigger");
        if (other.gameObject.layer == _shootable)
        {
            PlayerBluePrint playerScript = other.gameObject.GetComponentInParent<PlayerBluePrint>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(CharacterPrint.CRUSH);
                Debug.Log("Crush!");
            }
            else
            {
                Debug.Log("Script not found");
            }
        }
    }
}
