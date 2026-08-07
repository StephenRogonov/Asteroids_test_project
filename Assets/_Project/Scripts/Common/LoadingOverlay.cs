using UnityEngine;

public class LoadingOverlay : MonoBehaviour
{
    public void EnableObject()
    {
        gameObject.SetActive(true);
    }

    public void DisableObject()
    {
        gameObject.SetActive(false);
    }
}
