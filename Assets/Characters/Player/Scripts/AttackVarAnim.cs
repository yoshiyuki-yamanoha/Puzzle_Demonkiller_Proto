using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackVarAnim : MonoBehaviour
{

	Animation anim;
	Attackvariation attackvar;

	// Use this for initialization
	void Start()
	{
		anim = this.gameObject.GetComponent<Animation>();
		attackvar = GameObject.Find("GameObject").GetComponent<Attackvariation>();

	}

	// Update is called once per frame
	void Update()
	{

		if (attackvar.activeflg_orb)
		{

			anim.Play();



		}


	}
}