using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public BoolData simulate;
    public FloatData gravity;
    public FloatData fixedFPS;
    public StringData fps;

    static World instance;
    static public World Instance { get { return instance; } }

    public Vector2 Gravity { get { return new Vector2(0, gravity.value); } }
    public List<Body> bodies { get; set; } = new List<Body>();
    public float fixedDeltaTime { get { return 1.0f / fixedFPS.value; } }

    float timeAccumulator = 0;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (!simulate.value) return;

        timeAccumulator += Time.deltaTime;

        while (timeAccumulator > fixedDeltaTime)
        {
            bodies.ForEach(body => body.Step(fixedDeltaTime));
            bodies.ForEach(body => Integrator.SemiImplicitEuler(body, fixedDeltaTime));

            bodies.ForEach(body => body.force = Vector2.zero);
            bodies.ForEach(body => body.acceleration = Vector2.zero);

            timeAccumulator -= fixedDeltaTime;
        }

        smoothFPS();
    }

    void smoothFPS()
    {
        //Not smoothed. Need to do that, don't know how
        float smoothFPS = 1.0f / Time.deltaTime;
        fps.value = string.Format("FPS: {0:F}", smoothFPS);
    }
}
