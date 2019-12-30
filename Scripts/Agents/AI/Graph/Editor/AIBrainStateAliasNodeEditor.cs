﻿using System.Linq;
using Boo.Lang;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace TheBitCave.MMToolsExtensions.AI.Graph
{
    [CustomNodeEditor(typeof(AIBrainStateAliasNode))]
    public class AIBrainStateAliasNodeEditor : NodeEditor
    {
        private SerializedProperty _statesIn;

        protected AIBrainStateAliasNode _node;

        protected int _stateIndex = 0;
        
        public override void OnCreate()
        {
            base.OnCreate();
            _node = target as AIBrainStateAliasNode;
        }

        public override void OnHeaderGUI()
        {
            _node.name = _node.stateName;
            GUILayout.Label(_node.stateName, NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }
        
        public override void OnBodyGUI()
        {
            _statesIn = serializedObject.FindProperty("statesIn");

            if (Selection.activeObject == _node)
            {
                var optionsList = new List<string>();
                foreach (var node in _node.graph.nodes.OfType<AIBrainStateNode>())
                {
                    optionsList.Add(node.name);
                }
                foreach (var node in _node.graph.nodes.OfType<AIBrainSubgraphNode>())
                {
                    foreach (var stateName in node.inputStates.Select(inputState => GeneratorUtils.GetSubgraphStateName(node.name, inputState.fieldName)))
                    {
                        optionsList.Add(stateName);
                    }
                }

                var options = optionsList.ToArray();
                _stateIndex = EditorGUILayout.Popup(_stateIndex, options);

                EditorGUILayout.Space();
                _node.stateName = options[_stateIndex];
            }

            serializedObject.Update();
            NodeEditorGUILayout.PropertyField(_statesIn);
            serializedObject.ApplyModifiedProperties();
        }
    }
}