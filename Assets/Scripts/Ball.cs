using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    //commented lines are alternative draft Vector3.Slerp Method (doesnt work for long distances)

    [SerializeField]
    GameObject Path_Obj, Collision_Obj;
    List<GameObject> pathobjects = new List<GameObject>();
    List<GameObject> collisionobjects = new List<GameObject>();

    public bool Moving = false;

    Vector3 StartingPos, FinalPos, Velocity;
    float Height, Floor;
    int Speed, Bounce;
    bool Path, Collision;
    float time, vy, vx;

    //Vector3 CenterPos;
    //float progress = 0;

    void Start()
    {
        Floor = transform.parent.GetComponent<BallSettings>().Floor;
        Height = transform.parent.GetComponent<BallSettings>().Height;
        Speed = transform.parent.GetComponent<BallSettings>().Speed;
        Bounce = transform.parent.GetComponent<BallSettings>().Bounce;
        Path = transform.parent.GetComponent<BallSettings>().Show_Path;
        Collision = transform.parent.GetComponent<BallSettings>().Show_Collision;
    }

    void Update()
    {
        if (!Moving)
            return;
        if (Path)
            pathobjects.Add(Instantiate(Path_Obj, transform.position, Quaternion.identity));
        Move();
    }

    void Move()
    {
        Velocity += Physics.gravity * Time.deltaTime * (0.1f * Speed);
        transform.position += Velocity * Time.deltaTime * (0.1f * Speed);

        if (transform.position.y < Floor)
        {
            transform.position=new Vector3(transform.position.x,Floor,0);
            if (Collision)
                collisionobjects.Add(Instantiate(Collision_Obj, transform.position, Quaternion.identity));
            if (((Height / 10) * Bounce >= 1))
            {
                Height = (Height / 10) * Bounce;
                SetPath((FinalPos - StartingPos) + transform.position);
            }
            else
            {
                Moving = false;
                Remove();
            }
        }

        /* Slerp Method
        transform.position = Vector3.Slerp(StartingPos, FinalPos, progress / 100);
        if (Vector3.Slerp(StartingPos, FinalPos, 0.5f).y < (Height + 1))
            transform.position += Vector3.up * ((Height + 1) - Vector3.Slerp(StartingPos, FinalPos, 0.5f).y) * ((progress / 100 <= 0.5f) ? ((progress / 100) / 0.5f) : (2 - ((progress / 100) / 0.5f)));
        else
        {
            transform.position += Vector3.up * ((Height + 1) - Vector3.Slerp(StartingPos, FinalPos, 0.5f).y) * ((progress / 100 >= 0.5f) ? ((progress / 100) / 0.5f) : (2 - ((progress / 100) / 0.5f)));
            transform.position += Vector3.up * (0 - ((Height + 1) - Vector3.Slerp(StartingPos, FinalPos, 0.5f).y) * (((progress / 100) >= 0.5f) ? ((progress / 100) / 0.5f) : (2 - ((progress / 100) / 0.5f)))) * (((progress / 100) >= 0.5f) ? (((progress / 100) / 0.5f) - 1) : (1 - ((progress / 100) / 0.5f)));
        }

        // if((1 - transform.position.y)*((progress/100)+1)>0)  //tried to fix wrong parabola for long distances
        // {
        //     transform.position += Vector3.up * (1 - transform.position.y)*((progress/100)+1);
        // }
        // else if((transform.position.y)<(transform.position + (Vector3.up * (1 - transform.position.y)*(((progress-1)/100)+1))).y)
        // {
        //     transform.position += Vector3.up * ((transform.position + (Vector3.up * (1 - transform.position.y)*(((progress-1)/100)+1))).y - transform.position.y)*((progress/100)+1);
        // } 
        progress += (0.1f * Speed);
        transform.position += CenterPos;
        if (((progress / 100) >= 1))
        {
            if (Collision)
                collisionobjects.Add(Instantiate(Collision_Obj, transform.position, Quaternion.identity));
            progress = 0;
            if (((Height * Bounce) / 10 > 2))
            {
                Height = (Height * Bounce) / 10;
                SetPath((FinalPos - StartingPos) + transform.position);
            }
            else
            {
                Moving = false;
                Remove();
            }
        }
        */
    }

    void Remove()
    {
        transform.parent.GetComponent<BallSettings>().Available_Ball++;
        if (Path)
            foreach (GameObject o in pathobjects)
                Destroy(o);
        if (Collision)
            foreach (GameObject o in collisionobjects)
                Destroy(o);
        Destroy(gameObject);
    }

    public void SetPath(Vector3 finalpos)
    {
        StartingPos = transform.position;
        FinalPos = finalpos;

        vy = Mathf.Sqrt(2 * 9.81f * Height);         //Voy formula
        time = vy * 2 / (9.81f);                     //time formula
        vx = (FinalPos.x - StartingPos.x) / (time);  // Vox formula
        Velocity = new Vector3(vx, vy, 0);

        Moving = true;

        // CenterPos = (StartingPos + FinalPos) * 0.5f;
        // CenterPos += Vector3.down;
        // StartingPos = StartingPos - CenterPos;
        // FinalPos = FinalPos - CenterPos;
    }
}