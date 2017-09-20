﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : SkillBehaviour
{
	public ShieldBlock shieldBlock;
	private float castingTimer;
	
	protected override void PreCast ()
	{
		CommonOnPreCast();

		CommonOnCastSuccessfully();
		mc.ChangeMoveSpeed(skillDataInstance.optionalParams[0].value);
		mc.ChangeKnockResist(skillDataInstance.optionalParams[1].value);

		//Enable shield-block, or create the first one
		if(shieldBlock)
		{
			shieldBlock.gameObject.SetActive(true);
		}else
		{
			GameObject go = new GameObject("Shield");
			go.transform.SetParent(mc.transform);
			go.layer = LayerMask.NameToLayer("Block");
			go.transform.localPosition = new Vector3(0f, 0f, 0.6f);
			go.transform.localRotation = Quaternion.identity;
			BoxCollider bx = go.AddComponent<BoxCollider>();
			bx.size = new Vector3(1.5f, 1f, 0.2f);
			shieldBlock = go.AddComponent<ShieldBlock>();
		}

		shieldBlock.InitShield(skillDataInstance.damage);
	}

	protected override void Casting ()
	{
		castingTimer += Time.deltaTime;
		mc.Csu.ToggleCastingProgress(true);
		mc.Csu.UpdateCastingProgress(1 - castingTimer / skillDataInstance.skillDuration);

		if(castingTimer >= skillDataInstance.skillDuration || Input.GetButtonUp(mc.skill_2_Axis) || shieldBlock.IsBroken)	//TODO the axis..
		{
			EndCast();
		}
	}

	protected override void EndCast ()
	{
		CommonOnEndCast();

		hero.BreakSkillAnim(this.sIndex);
		mc.ResetMoveSpeed();
		mc.ResetKnockResist();
		shieldBlock.gameObject.SetActive(false);
		mc.Csu.ToggleCastingProgress(false);
		castingTimer = 0.0f;
	}
}
