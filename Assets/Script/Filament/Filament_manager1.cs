using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Filament_Manager1 : MonoBehaviour
{
    [SerializeField] GameObject Filament; // Filament ÇÁ¸®ÆÕ
    [SerializeField] Button FilamentButton1_1;

    void Start()
    {
        FilamentButton1_1.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        Quaternion rotation = Quaternion.Euler(0, 0, 90);
        GameObject FilamentSp = Instantiate(Filament, transform.position, rotation);
        Filament_increase1 filamentScript = FilamentSp.GetComponent<Filament_increase1>();

       if (filamentScript != null) { }
    }
}