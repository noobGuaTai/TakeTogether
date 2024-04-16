using UnityEngine;
using System.Collections;
using TMPro; // 确保使用TextMeshPro的命名空间

public class ConsoleToScreen : MonoBehaviour
{
    public TextMeshProUGUI consoleOutput; // 引用UI Text组件
    private string log = "";

    void OnEnable()
    {
        Application.logMessageReceived += LogMessage;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= LogMessage;
    }

    void LogMessage(string message, string stackTrace, LogType type)
    {
        log += message + "\n";
        if (type == LogType.Exception)
        {
            log += stackTrace + "\n"; // 可选，如果你想显示堆栈跟踪
        }
        consoleOutput.text = log; // 更新Text组件显示的文本
    }
}
