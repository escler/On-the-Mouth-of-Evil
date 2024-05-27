using UnityEngine;

public class BoxDamageCollider : MonoBehaviour
{
    public int damage;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            other.GetComponent<PlayerLifeHandler>().OnTakeDamage(damage);
        }
    }
}
