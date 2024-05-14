using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelectorUI : MonoBehaviour
{
    // List of skill button UI components.
    List<SkillButtonUI> skillButtons = new List<SkillButtonUI>();
    // Reference to the skill selection arrow.
    SkillSelectArrow skillArrow;
    // UI component of the currently selected skill button.
    SkillButtonUI selectedButtonUI;
    // Refresh button component.
    Button refresh;
    // Accept button component.
    Button accept;
    // Refuse button component.
    Button refuse;
    // Reference to the player manager.
    PlayerManager playerManager;
     
    // close to open, open to close
    public void Switch() {
        var isActive = gameObject.activeSelf;

        
        switch (isActive) {
            // open
            case false:
                gameObject.SetActive(true);
                RandomNewSkills(); break;
            case true:
                gameObject.SetActive(false);
                break;
        }
        
        // close
    }

    // Called when a skill button is selected.
    public void OnSkillButtonSelected(SkillButtonUI who) {
        selectedButtonUI = who;
        skillArrow.SelectButton(who);
    }

    // Retrieves the PlayerSkill component from the local player.
    PlayerSkill GetPlayerSkill() {
        var localPlayer = playerManager.localPlayer;
        var playerSkill = localPlayer.transform.Find("PlayerSkill").GetComponent<PlayerSkill>();
        return playerSkill;
    }

    // Action to perform when the Accept button is clicked.
    void OnButtonAcceptClicked() {
        var playerSkill = GetPlayerSkill();
        playerSkill?.Unlock(selectedButtonUI.skillName);
        gameObject.SetActive(false);

        RandomNewSkills();
    }

    // Action to perform when the Refuse button is clicked.
    void OnButtonRefuseClicked() {
        gameObject.SetActive(false);
    }

    void RandomNewSkills() {
        var playerSkill = GetPlayerSkill();
        if (playerSkill == null) return;
        var canUnlockList = playerSkill.canUnlockSkill.ToList();
        var randomPerm = GlobalEnv.instance.GetRandPerm(canUnlockList.Count);

        for (int i = 0; i < skillButtons.Count; i++) {
            var button = skillButtons[i];

            if (i >= randomPerm.Length) {
                button.LoadSkill("Null");
                continue;
            }
            button.LoadSkill(canUnlockList[randomPerm[i]].Key);
        }
    }

    void PlayRefreshAnimation() { 
        
    }

    // Action to perform when the Refresh button is clicked.
    void OnButtonRefreshClicked() {
        RandomNewSkills();
        PlayRefreshAnimation();
    }

    // Initializes component references and subscribes to button events upon component awakening.
    private void Awake() {
        var skillButtonPath = "SkillButtons/";
        var buttonCount = transform.Find(skillButtonPath).childCount;
        for (int i = 0; i < buttonCount; i++) {
            int ix = i + 1;
            var cntSkillButtonPath = skillButtonPath + $"SkillButtonUI ({ix})";
            var skillButtonUI = transform.Find(cntSkillButtonPath).GetComponent<SkillButtonUI>();

            skillButtonUI.onSelect += OnSkillButtonSelected;
            skillButtons.Add(skillButtonUI);
        }
        skillArrow = transform.Find("SelectArrow").GetComponent<SkillSelectArrow>();
        refresh = transform.Find("Buttons/Refresh").GetComponent<Button>();
        accept = transform.Find("Buttons/Accept").GetComponent<Button>();
        refuse = transform.Find("Buttons/Refuse").GetComponent<Button>();

        refresh.onClick.AddListener(OnButtonRefreshClicked);
        accept.onClick.AddListener(OnButtonAcceptClicked);
        refuse.onClick.AddListener(OnButtonRefuseClicked);

        playerManager = transform.Find("/PlayerManager").GetComponent<PlayerManager>();

        selectedButtonUI = skillButtons[0];
    }
}
