using Game;
using UnityEngine;

public class Apple : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;
        
        var effect = ObjectPool.instance.GetPooledObject();

        if(effect!= null)
        {
            effect.transform.position = transform.position;
            effect.transform.rotation = effect.transform.rotation;
            effect.SetActive(true);
        }
            
        FindObjectOfType<AudioManager>().PlaySound("PickUp");
        FindObjectOfType<PlayerManager>().ApplePickup(other.name);
        gameObject.SetActive(false);
    }
}
