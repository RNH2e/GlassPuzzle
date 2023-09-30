using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassScript : MonoBehaviour
{
    public List<GameObject> particle;
    public ParticleScript ps;
    public Transform pivot;
    public GameObject particleObj;
    public int sphereCount;
    public GameObject cube;
    public bool isOpen;
    public Vector3 targetPos;
    public float currentYPos;
    public bool merge;
    float dly;
    public bool occupied;
    public float shakeAmount = 0.1f;
    public float shakeSpeed = 5.0f;
    private Vector3 originalPosition;
    public int glassID;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        targetPos = new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z);
        currentYPos = transform.position.y;

        for (int i = 0; i < particle.Count; i++)
        {
            particle[i].GetComponent<Rigidbody>().isKinematic = false;
        }
        foreach (var item in particle)
        {
            Material selectedColor = GameScript.Instance.colorList[glassID];
            item.GetComponent<ParticleScript>().ChangeColor(selectedColor);
        }

    }
    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.up*.5f, out hit, .5f))
        {
            Debug.Log("Üstünde bir þey var: " + hit.collider.gameObject.name);
            occupied = true;
        }
        else
        {
            occupied = false;
        }

        if (!occupied)
        {
            if (isOpen)
            {
                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 5);

                for (int i = 0; i < particle.Count; i++)
                {
                    if (particle[i])
                    {
                        particle[i].GetComponent<Rigidbody>().isKinematic = false;
                        cube.GetComponent<Collider>().enabled = false;
                    }
                    if (merge && dly > .01f)
                    {
                        if (particle[i].GetComponent<ParticleScript>().onFloor)
                        {
                            StartCoroutine(particle[i].GetComponent<ParticleScript>().DestroyParticles());
                            dly = 0;
                            particle.Remove(particle[i]);
                            GetComponent<MeshRenderer>().enabled = false;
                            transform.GetChild(3).gameObject.SetActive(true);
                            Destroy(transform.GetChild(3).gameObject, 3);
                        }
                    }
                }
                dly += Time.deltaTime * 2;
                if (isOpen && merge)
                {
                    Destroy(gameObject, 4);

                }

            }

            else
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, currentYPos, transform.position.z), Time.deltaTime * 5);
                for (int i = 0; i < particle.Count; i++)
                {
                    if (particle[i] && particle[i].GetComponent<ParticleScript>().onFloor && !merge)
                    {
                        particle[i].GetComponent<Rigidbody>().isKinematic = true;
                        particle[i].transform.position = Vector3.Lerp(particle[i].transform.position, pivot.transform.position, Time.deltaTime * 5);

                        Destroy(particle[i], .5f);
                    }
                    if (!particle[i])
                    {
                        particle.Remove(particle[i]);
                        float posX = Random.Range(-0.15f, .2f);
                        float posY = Random.Range(.1f, .5f);
                        float posZ = Random.Range(-.1f, .1f);

                        if (sphereCount < 15)
                        {
                            cube.GetComponent<Collider>().enabled = true;
                            var a = Instantiate(particleObj, pivot.position, Quaternion.identity, transform);
                            Material selectedColor = GameScript.Instance.colorList[glassID];
                            a.GetComponent<ParticleScript>().ChangeColor(selectedColor);
                            a.transform.localPosition = new Vector3(posX, posY, posZ);
                            a.transform.parent = transform.GetChild(1);
                            a.GetComponent<Rigidbody>().isKinematic = false;
                            particle.Add(a);
                            sphereCount++;
                        }

                        sphereCount = 0;
                    }
                }
            }
        }
        else if (GameScript.Instance.selectedGlass && GameScript.Instance.onClick && GameScript.Instance.selectedGlass.GetComponent<GlassScript>().occupied)
        {
            Debug.Log("açýlmýyo");
            isOpen = false;
            StartCoroutine(GameScript.Instance.selectedGlass.GetComponent<GlassScript>().Shake());
            GameScript.Instance.selectedGlass = null;
        }
    }
    IEnumerator Shake()
    {
        float elapsedTime = 0f;

        while (elapsedTime < 0.2f) 
        {
            float offsetX = Mathf.PerlinNoise(Time.time * shakeSpeed, 0) * 2 - 1;
            Vector3 shakeOffset = new Vector3(offsetX, 0, 0) * shakeAmount;

            transform.position = originalPosition + shakeOffset;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition; 
    }
    public void CantOpen()
    {
        isOpen = false;
    }

    public static implicit operator Transform(GlassScript v)
    {
        throw new System.NotImplementedException();
    }
}
