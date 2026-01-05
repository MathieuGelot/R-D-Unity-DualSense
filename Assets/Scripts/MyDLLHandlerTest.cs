using UnityEngine;

public class MyDLLHandlerTest : MonoBehaviour
{
    MyDLL_Handler myDll;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myDll = new MyDLL_Handler();
    }

    // Update is called once per frame
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
