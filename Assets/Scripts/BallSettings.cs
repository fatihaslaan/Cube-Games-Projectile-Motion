using System.Collections.Generic;
using UnityEngine;

public class BallSettings : MonoBehaviour
{
    [HideInInspector]
    public int Available_Ball;
    [SerializeField]
    List<GameObject> Ball_Objects;
    [SerializeField]
    GameObject Ball_Prefab, Target_Obj, Floor_Obj;

    [HideInInspector]
    public int Height, Speed, Bounce;
    [HideInInspector]
    public bool Show_Path, Show_Collision, RefreshHeightObj;
    [HideInInspector]
    public Vector3 StartingPos, FinalPos;
    [HideInInspector]
    public float Floor;

    int clickcounter = 0;
    int currentball = 0;
    bool ballcreated=false;
    Camera cam;

    void Awake()
    {
        Available_Ball = Ball_Objects.Count;
        Height = 3;
        Speed = 10;
        Bounce = 8;
        Floor=(Floor_Obj.transform.localScale.y/2)+Floor_Obj.transform.position.y+0.5f;
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.name == "Ground")
                {
                    clickcounter++;
                    if (clickcounter % 2 == 1)
                    {
                        StartingPos = new Vector3(hit.point.x, Floor, 0);
                        CreateBall();
                    }
                    else if(ballcreated)
                    {
                        ballcreated=false;
                        FinalPos = new Vector3(hit.point.x, Floor, 0);
                        if (Target_Obj.active)
                            Target_Obj.transform.position = FinalPos;
                        else
                            Target_Obj = Instantiate(Target_Obj, FinalPos, Quaternion.identity);
                        Ball_Objects[currentball].GetComponent<Ball>().SetPath(FinalPos);
                        Available_Ball--;
                        RefreshHeightObj=true;
                    }
                }
            }
        }
    }

    void CreateBall()
    {
        if(Available_Ball<=0&&Ball_Objects.Count<24)
        {
            Ball_Objects.Add(Ball_Prefab);
            Available_Ball++;
        }
        for (int i = 0; i < Ball_Objects.Count; i++)
        {
            if(Ball_Objects[i]==null)
                Ball_Objects[i]=Ball_Prefab;
            if (!Ball_Objects[i].GetComponent<Ball>().Moving)
            {
                Ball_Objects[i]=(Instantiate(Ball_Objects[i], StartingPos, Quaternion.identity));
                Ball_Objects[i].transform.SetParent(transform);
                currentball = i;
                ballcreated=true;
                return;
            }
        }
    }
}