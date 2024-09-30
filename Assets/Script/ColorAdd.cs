using UnityEngine;

public class ColorAdd : MonoBehaviour
{
    public GameObject nozzle;
    public GameObject rod;
    public GameObject plate;

    public void SetColor()
    {
        // �ε� ���� ����
        Renderer[] rodRenderers = rod.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in rodRenderers)
        {
            Material newMaterial = new Material(renderer.sharedMaterial);
            newMaterial.color = Color.green; // ���ϴ� �������� ����
            renderer.material = newMaterial; // ���ο� Material �Ҵ�
        }

        // ���� ���� ����
        Renderer[] nozzleRenderers = nozzle.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in nozzleRenderers)
        {
            // sharedMaterial�� ����Ͽ� ���� Material�� ����
            Material newMaterial = new Material(renderer.sharedMaterial);
            newMaterial.color = Color.red; // ���ϴ� �������� ����
            renderer.material = newMaterial; // ���ο� Material �Ҵ�
        }

        // �÷���Ʈ ���� ����
        Renderer[] plateRenderers = plate.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in plateRenderers)
        {
            Material newMaterial = new Material(renderer.sharedMaterial);
            newMaterial.color = Color.blue; // ���ϴ� �������� ����
            renderer.material = newMaterial; // ���ο� Material �Ҵ�
        }
    }

}
