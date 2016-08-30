using System;
using System.Reflection;
using BehaviorLibrary;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class AIPreviewWindow : EditorWindow
{
    public static BehaviorTree Behavior;
    private static List<BehaviorNode> nodes = new List<BehaviorNode>();

    private static float x;
    private static float y;

    public const float CanvasSize = 50000;
    public static Color GridMinorColor = new Color(0,0,0, 0.18f);
    public static Color GridMajorColor = new Color(0, 0, 0, 0.28f);
    public static Rect GraphRect;
    public static Rect ViewRect = new Rect(0,0,50000,50000);

    private static Rect History;
    private static Rect SelectedNode;

    // Selected Node Properties
    private Type type;
    private FieldInfo[] fields;

    public static void Launch(BehaviorTree behaviour)
    {
        GraphRect = new Rect(0, 0, CanvasSize, CanvasSize);
        nodes.Clear();
        x = 100;
        y = 60;
        Behavior = behaviour;
        Behavior.BehaviorTreeUpdated = Launch;
        GetWindow<AIPreviewWindow>().titleContent.text = "Behavior Tree Previewer";
        GetNodes();
    }

    private static void GetNodes()
    {
        Debug.Log("Getting Nodes");
        nodes.Clear();
        BehaviorNode node = new BehaviorNode(Behavior.Root, new Vector2(x, y));
        nodes.Add(node);
        GetBehaviorChildNodes(Behavior.Root, node);
    }

    private static void GetBehaviorChildNodes(BehaviorComponent behaviorComponent, BehaviorNode topNode)
    {
        y = nodes[0].Position.y;
        for (int i = 0; i < nodes.Count; i++)
        {
            if (y < nodes[i].Position.y)
            {
                y = nodes[i].Position.y;
            }
        }
        if (y != topNode.Position.y)
        {
            y = y + 35;
        }

        topNode.Position = new Vector2(topNode.Position.x, y);
        x += 200;
        if (behaviorComponent.Behaviors != null)
        {
            float _y = y;
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Position == new Vector2(x, y))
                {
                    _y = _y + 35;
                    topNode.Position = new Vector2(topNode.Position.x, _y);
                }
            }
            BehaviorNode previousNode = null;
            for (int i = 0; i < behaviorComponent.Behaviors.Length; i++)
            {
                BehaviorComponent behavior = behaviorComponent.Behaviors[i];
                BehaviorNode node = new BehaviorNode(behavior, new Vector2(x, (_y + (35 * i))));
                if (previousNode != null)
                {
                    node.Position = new Vector2(x, previousNode.Position.y + 35);
                }
                node.BehaviorComponent = behavior;
                node.TopNode = topNode;
                nodes.Add(node);

                if (behavior.Behaviors != null)
                {
                    GetBehaviorChildNodes(behavior, node);
                }
                previousNode = node;
            }
        }
        x -= 200;
    }

    void OnGUI()
    {
        DrawGraphGUI();

        BeginWindows();
        for (int i = 0; i < nodes.Count; i++)
        {
            if (!Behavior.History.Contains(nodes[i].BehaviorComponent))
            {
                nodes[i].OnGUI();
            }

        }

        if (Behavior != null && Behavior.UpdateAIPreviewer)
        {
            History = GUILayout.Window(1, new Rect(position.width - (10 + History.width), 10, 150, 10), DrawHistoryWindow, "Behavior Tree History");

            if (BehaviorNode.Selection != null)
            {
                //TODO: make this public static so selected BehaviorNode can call it.
                GetSelectedNodeFields();
                SelectedNode = GUILayout.Window(2, new Rect(10, 10, 150, 10), DrawNodeSelected, BehaviorNode.Selection.BehaviorComponent.Name);
            }

            for ( int i = 0; i < Behavior.History.Count; i++ )
            {
                Behavior.History[ i ].Node.OnHistoryGUI();
            }
        }
        GUI.changed = false;

        wantsMouseMove = BehaviorNode.Selection != null;

        // If we have a selection, we're doing an operation which requires an update each mouse move
        switch (Event.current.type)
        {
            case EventType.mouseUp:
                // If we had a mouse up event which was not handled by the nodes, clear our selection
                BehaviorNode.Selection = null;
                Event.current.Use();
                break;
            case EventType.mouseDown:
                //if (Event.current.clickCount == 2)
                //// If we double-click and no node handles the event, create a new node there
                //{
                //    Node.Selection = new Node("Node " + nodes.Count, Event.current.mousePosition);
                //    nodes.Add(Node.Selection);
                //    Event.current.Use();
                //}
                break;
            case EventType.MouseDrag:
                DragNodes();
                break;
        }

        EndWindows();

        if (GUI.changed)
        {
            Repaint();
        }
    }

    void Update()
    {
        if (EditorApplication.isPlaying)
        {
            Repaint();
        }
    }

    private void DrawHistoryWindow(int id)
    {
        GUILayout.BeginVertical();
        for (int i = 0; i < Behavior.History.Count; i++)
        {
            string prfx = "";
            switch (Behavior.History[i].Status)
            {
                case Status.Running:
                    prfx = "<color=blue>R</color> ";
                    break;
                case Status.Success:
                    prfx = "<color=green>S</color> ";
                    break;
                case Status.Failure:
                    prfx = "<color=red>F</color> ";
                    break;
            }
            GUIStyle style = new GUIStyle();
            style.fontStyle = FontStyle.BoldAndItalic;
            GUILayout.Label(prfx + Behavior.History[i].Name, style);
        }
        GUILayout.EndVertical();
    }

    private void GetSelectedNodeFields()
    {
        type = BehaviorNode.Selection.BehaviorComponent.GetType();
        fields = type.GetFields();
    }

    private void DrawNodeSelected(int id)
    {
        BehaviorComponent component = BehaviorNode.Selection.BehaviorComponent;

        GUILayout.BeginVertical();
        GUIStyle style = new GUIStyle();
        style.fontStyle = FontStyle.Bold;
        GUILayout.Label("Behavior Type: " + component.GetType().Name, style);

        if (component.GetType() == typeof(ConditionalLambda))
        {
            ConditionalLambda conditional = ((ConditionalLambda)component);

            if (conditional.TestAction != null)
            {
                conditional.ExpressionObjects.Clear();
                conditional.TestAction.Invoke(conditional);

                GUILayout.Label("Conditional Description: " + conditional.ConditionalDescription);
                if (conditional.ExpressionObjects != null)
                {
                    for (int i = 0; i < conditional.ExpressionObjects.Count; i++)
                    {
                        GUILayout.Label("Conditional Object Type: " + conditional.ExpressionObjects[i].GetType().Name);
                        GUILayout.Label("Conditional Object Value: " + conditional.ExpressionObjects[i]);
                    }
                }

                GUILayout.Label("Conditional Result: " + conditional.Result);
            }
            else
            {
                GUILayout.Label("Conditional Result: " + conditional.Result);
                GUILayout.Label("Conditional Body: " + conditional.Expression);
                GUILayout.Label("Conditional Not Using Action Constructor for Debugging");
            }

        }
        else
        {
            foreach (FieldInfo field in fields)
            {
                string name = field.Name;
                object value = field.GetValue(component);

                if (value != null && field.Name != "Node" && field.Name != "NotifyOnExecute" && field.Name != "AddToHistory" && field.Name != "Action")
                {
                    GUILayout.Label("Field: " + name);
                    GUILayout.Label("Value: " + value);
                }

            }
        }
        component.NotifyOnExecute = GUILayout.Toggle(component.NotifyOnExecute, "Notify On Execute");
        GUILayout.EndVertical();
    }

    private void DragNodes()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            Vector2 pos = new Vector2(nodes[i].Position.x, nodes[i].Position.y);
            pos += Event.current.delta;
            nodes[i].Position = pos;
        }
        Event.current.Use();
    }

    private void DrawGraphGUI()
    {
        if (Event.current.type == EventType.repaint)
        {
            UnityEditor.Graphs.Styles.graphBackground.Draw(GraphRect, false, false, false, false);
            DrawGrid();
        }
    }

    private void DrawGrid()
    {
        if (Event.current.type != EventType.repaint)
        {
            return;
        }

        GL.PushMatrix();
        GL.Begin(1);
        DrawGridLines(10.0f, GridMinorColor);
        DrawGridLines(100.0f, GridMajorColor);
        GL.End();
        GL.PopMatrix();
    }

    private void DrawGridLines(float gridSize, Color gridColor)
    {
        GL.Color(gridColor);
        for (float i = GraphRect.x + gridSize; i < GraphRect.x + GraphRect.width; i = i + gridSize)
        {
            DrawLine(new Vector2(i, GraphRect.y), new Vector2(i, GraphRect.y + GraphRect.height));
        }
        for (float j = GraphRect.y + gridSize; j < GraphRect.y + GraphRect.height; j = j + gridSize)
        {
            DrawLine(new Vector2(GraphRect.x, j), new Vector2(GraphRect.x + GraphRect.width, j));
        }
    }

    private void DrawLine(Vector2 p1, Vector2 p2)
    {
        GL.Vertex(p1);
        GL.Vertex(p2);
    }

    public void OnDestroy()
    {
        if (Behavior.BehaviorTreeUpdated != null)
        {
            Behavior.BehaviorTreeUpdated -= Launch;
        }
    }
}
