using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Meal : MonoBehaviour
{
    [SerializeField]private AudioClip eatVoice;

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            Character.Instance.IncreaseHealth();
            //other.GetComponent<AudioSource>().PlayOneShot(eatVoice);
            // Et objesini yok et
            Destroy(gameObject);
            
        }
    }
}
