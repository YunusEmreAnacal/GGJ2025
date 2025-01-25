using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(10)]

public class Bar_Manager : MonoBehaviour
{
    public Slider healthSlider;
    public Slider easeHealthSlider;


    public float lerpSpeed = 0.5f;

    //public Character Character; // Karakter referans�

    private void Start()
    {
        Debug.Log("bar health1:" + Character.Instance.Health);
        if (Character.Instance != null) 
        {
            Character.Instance.OnHealthChanged += UpdateHealthUI;
        }

        healthSlider.maxValue = Character.Instance.MaxHealth;//fonks
        easeHealthSlider.maxValue = Character.Instance.MaxHealth;

        Debug.Log("bar health2:" + Character.Instance.Health);
        UpdateHealthUI(Character.Instance.Health);


        Debug.Log("bar health3:" + Character.Instance.Health);
        Debug.Log("bar maxh:" + Character.Instance.MaxHealth);
        Debug.Log("heslid:" + healthSlider.maxValue);
        Debug.Log("easslid:" + easeHealthSlider.maxValue);

        healthSlider.interactable = false;//fonks
        easeHealthSlider.interactable = false;

    }

    private void OnDestroy()
    {
        if (Character.Instance != null)
        {
            Character.Instance.OnHealthChanged -= UpdateHealthUI;
        }
        
    }

    private void UpdateHealthUI(float currentHealth)
    {
        healthSlider.value = currentHealth;
    }


    private void Update()
    {
        // Ease bar, healthSlider'�n yeni de�erine yava��a yakla��r
        if (easeHealthSlider.value != healthSlider.value) // fonks
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, healthSlider.value, lerpSpeed );

        }
    }
}
