using UnityEngine;
using System.Collections;

public class actionHandler : MonoBehaviour {
    
    void OnTriggerEnter(Collider col)
    {
        Debug.Log(col.tag);
        if(col.tag == "IObject")
        {
            col.gameObject.GetComponent<IObject>().Action();
            
        }
    }
	
}
