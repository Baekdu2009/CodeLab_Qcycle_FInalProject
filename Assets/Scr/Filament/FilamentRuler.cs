using UnityEngine;
using UnityEngine.UIElements;

public class FilamentRuler : MonoBehaviour
{
    [SerializeField] GameObject Ruler1;
    [SerializeField] GameObject Ruler2;
    [SerializeField] GameObject FMR1;  // FilamentMoveRuler1
    [SerializeField] GameObject FMR2;
    [SerializeField] GameObject FMR3;
    [SerializeField] GameObject FMR4;
    [SerializeField] GameObject FMR5;

    private float RotationSpeed = 100f;

      // Update is called once per frame
    void Update()
    {
        RotateRuler(Ruler1, RotationSpeed);
        RotateRuler(Ruler2, -RotationSpeed);
        RotateRuler(FMR1, RotationSpeed);
        RotateRuler(FMR2, RotationSpeed);
        RotateRuler(FMR3, RotationSpeed);
        RotateRuler(FMR4, RotationSpeed);
        RotateRuler(FMR5, RotationSpeed);
    }

    private void RotateRuler(GameObject ruler, float rotationSpeed)
    {
        if(ruler != null)
        {
            ruler.transform.Rotate(new Vector3(rotationSpeed, 0, 0) * Time.deltaTime, Space.World);
        }
    }


}
