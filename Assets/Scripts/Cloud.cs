using UnityEngine;

public class Cloud : MonoBehaviour
{
    private readonly float edge = 20f;
    private float speed;

    // Start is called before the first frame update
    private void Start()
    {
        transform.localScale *= Random.Range(0.3f, 1f);
        speed = Random.Range(0.5f, 2f);
        transform.Rotate(new Vector3(0, 0, Random.Range(-16f, 16f)));
    }

    // Update is called once per frame
    private void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime * 0.1f;
        if (transform.localPosition.x >= edge) transform.localPosition += Vector3.left * 2f * edge;
    }
}