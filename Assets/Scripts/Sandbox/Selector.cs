using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Action
{
    public SpriteRenderer spriteRenderer;

	public override eActionType actionType => eActionType.Selector;

	Body selectedBody = null;

	public override void StartAction()
	{
		selectedBody = Utilities.GetBodyFromPosition(Input.mousePosition);
	}

	public override void StopAction()
	{
		selectedBody = null;
	}

	void Update()
    {
		Body body = Utilities.GetBodyFromPosition(Input.mousePosition);
		if (body != null || selectedBody != null)
		{
			if (selectedBody) body = selectedBody;

            spriteRenderer.enabled = true;
            transform.position = body.position;
			transform.rotation = Quaternion.AngleAxis(Time.time * 90, Vector3.forward);
			transform.localScale = Vector2.one * body.shape.size * 1.25f;
        }
        else
		{
            spriteRenderer.enabled = false;
		}

		if (selectedBody)
		{
			Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			
			if (selectedBody.type == Body.eType.Static)
			{
				selectedBody.position = position;
			}
			if (selectedBody.type == Body.eType.Kinematic || selectedBody.type == Body.eType.Dynamic)
			{
				Vector2 force = Utilities.SpringForce(position, selectedBody.position, 0, 5);
				selectedBody.AddForce(force, Body.eForceMode.Velocity);
			}
		}
	}
}
