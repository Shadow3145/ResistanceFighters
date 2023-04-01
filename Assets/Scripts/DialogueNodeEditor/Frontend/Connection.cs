using System;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Connection
{
    public ConnectionKnob inKnob;
    public ConnectionKnob outKnob;

    public int id;

    public Action<Connection> RemoveConnection;

    public Connection(int id, ConnectionKnob inKnob, ConnectionKnob outKnob, Action<Connection> RemoveConnection)
    {
        this.id = id;
        this.inKnob = inKnob;
        this.outKnob = outKnob;

        this.RemoveConnection = RemoveConnection;
    }

#if UNITY_EDITOR
    public Connection DrawConnection()
    {
        Handles.DrawBezier(inKnob.rect.center, outKnob.rect.center,
            inKnob.rect.center + Vector2.left * 50f, outKnob.rect.center - Vector2.left * 50f,
            Color.white, null, 2f);

        if (Handles.Button((inKnob.rect.center + outKnob.rect.center) * 0.5f, Quaternion.identity, 4, 8, Handles.RectangleHandleCap))
        {
            return this;
        }

        return null;
    }

    public void DeleteConnection()
    {
        RemoveConnection(this);
    }
#endif
}
