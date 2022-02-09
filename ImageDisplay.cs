using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ImageDisplay : MonoBehaviour
{
    //create system that finds image file within asset folder and inserts it into a canvas image component.
    //image files must be assoicated with corresponding game object
    //can have bool for each game object and set their image active/off based on their bool

    public ItemList DisplayTopThree; //reference for item controller
    public int[] UpdateList = new int[3];
    public RawImage[] rawImages = new RawImage[3];

    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            //rawImages[i].enabled = false;
        }
        DisplayTopThree = FindObjectOfType<ItemList>();
    }

    bool CheckForChange()
    {
        return !DisplayTopThree.topThree.SequenceEqual(UpdateList);
    }
    void Update()
    {
        if (DisplayTopThree != null && Photon.Pun.PhotonNetwork.IsMasterClient)
        {
            if (CheckForChange())
            {
                for (int i = 0; i < DisplayTopThree.topThree.Length; i++)
                {
                    UpdateList[i] = DisplayTopThree.topThree[i];
                    if (DisplayTopThree.topThree[i] != -1)
                    {
                        rawImages[i].texture = ObjectScriptableObject.Instance.items[DisplayTopThree.topThree[i]].displayImage;
                    }
                    else
                    {
                        rawImages[i].texture = null;
                    }
                }
            }
        }
    }
}
