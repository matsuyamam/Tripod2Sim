using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelControl : MonoBehaviour
{
    [SerializeField]
    private GameObject Arm;
    [SerializeField]
    private GameObject Box;

    private GameObject[] boxs;
    private GameObject[] arms;

    private float wheelRotation;
    private float wheelSpeedCurrent;
    private float wheelSpeedTarget;
    private float wheelGoAroundTime;
    private float wheelAccelerationTime;
    private float wheelAcceleration;

    public float WheelGoAroundTime
    {
        get { return wheelGoAroundTime; }
        set
        {
            wheelGoAroundTime = value;
            ApplySpeedAndAcceleration();
        }
    }
    public float WheelAccelerationTime
    {
        get { return wheelAccelerationTime; }
        set { wheelAccelerationTime = value; }
    }

    public GameControl gameControl { get; set; }

    private LineRenderer lineRenderer;

    private const int NumArms = 15;

    private void Awake()
    {
        boxs = new GameObject[NumArms];
        for (int i = 0; i < NumArms; i++)
        {
            float radius = 0.33f;
            float rotation = 2.0f * Mathf.PI * i / NumArms;
            float x = radius * Mathf.Sin(rotation);
            float z = radius * Mathf.Cos(rotation);

            boxs[i] = Instantiate(Box, new Vector3(x, 0.0f, -z), Quaternion.Euler(new Vector3(0.0f, Mathf.Rad2Deg * -rotation, 0.0f)));
            boxs[i].transform.SetParent(gameObject.transform);
        }

        arms = new GameObject[NumArms];
        for (int i = 0; i < NumArms; i++)
        {
            arms[i] = Instantiate(Arm);
            arms[i].GetComponent<ArmControl>().wheelControl = this;
        }

        wheelGoAroundTime = 40f;
        WheelAccelerationTime = 1f;
    }


    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        wheelSpeedCurrent += wheelAcceleration * Time.deltaTime;
        if (wheelAcceleration < 0f)
        {
            if (wheelSpeedCurrent < wheelSpeedTarget)
            {
                wheelSpeedCurrent = wheelSpeedTarget;
            }
        }
        if (wheelAcceleration > 0f)
        {
            if (wheelSpeedCurrent > wheelSpeedTarget)
            {
                wheelSpeedCurrent = wheelSpeedTarget;
            }
        }

        wheelRotation += wheelSpeedCurrent * Time.deltaTime;
        gameObject.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(0, -wheelRotation, 0));

        for (int i = 0; i < NumArms; i++)
        {
            Rigidbody[] bodys = arms[i].GetComponentsInChildren<Rigidbody>();
            if (bodys[0].isKinematic)
            {
                float radius = 0.33f;
                float rotation = Mathf.Deg2Rad * wheelRotation + 2.0f * Mathf.PI * i / 15.0f;
                float x = radius * Mathf.Sin(rotation);
                float z = radius * Mathf.Cos(rotation);
                bodys[0].MovePosition(new Vector3(x, 0.0f, -z));
                bodys[0].MoveRotation(Quaternion.Euler(0.0f, Mathf.Rad2Deg * -rotation, 0.0f));
            }
        }

        DrawGizmos();
    }

    void ApplySpeedAndAcceleration()
    {
        wheelSpeedTarget = 360f / wheelGoAroundTime;
        wheelAcceleration = (wheelSpeedTarget - wheelSpeedCurrent) / wheelAccelerationTime;
    }

    public void Reset()
    {
        wheelRotation = 0.0f;
        wheelSpeedCurrent = 0.0f;
        ApplySpeedAndAcceleration();

        for (int i = 0; i < NumArms; i++)
        {
            float radius = 0.33f;
            float rotation = 2.0f * Mathf.PI * i / NumArms;
            float x = radius * Mathf.Sin(rotation);
            float z = radius * Mathf.Cos(rotation);

            arms[i].transform.localPosition = new Vector3(x, 0.0f, -z);
            arms[i].transform.localRotation = Quaternion.Euler(0.0f, Mathf.Rad2Deg * -rotation, 0.0f);

            arms[i].GetComponentsInChildren<Rigidbody>()[0].isKinematic = true;

            HingeJoint[] joints = arms[i].GetComponentsInChildren<HingeJoint>();
            foreach (var j in joints)
            {
                Rigidbody[] bodys = gameObject.GetComponentsInChildren<Rigidbody>();
                j.connectedBody = bodys[0];
            }
        }
    }

    public void StartRotation(float goAroundTime)
    {
        wheelGoAroundTime = goAroundTime;
        ApplySpeedAndAcceleration();
    }

    public void StopRotation()
    {
        wheelGoAroundTime = float.PositiveInfinity;
        ApplySpeedAndAcceleration();
    }

    public void CheckImpact(float impactWidth)
    {
        for (int i = 0; i < NumArms; i++)
        {
            if (arms[i].transform.eulerAngles.y <= 0.0f + impactWidth / 2 || arms[i].transform.eulerAngles.y >= 360.0f - impactWidth / 2)
            {
                arms[i].GetComponentsInChildren<Rigidbody>()[0].isKinematic = false;
            }
        }
    }

    public void SupportDisplay(bool enabled)
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        if (!lineRenderer)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        lineRenderer.enabled = enabled;
        for (int i = 0; i < NumArms; i++)
        {
            arms[i].GetComponent<ArmControl>().SupportDisplay(enabled);
        }

    }

    private void DrawGizmos()
    {
        if (lineRenderer)
        {
            var positions = new Vector3[]
            {
                new Vector3(0,0.05f,0),               // 開始点
                new Vector3(0,0.02f,-0.5f),           // 終了点
            };

            // 線を引く場所を指定する
            lineRenderer.SetPositions(positions);

            lineRenderer.startWidth = 0.001f;                   // 開始点の太さを0.1にする
            lineRenderer.endWidth = 0.001f;                     // 終了点の太さを0.1にする
        }
    }
}