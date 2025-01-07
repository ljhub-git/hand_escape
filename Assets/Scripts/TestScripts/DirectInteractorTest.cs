using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DirectInteractorTest : MonoBehaviour
{
    public void OnSelectEntered(SelectEnterEventArgs _event)
    {
        Debug.Log("Hello~~~");
    }
}
