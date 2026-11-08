#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ButtonSoundAdder : EditorWindow
{
    [MenuItem("Tools/Add Button Sound To All Buttons")]
    public static void AddSoundToAllButtons()
    {
        Button[] allButtons = FindObjectsByType<Button>(FindObjectsSortMode.None);
        int count = 0;

        foreach (Button btn in allButtons)
        {
            // Add ButtonSound component if missing
            ButtonSound buttonSound = btn.GetComponent<ButtonSound>();
            if (buttonSound == null)
                buttonSound = btn.gameObject.AddComponent<ButtonSound>();

            // Check if PlaySound already wired to avoid duplicates
            bool alreadyWired = false;
            for (int i = 0; i < btn.onClick.GetPersistentEventCount(); i++)
            {
                if (btn.onClick.GetPersistentMethodName(i) == "PlaySound")
                {
                    alreadyWired = true;
                    break;
                }
            }

            // Wire PlaySound to OnClick
            if (!alreadyWired)
            {
                UnityAction action = buttonSound.PlaySound;
                UnityEditor.Events.UnityEventTools.AddPersistentListener(btn.onClick, action);
                EditorUtility.SetDirty(btn);
                count++;
            }
        }

        Debug.Log($"ButtonSound added and wired to {count} buttons.");
    }
}
#endif