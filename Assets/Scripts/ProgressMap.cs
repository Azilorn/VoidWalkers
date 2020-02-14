using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum FloorContent { Battle, Tavern, Reward, Shop, Boss, Event, Empty }
public class ProgressMap : MonoBehaviour
{

    public static ProgressMap Instance;
    public Canvas canvas;
    public ProgressNode[,] floors;
    public int[,] floorContent;
    public GameObject bossGO; 
    public GameObject template;
    public GameObject parent;
    public List<GameObject> objectPool = new List<GameObject>();
    public List<Sprite> uiSprites;

    public GameObject lineRendererParent;
    private List<GameObject> lineRenderers = new List<GameObject>();
    [SerializeField] private int maxFloors = 10;
   [SerializeField] private int maxRooms = 4;
    [SerializeField] private int minRooms;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        SetFloorsNewGame();
        minRooms = maxRooms - 2;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2)) {
            SetFloorsNewGame();
        }
    }
    public void SetFloorsLoadedGame()
    {
        
    }
    public void SetFloorsNewGame()
    {
        foreach (GameObject go in lineRenderers)
        {
            Destroy(go);
        }
        lineRenderers.Clear();
        floors = new ProgressNode[maxFloors, maxRooms];
        int currentCount = 0;
        int roomCount = 0;
        for (int f = 0; f < maxFloors; f++)
        {
            roomCount = Random.Range(minRooms, maxRooms + 1);
            for (int r = 0; r < roomCount; r++)
            {
                floors[f, r] = new ProgressNode();

                if (f == 0)
                {

                    floors[f, r].type = FloorContent.Battle;
                    AddFloorUIObject(currentCount, FloorContent.Battle, f, r, floors[f, r], roomCount);
                    currentCount++;
                    continue;

                }
                if (f == maxFloors -1)
                {
                    floors[f, r].type = FloorContent.Tavern;
                    AddFloorUIObject(currentCount, FloorContent.Tavern, f, r, floors[f, r], roomCount);
                    currentCount++;
                    continue;
                }
                if (f == maxFloors - 2)
                {
                    roomCount = 1;

                    floors[f, r].type = FloorContent.Boss;
                    AddFloorUIObject(currentCount, FloorContent.Boss, f, r, floors[f, r], roomCount);
                    currentCount++;
                    continue;

                }
                int rnd = Random.Range(0, 100);


                if (rnd > 0 && rnd <= 50)
                {
                    floors[f, r].type = FloorContent.Battle;
                    AddFloorUIObject(currentCount, FloorContent.Battle, f, r, floors[f, r], roomCount);
                    currentCount++;
                    continue;
                }
                else if (rnd > 50 && rnd <= 63)
                {
                    floors[f, r].type = FloorContent.Tavern;
                    AddFloorUIObject(currentCount, FloorContent.Tavern, f, r, floors[f, r], roomCount);
                    currentCount++;
                    continue;
                }
                else if (rnd > 63 && rnd <= 76)
                {
                    floors[f, r].type = FloorContent.Reward;
                    AddFloorUIObject(currentCount, FloorContent.Reward, f, r, floors[f, r], roomCount);
                    currentCount++;
                    continue;
                }
                else if (rnd > 76 && rnd <= 89)
                {
                    floors[f, r].type = FloorContent.Shop;
                    AddFloorUIObject(currentCount, FloorContent.Shop, f, r, floors[f, r], roomCount);
                    currentCount++;
                    continue;
                }
                else if (rnd > 89 && rnd <= 100)
                {
                    floors[f, r].type = FloorContent.Event;
                    AddFloorUIObject(currentCount, FloorContent.Event, f, r, floors[f, r], roomCount);
                    currentCount++;
                    continue;
                }
            }
        }

        bool removingStrayIcons = true;
        //While true, Remove all stray icons that do not form to the required formatting, which is next row must not be > +1 or < -1 column length
        while (removingStrayIcons)
            removingStrayIcons = RemoveStrayIcons();

        for (int f = 0; f < maxFloors - 1; f++)
        {
            for (int r = 0; r < maxRooms; r++)
            {
                if (floors[f, r] != null)
                {
                    //Set Loaded objects targets
                    SetTargets(f, r);
                    SetRoute(f, r);
                }
            }
        }
    }

    public bool RemoveStrayIcons()
    {
        for (int f = 1; f < maxFloors; f++)
        {
            int columnPreviousCount = 0;
            int columnCurrentCount = 0;
            for (int r = 0; r < maxRooms; r++)
            {
                if (floors[f - 1, r] != null)
                {
                    columnPreviousCount++;
                }
                if (floors[f, r] != null)
                {
                    columnCurrentCount++;
                }            
            }
            if (columnCurrentCount > columnPreviousCount + 1)
            {
               
                floors[f, columnCurrentCount - 1] = null;
                return true;
            }
            else if (columnPreviousCount > columnCurrentCount + 1)
            {
                floors[f, columnPreviousCount - 1] = null;
                return true;
            }
        }
        return false;
    }

    public bool CountGreaterThanPool(int count)
    {
        Debug.Log("count: " + count + " objects: " + objectPool.Count);
        if (objectPool.Count == 0)
            return false;
     
        if (count < objectPool.Count)      
            return true;
        else
            return false;
        
    }
    private Sprite ReturnUISprite(FloorContent floorContent)
    {
        switch (floorContent)
        {
            case FloorContent.Battle:
                return uiSprites[0];
            case FloorContent.Tavern:
                return uiSprites[1];
            case FloorContent.Reward:
                return uiSprites[2];
            case FloorContent.Shop:
                return uiSprites[3];
            case FloorContent.Boss:
                return uiSprites[4];
            case FloorContent.Event:
                return uiSprites[5];
            case FloorContent.Empty:
                return null;
        }
        return null;

    }
    private void AddFloorUIObject(int currentCount, FloorContent floorContent, int floor, int room, ProgressNode pNode, int roomCount)
    {

        if (floor == maxFloors - 1)
        {
            pNode.floorNum = floor;
            pNode.roomNum = room;
            pNode.go = bossGO;
            pNode.go.name = "Boss GO";
            pNode.xScreenLocation = bossGO.transform.position.x;
            pNode.yScreenLocation = bossGO.transform.position.y;
            pNode.locked = false;
            pNode.complete = false;
            return;
        }

        if (CountGreaterThanPool(currentCount))
        {
            objectPool[currentCount].GetComponent<Image>().sprite = ReturnUISprite(floorContent);
            pNode.floorNum = floor;
            pNode.roomNum = room;
            RectTransform rect = objectPool[currentCount].GetComponent<RectTransform>();
            objectPool[currentCount].transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(((1080/ roomCount) * (room + 1 /  roomCount )) + (Random.Range(-25, 25)), (300 * floor) + (Random.Range(-50, 50) + 100));
            objectPool[currentCount].name = "f: " + floor + " r: " + room;
            pNode.go = objectPool[currentCount];
            pNode.xScreenLocation = objectPool[currentCount].transform.position.x;
            pNode.yScreenLocation = objectPool[currentCount].transform.position.y;
            pNode.locked = false;
            pNode.complete = false;
        }
        else if (!CountGreaterThanPool(currentCount))
        {
            GameObject go = Instantiate(template, parent.transform);
            objectPool.Add(go);
            go.GetComponent<Image>().sprite = ReturnUISprite(floorContent);
            pNode.floorNum = floor;
            pNode.roomNum = room;
            RectTransform rect = go.GetComponent<RectTransform>();
            go.transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(((1080 / roomCount) * (room + 1 /  roomCount)) + (Random.Range(-25, 25)), (300 * floor) + (Random.Range(-50, 50) + 100));
            go.name = "f: " + floor + " r: " + room;
            pNode.go = go;
            pNode.xScreenLocation = go.transform.position.x;
            pNode.yScreenLocation = go.transform.position.y;
            pNode.locked = false;
            pNode.complete = false;
        }
    }
    public void SetTargets(int f, int r)
    {
        for (int i = 0; i < maxRooms; i++)
        {
            if (f == maxFloors - 2)
            {
                floors[f, r].targets.Add(floors[maxFloors - 1, 0]);
                floors[maxFloors - 1, 0].childTargets.Add(floors[f, r]);
            }
            else if (floors[(f + 1), i] != null)
            {
                if (i > r + 1 || i < r - 1)
                {
                    continue;
                }
                else
                {
                    floors[f, r].targets.Add(floors[f + 1, i]);
                    floors[f + 1, i].childTargets.Add(floors[f, r]);
                }
            }
        }
    }

    public void SetRoute(int f, int r)
    {
        if (f == maxFloors - 1)
        {
            //floors[9, 2].go.GetComponent<RectTransform>().localScale = Vector3.one * 3;
            return;
        }
     
        for (int i = 0; i < floors[f, r].targets.Count; i++)
        {
            floors[f, r].go.SetActive(true);
           
            GameObject lineRenderer = new GameObject();
            lineRenderer.AddComponent<Image>();
            lineRenderer.transform.SetParent(lineRendererParent.transform);
            lineRenderer.GetComponent<RectTransform>();
            RectTransform rect = lineRenderer.GetComponent<RectTransform>();
           // lineRenderers.Add(lineRenderer);

            rect.pivot = new Vector2(0, 1);
            rect.localScale = Vector3.one;
            Vector3 pointA = floors[f, r].go.GetComponent<RectTransform>().anchoredPosition;
            Vector3 pointB = Vector3.zero;
            rect.anchoredPosition = pointA;

            try
            {
                pointB = floors[f, r].targets[i].go.GetComponent<RectTransform>().anchoredPosition;
            }
            catch(InvalidCastException e) {
                if (e.Source != null)
                    Console.WriteLine("IOException source: {0}", e.Source);
                throw;
            }

            Vector3 differenceVector = pointB - pointA;
            float lineWidth = 5f;
            float angle = Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;

            rect.sizeDelta = new Vector2(differenceVector.magnitude, lineWidth);
            rect.localRotation = Quaternion.Euler(0, 0, angle);
            floors[f, r].targets[i].go.SetActive(true);
        }

    }
}
[Serializable]
public class ProgressNode {

    public FloorContent type;
    public int content;

    public float xScreenLocation;
    public float yScreenLocation;

    public GameObject go;
    public List<ProgressNode> childTargets = new List<ProgressNode>();
    public List<ProgressNode> targets = new List<ProgressNode>();

    public int roomNum;
    public int floorNum;
    public bool locked;
    public bool complete;
}
