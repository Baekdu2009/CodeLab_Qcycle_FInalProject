using UnityEngine;

public class ObjectCreate : MonoBehaviour
{
    public GameObject cubeObjPrefab;
    public GameObject cylinderObjPrefab;
    public GameObject filament;
    public Transform createPos;

    private GameObject cubeObj;
    private GameObject cylinderObj;

    private PrinterGcode printerGcode;

    float resolution;
    float scaleFactor;

    private void Start()
    {
        
    }

    private void Update()
    {
        resolution = printerGcode.printingResolution;
    }

    public void OnBtnCube()
    {

    }

    public void OnBtnCylinder()
    {

    }
    
    void PositionLocate()
    {
        Vector3 pos = new Vector3(printerGcode.rod.position.x, printerGcode.rod.position.y - 0.01f, printerGcode.rod.position.z);
        createPos.position = pos;
        cubeObj.transform.localScale = new Vector3(0, 0, 0); // 초기 스케일을 0으로 설정
        MeshRenderer cubeMesh = cubeObj.GetComponent<MeshRenderer>();
        MeshRenderer filamentMesh = filament.GetComponent<MeshRenderer>();
        cubeMesh.material.color = filamentMesh.material.color;
    }

    void CubeObjectCreate()
    {

    }
}
