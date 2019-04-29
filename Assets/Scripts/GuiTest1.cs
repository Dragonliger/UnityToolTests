using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

enum CopyOrder{
    InPlace = 0,
    Straight = 1,
    Circular = 2,
};

public class GuiTest1 : EditorWindow
{
    int Copies = 1;
    float Separation = 20.0f;
    Vector3 Direction = Vector3.right;
    CopyOrder Arrangement = CopyOrder.InPlace;
    bool UseAsPivot = false;
    float Radius = 2.0f;

    //Add to the unity menu
    [MenuItem ("Window/GuiTest1")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(GuiTest1));
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Advanced Copy", EditorStyles.boldLabel);
        Copies = EditorGUILayout.IntField("Copies: ", Copies);
        Arrangement = (CopyOrder)EditorGUILayout.EnumPopup("Copy Arrange: ", Arrangement);
        switch (Arrangement)
        {
            case CopyOrder.Straight:
            EditorGUILayout.LabelField("Separation between objects: ");
            Separation = EditorGUILayout.Slider(Separation, 0, 1000);
            Direction = EditorGUILayout.Vector3Field("Direction: ", Direction);
                break;
            case CopyOrder.Circular:
            UseAsPivot = EditorGUILayout.Toggle("Use the object as center: ", UseAsPivot);
            Radius = EditorGUILayout.FloatField("Radius: ", Radius);
                break;

        }
        if(GUILayout.Button("Click to copy selection"))
        {
            GameObject obj = Selection.activeGameObject;
            if (obj != null)
                switch (Arrangement)
                {
                    case CopyOrder.InPlace:
                        IterateCopy(obj, Copies);
                        break;
                    case CopyOrder.Straight:
                        IterateCopyLinear(obj, Copies, Separation, Direction);
                        break;
                    case CopyOrder.Circular:
                        IterateCopyCircular(obj, Copies, Radius, Direction, UseAsPivot);
                        break;
                }
            else
                Debug.LogWarning("Please select an object to copy", this);
        }
    }

    static void SelectObject()
    {
        GameObject obj = Selection.activeGameObject;
        if(obj != null)
            Debug.Log(obj.name + " is selected");
    }

    private void IterateCopyLinear(GameObject ObjectToCopy, int Times, float separation, Vector3 direction)
    {
        direction = Vector3.ClampMagnitude(direction, 1);
        Vector3 NewElementPostion = ObjectToCopy.transform.position;
        for (int i = 1; i <= Times; i++)
        {
            Renderer Ruler = ObjectToCopy.GetComponent<Renderer>();
            if(Ruler != null)
            {
                NewElementPostion = NewElementPostion + Vector3.Scale(direction, (Ruler.bounds.size / 2)) + (Separation * direction);
            }
            else
                NewElementPostion = NewElementPostion + (Separation * direction);
            CopyObject(ObjectToCopy, NewElementPostion, ObjectToCopy.transform.rotation);
        }
    }

    private void IterateCopyCircular(GameObject objectToCopy, int times, float radius,  Vector3 direction, bool useAsPivot)
    {
        direction = Vector3.ClampMagnitude(direction, 1);
        Vector3 NewElementPostion = objectToCopy.transform.position;
        Vector3 NormalVector = Vector3.Cross(objectToCopy.transform.position, Center);
        if (!useAsPivot)
        {
            Vector3 Center = direction * radius;
            for(int i = 1; i < times; i++)
            {

            }
        }
    }

    private void IterateCopy(GameObject ObjectToCopy, int Times)
    {
        for(int i = 0; i < Times; i++)
            CopyObject(ObjectToCopy, ObjectToCopy.transform.position, ObjectToCopy.transform.rotation);
    }

    private void CopyObject(GameObject ObjectToCopy, Vector3 position, Quaternion rotation)
    {
        Instantiate(ObjectToCopy, position, rotation);
    }

    private Vector3 getPointFromNormal(Vector3 normal, float degree)
    {
        return new Vector3();
    }
}
