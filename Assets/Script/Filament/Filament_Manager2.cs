using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Filament_Manager2 : MonoBehaviour
{
    [SerializeField] Button FilamentButton1_2;
    [SerializeField] GameObject Filament; // Filament ÇÁ¸®ÆÕ

    private void Start()
    {
        FilamentButton1_2.onClick.AddListener(OnButtonClick);
    }
    void OnButtonClick()
    {
        Quaternion rotation = Quaternion.Euler(0, 90, 90);
        GameObject FilamentSp = Instantiate(Filament, transform.position, rotation);
        Filament_increase2 filamentScript = FilamentSp.GetComponent<Filament_increase2>();
       if (filamentScript != null) { }
    }
}
