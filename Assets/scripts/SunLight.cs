using UnityEngine;

public class SunLight  {

    //用于配置的变量，之后会与天气、季节、温度等关联
    public float m_sunRiseTime;                 //日出时间
    public float m_sunSetTime;                  //日落时间
    public float m_lightIntensityAtNight;       //夜间光照强度
    public float m_lightIntensityAtMidDay;      //正午光照强度
    private float m_sunDefalutZ = 50;            //将来与纬度有关

    //太阳颜色
    private Color m_SunRiseColor;
    private Color m_SunColor;
    private Color m_color;

    //计算中的变量
    private float m_lightIntensity;
    private float m_sunAngle;
    private float m_hourInMinuts;               //多少游戏分钟是游戏1小时
    private float m_totalDayMinutes;            //夜晚一共多少分钟

    private float m_latitude;

    private GameDayTime m_dayTime;              //游戏时间类
    private Vector3 m_sunPos;                   //太阳的位置


    public SunLight(float sunRiseTime, float sunSetTime, int hourInMinuts, GameDayTime dayTime, float lightIntensityAtMidDay, Color SunColor, Color SunRiseColor)
    {
        m_sunRiseTime = sunRiseTime;
        m_sunSetTime = sunSetTime;
        m_hourInMinuts = hourInMinuts;
        m_dayTime = dayTime;
        m_SunColor = SunColor;
        m_SunRiseColor = SunRiseColor;
        m_totalDayMinutes = (sunSetTime - sunRiseTime) * hourInMinuts;
        m_lightIntensity = lightIntensityAtMidDay;
        m_sunPos = new Vector3();
    }

    public void init( float latitude)
    {
        m_latitude = latitude;
        m_sunPos.z = m_sunDefalutZ;
        SunPosCal();
    }

    //
    public float LightIntensity()
    {
        return m_lightIntensity;
    }

    public Vector3 SunPos()
    {
        return m_sunPos;
    }

    public Color SunColor()
    {
        return m_color;
    }

    //
    public void SunMove()
    {
        //太阳位置计算
        SunPosCal();

        //太阳的颜色
        SunColorCal();
    }

    void SunColorCal()
    {
        float hour = m_dayTime.Hour();
        if ((hour>= m_sunRiseTime&& hour<= m_sunRiseTime+2))
        {
            m_color = m_SunRiseColor;
        }
        else if((hour >= m_sunSetTime-2 && hour <= m_sunSetTime))
        {
            m_color = m_SunRiseColor;
        }
        else
        {
            m_color = m_SunColor;
        }
    }


    //
    void SunPosCal()
    {
        float hour = m_dayTime.Hour();
        float minute = m_dayTime.Minute();

        if (hour >= m_sunSetTime && hour < 24)
        {
            m_sunAngle = 6.28f * ((minute + (hour - m_sunRiseTime) * m_hourInMinuts) / m_dayTime.MinutsInDay());

            //Debug.Log("hour="+ hour+" , angle=" + (minute + hour * m_hourInMinuts) / (m_totalDayMinutes));
            m_sunPos.y = 100 * Mathf.Sin(m_sunAngle);
            m_sunPos.x = 100 * Mathf.Cos(m_sunAngle);
        }
        else if (hour > 0 && hour < m_sunRiseTime)
        {
            m_sunAngle = 6.28f * ((minute + (hour + 24 - m_sunRiseTime) * m_hourInMinuts) / m_dayTime.MinutsInDay());

            //Debug.Log("hour="+ hour+" , angle=" + (minute + hour * m_hourInMinuts) / (m_totalDayMinutes));
            m_sunPos.y = 100 * Mathf.Sin(m_sunAngle);
            m_sunPos.x = 100 * Mathf.Cos(m_sunAngle);
        }
        else if (hour >= m_sunRiseTime && hour < m_sunSetTime)
        {
            m_sunAngle = 3.14f * ((minute + (hour - m_sunRiseTime) * m_hourInMinuts) / (m_totalDayMinutes));

            //Debug.Log("hour="+ hour+" , angle=" + (minute + hour * m_hourInMinuts) / (m_totalDayMinutes));
            m_sunPos.y = 100 * Mathf.Sin(m_sunAngle);
            m_sunPos.x = 100 * Mathf.Cos(m_sunAngle);
        }
    }
}
