using UnityEditor;
using UnityEngine;
using System.Collections.Generic;


public class AnimationBaker : EditorWindow
{
    private GameObject selectedObject;
    private Animator animator;
    private List<string> animationClipNames = new List<string>();
    private int selectedClipIndex = 0;

    [MenuItem("Animation Rigging/Bake IK to Bones")]
    public static void ShowWindow()
    {
        GetWindow<AnimationBaker>("Bake IK to Bones");
    }

    private void OnGUI()
    {
        GUILayout.Label("Bake IK to Bones", EditorStyles.boldLabel);

        selectedObject = Selection.activeGameObject;

        if (selectedObject == null || (animator = selectedObject.GetComponent<Animator>()) == null)
        {
            GUILayout.Label("Select a GameObject with an Animator component.");
            return;
        }

        if (animator.runtimeAnimatorController != null)
        {
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            animationClipNames.Clear();

            foreach (var clip in clips)
            {
                animationClipNames.Add(clip.name);
            }

            selectedClipIndex = EditorGUILayout.Popup("Select Animation Clip", selectedClipIndex, animationClipNames.ToArray());

            if (GUILayout.Button("Bake Selected Animation"))
            {
                BakeSelectedAnimation(animator, clips[selectedClipIndex]);
            }
        }
        else
        {
            GUILayout.Label("The selected object has no animations.");
        }
    }

    private void BakeSelectedAnimation(Animator animator, AnimationClip originalClip)
    {
        if (originalClip == null)
        {
            Debug.LogError("No animation clip selected.");
            return;
        }

        Debug.Log($"Baking animation: {originalClip.name}");

        // Create a new animation clip for baking
        AnimationClip newClip = new AnimationClip();
        float sampleRate = originalClip.frameRate;
        float clipLength = originalClip.length;

        // Get all the bones (transforms) in the hierarchy and their relative paths
        Dictionary<Transform, string> bonePaths = new Dictionary<Transform, string>();
        Transform[] bones = selectedObject.GetComponentsInChildren<Transform>();
        foreach (Transform bone in bones)
        {
            string path = GetRelativePath(selectedObject.transform, bone);
            bonePaths[bone] = path;
        }

        // Use the AnimationMode API to evaluate the clip at different time intervals
        AnimationMode.StartAnimationMode();
        AnimationMode.BeginSampling();
        AnimationMode.SampleAnimationClip(selectedObject, originalClip, 0f);  // Start from time 0

        for (float time = 0; time <= clipLength; time += 1f / sampleRate)
        {
            AnimationMode.SampleAnimationClip(selectedObject, originalClip, time);

            foreach (Transform bone in bones)
            {
                string path = bonePaths[bone];

                // Capture the bone's position, rotation, and scale at this time
                Vector3 position = bone.localPosition;
                Quaternion rotation = bone.localRotation;
                Vector3 scale = bone.localScale;

                // Record the keyframes for the position, rotation, and scale in the new animation clip
                newClip.SetCurve(path, typeof(Transform), "localPosition.x", AnimationCurve.Linear(time, position.x, time, position.x));
                newClip.SetCurve(path, typeof(Transform), "localPosition.y", AnimationCurve.Linear(time, position.y, time, position.y));
                newClip.SetCurve(path, typeof(Transform), "localPosition.z", AnimationCurve.Linear(time, position.z, time, position.z));

                newClip.SetCurve(path, typeof(Transform), "localRotation.x", AnimationCurve.Linear(time, rotation.x, time, rotation.x));
                newClip.SetCurve(path, typeof(Transform), "localRotation.y", AnimationCurve.Linear(time, rotation.y, time, rotation.y));
                newClip.SetCurve(path, typeof(Transform), "localRotation.z", AnimationCurve.Linear(time, rotation.z, time, rotation.z));
                newClip.SetCurve(path, typeof(Transform), "localRotation.w", AnimationCurve.Linear(time, rotation.w, time, rotation.w));

                newClip.SetCurve(path, typeof(Transform), "localScale.x", AnimationCurve.Linear(time, scale.x, time, scale.x));
                newClip.SetCurve(path, typeof(Transform), "localScale.y", AnimationCurve.Linear(time, scale.y, time, scale.y));
                newClip.SetCurve(path, typeof(Transform), "localScale.z", AnimationCurve.Linear(time, scale.z, time, scale.z));
            }
        }

        // Stop sampling the animation
        AnimationMode.EndSampling();
        AnimationMode.StopAnimationMode();

        // Save the new baked animation clip as a new asset
        string newClipPath = $"Assets/Art/Animations/Baked_{originalClip.name}.anim";
        AssetDatabase.CreateAsset(newClip, newClipPath);
        AssetDatabase.SaveAssets();

        Debug.Log($"Animation baked successfully and saved as: {newClipPath}");
    }

    private string GetRelativePath(Transform root, Transform target)
    {
        if (target == root)
        {
            return ""; // Return empty string for root object
        }

        string path = target.name;
        while (target.parent != null && target.parent != root)
        {
            target = target.parent;
            path = target.name + "/" + path;
        }

        return path;
    }
}
