using UnityEngine;


public class Filament_Manager1 : MonoBehaviour
{
    [SerializeField] GameObject Filament; // Filament ÇÁ¸®ÆÕ


    void Start()
{
        Quaternion rotation = Quaternion.Euler(0, 0, 90);
        GameObject FilamentSp = Instantiate(Filament, transform.position, rotation);


        Filament_increase1 filamentScript = FilamentSp.GetComponent<Filament_increase1>();
      if(filamentScript != null) { }
}
}