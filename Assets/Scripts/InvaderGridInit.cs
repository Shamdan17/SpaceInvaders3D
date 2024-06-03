using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvaderGridInit : MonoBehaviour
{
    public Invader[] prefabs;

    public int rows = 5;
    public int columns = 11;
    public float spacingH = 2.5f;
    public float spacingV = 2.5f;

    public float speed = 1;
    private Vector3 direction = Vector3.right;
    private Camera _camera;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        _camera = Camera.main;
        var cameraTransformPosition = _camera.transform.position;
        var spawnPosition =
            _camera.ScreenToWorldPoint(new Vector3(UnityEngine.Device.Screen.width / 2,
                UnityEngine.Device.Screen.height,
                -cameraTransformPosition.z));
        
        Vector3 offsetToCenter = spawnPosition; // new Vector3((columns - 1) * spacingH / 2, (rows - 1) * spacingV / 2, 0);
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                this.transform.position = spawnPosition;
                Invader invader = Instantiate(this.prefabs[Random.Range(0, this.prefabs.Length)], this.transform);
                Vector3 position = new Vector3(j * spacingH, i * spacingV, 0) - offsetToCenter;
                invader.transform.localPosition = position;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float zcam = (float)this.transform.position.z - Camera.main.transform.position.z;


        Vector3 LeftEdge = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0.5f, zcam));
        Vector3 RightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0.5f, zcam));

        this.transform.position += this.direction * this.speed * Time.deltaTime;
        // if (this.transform.position.x > 10 || this.transform.position.x < -10)
        // {
        //     this.direction = -this.direction;
        //     // this.transform.position += Vector3.down * 1;
        // }
        foreach (Transform child in this.transform)
        {
            // Skip if inactive or not an Invader
            if (child.GetComponent<Invader>() == null || !child.gameObject.activeSelf)
            {
                continue;
            }

            if (child.position.x > RightEdge.x || child.position.x < LeftEdge.x)
            {
                this.direction = -this.direction;
                this.transform.position += Vector3.down * 1;
                break;
            }
        }
    }
}