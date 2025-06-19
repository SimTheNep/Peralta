using UnityEngine;

public enum BoxType { Light, Heavy }

public class Box : MonoBehaviour
{
    public BoxType boxType = BoxType.Light;
}
