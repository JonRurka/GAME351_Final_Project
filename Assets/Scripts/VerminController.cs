using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerminController : MonoBehaviour
{
    public static VerminController Instance { get; private set; }

    public GameObject Vermin_prefab;
    public int spawn_amount = 10;

    


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Attempted to spawn more than one VerminController!");
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnInitialVermin()
    {
        for (int i = 0; i < spawn_amount; ++i)
        {
            SpawnVermin();
        }
    }

    public GameObject SpawnVermin()
    {
        WheatMaze.MazeGridTile tile = WheatMaze.Instance.GetRandomOpenTile();
        Vector3 position = new Vector3(tile.GlobalPosition.x + 0.5f, tile.GlobalPosition.y, tile.GlobalPosition.z + 0.5f);

        GameObject obj = Instantiate(Vermin_prefab, position, Vermin_prefab.transform.rotation);
        obj.GetComponent<Vermin>().Init(tile);
        return obj;
    }
}
