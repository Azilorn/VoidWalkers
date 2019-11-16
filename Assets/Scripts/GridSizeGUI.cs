using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSizeGUI : MonoBehaviour
{
    [SerializeField] int width, height;

    public int Width { get => width; set => width = value; }
    public int Width1 { get => width; set => width = value; }

    // Start is called before the first frame update
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, new Vector3(Width, height));
    }
}
