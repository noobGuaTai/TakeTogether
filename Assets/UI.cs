using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public SkillSelectorUI skillSelectorUI;

    private void Awake()
    {
        skillSelectorUI = transform.Find("SkillSelectorUI").GetComponent<SkillSelectorUI>();
    }

    public void Update()
    {
        if (GlobalEnv.instance.EnableDebugKey) {
            if (Input.GetButtonUp("DEBUG_SKILL_SELECT")) {
                skillSelectorUI.Switch();
            }
        }
    }
}
