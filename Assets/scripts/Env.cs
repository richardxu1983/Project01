using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Env : MonoBehaviour {

    //用于配置的变量
    public int m_minInTick;             //多少个逻辑帧过游戏里的1分钟
    public int m_hourInMinuts = 60;     //多少游戏分钟是游戏1小时
    public int m_initHour = 3;          //初始的小时
    public int m_monthInDays = 15;      //一个月多少天

    //地理位置信息，游戏数据，需要存储
    public float m_elevation = 0f;      //海拔
    public float m_latitude = 30f;      //纬度
    public float m_longitude = 30f;     //经度

    //温度信息，之后是根据地理位置自动生成
    [Range(0f, 50f)]
    public float m_TRange = 15f;
    [Range(-50f, 40f)]
    public float m_TAverage = 15f;
    [Range(0.001f, 0.03f)]
    public float m_TC = 0.003f;

    //用于配置的变量，之后会与天气、季节、温度等关联
    public float m_sunRiseTime = 5;                     //日出时间
    public float m_sunSetTime = 20;                     //日落时间
    public float m_lightIntensityAtNight = 0.1f;        //夜间光照强度
    public float m_lightIntensityAtMidDay = 0.75f;      //正午光照强度


    public Text m_timeInfoText;         //文字控件
    public Light m_light;

    private float m_temperatureNow = 0;
    private float m_temperatureLast = 0;
    private float m_temperatureSunNow = 0;
    private float m_temperatureSunLast = 0;

    private GameDayTime m_dayTime;      //游戏时间
    private SunLight m_sunLight;        //游戏里的太阳

    // Use this for initialization
    void Start ()
    {
        //时间
        m_dayTime = new GameDayTime(m_minInTick, m_hourInMinuts, m_initHour, m_monthInDays);
        m_dayTime.initTime();
        m_dayTime.notifier += new GameDayTime.Notify(OnChangeMinute);

        //太阳
        m_sunLight = new SunLight(m_sunRiseTime, m_sunSetTime, m_hourInMinuts, m_dayTime, m_lightIntensityAtMidDay);
        m_sunLight.init();

        m_light.intensity = m_sunLight.LightIntensity();
        m_light.transform.position = m_sunLight.SunPos();

        //温度
        m_temperatureNow = SunTemperatureCal();
        m_temperatureLast = m_temperatureNow;
        //print("m_temperatureNow = " + m_temperatureNow);

        RefreshTextUI();
    }
	
	// Update is called once per frame
	void Update ()
    {
        SunMove();

        //for Debug
        if (Input.GetKeyDown(KeyCode.Q))
        {
            m_dayTime.DebugAddHour(2);
        }
    }

    //This function is called every fixed framerate frame, if the MonoBehaviour is enabled.FixedUpdate should be used instead of Update when dealing with Rigidbody.For example when adding a force to a rigidbody, you have to apply the force every fixed frame inside FixedUpdate instead of every frame inside Update.
    void FixedUpdate()
    {
        m_dayTime.StepTickTime();
    }

    //
    public GameDayTime GetTime()
    {
        return m_dayTime;
    }

    //
    public float GetTemperature()
    {
        return m_temperatureNow;
    }

    //
    void SunMove()
    {
        m_light.transform.position = Vector3.Lerp(m_light.transform.position, m_sunLight.SunPos(), Time.deltaTime);
        //print(transform.position + " , " + m_sunLight.SunPos());
        m_light.transform.LookAt(Vector3.zero);
    }

    //分钟变化的时候
    void OnChangeMinute()
    {

        //m_sunLight.SunLightIntensity();
        m_sunLight.SunMove();

        //
        temperatureCal();

        //
        RefreshTextUI();
    }

    //
    void RefreshTextUI()
    {
        //刷新界面文字
        m_timeInfoText.text = m_temperatureNow.ToString("f0") + "C" + "\n" + m_dayTime.TimeString();// + "\n" + m_temperatureSunNow.ToString("f1");
    }

    //
    void temperatureCal()
    {
        //计算太阳温度
        SunTemperatureCal();

        //计算环境影响后的温度
        EnvTemperatureCal();

        //计算地面温度
        GroundTemperatureCal();
    }

    //
    float SunTemperatureCal()
    {
        m_temperatureSunLast = m_temperatureSunNow;
        float x = (m_dayTime.MinutNow() / m_dayTime.MinutsInDay()) * 3.14f;
        m_temperatureSunNow = Mathf.Sin(x) * m_TRange + (m_TAverage - m_TRange/2);
        return m_temperatureSunNow;
    }

    float EnvTemperatureCal()
    {
        return m_temperatureSunNow;
    }

    float GroundTemperatureCal()
    {
        m_temperatureLast = m_temperatureNow;
        m_temperatureNow = (m_temperatureSunLast - m_temperatureLast) * m_TC + m_temperatureLast;
        return m_temperatureNow;
    }
}
