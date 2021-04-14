using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using IceMilkTea.Core;

public class GameControl : MonoBehaviour
{
    enum GameStateEventID
    {
        Initial,
        Demo,
        Game,
    }

    [SerializeField]
    private GameObject Wheel;
    [SerializeField]
    private GameObject Hammer;
    [SerializeField]
    private GameObject Prize;

    private ImtStateMachine<GameControl> gameState;

    private GameObject wheel;
    private GameObject hammer;
    private GameObject[] prizes;

    private bool supportDisdplay;

    private float goAroundTime;

    private const int NumPrizes1 = 11;
    private const int NumPrizes2 = 10;
    private const int NumPrizes3 = 9;
    private const int NumPrizes = NumPrizes1+ NumPrizes2+ NumPrizes3;

    public WheelControl WheelControl
    {
        get { return wheel.GetComponent<WheelControl>(); }
    }
    public HammerControl HammerControl
    {
        get { return hammer.GetComponent<HammerControl>(); }
    }

    public void Awake()
    {
        gameState = new ImtStateMachine<GameControl>(this);
        gameState.AddTransition<StateInitial, StateDemo>((int)GameStateEventID.Demo);
        gameState.AddTransition<StateDemo, StateGame>((int)GameStateEventID.Game);
        gameState.AddTransition<StateDemo, StateInitial>((int)GameStateEventID.Initial);
        gameState.AddTransition<StateGame, StateDemo>((int)GameStateEventID.Demo);
        gameState.AddTransition<StateGame, StateInitial>((int)GameStateEventID.Initial);
        gameState.SetStartState<StateInitial>();

        hammer = Instantiate(Hammer, new Vector3(0.0f, -0.0533f, -0.4927f), Quaternion.Euler(new Vector3(10.0f, 0.0f, 0.0f)));
        hammer.GetComponent<HammerControl>().gameControl = this;

        wheel = Instantiate(Wheel, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        wheel.GetComponent<WheelControl>().gameControl = this;
        goAroundTime = wheel.GetComponent<WheelControl>().WheelGoAroundTime;

        prizes = new GameObject[NumPrizes];
        for (int i = 0; i < NumPrizes; i++)
        {
            prizes[i] = Instantiate(Prize);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetSupportDisplay(false);

        GameObject canvas = GameObject.Find("Canvas");
        InputField[] inputs = canvas.GetComponentsInChildren<InputField>();
        inputs[0].text = wheel.GetComponent<WheelControl>().WheelGoAroundTime.ToString();
        inputs[1].text = wheel.GetComponent<WheelControl>().WheelAccelerationTime.ToString();
        inputs[2].text = hammer.GetComponent<HammerControl>().ImpactTime.ToString();
        inputs[3].text = hammer.GetComponent<HammerControl>().ImpactWidth.ToString();
        Toggle[] toggls = canvas.GetComponentsInChildren<Toggle>();
        toggls[0].isOn = false;

        gameState.Update();
    }

    // Update is called once per frame
    void Update()
    {
        gameState.Update();

        if (gameState.IsCurrentState<StateDemo>() || gameState.IsCurrentState<StateGame>())
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                gameState.SendEvent((int)GameStateEventID.Initial);
            }
        }
    }

    void ResetScene()
    {
        CameraControl camera = gameObject.GetComponent<CameraControl>();
        camera.baseObject = wheel;
        camera.targetObject = wheel;

        wheel.GetComponent<WheelControl>().Reset();

        hammer.GetComponent<HammerControl>().Reset();

        for (int i = 0; i < NumPrizes; i++)
        {
            float rotation = 0.0f;
            float radius = 0.0f;
            float height = 0.0f;
            if (i < NumPrizes1)
            {
                rotation = 2.0f * Mathf.PI * i / NumPrizes1;
                radius = 0.160f;
                height = 0.041f;
            }
            else
            if (i < NumPrizes1 + NumPrizes2)
            {
                rotation = 2.0f * Mathf.PI * (i - NumPrizes1) / NumPrizes2;
                radius = 0.150f;
                height = 0.131f;
            }
            else
            if (i < NumPrizes1 + NumPrizes2 + NumPrizes3)
            {
                rotation = 2.0f * Mathf.PI * (i - (NumPrizes1 + NumPrizes2)) / NumPrizes3;
                radius = 0.140f;
                height = 0.221f;
            }

            float x = radius * Mathf.Sin(rotation);
            float z = radius * Mathf.Cos(rotation);

            prizes[i].transform.localPosition = new Vector3(x, height, -z);
            prizes[i].transform.localRotation =  Quaternion.Euler(0.0f, Mathf.Rad2Deg * -rotation, 0.0f);
        }
    }

    public void ChangeWheelGoAroundTime(InputField input)
    {
        float value;
        if (float.TryParse(input.text, out value))
        {
            value = Mathf.Clamp(value, 1f, 120f);
            input.text = value.ToString();
            wheel.GetComponent<WheelControl>().WheelGoAroundTime = value;

            goAroundTime = value;
        }
    }

    public void ChangeWheelAccelerationTime(InputField input)
    {
        float value;
        if (float.TryParse(input.text, out value))
        {
            value = Mathf.Clamp(value, 0.1f, 10f);
            input.text = value.ToString();
            wheel.GetComponent<WheelControl>().WheelAccelerationTime = value;
        }
    }

    public void ChangeImpactTime(InputField input)
    {
        float value;
        if (float.TryParse(input.text, out value))
        {
            value = Mathf.Clamp(value, 0.1f, 10f);
            input.text = value.ToString();
            hammer.GetComponent<HammerControl>().ImpactTime = value;
        }
    }

    public void ChangeImpactWidth(InputField input)
    {
        float value;
        if (float.TryParse(input.text, out value))
        {
            value = Mathf.Clamp(value, 0f, 24f);
            input.text = value.ToString();
            hammer.GetComponent<HammerControl>().ImpactWidth = value;
        }
    }

    public void ChangeSupportDisplay(Toggle toggle)
    {
        SetSupportDisplay(toggle.isOn);
    }

    void SetSupportDisplay(bool enabled)
    {
        supportDisdplay = enabled;
        WheelControl.SupportDisplay(enabled);
    }

    void WheelStart()
    {
	    wheel.GetComponent<WheelControl>().StartRotation(goAroundTime);
    }

    void WheelStop()
    {
        wheel.GetComponent<WheelControl>().StopRotation();
    }

    void HammerStart()
    {
        hammer.GetComponent<HammerControl>().StartAction();
    }

    bool IsHammerReady()
    {
        return hammer.GetComponent<HammerControl>().IsReady();
    }

    private class StateInitial : ImtStateMachine<GameControl>.State
    {
        protected internal override void Enter()
        {
            Context.ResetScene();
        }

        protected internal override void Update()
        {
            StateMachine.SendEvent((int)GameStateEventID.Demo);
        }

        protected internal override void Exit()
        {
        }
    }

    private class StateDemo : ImtStateMachine<GameControl>.State
    {
        protected internal override void Enter()
        {
            Context.WheelStart();
        }

        protected internal override void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
                StateMachine.SendEvent((int)GameStateEventID.Game);
            }
        }

        protected internal override void Exit()
        {
        }
    }

    private class StateGame : ImtStateMachine<GameControl>.State
    {
        protected internal override void Enter()
        {
            Context.WheelStop();
            Context.HammerStart();
        }

        protected internal override void Update()
        {
            if(Context.IsHammerReady())
            {
                StateMachine.SendEvent((int)GameStateEventID.Demo);
            }
        }

        protected internal override void Exit()
        {
        }
    }
}

