using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public BoolData simulate;
    public BoolData collision;
    public BoolData wrap;
    public FloatData gravity;
    public FloatData gravitation;
    public FloatData fixedFPS;
    public StringData fpsText;

    static World instance;
    static public World Instance { get { return instance; } }

    public Vector2 Gravity { get { return new Vector2(0, gravity.value); } }
    public List<Body> bodies { get; set; } = new List<Body>();
    public List<Spring> springs { get; set; } = new List<Spring>();

    public float fixedDeltaTime { get { return 1.0f / fixedFPS.value; } }

    Vector2 size;
    float timeAccumulator = 0;
    float fps = 0;
    float fpsAverage = 0;
    float smoothing = 0.975f;

    void Awake()
    {
        instance = this;
        size = Camera.main.ViewportToWorldPoint(Vector2.one);
    }

    void Start()
    {
        Object[] tempBodies = FindObjectsOfType(typeof(Body));
        foreach (Body body in tempBodies)
        {
            Shape box = body.GetComponent<BoxShape>();
            Shape circle = body.GetComponent<CircleShape>();
            body.shape = (circle != null) ? circle : box;
            bodies.Add(body);
        }
        //Object[] tempSprings = FindObjectsOfType(typeof(Spring));
        //foreach (Spring spring in tempSprings)
        //{
        //    springs.Add(spring);
        //}
    }

    void Update()
    {
        springs.ForEach(spring => spring.Draw());
        if (!simulate.value) return;

        fps = 1.0f / Time.deltaTime;
        fpsAverage = (fpsAverage * smoothing) + (fps * (1.0f - smoothing));
        fpsText.value = string.Format("FPS: {0:F}", fpsAverage);

        GravitationalForce.ApplyForce(bodies, gravitation.value);
        springs.ForEach(spring => spring.ApplyForce());

        timeAccumulator += Time.deltaTime;
        while (timeAccumulator > fixedDeltaTime)
        {
            bodies.ForEach(body => body.Step(fixedDeltaTime));
            bodies.ForEach(body => Integrator.SemiImplicitEuler(body, fixedDeltaTime));

            bodies.ForEach(body => body.shape.color = Color.white);

            if (collision)
            {
                Collision.CreateContacts(bodies, out List<Contact> contacts);
                contacts.ForEach(contact => { contact.bodyA.shape.color = Color.red; contact.bodyB.shape.color = Color.red; });
                ContactSolver.Resolve(contacts);
            }

            timeAccumulator -= fixedDeltaTime;
        }

        if(wrap)
        {
            bodies.ForEach(body => body.position = Utilities.Wrap(body.position, -size, size)); 
        }

        bodies.ForEach(body => body.force = Vector2.zero);
        bodies.ForEach(body => body.acceleration = Vector2.zero);
    }
}
