using UnityEngine;

public class WrapperDS5WTest : MonoBehaviour
{
    WrapperDS5W_Handler wrapperDS5W;

    void Start()
    {
        wrapperDS5W = new WrapperDS5W_Handler();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        wrapperDS5W?.Dispose();
    }
}
