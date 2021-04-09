using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmControl : MonoBehaviour
{
    public WheelControl wheelControl { get; set; }

    private LineRenderer lineRenderer;


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
                Quaternion.Euler(0f, +halfAngle, 0f) * (transform.position + new Vector3(0, 0.03f, 0)),           // 終了点
                new Vector3(0,0.05f,0),               // 開始点
                Quaternion.Euler(0f, -halfAngle, 0f) * (transform.position + new Vector3(0, 0.03f, 0)),           // 終了点
            };

            lineRenderer.positionCount = positions.Length;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = Color.white;
            lineRenderer.endColor = Color.white;
            lineRenderer.startWidth = 0.02f;                   // 開始点の太さを0.1にする
            lineRenderer.endWidth = 0.02f;                     // 終了点の太さを0.1にする
                                                               // 線を引く場所を指定する
            lineRenderer.SetPositions(positions);
        }

    }
}
