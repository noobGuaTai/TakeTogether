using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEditor.UI;
using UnityEngine;

public class SkillSelectArrow : MonoBehaviour
{
    Tween tweenLR = new Tween();
    Tween tweenFloatUp = new Tween();
    Tween tweenFloatDw = new Tween();
    GameObject upArrow;
    GameObject downArrow;

    public void TweenSetPosition(Vector3 pos) { 
        transform.position = pos;
    }

    public void SelectButton(SkillButtonUI who) {
        tweenLR.Clear();

        tweenLR.AddTween<Vector3>(TweenSetPosition,
            transform.position, who.transform.position, 0.5f, 
            Tween.TransitionType.QUAD, Tween.EaseType.OUT);
        tweenLR.Play();
    }
    



    private void Awake()
    {
        tweenFloatUp.playMode = Tween.PlayMode.REPEAT;
        tweenFloatDw.playMode = Tween.PlayMode.REPEAT;

        upArrow = transform.Find("SelectArrowUp").gameObject;
        downArrow = transform.Find("SelectArrowDown").gameObject;
        Action<Vector3> setterUp = (Vector3 pos) =>
        {
            var cntPos = upArrow.transform.localPosition;
            upArrow.transform.localPosition = new Vector3(cntPos.x, pos.y, cntPos.z);
        };
        Action<Vector3> setterDw = (Vector3 pos) =>
        {
            var cntPos = downArrow.transform.localPosition;
            downArrow.transform.localPosition = new Vector3(cntPos.x, pos.y, cntPos.z);
        };
        float offsetLength = 2;
        Vector3[] upPos = 
            new Vector3[2]{ 
                upArrow.transform.localPosition, 
                upArrow.transform.localPosition - new Vector3(0, offsetLength, 0) };
        Vector3[] dwPos =
            new Vector3[2]{
                downArrow.transform.localPosition,
                downArrow.transform.localPosition + new Vector3(0, offsetLength, 0) };
        tweenFloatUp.AddTween<Vector3>(setterUp, upPos[0], upPos[1], 1f, Tween.TransitionType.QUAD, Tween.EaseType.IN_OUT);
        tweenFloatUp.AddTween<Vector3>(setterUp, upPos[1], upPos[0], 1f, Tween.TransitionType.QUAD, Tween.EaseType.IN_OUT);
        tweenFloatDw.AddTween<Vector3>(setterDw, dwPos[0], dwPos[1], 1f, Tween.TransitionType.QUAD, Tween.EaseType.IN_OUT);
        tweenFloatDw.AddTween<Vector3>(setterDw, dwPos[1], dwPos[0], 1f, Tween.TransitionType.QUAD, Tween.EaseType.IN_OUT);
        tweenFloatUp.Play();
        tweenFloatDw.Play();
    }

    private void Update()
    {
        tweenLR.Update();
        tweenFloatUp.Update();
        tweenFloatDw.Update();
    }
}
