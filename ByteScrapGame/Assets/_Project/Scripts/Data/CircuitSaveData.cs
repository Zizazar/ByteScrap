using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CircuitData
{
    public List<ComponentData> components = new List<ComponentData>();
    public List<ConnectionData> connections = new List<ConnectionData>();
}

[Serializable]
public class ComponentData
{
    public string id;
    public string type;
    public Vector3 position;
    public Vector3 rotation;
}

[Serializable]
public class PinData
{
    public string id;
    public string componentId;
    public bool isInput;
}

[Serializable]
public class ConnectionData
{
    public PinData startPin;
    public PinData endPin;
}