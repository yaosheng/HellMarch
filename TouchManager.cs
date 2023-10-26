using UnityEngine;
using System.Collections;

public class TouchManager : MonoBehaviour {

    private Camera mainCamera;
    public Vector2 screentoWorld;
    public SpriteRenderer[] heroSprites;
    private float XPositiuon, YPosition = -7.5f;
    //private bool isheroRun = false, isHited = false;

    public static bool isFinished = false;
    public static bool isCreated = false;
    public Transform heroRoot;
    public static int line = 0;
    private SpriteRenderer sr;
    
    void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        isFinished = true;
    }

    void Update()
    {
        CreateHero();
    }

    public void CreateHero()
    {
        if (Input.GetButton("Fire1"))
        {
            screentoWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            if (isCreated == false && isFinished == true)
            {
                sr = Instantiate(heroSprites[UnityEngine.Random.Range(0, heroSprites.Length)]) as SpriteRenderer;
                sr.transform.parent = heroRoot;
                isCreated = true;
                isFinished = false;
            }
            if(isCreated == true)
            {
                SetPosition(screentoWorld.x, sr);
            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            isCreated = false;
            Debug.Log("isFinished : " + isFinished + " , " + "isCreated : " + isCreated);
        }
    }
    public void SetPosition(float pos, SpriteRenderer sr)
    {
        if (pos > 3.2f)
        {
            line = 5;
            sr.transform.localPosition = new Vector2(4.0f, YPosition);
        }
        else if (pos > 1.6f && pos <= 3.2f)
        {
            line = 4;
            sr.transform.localPosition = new Vector2(2.5f, YPosition);
        }
        else if (pos > 0 && pos <= 1.6f)
        {
            line = 3;
            sr.transform.localPosition = new Vector2(0.8f, YPosition);
        }
        else if (pos <= 0 && pos > -1.6f)
        {
            line = 2;
            sr.transform.localPosition = new Vector2(-0.8f, YPosition);
        }
        else if (pos <= -1.6f && pos > -3.2f)
        {
            line = 1;
            sr.transform.localPosition = new Vector2(-2.5f, YPosition);
        }
        else if (pos <= -3.2f)
        {
            line = 0;
            sr.transform.localPosition = new Vector2(-4.0f, YPosition);
        }
    }

}
