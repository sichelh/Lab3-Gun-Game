using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shoot : MonoBehaviour
{
    public Camera FPSCamera;
    public float range = 20;
    public Transform gunEndPoint;
    public float shootingForce = 100;

    private LineRenderer _lineRenderer;
    public LayerMask shootableMask;
    public AudioSource audioSource;

    private GameObject pin;
    public int KillEnemyCount = 0;
    public int ShootPinCount = 0;

    public float timeRemaining = 10;
    public bool timerIsRunning = false;
    public Text timeText;

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();

        // Starts the timer automatically
        timerIsRunning = true;
    }
    void Update()
    {
        // the visualized line starts at the end of the gun
        _lineRenderer.SetPosition(0, gunEndPoint.position);

        // here we use a ray from from the camera to the mouse position
        Ray ray = FPSCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // play sound when mouse is clicked
        if (Input.GetMouseButtonDown(0))
            audioSource.Play();

        // when mouse is pressed and raycast hits a target...
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit, range, shootableMask))
        {
            // set the end of the line to the raycast hit point
            _lineRenderer.SetPosition(1, hit.point);

            if (hit.collider.gameObject.CompareTag("Pin"))
            {
                // apply some force to the target object
                hit.collider.attachedRigidbody.AddForce(ray.direction * shootingForce);
                hit.collider.gameObject.GetComponent<Renderer>().material.color = Color.red;
                ShootPinCount++;
                print("You Shooted Pin: " + ShootPinCount);
            }
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                Destroy(hit.collider.gameObject);
                KillEnemyCount++;
                print("You killed Enemy: " + KillEnemyCount);
            }
            
            
        }
        else
        {
            // if mouse was not pressed or the ray did not hit a target,
            // the line ends at the range of the gun.
            _lineRenderer.SetPosition(1, ray.GetPoint(range));
        }


        //timer
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
                Debug.Log("Shooted Pin number:" + ShootPinCount + " Killed Enemy number:" + KillEnemyCount);
            }
        }
    }
    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}