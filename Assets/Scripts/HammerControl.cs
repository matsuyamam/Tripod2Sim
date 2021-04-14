using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using IceMilkTea.Core;

public class HammerControl : MonoBehaviour
{
    enum HammerStateEventID
    {
        Initial,
        Wait,
        Action,
        Pause,
        Return,
    }

    private float rotation;
    private ImtStateMachine<HammerControl> hammerState;

    public float ImpactTime { get; set; }
    public float ImpactWidth { get; set; }

    public GameControl gameControl { get; set; }

    public void Awake()
    {
        hammerState = new ImtStateMachine<HammerControl>(this);
        hammerState.AddAnyTransition<StateInitial>((int)HammerStateEventID.Initial);
        hammerState.AddTransition<StateInitial, StateWait>((int)HammerStateEventID.Wait);
        hammerState.AddTransition<StateWait, StateAction>((int)HammerStateEventID.Action);
        hammerState.AddTransition<StateAction, StatePause>((int)HammerStateEventID.Pause);
        hammerState.AddTransition<StatePause, StateReturn>((int)HammerStateEventID.Return);
        hammerState.AddTransition<StateReturn, StateWait>((int)HammerStateEventID.Wait);
        hammerState.SetStartState<StateInitial>();
        hammerState.Update();

        ImpactTime = 0.3f;
        ImpactWidth = 6f;
    }
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        hammerState.Update();
        gameObject.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(rotation, 0.0f, 0.0f));
    }

    public void Reset()
    {
        hammerState.SendEvent((int)HammerStateEventID.Initial);
    }

    private class StateInitial : ImtStateMachine<HammerControl>.State
    {
        protected internal override void Enter()
        {
            Context.rotation = 10.0f;
        }

        protected internal override void Update()
        {
            StateMachine.SendEvent((int)HammerStateEventID.Wait);
        }

        protected internal override void Exit()
        {
        }
    }

    private class StateWait : ImtStateMachine<HammerControl>.State
    {
        protected internal override void Enter()
        {
        }

        protected internal override void Update()
        {
        }

        protected internal override void Exit()
        {
        }
    }

    private class StateAction : ImtStateMachine<HammerControl>.State
    {
        protected internal override void Enter()
        {
            //Context.GetComponent<Rigidbody>().isKinematic = false;
        }

        protected internal override void Update()
        {
            Context.rotation += (60f - 10f) / Context.ImpactTime * Time.deltaTime;
            if (Context.rotation > 60.0f)
            {
                Context.rotation = 60.0f;
                Context.gameControl.WheelControl.CheckImpact(Context.ImpactWidth);
                stateMachine.SendEvent((int)HammerStateEventID.Pause);
            }
            Context.gameObject.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(Context.rotation, 0.0f, 0.0f));
        }

        protected internal override void Exit()
        {
        }
    }

    private class StatePause : ImtStateMachine<HammerControl>.State
    {
        float waitTime;

        protected internal override void Enter()
        {
            waitTime = 2.0f;
        }

        protected internal override void Update()
        {
            waitTime -= Time.deltaTime;
            if (waitTime <= 0.0f)
            {
                stateMachine.SendEvent((int)HammerStateEventID.Return);
            }
        }

        protected internal override void Exit()
        {
        }
    }


    private class StateReturn : ImtStateMachine<HammerControl>.State
    {
        protected internal override void Enter()
        {
            //Context.GetComponent<Rigidbody>().isKinematic = true;
        }

        protected internal override void Update()
        {
            Context.rotation -= 10.0f * Time.deltaTime;
            if(Context.rotation <= 10.0f)
            {
                Context.rotation = 10.0f;
                StateMachine.SendEvent((int)HammerStateEventID.Wait);
            }
            Context.gameObject.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(Context.rotation, 0.0f, 0.0f));
        }

        protected internal override void Exit()
        {
        }
    }

    public void StartAction()
    {
        if(hammerState.IsCurrentState<StateWait>())
        {
            hammerState.SendEvent((int)HammerStateEventID.Action);
        }
    }

    public bool IsReady()
    {
        return hammerState.IsCurrentState<StateWait>();
    }
}
