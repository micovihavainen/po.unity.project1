using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float offset;
    public float offset_y;
    private Vector3 playerPosition;
    public float offsetSmoothing;

    // Transform of the GameObject you want to shake
    private Transform Cameratrans;

    // Desired duration of the shake effect
    private float shakeDuration = 0f;

    // A measure of magnitude for the shake. Tweak based on your preference
    private float shakeMagnitude = 0.03f;

    // A measure of how quickly the shake effect should evaporate
    private float dampingSpeed = 7.5f;

    public void TriggerShake()
    {
        shakeDuration = 3.0f;
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        if (shakeDuration > 0)
        {
            playerPosition = new Vector3(playerPosition.x, playerPosition.y, playerPosition.z);
            transform.localPosition = Vector3.Lerp(transform.position, playerPosition, offsetSmoothing * Time.deltaTime) + Random.insideUnitSphere * shakeMagnitude;
            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        if (player.transform.localScale.x > 0f)
        {
            playerPosition = new Vector3(playerPosition.x + offset, playerPosition.y + offset_y, playerPosition.z);

        }
        else
        {
            playerPosition = new Vector3(playerPosition.x - offset, playerPosition.y +offset_y, playerPosition.z);
        }
        transform.position = Vector3.Lerp(transform.position, playerPosition, offsetSmoothing * Time.deltaTime);

    }
}