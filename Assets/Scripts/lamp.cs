using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class lamp : MonoBehaviour {
    public float oilFull = 100.0f;
    public float oilLevel;
    public float lightIntesity;
    public float oilDepletion = 5f;
    float maxParticles;
    Vector3 fireSize;
    public Light _light;
    public Slider _slider;
    public ParticleSystem _particle;
    public static lamp instance;
    public float delay = 3;
    public AudioSource backgroundMusic;

	// Use this for initialization
	void Start () {
        lightIntesity = _light.intensity;
        oilLevel = oilFull;
        maxParticles = _particle.maxParticles;
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        } else
        {
            instance = this;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if(_light.intensity > 0)
        {
            oilLevel -= Time.deltaTime * oilDepletion;
            _light.intensity = lightIntesity * (oilLevel / oilFull);
            _slider.value = 1 *_light.intensity/lightIntesity;
            _particle.maxParticles = (int)(maxParticles * (oilLevel / oilFull));
            if (_light.intensity < .3)
            {
                backgroundMusic.pitch = 1.3f;
            } else if(_light.intensity < .7)
            {
                backgroundMusic.pitch = 1.2f;
            }
            else
            {
                backgroundMusic.pitch = 1f;
            }
        } else
        {
            _particle.Stop(false);
            Invoke("GameOver", delay);
        }
	}

    void GameOver()
    {
        ProGameManger.instance.GameOver();
    }
}
