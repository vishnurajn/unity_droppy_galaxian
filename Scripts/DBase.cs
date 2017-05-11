using UnityEngine;
using System.Collections;
using System;

public class DBase : MonoBehaviour 
{
	public DColor color;

	[NonSerialized] public GameManager gm;

	[NonSerialized] public SpriteRenderer sr;

	[NonSerialized] public TrailRenderer tr;

	[NonSerialized] public Vector3 position;


	virtual public void Awake()
	{
		gm = FindObjectOfType<GameManager>();

		sr = GetComponent<SpriteRenderer>();

		tr = GetComponent<TrailRenderer>();
	}

	public virtual void SetColor(DColor c)
	{
		if(c == DColor.RED)
		{
			color = DColor.RED;

			sr.color = gm.red;

	
		}
		else if(c == DColor.GREEN)
		{
			color = DColor.GREEN;

			sr.color = gm.green;


		}
		else if(c == DColor.BLUE)
		{
			color = DColor.BLUE;

			sr.color = gm.blue;

		
		}
		else if(c == DColor.YELLOW)
		{
			color = DColor.YELLOW;

			sr.color = gm.yellow;

	
		}

		if(tr != null)
		{
			Color cc = sr.color;
			cc.a = 0.3f;
			tr.material.SetColor("_TintColor", cc);
		}
		
	}
}
