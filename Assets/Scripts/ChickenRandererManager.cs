
using UnityEngine;

public class ChickenRandererManager : MonoBehaviour
{
    public Material[] colors;

    [SerializeField]
    public bool isCrowd = false;

    public void ChangeChickenColor(int color_num)
    {
        if(color_num >= colors.Length)
        {
            Debug.LogError("no color with this number");
        }

        foreach(Renderer randerer in this.GetComponentsInChildren<Renderer>())
        {
            randerer.material = colors[color_num];
        }
    }

    private void Start()
    {
        if(isCrowd)
        {
            foreach (Renderer randerer in this.GetComponentsInChildren<Renderer>())
            {
                randerer.material = colors[Random.Range(0,colors.Length - 1)];//assign random color
            }
        }
    }

}
