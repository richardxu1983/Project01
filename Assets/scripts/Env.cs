﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Env : MonoBehaviour {

    
    public int m_minInTick;             //多少个逻辑帧过游戏里的1分钟
    public int m_hourInMinuts = 60;     //多少游戏分钟是游戏1小时
    public int m_initHour = 3;          //初始的小时
    public int m_monthInDays = 15;      //一个月多少天

    public float m_sunRiseTime = 5;       //日出时间
    public float m_sunSetTime = 20;       //日落时间
    public float m_morningPoint = 10;     //上午强光照时间
    public float m_afternoonPoint = 16;     //下午强光照时间
    public float m_lightIntensityAtNight = 0.1f;   //夜间光照强度
    public float m_lightIntensityAtMidDay = 0.75f;   //正午光照强度


    public Text m_timeInfoText;         //文字控件
    public Light m_light;

    private int m_temperature;

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
        m_sunLight = new SunLight(m_sunRiseTime, m_sunSetTime, m_morningPoint, m_afternoonPoint, m_lightIntensityAtNight, m_lightIntensityAtMidDay, m_hourInMinuts, m_dayTime);
        m_sunLight.init();

        //
        OnChangeMinute();
    }
	
	// Update is called once per frame
	void Update ()
    {

    }

    //This function is called every fixed framerate frame, if the MonoBehaviour is enabled.FixedUpdate should be used instead of Update when dealing with Rigidbody.For example when adding a force to a rigidbody, you have to apply the force every fixed frame inside FixedUpdate instead of every frame inside Update.
    void FixedUpdate()
    {
        m_dayTime.StepTickTime();
    }

    //
    public GameDayTime Time()
    {
        return m_dayTime;
    }

    //分钟变化的时候
    void OnChangeMinute()
    {
        //刷新界面文字
        m_timeInfoText.text = m_dayTime.TimeString();

        //
        m_sunLight.SunLightIntensity();

        //
        m_light.intensity = m_sunLight.LightIntensity();

        Vector3 vec = new Vector3(m_sunLight.SunAngle(), 45, 0);

        m_light.transform.eulerAngles = vec;

        //Debug.Log(m_sunLight.SunAngle());
    }
}
