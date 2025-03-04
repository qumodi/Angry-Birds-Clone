using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using Unity.VisualScripting;
using TouchPhase = UnityEngine.TouchPhase;

public class SlingshotHandler : MonoBehaviour
{
    [SerializeField] private SlingshotArea area;
    [SerializeField] private CameraManager CamManager;
    [SerializeField] private GameObject RedPrefab;
    [SerializeField] private GameObject CurrBird;
    [SerializeField] private int CurrBirdIdx = -1;
    [SerializeField] private Trajectory SlingshotTrajectory;

    [SerializeField] private LineRenderer LeftLineRenderer;
    [SerializeField] private LineRenderer RightLineRenderer;

    [SerializeField] private Transform LeftString;
    [SerializeField] private Transform RightString;
    [SerializeField] private Transform SlingshotCenter;
    [SerializeField] private Transform SlingshotIdle;
    [SerializeField] private Transform Elastic;

    [SerializeField] private AnimationCurve SlingshotCurve;

    [SerializeField] private AudioClip Stretching;
    [SerializeField] private AudioClip Releasing;
    [SerializeField] private AudioSource SoundSource;

    private Vector3 LinesPoint;
    public Vector2 direction;
    //public Vector2 directionNorm;
    private float dirLength = 3f;
    public float shotStrength = 5f;
    bool pressed = false;
    bool pressedThisFrame = false;
    bool releasedThisFrame = false;
    public float slingshotSpeed;
    public float elasticDivider = 1f;

    public float birdMoveSpeed = 0.7f;
    public float slingshotElasticSpeed = 0.5f;
    private void Awake()
    {
        SoundSource = GetComponent<AudioSource>();

    }
    void Start()
    {
        SetLines(SlingshotIdle.position);

        SpawnBird();

    }

    // Update is called once per frame
    void Update()
    {
        Touch touch = new Touch();
        if(Input.touches.Length > 0)
        {
            touch = Input.touches[0];
        }
        //Touch touch = Input.touches[0];

        if(GameManager.GameInputType == InputType.Mobile)
        {
            pressedThisFrame = touch.phase == TouchPhase.Began;
            releasedThisFrame = touch.phase == TouchPhase.Ended;
        }
        else if (GameManager.GameInputType == InputType.PC)
        {
            pressedThisFrame = Mouse.current.leftButton.wasPressedThisFrame;
            releasedThisFrame = Mouse.current.leftButton.wasReleasedThisFrame;
        }
        

        //Debug.Log(CurrBird.GetComponent<Rigidbody2D>().velocity.magnitude);
        if (pressedThisFrame && area.InSlingshotArea())
        {
            pressed = true;
            SoundManager.Instance.PlayClip(Stretching, SoundSource);
            CamManager.SwitchToFollow(CurrBird.transform);

            SlingshotTrajectory.Show();
        }
        else if (releasedThisFrame && pressed)
        {
            pressed = false;

            if (CurrBird.GetComponent<Bird>().OnSlingshot && GameManager.Instance.CanShoot())
            {
                CurrBird.GetComponent<Bird>().LaunchBird(direction, shotStrength);
                CurrBird.GetComponent<Bird>().PlayLaunchSound();
                GameManager.Instance.UseShot();
                
                if (GameManager.Instance.CanShoot())
                {
                    StartCoroutine(nameof(SpawnBirdAfterTime), 1f);
                }

                AnimateElastic();
                //SoundManager.Instance.PlayClip(Releasing, SoundSource);
                SlingshotTrajectory.Hide();

            }

        }

        //Mouse.current.leftButton.IsPressed()
        if (pressed)
        {
            if (CurrBird.GetComponent<Bird>().OnSlingshot) {
                DrawSlingshot();
                SetBirdPosition(LinesPoint);

                //SlingshotTrajectory.UpdateDots(SlingshotCenter.position, direction * shotStrength );
                SlingshotTrajectory.UpdateDots(LinesPoint, direction * shotStrength );

            }

        }
        else
        {

            //SetLines(SlingshotIdle.position);
        }
    }

    void DrawSlingshot()
    {

        Vector3 touch = new Vector3() ;
        if (GameManager.GameInputType == InputType.Mobile) {
            if (Input.touches.Length > 0)
            {
                touch = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            }
        }
        else if (GameManager.GameInputType == InputType.PC) {
            touch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else
        {
            touch = new Vector3(-11, -11, -11);
        }
        Vector3 dir;

        //Debug.Log("Vec : "+Vector2.Distance(touch, SlingshotCenter.position));
        if (Mathf.Abs(Vector2.Distance(touch, SlingshotCenter.position)) >= dirLength)//
        {
            dir = Vector2.ClampMagnitude((touch - SlingshotCenter.position), dirLength);
            dir = dir + SlingshotCenter.position;
            //dir = SlingshotCenter.position + new Vector3(Mathf.Clamp(touch), dirLength)
            //Debug.Log("Clamp" + Vector2.ClampMagnitude((touch - SlingshotCenter.position), dirLength));
        }
        else
        {
            dir = touch;
        }

        //Debug.Log("Dir : " + dir);
        SetLines(dir);
        LinesPoint = dir;
        direction = SlingshotCenter.position - dir;
    }

    void SetLines(Vector2 pos)
    {
        LeftLineRenderer.SetPosition(0, LeftString.position);
        RightLineRenderer.SetPosition(0, RightString.position);

        LeftLineRenderer.SetPosition(1, pos);
        RightLineRenderer.SetPosition(1, pos);

    }

    private void SpawnBird()
    {
        CurrBirdIdx++;

        CurrBird = GameManager.Instance.Birds[CurrBirdIdx].gameObject;
        CurrBird.GetComponent<Bird>().OnSlingshot = true;
        StartCoroutine(BezierManager.Instance.MoveToSlingshot(CurrBird.transform, 10, birdMoveSpeed));
        //CurrBird = Instantiate(GameManager.Instance.Birds[CurrBirdIdx], SlingshotIdle.position, Quaternion.identity).gameObject; - Old method
        //CurrBird = Instantiate(RedPrefab, SlingshotIdle.position, Quaternion.identity);

    }

    private void SetBirdPosition(Vector2 pos)
    {
        CurrBird.transform.position = pos;
        CurrBird.transform.right = direction;
    }

    private IEnumerator SpawnBirdAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        SpawnBird();

        //CamManager.SwitchToIdle();
    }

    private void AnimateElastic() {
        Elastic.position = LeftLineRenderer.GetPosition(1);
        float dist = Vector2.Distance(Elastic.position,SlingshotIdle.position);
        float time = dist / elasticDivider;
        Debug.Log("time : " + time);
        //Elastic.transform.DOMove(SlingshotIdle.position,time).SetEase(SlingshotCurve);
        StartCoroutine(AnimateElasticLines(Elastic, slingshotElasticSpeed));
    }

    private IEnumerator AnimateElasticLines(Transform trans,float time)
    {
        float elapsedTime = 0f;
        Vector2 norm = (SlingshotIdle.position - Elastic.position);

        while (elapsedTime < time) { 
            elapsedTime += Time.deltaTime;
            float progres = SlingshotCurve.Evaluate(elapsedTime);
            Vector2 newPos = Vector2.LerpUnclamped(Elastic.position, SlingshotIdle.position, progres);
            
            SetLines(newPos);
            yield return null;
        }
    }
}

