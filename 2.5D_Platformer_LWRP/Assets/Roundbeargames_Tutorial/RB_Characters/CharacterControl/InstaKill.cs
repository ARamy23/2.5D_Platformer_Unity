﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Roundbeargames
{
    public class InstaKill : MonoBehaviour
    {
        CharacterControl control;

        private void Start()
        {
            control = this.gameObject.GetComponentInParent<CharacterControl>();
        }

        private void FixedUpdate()
        {
            if (control.SubComponentsDic.ContainsKey(SubComponents.MANUALINPUT))
            {
                return;
            }

            if (!control.SkinnedMeshAnimator.GetBool(HashManager.Instance.DicMainParams[TransitionParameter.Grounded]))
            {
                return;
            }
            
            foreach(KeyValuePair<TriggerDetector, List<Collider>> data in control.animationProgress.CollidingBodyParts)
            {
                foreach(Collider col in data.Value)
                {
                    CharacterControl c = CharacterManager.Instance.GetCharacter(col.transform.root.gameObject);

                    if (c == control)
                    {
                        continue;
                    }

                    if (!c.SubComponentsDic.ContainsKey(SubComponents.MANUALINPUT))
                    {
                        continue;
                    }

                    if (!c.SkinnedMeshAnimator.GetBool(HashManager.Instance.DicMainParams[TransitionParameter.Grounded]))
                    {
                        continue;
                    }

                    if (c.animationProgress.IsRunning(typeof(Attack)))
                    {
                        continue;
                    }

                    if (control.animationProgress.IsRunning(typeof(Attack)))
                    {
                        continue;
                    }

                    if (c.animationProgress.StateNameContains("RunningSlide"))
                    {
                        continue;
                    }

                    if (c.damageDetector.IsDead())
                    {
                        continue;
                    }

                    if (control.damageDetector.IsDead())
                    {
                        continue;
                    }

                    Debug.Log("instaKill");
                    c.damageDetector.DeathByInstaKill(control);

                    return;
                }
            }
        }
    }
}