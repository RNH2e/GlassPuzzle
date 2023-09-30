using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    public static GameScript Instance;
    public Transform selectedGlass, otherGlass, oldGlass;
    public List<GlassScript> glassList;
    public List<Material> colorList;
    public bool onClick;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        glassList = new List<GlassScript>(FindObjectsOfType<GlassScript>());

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0))
        {
            onClick = true;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 10, 1 << 6))
            {

                // Bardaða týklandý
                if (selectedGlass == null)
                {
                    // Seçili bardak yoksa, seçili bardaðý ayarla
                    selectedGlass = hit.transform;
                    oldGlass = selectedGlass;
                    selectedGlass.GetComponent<GlassScript>().isOpen = true;
                }
                else if (selectedGlass == hit.transform)
                {
                    // Seçili bardaða tekrar týklandý, bardaðý eski yerine getir
                    selectedGlass.GetComponent<GlassScript>().isOpen = false;
                    selectedGlass = null;
                }
                else if (selectedGlass)
                {
                    // Baþka bir bardaða týklandý, önceki seçili bardaðý eski yerine getir
                    // selectedGlass.GetComponent<GlassScript>().isOpen = false;
                    selectedGlass = hit.transform;
                    otherGlass = selectedGlass;
                    selectedGlass.GetComponent<GlassScript>().isOpen = true;

                }
            }

        }
        else
        {
            onClick = false;
        }

        if (oldGlass && otherGlass)
        {
            if (oldGlass.GetComponent<GlassScript>().isOpen && otherGlass.GetComponent<GlassScript>().isOpen && otherGlass.GetComponent<GlassScript>().glassID == oldGlass.GetComponent<GlassScript>().glassID &&
                !oldGlass.GetComponent<GlassScript>().occupied && !otherGlass.GetComponent<GlassScript>().occupied)
            {
                oldGlass.GetComponent<GlassScript>().merge = true;
                otherGlass.GetComponent<GlassScript>().merge = true;
                if (oldGlass.GetComponent<GlassScript>().merge == true && otherGlass.GetComponent<GlassScript>().merge == true)
                {
                    otherGlass = null;
                    oldGlass = null;
                    selectedGlass = null;
                }

            }
            else
            {
                oldGlass.GetComponent<GlassScript>().isOpen = false;
                otherGlass.GetComponent<GlassScript>().isOpen = false;
                selectedGlass = otherGlass = oldGlass = null;
            }
        }

        glassList.RemoveAll(item => item == null);

        // for (int i = 0; i < glassList.Count; i++)
        // {
        //     if (glassList[i].isOpen && glassList[(i + 1) % glassList.Count].isOpen && glassList[i].glassID == glassList[(i + 1) % glassList.Count].glassID && !glassList[(i + 1) % glassList.Count].occupied)
        //     {
        //         glassList[i].merge = true;
        //         glassList[(i + 1) % glassList.Count].merge = true;
        //         glassList.RemoveAll(item => item == null);
        //     }
        // }
        if (glassList.Count <= 0)
        {
            UIManager.Instance.CheckSucces(0);
        }
    }


}
