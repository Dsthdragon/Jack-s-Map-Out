using UnityEngine;
using System.Collections;

public class barrel : MonoBehaviour, IObject {

    public ParticleSystem particles;
    public TextMesh remaining;
    public int refillCount = 3;
    GameObject player;
	
    // Use this for initialization
	void Start () {
        particles.Stop(true);
        remaining.text = "";
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	public void Refill () {
        if(refillCount > 0)
        {
            particles.Play(true);
            lamp.instance.oilLevel = lamp.instance.oilFull;
            refillCount -= 1;
            remaining.text = "0"+refillCount.ToString();
        } else
        {
            remaining.text = "NO REFILLS";
        }
        Invoke("ClearText", 3);
    }

    public void Action()
    {
        Refill();
    }


    public void ClearText()
    {
        remaining.text = "";
    }

    public void Update()
    {
        Vector3 mainCamLoc = player.transform.position - remaining.transform.position;

        mainCamLoc.x = mainCamLoc.z = 0.0f;
        remaining.transform.LookAt(player.transform.position - mainCamLoc);
        remaining.transform.Rotate(0,180,0);
        //remaining.transform.Rotate(mainCam.rotation.eulerAngles);
    }

}
