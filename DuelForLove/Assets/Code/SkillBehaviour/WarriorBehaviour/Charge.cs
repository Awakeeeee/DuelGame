﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : SkillBehaviour
{
	private float chargeTimer;

	protected override void PreCast ()
	{
		CommonOnPreCast();

		CommonOnCastSuccessfully();
		if(skillDataInstance.castEffect)
			Instantiate(skillDataInstance.castEffect, mc.transform.position, Quaternion.identity);
		mc.SetMovementPermission(false, false);
	}

	protected override void Casting ()
	{
		chargeTimer += Time.deltaTime;

		Ray ray = new Ray(mc.transform.position, mc.transform.forward);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, skillDataInstance.range, skillDataInstance.targetLayer, QueryTriggerInteraction.Ignore))
		{
			PlayRandomSkillAudio(skillDataInstance.hitClips);
			if(skillDataInstance.hitEffect)
				Instantiate(skillDataInstance.hitEffect, hit.point, Quaternion.LookRotation(mc.transform.forward));

			Character cc = hit.transform.GetComponent<Character>();
			if(cc)
			{
				cc.Chp.TakeDamage(skillDataInstance.damage);
				cc.Stunned(skillDataInstance.effectDuration);
				cc.Cmm.TriggerShake();
			}
		}

		if(chargeTimer < skillDataInstance.skillDuration && hit.transform == null)
		{
			mc.transform.position += mc.transform.forward * skillDataInstance.buffs[0].value * Time.deltaTime;
		}else
		{
			EndCast();
		}
	}

	protected override void EndCast ()
	{
		CommonOnEndCast();

		hero.BreakSkillAnim(this.sIndex);
		chargeTimer = 0.0f;
		mc.SetMovementPermission(true, true);
	}
}