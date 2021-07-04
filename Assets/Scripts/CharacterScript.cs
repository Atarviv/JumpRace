using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharacterScript : MonoBehaviour
    {
    int encourageAmount;
    Animator animator;
    public int numOfAnimations, lastplatform, EncourageTextSize;
    public float Forward_Speed, rotation_Speed, numOfPlatforms;
    public GameObject camera, SplashEffect, LineHitPrefab, LandIndicator;
    public Vector3 mouseOffset, lastmousePos;
    bool mousepressed, _gameRunning;
    public float BottomPos;
    public Image Bar, BarBG;
    public Text EncouragementText;
    LineRenderer line;
    RaycastHit hit;
    GameObject LineHit, closestPlatform;
    GameObject[] platforms;
    void Start()
        {
        encourageAmount = 0;
        EncourageTextSize = EncouragementText.fontSize;
        line = GetComponent<LineRenderer>();
        lastplatform = 0;
        _gameRunning = true;
        numOfAnimations = 1;
        animator = GetComponent<Animator>();
        mousepressed = false;
        rotation_Speed = 0.02f;
        for (int i = 0; i < transform.childCount; i++)
            {
            if (transform.GetChild(i).name == "Main Camera")
                camera = transform.GetChild(i).gameObject;
            }
        StartCoroutine(nameof(MouseRotation));
        }
    public IEnumerator MouseRotation()
        {
        while (_gameRunning)
            {
            if (Input.GetMouseButtonDown(0))
                {
                mousepressed = true;
                mouseOffset = Input.mousePosition;
                }
            if (Input.GetMouseButton(0))
                {
                transform.position += transform.forward * Forward_Speed * Time.deltaTime;
                if (lastmousePos != Input.mousePosition)
                    {
                    Vector3 offset = (Input.mousePosition - mouseOffset);
                    transform.Rotate(new Vector3(0, offset.x * rotation_Speed, 0));
                    }
                else
                    mouseOffset = Input.mousePosition;
                lastmousePos = Input.mousePosition;
                }
            if (Input.GetMouseButtonUp(0))
                mousepressed = false;
            yield return new WaitForSeconds(Time.deltaTime / 2);
            }
        }
    void Update()
        {
        if (GameObject.FindGameObjectsWithTag("platform").Length != 0 && platforms == null)
            {
            platforms = GameObject.FindGameObjectsWithTag("platform");
            closestPlatform = platforms[0].transform.parent.gameObject;
            }
        if (platforms != null)
            {
            for (int i = 0; i < platforms.Length - 1; i++)
                {
                if (platforms[i] != null)
                    {
                    if (closestPlatform == null)
                        {
                        closestPlatform = platforms[i].transform.parent.gameObject;
                        closestPlatform.GetComponent<PlatformScript>().closestPlatformCircle.SetActive(true);
                        closestPlatform.GetComponent<PlatformScript>().closestPlatform = true;
                        closestPlatform.GetComponent<PlatformScript>().FarestFromCharacter = Vector3.Distance(transform.position, closestPlatform.transform.position);
                        }
                    if (Vector3.Distance(transform.position, platforms[i].transform.position) < Vector3.Distance(transform.position, closestPlatform.transform.position) && closestPlatform != platforms[i].transform.parent.gameObject)
                        {
                        closestPlatform.GetComponent<PlatformScript>().closestPlatformCircle.SetActive(false);
                        closestPlatform.GetComponent<PlatformScript>().closestPlatform = false;
                        closestPlatform = platforms[i].transform.parent.gameObject;
                        closestPlatform.GetComponent<PlatformScript>().closestPlatformCircle.SetActive(true);
                        closestPlatform.GetComponent<PlatformScript>().closestPlatform = true;
                        closestPlatform.GetComponent<PlatformScript>().FarestFromCharacter = Vector3.Distance(transform.position, closestPlatform.transform.position);
                        }
                    }
                }
            }
        if (LineHit != null)
            Destroy(LineHit);
        line.SetPosition(0, transform.position);
        line.startWidth = transform.localScale.x * 0.8f;
        if (Physics.Raycast(transform.position, transform.up * -1, out hit, Mathf.Abs(transform.position.y - BottomPos)))
            {
            line.startColor = Color.green - new Color(0, 0, 0, 0.5f);
            line.SetPosition(1, hit.point + transform.forward * 0.5f);
            if (hit.transform.tag == "platform")
                line.endWidth = transform.localScale.x;
            else
                line.endWidth = transform.localScale.x * 2;
            }
        else
            {
            line.SetPosition(1, new Vector3(transform.position.x, BottomPos, transform.position.z) + transform.forward * 0.5f);
            line.startColor = Color.red - new Color(0, 0, 0, 0.5f);
            line.endWidth = transform.localScale.x * 2;
            }
        line.endColor = line.startColor;
        if (line.enabled)
            LineHit = Instantiate(LineHitPrefab, line.GetPosition(1) + transform.up * 0.01f + transform.forward * -0.5f, LineHitPrefab.transform.rotation);
        GetComponent<Rigidbody>().AddForce(transform.up * -Time.deltaTime * 9.81f);
        transform.Rotate(new Vector3(-transform.rotation.x, 0, -transform.rotation.z));
        if (transform.position.y <= BottomPos && _gameRunning)
            StartCoroutine(nameof(EndOfGame));
        }
    public IEnumerator EndOfGame()
        {
        _gameRunning = false;
        camera.transform.parent = null;
        line.enabled = false;
        Instantiate(SplashEffect, new Vector3(transform.position.x, BottomPos, transform.position.z), Quaternion.identity);
        yield return new WaitForSeconds(2);
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
        }
    private void OnCollisionEnter(Collision collision)
        {
        if (transform.position.y > collision.transform.position.y)
            {
            GameObject Indicator = Instantiate(LandIndicator, collision.transform.position, LandIndicator.transform.rotation);
            if (collision.transform.tag == "platform")
                {
                GameObject ContainsScript = null;
                if (collision.transform.parent == null) ContainsScript = collision.gameObject;
                else ContainsScript = collision.transform.parent.gameObject;
                if (!ContainsScript.GetComponent<PlatformScript>().lastPlatform)
                    {
                    ContainsScript.GetComponent<PlatformScript>().closestPlatform = false;
                    ContainsScript.GetComponent<PlatformScript>().closestPlatformCircle.SetActive(false);
                    animator.SetTrigger("Jump" + Random.Range(1, numOfAnimations + 1).ToString());
                    Indicator.GetComponent<landIndicatorScript>().StartCoroutine("Land", collision.transform.localScale.x);
                    GetComponent<Rigidbody>().AddForce(transform.up * 500);
                    if (collision.transform.parent.GetComponent<PlatformScript>().Currentplatform.Index != 0)
                        Bar.rectTransform.sizeDelta = new Vector2(BarBG.rectTransform.sizeDelta.x / (numOfPlatforms - 1) * collision.transform.parent.GetComponent<PlatformScript>().Currentplatform.Index, BarBG.rectTransform.sizeDelta.y);
                    if (!Encouragement(collision.transform.position, transform.position, collision.transform.parent.GetComponent<PlatformScript>().Currentplatform.Index))
                        if (collision.transform.parent.GetComponent<PlatformScript>().Currentplatform.Index - lastplatform >= 2)
                            StartCoroutine("Encourage", "Long Jump!");
                    lastplatform = collision.transform.parent.GetComponent<PlatformScript>().Currentplatform.Index;

                    }
                else
                    {
                    Bar.rectTransform.sizeDelta = new Vector2(BarBG.rectTransform.sizeDelta.x, BarBG.rectTransform.sizeDelta.y);
                    GetComponent<Rigidbody>().useGravity = false;
                    animator.SetTrigger("Stand");
                    line.enabled = false;
                    Destroy(LineHit);
                    GetComponent<CharacterScript>().enabled = false;
                    StartCoroutine(nameof(EndOfGame));
                    }
                }
            else if (collision.transform.tag == "BP")
                {
                animator.SetTrigger("Jump" + Random.Range(1, numOfAnimations + 1).ToString());
                Indicator.GetComponent<landIndicatorScript>().StartCoroutine("Land", collision.transform.localScale.x);
                GetComponent<Rigidbody>().velocity += new Vector3(0, -GetComponent<Rigidbody>().velocity.y + 40, 0);
                collision.gameObject.GetComponent<PlatformScript>().StartCoroutine("BottomPlatformDestroy");
                }
            }
        }
    public bool Encouragement(Vector3 Platform, Vector3 Player, int PlatformIndex)
        {
        if (lastplatform != PlatformIndex)
            {
            if (Mathf.Abs(Player.x - Platform.x) <= 0.3f && Mathf.Abs(Player.z - Platform.z) <= 0.3f)
                {
                StartCoroutine(nameof(Encourage), "Perfect");
                return true;
                }
            else if (Mathf.Abs(Player.x - Platform.x) <= 0.5f && Mathf.Abs(Player.z - Platform.z) <= 0.5f)
                {
                StartCoroutine(nameof(Encourage), "Good");
                return true;
                }
            }
        return false;
        }
    public IEnumerator Encourage(string massege)
        {
        encourageAmount++;
        EncouragementText.fontSize = EncourageTextSize;
        Color color = new Color();
        EncouragementText.text = massege;
        EncouragementText.gameObject.SetActive(true);
        if (massege == "Good")
            color = new Color(1, 0.65f, 0, 1);
        else if (massege == "Perfect")
            color = Color.green;
        else
            color = new Color(0.75f, 0, 1, 1);
        EncouragementText.color = color;
        int TextSize = EncouragementText.fontSize;
        for (int i = 0; i < 50; i++)
            {
            EncouragementText.fontSize = Mathf.RoundToInt(EncouragementText.fontSize * 1.01f);
            yield return new WaitForSeconds(0.001f);
            }
        yield return new WaitForSeconds(1);
        for (int i = 0; i < 50; i++)
            {
            if (encourageAmount == 1)
                {
                EncouragementText.fontSize = Mathf.RoundToInt(EncouragementText.fontSize / 1.1f);
                yield return new WaitForSeconds(0.0005f);
                }
            }
        if (encourageAmount == 1)
            {
            EncouragementText.fontSize = TextSize;
            EncouragementText.gameObject.SetActive(false);
            }
        encourageAmount--;
        }
    private void OnCollisionExit(Collision collision)
        {
        if (collision.transform.tag == "platform" && !mousepressed)
            {
            int index = collision.transform.parent.GetComponent<PlatformScript>().Currentplatform.Index;
            GameObject nextPlatform = platforms[index+1];
            while (nextPlatform == null)
                {
                index++;
                nextPlatform = platforms[index + 1];
                }
            StartCoroutine("RotateCourotine", nextPlatform.transform.position);

            }
        }
    public IEnumerator RotateCourotine(Vector3 target)
        {
        Vector3 newTarget = new Vector3(target.x, transform.position.y, target.z);
        var targetRotation = Quaternion.LookRotation(newTarget - transform.position);
        while (transform.rotation != targetRotation && !mousepressed)
            {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * Time.deltaTime);
            yield return new WaitForSeconds(Time.deltaTime/2);
            }
        newTarget.y = transform.position.y;
        transform.LookAt(newTarget);
        }
    }
