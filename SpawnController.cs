using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(ItemList))]
public class SpawnController : MonoBehaviour
{
    public GameManager2 gameManager;
    protected Transform parent;
    [Tooltip("The size of the space in which items spawn in.")] [SerializeField] Vector3 cubeSize;
    protected Vector3 itemInterval;
    protected Vector3 startPoint;
    public Vector3 itemPerRow;
    [HideInInspector] public bool hasSpawned;
    protected int itemCount;

    void Start()
    {
        gameManager = gameManager == null ? GameObject.Find("GameManager")?.GetComponent<GameManager2>() : gameManager; //checks to see if there is a GameManager within the scene
        parent = GameObject.FindGameObjectWithTag("Object Container").transform; //finds the empty object within the scene to store all the spawned items under 1 parent location
        hasSpawned = false;
        SetSpawns();
    }
    /// <summary>
    /// Math Init Stuff
    /// </summary>
    public void SetSpawns() 
    {
        itemInterval = new Vector3(cubeSize.x / itemPerRow.x, cubeSize.y / itemPerRow.y, cubeSize.z / itemPerRow.z);  //determines how many objects can spawn based on user input on room size and 
        startPoint = transform.position - (.5f * cubeSize); //sets starting 
    }

    //This spawns the items
    public void Spawn()
    {
        int[] itemlist = new int[ObjectScriptableObject.Instance.items.Count]; //we made a scriptable object within our project which basically stores assets like game objects ready to be called within programs.
        itemCount = ObjectScriptableObject.Instance.items.Count; //total game objects within the scriptable object
        for (int i = 0; i < itemlist.Length; i++)
        {
            itemlist[i] = ObjectScriptableObject.Instance.items[i].iD; //goes through the list of objects and assigns them their ID during runtime.
        }
        //Randomizes the itemlist
        //for (int i = 0; i < itemlist.Length; i += 1)
        //{
        //    int j = Random.Range(i, itemlist.Length - 1);
        //    int temp = itemlist[i];
        //    itemlist[i] = itemlist[j];
        //    itemlist[j] = temp;
        //}
        for (int k = 0; k < itemPerRow.z; k += 1) //runs through the item limit starting with the x row
        {
            for(int j = 0; j < itemPerRow.y; j += 1)
            {
                for(int i = 0; i < itemPerRow.x; i += 1)
                {
                   // Debug.Log("spawned an object");
                    if (itemCount > 0 && !ObjectScriptableObject.Instance.items[itemlist[itemCount - 1]].itemPrefab.CompareTag("Filler"))
                    {
                        Vector3 positionOffset = new Vector3(i * itemInterval.x + Random.Range(0, itemInterval.x), j * itemInterval.y + Random.Range(-itemInterval.y, 0), k * itemInterval.z + Random.Range(0, itemInterval.z)); //calculates random position of each object during loop
                        GameObject temp = Photon.Pun.PhotonNetwork.Instantiate(ObjectScriptableObject.Instance.items[itemlist[itemCount - 1]].itemPrefab.name, startPoint + positionOffset, Quaternion.Euler(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180)));
                        //generates (instantiates) current object in array into the game world using Photon, the networking library of Unity.
                        temp.transform.parent = parent;
                        //temp.GetComponent<ItemGrabQueue>().iD = itemlist[itemCount - 1];
                    }
                    itemCount -= 1;  //start at top of list so itemCount+=1;  also start itemCount=2; get rid of the debug and itemCount=40;
                }
            }
        }
        hasSpawned = true;
        itemCount = ObjectScriptableObject.Instance.items.Count;
        
        //sends word to the ItemList to initialize
        gameObject.GetComponent<ItemList>().Initialize();
        
    }
}