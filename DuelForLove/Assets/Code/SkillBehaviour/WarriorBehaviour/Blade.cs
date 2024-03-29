﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : SkillBehaviour
{
	protected override void PreCast ()
	{
		CommonOnPreCast();

		CommonOnCastSuccessfully();	//there is no if statement to call Fail Cast, meaning this skill cannot be s-ed
	}

	protected override void Casting ()
	{
		//TODO gameplay implementation

		EndCast();
	}

	protected override void EndCast ()
	{
		CommonOnEndCast();
	}

	//animation event
	public void BladeAttackEvent()
	{
		Ray ray = new Ray(mc.transform.position, mc.transform.forward);
		RaycastHit hit;
		//Debug.DrawRay(ray.origin, ray.direction * skillDataInstance.range, Color.red, 1f);
		if(Physics.Raycast(ray, out hit, skillDataInstance.range, skillDataInstance.targetLayer))
		{
			PlayRandomSkillAudio(skillDataInstance.hitClips);
			HealthBase hpObj = hit.transform.GetComponent<HealthBase>();
			Character cc = hit.transform.GetComponent<Character>();
			if(hpObj)
			{
				hpObj.TakeDamage(skillDataInstance.damage);	//could be another chatacter or block
				ShowHitEffect(hit.transform.position, Quaternion.LookRotation(mc.transform.forward));
			}

			if(cc)
			{
				cc.Cmm.AddForce(skillDataInstance.knockForce, mc.transform.forward);
			}
		}
	}
}
