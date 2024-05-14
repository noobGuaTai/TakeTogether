using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtonUI : MonoBehaviour
{
    public delegate void OnSelect(SkillButtonUI who);

    public string skillName;
    public Button button;
    public OnSelect onSelect;
    public SkillManager skillManager;

    void GetSkillManager() {
        skillManager = transform.Find("/SkillManager").GetComponent<SkillManager>();
    }

    private void Awake()
    {
        button = GetComponent<Button>();
        
    }

    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            onSelect(this);
            if (GlobalEnv.instance.EnableDebugOutput)
                print("skill button clicked");
        });
    }

    internal void LoadSkill(string key) {
        try {
            skillName = key;
            var image = transform.Find("SkillIcon").GetComponent<Image>();
            if (skillManager is null) GetSkillManager();
            image.sprite = skillManager.skillIcons[key];
            GetComponent<Button>().interactable = (key != "Null");
        }
        catch(Exception e) {
            print(e);
        }
    }
}
