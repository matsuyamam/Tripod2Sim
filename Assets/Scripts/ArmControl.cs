using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmControl : MonoBehaviour
{
    public WheelControl wheelControl { get; set; }

    private LineRenderer lineRenderer;
    private Material lineMaterial;

    private void Awake()
    {
        lineMaterial = new Material(Shader.Find("Sprites/Default"));
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        DrawGizmos();
    }

    public void SupportDisplay(bool enabled)
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        if (!lineRenderer)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        lineRenderer.enabled = enabled;
    }

    private void DrawGizmos()
    {
        if (lineRenderer)
        {

            float halfAngle = wheelControl.gameControl.HammerControl.ImpactWidth / 2f;
            var positions = new Vector3[]
            {
                Quaternion.Euler(0f, +halfAngle, 0f) * (transform.position + new Vector3(0, 0.03f, 0)),           // �I���_
                new Vector3(0,0.05f,0),               // �J�n�_
                Quaternion.Euler(0f, -halfAngle, 0f) * (transform.position + new Vector3(0, 0.03f, 0)),           // �I���_
            };

            lineRenderer.positionCount = positions.Length;
            lineRenderer.material = lineMaterial;
            lineRenderer.startColor = Color.white;
            lineRenderer.endColor = Color.white;
            lineRenderer.startWidth = 0.02f;                   // �J�n�_�̑�����0.1�ɂ���
            lineRenderer.endWidth = 0.02f;                     // �I���_�̑�����0.1�ɂ���
                                                               // ���������ꏊ���w�肷��
            lineRenderer.SetPositions(positions);
        }

    }
}
