using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public BoolData simulate;
    public FloatData gravity;
    public FloatData fixedFPS;
    public StringData fpsText;

    static World instance;
    static public World Instance { get { return instance; } }

    public Vector2 Gravity { get { return new Vector2(0, gravity.value); } }
    public List<Body> bodies { get; set; } = new List<Body>();

    public float fixedDeltaTime { get { return 1.0f / fixedFPS.value; } }

    float timeAccumulator = 0;
    float fps = 0;
    float fpsAverage = 0;
    float smoothing = 0.975f;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (!simulate.value) return;

        fps = 1.0f / Time.deltaTime;
        fpsAverage = (fpsAverage * smoothing) + (fps * (1.0f - smoothing));
        fpsText.value = string.Format("FPS: {0:F}", fpsAverage);

        timeAccumulator += Time.deltaTime;

        while (timeAccumulator > fixedDeltaTime)
        {
            bodies.ForEach(body => body.Step(fixedDeltaTime));
            bodies.ForEach(body => Integrator.SemiImplicitEuler(body, fixedDeltaTime));

            timeAccumulator -= fixedDeltaTime;
        }
        
        bodies.ForEach(body => body.force = Vector2.zero);
        bodies.ForEach(body => body.acceleration = Vector2.zero);
    }
}
