using UnityEngine;

public class ColorAdd : MonoBehaviour
{
    public GameObject nozzle;
    public GameObject rod;
    public GameObject plate;

    public void SetColor()
    {
        // 로드 색상 설정
        Renderer[] rodRenderers = rod.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in rodRenderers)
        {
            Material newMaterial = new Material(renderer.sharedMaterial);
            newMaterial.color = Color.green; // 원하는 색상으로 변경
            renderer.material = newMaterial; // 새로운 Material 할당
        }

        // 노즐 색상 설정
        Renderer[] nozzleRenderers = nozzle.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in nozzleRenderers)
        {
            // sharedMaterial을 사용하여 기존 Material을 수정
            Material newMaterial = new Material(renderer.sharedMaterial);
            newMaterial.color = Color.red; // 원하는 색상으로 변경
            renderer.material = newMaterial; // 새로운 Material 할당
        }

        // 플레이트 색상 설정
        Renderer[] plateRenderers = plate.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in plateRenderers)
        {
            Material newMaterial = new Material(renderer.sharedMaterial);
            newMaterial.color = Color.blue; // 원하는 색상으로 변경
            renderer.material = newMaterial; // 새로운 Material 할당
        }
    }

}
