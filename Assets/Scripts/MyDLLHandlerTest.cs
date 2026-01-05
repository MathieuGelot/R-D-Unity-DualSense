using UnityEngine;

public class MyDLLHandlerTest : MonoBehaviour
{
    MyDLL_Handler myDll;
 
    void Start()
    {
        myDll = new MyDLL_Handler();
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.R))
        {
            Debug.Log($"Add Int : {myDll.AddInt(10, 50)}");
        }
        if (Input.GetKey(KeyCode.T))
        {
            Debug.Log($"Add Float : {myDll.AddFloat(2.5f, 3.4f)}");
        }
    }

    private void OnDestroy()
    {
        myDll?.Dispose();
    }
}
