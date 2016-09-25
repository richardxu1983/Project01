using UnityEngine;
using System.Collections;

public class GuiComponent : MonoBehaviour {

    public GUISkin guiSkin; // choose a guiStyle (Important!)

    public string text = "Player Name"; // choose your name
    public Color color = Color.white;   // choose font color/size

    public int fontSize = 10;
    //public float offsetX = 0;
    public float offsetY = 0.5f;

    //float boxW = 150f;
    //float boxH = 20f;
    Vector3 vec;
    Rect rect;
    Vector2 content;
    public bool messagePermanent = true;
    public float messageDuration { get; set; }
    Vector2 boxPosition;

    void Start()
    {
        if (messagePermanent)
        {
            messageDuration = 1;
        }

        vec = new Vector3(0f,0f,0f);
        rect = new Rect();
        content = new Vector2();
    }

    void OnGUI()
    {
        if (messageDuration > 0)
        {
            if (!messagePermanent) // if you set this to false, you can simply use this script as a popup messenger, just set messageDuration to a value above 0
            {
                messageDuration -= Time.deltaTime;
            }

            GUI.skin = guiSkin;

            vec.Set(transform.position.x, transform.position.y, transform.position.z - offsetY * gameObject.GetComponent<MeshFilter>().mesh.bounds.size.y);
            //print(gameObject.GetComponent<MeshFilter>().mesh.bounds.size + " , " +transform.position+" , "+ vec);

            boxPosition = Camera.main.WorldToScreenPoint(vec);
            boxPosition.y = Screen.height - boxPosition.y;
            boxPosition.x -= 50 * 0.5f;
            //boxPosition.y -= boxH * 0.5f;
            guiSkin.box.fontSize = fontSize;

            GUI.contentColor = color;

            content = (guiSkin.box.CalcSize(new GUIContent(text)));

            rect.Set(boxPosition.x, boxPosition.y, content.x, content.y);

            GUI.Box(rect, text);
        }
    }
}
