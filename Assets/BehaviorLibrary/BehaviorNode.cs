using UnityEngine;
using UnityEditor;

namespace BehaviorLibrary
{
    // TODO: Move this into BehaviorLibary.Editor
    public class BehaviorNode
    {
        public BehaviorComponent BehaviorComponent;

        const float kNodeHeight = 20.0f;
        private const float kNodeWidth = 150.0f;

        static BehaviorNode selection = null;
        static bool connecting = false;

        Vector2 position;
        Rect nodeRect;

        private Material material;

        public BehaviorNode(BehaviorComponent behaviorComponent, Vector2 position)
        {
            BehaviorComponent = behaviorComponent;
            Position = position;
            BehaviorComponent.Node = this;
        }

        public static BehaviorNode Selection
        {
            get
            {
                return selection;
            }
            set
            {
                selection = value;
                if (selection == null)
                {
                    connecting = false;
                }
            }
        }

        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;

                nodeRect = new Rect(
                    position.x - kNodeWidth * 0.5f,
                    position.y - kNodeHeight * 0.5f,
                    kNodeWidth,
                    kNodeHeight
                );
            }
        }

        public BehaviorNode TopNode;

        public void OnGUI()
        {
            switch (Event.current.type)
            {
                case EventType.mouseDown:
                    break;
                case EventType.mouseUp:
                    if (nodeRect.Contains(Event.current.mousePosition))
                    // Select this node if we clicked it
                    {
                        if ( selection != this )
                        {
                            selection = this;
                        }
                        else
                        {
                            selection = null;
                        }

                        Event.current.Use();
                    }
                    break;
                case EventType.mouseDrag:
                    //if (selection == this)
                    //// If doing a mouse drag with this component selected...
                    //{
                    //    Position += Event.current.delta;
                    //    Event.current.Use();
                    //}
                    break;
                case EventType.repaint:
                    DrawBox( nodeRect, new Color(0.65f, 0.65f, 0.65f, .65f ));
                    GUIContent content = new GUIContent( BehaviorComponent.Name );
                    GUIStyle style = new GUIStyle();
                    style.fontStyle = FontStyle.BoldAndItalic;
                    style.alignment = TextAnchor.MiddleCenter;
                    style.Draw( nodeRect, content, 0 );
                    GUI.skin.box.Draw( nodeRect, new GUIContent(), false, false, false, false );
                    break;
            }

            GUI.color = new Color(.5f, .5f, .5f, .65f);
            if (TopNode != null)
            {
                Color color = Color.gray;
                DrawConnection(TopNode.Position, Position, color);
            }
        }

        public void OnHistoryGUI()
        {
            Color color = new Color(0.05f, 0.05f, 0.05f, .65f);

            if (BehaviorComponent.Status == Status.Running)
            {
                color = new Color(0, 0, 1, .65f);
            }
            if (BehaviorComponent.Status == Status.Success)
            {
                color = new Color(0, 1, 0, .65f);
            }
            if (BehaviorComponent.Status == Status.Failure)
            {
                color = new Color(1, 0, 0, .65f);
            }

            switch (Event.current.type)
            {
                case EventType.mouseDown:
                    break;
                case EventType.mouseUp:
                    // Select this node if we clicked it
                    if ( nodeRect.Contains(Event.current.mousePosition))
                    {
                        if (selection != this)
                        {
                            selection = this;
                        }
                        else
                        {
                            selection = null;
                        }

                        Event.current.Use();
                    }
                    break;
                case EventType.mouseDrag:
                    //if (selection == this)
                    //// If doing a mouse drag with this component selected...
                    //{
                    //    if (connecting)
                    //    // ... and in connect mode, just use the event as we'll be painting the new connection
                    //    {
                    //        Event.current.Use();
                    //    }
                    //    else
                    //    // ... and not in connect mode, drag the component
                    //    {
                    //        Position += Event.current.delta;
                    //        Event.current.Use();
                    //    }
                    //}
                    break;
                case EventType.repaint:
                    DrawBox(nodeRect, color);
                    GUIContent content = new GUIContent(BehaviorComponent.Name);
                    GUIStyle style = new GUIStyle();
                    style.fontStyle = FontStyle.BoldAndItalic;
                    style.alignment = TextAnchor.MiddleCenter;
                    style.Draw(nodeRect, content, 0);
                    GUI.skin.box.Draw(nodeRect, new GUIContent(), false, false, false, false);
                    break;
            }

            GUI.color = color;
            if (TopNode != null)
            {
                color = Color.gray;
                if (TopNode.BehaviorComponent.Status == Status.Running)
                {
                    color = Color.blue;
                }
                if (TopNode.BehaviorComponent.Status == Status.Success)
                {
                    color = Color.green;
                }
                if (TopNode.BehaviorComponent.Status == Status.Failure)
                {
                    color = Color.red;
                }
                DrawConnection(TopNode.Position, Position, color);
            }
        }

        private void DrawBox(Rect rect, Color color)
        {
            if (rect.y < 0)
            {
                rect.height += rect.y;
                if (rect.height < 0)
                {
                    rect.height = 0;
                }
                rect.y = 0;
            }

            CreateMaterial();
            material.SetPass(0);

            GL.Color(color);
            GL.Begin(GL.QUADS);
            GL.Vertex3(rect.x, rect.y, 0);
            GL.Vertex3(rect.x + rect.width, rect.y, 0);
            GL.Vertex3(rect.x + rect.width, rect.y + rect.height, 0);
            GL.Vertex3(rect.x, rect.y + rect.height, 0);
            GL.End();
        }

        private void CreateMaterial()
        {
            if (material != null)
                return;

            material = new Material( "Shader \"Lines/Colored Blended\" {" +
                                     "SubShader { Pass { " +
                                     "    Blend SrcAlpha OneMinusSrcAlpha " +
                                     "    ZWrite Off Cull Off Fog { Mode Off } " +
                                     "    BindChannels {" +
                                     "      Bind \"vertex\", vertex Bind \"color\", color }" +
                                     "} } }" );
            material.hideFlags = HideFlags.HideAndDontSave;
            material.shader.hideFlags = HideFlags.HideAndDontSave;
        }

        public static void DrawConnection(Vector2 from, Vector2 to, Color color)
        {
            bool left = from.x > to.x;
            Handles.DrawBezier(
                new Vector3(from.x + (left ? -kNodeWidth : kNodeWidth) * 0.5f, from.y, 0.0f),
                new Vector3(to.x + (left ? kNodeWidth : -kNodeWidth) * 0.5f, to.y, 0.0f),
                new Vector3(from.x, from.y, 0.0f) + Vector3.right * 100.0f * (left ? -1.0f : 1.0f),
                new Vector3(to.x, to.y, 0.0f) + Vector3.right * 100.0f * (left ? 1.0f : -1.0f),
                color,
                null,
                4.0f
            );
        }
    }
}