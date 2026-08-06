using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class CanvasActiveShifter : MonoBehaviour
{
    [SerializeField] private Canvas controlCanvas;
    public Canvas ControlCanvas { get => controlCanvas; }
    void OnEnable()
    {
        if(controlCanvas == null) controlCanvas = GetComponent<Canvas>();
    }

    public void ShiftActivate()
    {
        if(controlCanvas!=null)
        {
            controlCanvas.enabled = !controlCanvas.enabled;
        }
    }
}
