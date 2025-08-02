using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(PlayerWeaponManager))]
public class PlayerWeaponManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlayerWeaponManager weaponManager = (PlayerWeaponManager)target;

        if (Application.isPlaying == false)
        {
            if (GUILayout.Button("Spawn Gun (Editor Preview)"))
            {
                weaponManager.EditorSpawnGun();
            }

            if (GUILayout.Button("Delete Gun (Editor Preview)"))
            {
                weaponManager.DeleteGun();
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Editor preview is only available in Edit Mode.", MessageType.Info);
        }
    }
}

