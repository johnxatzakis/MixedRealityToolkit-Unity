﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.﻿

using Microsoft.MixedReality.Toolkit.Internal.Definitions.InputSystem;
using UnityEditor;
using UnityEngine;

namespace Microsoft.MixedReality.Toolkit.Inspectors.Profiles
{
    [CustomEditor(typeof(MixedRealityInputActionsProfile))]
    public class MixedRealityInputActionsProfileInspector : MixedRealityBaseConfigurationProfileInspector
    {
        private static readonly GUIContent MinusButtonContent = new GUIContent("-", "Remove Action");
        private static readonly GUIContent AddButtonContent = new GUIContent("+ Add a New Action", "Add New Action");
        private static readonly GUIContent ActionContent = new GUIContent("Action", "The Name of the Action.");
        private static readonly GUIContent AxisConstraintContent = new GUIContent("Axis Constraint", "Optional Axis Constraint for this input source.");

        private static Vector2 scrollPosition = Vector2.zero;

        private SerializedProperty inputActionList;

        private void OnEnable()
        {
            if (!CheckMixedRealityManager(false))
            {
                return;
            }

            inputActionList = serializedObject.FindProperty("inputActions");
        }

        public override void OnInspectorGUI()
        {
            RenderMixedRealityToolkitLogo();

            EditorGUILayout.LabelField("Input Actions", EditorStyles.boldLabel);

            if (!CheckMixedRealityManager())
            {
                return;
            }

            EditorGUILayout.HelpBox("Input Actions are any/all actions your users will be able to make when interacting with your application.\n\n" +
                                    "After defining all your actions, you can then wire up these actions to hardware sensors, controllers, and other input devices.", MessageType.Info);

            serializedObject.Update();
            RenderList(inputActionList);
            serializedObject.ApplyModifiedProperties();
        }

        private static void RenderList(SerializedProperty list)
        {
            EditorGUILayout.Space();
            GUILayout.BeginVertical();
            if (GUILayout.Button(AddButtonContent, EditorStyles.miniButton))
            {
                list.arraySize += 1;
                var inputAction = list.GetArrayElementAtIndex(list.arraySize - 1);
                var inputActionId = inputAction.FindPropertyRelative("id");
                var inputActionDescription = inputAction.FindPropertyRelative("description");
                var inputActionConstraint = inputAction.FindPropertyRelative("axisConstraint");
                inputActionConstraint.intValue = 0;
                inputActionDescription.stringValue = $"New Action {inputActionId.intValue = list.arraySize}";
            }

            GUILayout.Space(12f);

            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            var labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 36f;
            EditorGUILayout.LabelField(ActionContent, GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField(AxisConstraintContent, list.arraySize > 7 ? GUILayout.Width(112f) : GUILayout.Width(96f));
            EditorGUILayout.LabelField(string.Empty, GUILayout.Width(24f));
            EditorGUIUtility.labelWidth = labelWidth;
            GUILayout.EndHorizontal();

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            for (int i = 0; i < list.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();
                var previousLabelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 64f;
                SerializedProperty inputAction = list.GetArrayElementAtIndex(i);
                SerializedProperty inputActionDescription = inputAction.FindPropertyRelative("description");
                var inputActionConstraint = inputAction.FindPropertyRelative("axisConstraint");
                EditorGUILayout.PropertyField(inputActionDescription, GUIContent.none);
                EditorGUILayout.PropertyField(inputActionConstraint, GUIContent.none, GUILayout.Width(96f));
                EditorGUIUtility.labelWidth = previousLabelWidth;

                if (GUILayout.Button(MinusButtonContent, EditorStyles.miniButtonRight, GUILayout.Width(24f)))
                {
                    list.DeleteArrayElementAtIndex(i);
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.EndVertical();
        }
    }
}
