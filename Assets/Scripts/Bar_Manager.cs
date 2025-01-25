using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(10)]

public class Bar_Manager : MonoBehaviour
{
    public Slider healthSlider;
    public Slider easeHealthSlider;

    public Slider foodSlider;
    public Slider easefoodSlider;

    public Slider waterSlider;
    public Slider easeWaterSlider;

    public float lerpSpeed = 0.5f;

    //public Character Character; // Karakter referans�

    private void Start()
    {
        Debug.Log("bar health1:" + Character.Instance.Health);
        if (Character.Instance != null) 
        {
            Character.Instance.OnHealthChanged += UpdateHealthUI;
            Character.Instance.OnFoodChanged += UpdateFoodUI;
            Character.Instance.OnWaterChanged += UpdateWaterUI;
        }

        healthSlider.maxValue = Character.Instance.MaxHealth;//fonks
        easeHealthSlider.maxValue = Character.Instance.MaxHealth;

        foodSlider.maxValue = Character.Instance.MaxFood;
        easefoodSlider.maxValue = Character.Instance.MaxFood;

        waterSlider.maxValue = Character.Instance.MaxWater;
        easeWaterSlider.maxValue = Character.Instance.MaxWater;

        Debug.Log("bar health2:" + Character.Instance.Health);
        UpdateHealthUI(Character.Instance.Health);
        UpdateFoodUI(Character.Instance.Food);
        UpdateWaterUI(Character.Instance.Water);

        Debug.Log("bar health3:" + Character.Instance.Health);
        Debug.Log("bar maxh:" + Character.Instance.MaxHealth);
        Debug.Log("heslid:" + healthSlider.maxValue);
        Debug.Log("easslid:" + easeHealthSlider.maxValue);

        healthSlider.interactable = false;//fonks
        easeHealthSlider.interactable = false;
        foodSlider.interactable = false;
        easefoodSlider.interactable = false;
        waterSlider.interactable = false;
        easeWaterSlider.interactable = false;

    }

    private void OnDestroy()
    {
        if (Character.Instance != null)
        {
            Character.Instance.OnHealthChanged -= UpdateHealthUI;
            Character.Instance.OnFoodChanged -= UpdateFoodUI;
            Character.Instance.OnWaterChanged -= UpdateWaterUI;
        }
        
    }

    private void UpdateHealthUI(float currentHealth)
    {
        healthSlider.value = currentHealth;
    }

    private void UpdateFoodUI(float currentFood)
    {
        foodSlider.value = currentFood;
    }

    private void UpdateWaterUI(float currentWater)
    {
        waterSlider.value = currentWater;
    }

    private void Update()
    {
        // Ease bar, healthSlider'�n yeni de�erine yava��a yakla��r
        if (easeHealthSlider.value != healthSlider.value) // fonks
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, healthSlider.value, lerpSpeed );

        }
        // Ease bar, foodSlider'�n yeni de�erine yava��a yakla��r
        if (easefoodSlider.value != foodSlider.value)
        {
            easefoodSlider.value = Mathf.Lerp(easefoodSlider.value, foodSlider.value, lerpSpeed);
        }

        if (easeWaterSlider.value != waterSlider.value)
        {
            easeWaterSlider.value = Mathf.Lerp(easeWaterSlider.value, waterSlider.value, lerpSpeed);
        }
    }
}
