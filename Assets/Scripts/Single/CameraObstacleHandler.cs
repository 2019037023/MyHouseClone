using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraObstacleHandler : MonoBehaviour
{
    public Transform target; // ĳ����
    public LayerMask obstacleLayer; // ��ֹ� ���̾�
    private List<Renderer> currentObstacles = new List<Renderer>();

    void Update()
    {
        HandleObstacles();
    }

    void HandleObstacles()
    {
        // ������ ����ȭ�� ��ֹ� ����
        foreach (Renderer renderer in currentObstacles)
        {
            SetObstacleTransparency(renderer, 1f);
        }
        

        // ī�޶�� ĳ���� ���� ����ĳ��Ʈ
        Vector3 direction = target.position - transform.position;
        Ray ray = new Ray(transform.position, direction);
        RaycastHit[] hits = Physics.RaycastAll(ray, direction.magnitude, obstacleLayer);

        // ������ ��ֹ� ����ȭ
        foreach (RaycastHit hit in hits)
        {
            Renderer renderer = hit.collider.GetComponent<Renderer>();
            if (renderer != null)
            {
                SetObstacleTransparency(renderer, 0.2f); // ���� ����
                currentObstacles.Add(renderer);
            }
        }
    }

    void SetObstacleTransparency(Renderer renderer, float alpha)
    {
        Material[] materials = renderer.materials;
        foreach (Material material in materials)
        {
            if (alpha < 1f)
            {
                material.SetFloat("_Mode", 2); // Transparent ���
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
            }
            else
            {
                material.SetFloat("_Mode", 0); // Opaque ���
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;
            }

            Color color = material.color;
            color.a = alpha;
            material.color = color;
        }
    }
}
