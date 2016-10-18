using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

namespace UI.Tables
{
    [CustomEditor(typeof(TableRow))]
    public class TableRowEditor : Editor
    {
        private SerializedObject SO_Target;
        private TableRow TableRow;

        private SerializedProperty preferredHeight;
        private SerializedProperty dontUseTableRowBackground;
              

        public void OnEnable()
        {
            SO_Target = new SerializedObject(target);
            TableRow = (TableRow)target;

            preferredHeight = SO_Target.FindProperty("preferredHeight");
            dontUseTableRowBackground = SO_Target.FindProperty("dontUseTableRowBackground");
            
        }

        public void OnDisable()
        {
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(preferredHeight);
            EditorGUILayout.PropertyField(dontUseTableRowBackground);                        

            if (EditorGUI.EndChangeCheck())
            {                                
                SO_Target.ApplyModifiedProperties();

                TableRow.preferredHeight = preferredHeight.floatValue;
                TableRow.dontUseTableRowBackground = dontUseTableRowBackground.boolValue;                                

                Repaint();           
            }

            if (GUILayout.Button("Add Cell"))
            {
                TableRow.AddCell();
            }

            TableRow.NotifyTableRowPropertiesChanged();
        }
    }
}
