using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class ClintMove : MonoBehaviour
{
    public List<Transform> transformList = new List<Transform>();
    public int currentNum;

    public float speed = 0.2f;

    void Start()
    {
        TransformExtract();
        NumberExtract();
    }

    private void TransformExtract()
    {
        GameObject[] allClints = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach (GameObject clint in allClints)
        {
            if (clint.name.Contains("CLINT"))
            {
                string[] splitParts = clint.name.Split('_');
                if (splitParts.Length > 1 && int.TryParse(splitParts[1], out int number))
                {
                    transformList.Add(clint.transform);
                }
            }
        }
        transformList.Sort((a, b) => int.Parse(a.name.Split('_')[1]).CompareTo(int.Parse(b.name.Split('_')[1])));
    }

    private void NumberExtract()
    {
        string[] splitParts = name.Split('_');
        currentNum = int.Parse(splitParts[1]);

    }


    private void Update()
    {
        Move();
    }

    private void Move()
    {
        
    }
}
