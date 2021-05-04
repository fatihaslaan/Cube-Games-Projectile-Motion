using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField]
    InputField inpt_Height;
    [SerializeField]
    Slider sldr_Speed, sldr_Bounce;
    [SerializeField]
    Toggle tggl_Path, tggl_Collision, tggl_Height;
    [SerializeField]
    Text txt_Ball_Count;

    [SerializeField]
    GameObject Height_Obj;
    bool Show_Height = false;

    void Start()
    {
        AddListener();
        Values();
    }

    void AddListener()
    {
        inpt_Height.onValueChanged.AddListener(ChangeHeight);
        sldr_Speed.onValueChanged.AddListener(ChangeSpeed);
        sldr_Bounce.onValueChanged.AddListener(ChangeBounce);
        tggl_Path.onValueChanged.AddListener(ChangePath);
        tggl_Collision.onValueChanged.AddListener(ChangeCollision);
        tggl_Height.onValueChanged.AddListener(ChangeHeight);
    }

    void Values()
    {
        inpt_Height.text = transform.parent.GetComponent<BallSettings>().Height.ToString();
        sldr_Speed.value = transform.parent.GetComponent<BallSettings>().Speed;
        sldr_Bounce.value = transform.parent.GetComponent<BallSettings>().Bounce;
        tggl_Path.isOn = transform.parent.GetComponent<BallSettings>().Show_Path;
        tggl_Collision.isOn = transform.parent.GetComponent<BallSettings>().Show_Collision;
        tggl_Height.isOn = Show_Height;
    }

    void ChangeHeight(string text)
    {
        if (System.Convert.ToInt32(text) < 1)
            inpt_Height.text = "" + 1;
        transform.parent.GetComponent<BallSettings>().Height = System.Convert.ToInt32(text);
        if (!tggl_Height.isOn)
            return;
        if (Height_Obj.active)
            Height_Obj.transform.position = new Vector3(0, transform.parent.GetComponent<BallSettings>().Height+transform.parent.GetComponent<BallSettings>().Floor, 0);
        else
            Height_Obj = Instantiate(Height_Obj, new Vector3(0, transform.parent.GetComponent<BallSettings>().Height+transform.parent.GetComponent<BallSettings>().Floor, 0), Quaternion.identity);
    }

    void ChangeSpeed(float val)
    {
        sldr_Speed.value = val;
        transform.parent.GetComponent<BallSettings>().Speed = (int)val;
        sldr_Speed.GetComponentInChildren<Text>().text = val + "/50 Speed";
    }

    void ChangeBounce(float val)
    {
        sldr_Bounce.value = val;
        transform.parent.GetComponent<BallSettings>().Bounce = (int)val;
        sldr_Bounce.GetComponentInChildren<Text>().text = val + "/10 Bounce";
    }

    void ChangePath(bool val)
    {
        tggl_Path.isOn = val;
        transform.parent.GetComponent<BallSettings>().Show_Path = val;
    }

    void ChangeCollision(bool val)
    {
        tggl_Collision.isOn = val;
        transform.parent.GetComponent<BallSettings>().Show_Collision = val;
    }

    void ChangeHeight(bool val)
    {
        tggl_Height.isOn = val;
    }

    void Update()
    {
        txt_Ball_Count.text = transform.parent.GetComponent<BallSettings>().Available_Ball.ToString();
        if (transform.parent.GetComponent<BallSettings>().RefreshHeightObj)
        {
            transform.parent.GetComponent<BallSettings>().RefreshHeightObj = false;
            if (tggl_Height.isOn)
            {
                if (Height_Obj.active)
                    Height_Obj.transform.position = new Vector3((transform.parent.GetComponent<BallSettings>().FinalPos.x+transform.parent.GetComponent<BallSettings>().StartingPos.x)/2, transform.parent.GetComponent<BallSettings>().Height+transform.parent.GetComponent<BallSettings>().Floor, 0);
                else
                    Height_Obj = Instantiate(Height_Obj, new Vector3((transform.parent.GetComponent<BallSettings>().FinalPos.x+transform.parent.GetComponent<BallSettings>().StartingPos.x)/2, transform.parent.GetComponent<BallSettings>().Height+transform.parent.GetComponent<BallSettings>().Floor, 0), Quaternion.identity);
            }
        }
    }
}