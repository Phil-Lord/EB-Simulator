using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(CompoundRule))]
public class CompositeRuleEditor : Editor
{
    /// <summary>
    /// Custom editor GUI for altering compound rule rules and weights.
    /// </summary>
    /// <see cref="https://gist.github.com/RapidR3D/b245f7b72f476f733d751ecc092d44e2">Code from Github</see>
    public override void OnInspectorGUI()
    {
        // Setup inspector
        CompoundRule cr = (CompoundRule)target;

        // Check for rules
        if (cr.rules == null || cr.rules.Length == 0)
        {
            // Output 'no rules' message
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.HelpBox("No rules in array.", MessageType.Warning);
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            // 'Rules' and 'Weights' column titles
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Rules", GUILayout.MinWidth(60f), GUILayout.MaxWidth(290f));
            EditorGUILayout.LabelField("Weights", GUILayout.MinWidth(65f), GUILayout.MaxWidth(65f));
            EditorGUILayout.EndHorizontal();
            EditorGUI.BeginChangeCheck();

            // List each rule and weight
            for (int i = 0; i < cr.rules.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(i.ToString(), GUILayout.MinWidth(20f), GUILayout.MaxWidth(20f));
                cr.rules[i] = (GroupRule)EditorGUILayout.ObjectField(cr.rules[i], typeof(GroupRule), false, GUILayout.MinWidth(20f));
                cr.weights[i] = EditorGUILayout.FloatField(cr.weights[i], GUILayout.MinWidth(60f), GUILayout.MaxWidth(60f));
                EditorGUILayout.EndHorizontal();
            }

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(target);
                GUIUtility.ExitGUI();
            }
        }

        // "Add" and "Remove" rules buttons
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Add Rule"))
        {
            AddRule(cr);
            GUIUtility.ExitGUI();
        }

        if (cr.rules != null && cr.rules.Length > 0)
        {
            if(GUILayout.Button("Remove Rule"))
            {
                RemoveRule(cr);
                GUIUtility.ExitGUI();
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// Add a new empty rule with a weight of 1.
    /// </summary>
    /// <param name="cr">Compound rule</param>
    void AddRule(CompoundRule cr)
    {
        // Create rule and weight arrays with:
        // length = amount of pre-existing rules or weights + 1
        int oldCount = (cr.rules != null) ? cr.rules.Length : 0;
        GroupRule[] newRules = new GroupRule[oldCount + 1];
        float[] newWeights = new float[oldCount + 1];

        // Fill arrays with pre-existing rules and weights
        for (int i = 0; i < oldCount; i++)
        {
            newRules[i] = cr.rules[i];
            newWeights[i] = cr.weights[i];
        }

        // Reset compound rules with empty last element
        cr.rules = newRules;
        
        // Reset weights with new weight = 1
        newWeights[oldCount] = 1f;
        cr.weights = newWeights;
    }

    /// <summary>
    /// Remove the last rule of a compound rule.
    /// </summary>
    /// <param name="cr">Compound rule</param>
    void RemoveRule(CompoundRule cr)
    {
        int oldCount = cr.rules.Length;

        // If only one rule, set rules and weights to null
        if (oldCount == 1)
        {
            cr.rules = null;
            cr.weights = null;
            return;
        }

        // Remove last rule
        GroupRule[] newRules = new GroupRule[oldCount - 1];
        float[] newWeights = new float[oldCount - 1];

        for (int i = 0; i < oldCount - 1; i++)
        {
            newRules[i] = cr.rules[i];
            newWeights[i] = cr.weights[i];
        }

        cr.rules = newRules;
        cr.weights = newWeights;
    }
}
#endif