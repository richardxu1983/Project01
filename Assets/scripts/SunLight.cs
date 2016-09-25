using UnityEngine;

public class SunLight  {

    //用于配置的变量，之后会与天气、季节、温度等关联
    public float m_sunRiseTime;                 //日出时间
    public float m_sunSetTime;                  //日落时间
    public float m_lightIntensityAtNight;       //夜间光照强度
    public float m_lightIntensityAtMidDay;      //正午光照强度
    private float m_sunDefalutZ = 5;            //将来与纬度有关

    //计算中的变量
    public float m_morningPoint;                //上午强光照时间
    public float m_afternoonPoint;              //下午强光照时间
    private float m_morningMinuts1;             //上午分钟数1
    private float m_afternoonMinuts1;           //下午分钟数1
    private float m_afternoonMinuts2;           //下午分钟数2
    private float m_morninglightIntensity;      //上午光照分界点强度
    private float m_afternoonlightIntensity;    //下午光照分界点强度
    private float m_x = 0f;
    private float m_y = 0f;
    private float m_lightIntensity;
    private float m_lightPower;
    private float m_sunAngle;
    private float m_hourInMinuts;               //多少游戏分钟是游戏1小时
    private float m_totalDayMinutes;            //夜晚一共多少分钟

    private GameDayTime m_dayTime;              //游戏时间类
    private Vector3 m_sunPos;                   //太阳的位置


    public SunLight(float sunRiseTime, float sunSetTime, float morningPoint, float afternoonPoint, float lightIntencityAtNight, float lightIntencityAtMidDay, int hourInMinuts, GameDayTime dayTime)
    {
        m_sunRiseTime = sunRiseTime;
        m_sunSetTime = sunSetTime;
        m_morningPoint = morningPoint;
        m_afternoonPoint = afternoonPoint;
        m_lightIntensityAtNight = lightIntencityAtNight;
        m_lightIntensityAtMidDay = lightIntencityAtMidDay;
        m_hourInMinuts = hourInMinuts;
        m_dayTime = dayTime;
        m_lightIntensity = m_lightIntensityAtMidDay;
        m_sunPos = new Vector3();
    }

    public void init()
    {
        //计算分界点时间分钟数
        m_morningMinuts1 = (m_morningPoint - m_sunRiseTime) * m_hourInMinuts;
        m_afternoonMinuts1 = (m_afternoonPoint - 12) * m_hourInMinuts;
        m_afternoonMinuts2 = (m_sunSetTime - m_afternoonPoint) * m_hourInMinuts;

        //计算上午光照分界点强度
        m_x = (m_morningPoint - m_sunRiseTime) / (12 - m_sunRiseTime);
        m_y = -0.3f * (m_x - 1) * (m_x - 1) + 1;
        m_morninglightIntensity = m_y * (m_lightIntensityAtMidDay - m_lightIntensityAtNight) + m_lightIntensityAtNight;

        //计算下午光照分界点强度
        m_x = (m_afternoonPoint - 12) / (m_sunSetTime - 12);
        m_y = -0.3f * (m_x) * (m_x) + 1;
        m_afternoonlightIntensity = (m_lightIntensityAtMidDay - m_lightIntensityAtNight) * m_y + m_lightIntensityAtNight;

        m_totalDayMinutes = (m_sunSetTime - m_sunRiseTime) * m_hourInMinuts;

        m_sunAngle = 3.14f * ((m_dayTime.Minute() + m_dayTime.Hour() * m_hourInMinuts)/(m_totalDayMinutes * m_hourInMinuts));
        m_sunPos.z = m_sunDefalutZ;
        m_sunPos.y = 10 * Mathf.Sin(m_sunAngle);
        m_sunPos.x = 10 * Mathf.Cos(m_sunAngle);
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

    public float LightPower()
    {
        return m_lightPower;
    }

    //
    public void SunMove()
    {
        float hour = m_dayTime.Hour();
        float minute = m_dayTime.Minute();

        if (hour >= m_sunSetTime && hour < 24)
        {
            m_sunAngle = 6.28f * ((minute + (hour - m_sunRiseTime) * m_hourInMinuts) / (24 * m_hourInMinuts));

            //Debug.Log("hour="+ hour+" , angle=" + (minute + hour * m_hourInMinuts) / (m_totalDayMinutes));
            m_sunPos.z = m_sunDefalutZ;
            m_sunPos.y = 10 * Mathf.Sin(m_sunAngle);
            m_sunPos.x = 10 * Mathf.Cos(m_sunAngle);
        }
        else if(hour > 0 && hour < m_sunRiseTime)
        {
            m_sunAngle = 6.28f * ((minute + (hour + 24 - m_sunRiseTime) * m_hourInMinuts) / (24 * m_hourInMinuts));

            //Debug.Log("hour="+ hour+" , angle=" + (minute + hour * m_hourInMinuts) / (m_totalDayMinutes));
            m_sunPos.z = m_sunDefalutZ;
            m_sunPos.y = 10 * Mathf.Sin(m_sunAngle);
            m_sunPos.x = 10 * Mathf.Cos(m_sunAngle);
        }
        else if(hour >= m_sunRiseTime && hour < m_sunSetTime)
        {
            m_sunAngle = 3.14f * ((minute + (hour- m_sunRiseTime) * m_hourInMinuts) / (m_totalDayMinutes));

            //Debug.Log("hour="+ hour+" , angle=" + (minute + hour * m_hourInMinuts) / (m_totalDayMinutes));
            m_sunPos.z = m_sunDefalutZ;
            m_sunPos.y = 10 * Mathf.Sin(m_sunAngle);
            m_sunPos.x = 10 * Mathf.Cos(m_sunAngle);
        }

        SunLightPowerCal();
    }


    //
    public void SunLightPowerCal()
    {
        int hour = m_dayTime.Hour();
        float minutsNow = 0f;

        if (hour >= m_sunSetTime || hour < m_sunRiseTime)
        {
            m_lightPower = m_lightIntensityAtNight / m_lightIntensityAtMidDay;
        }
        else if (hour >= m_sunRiseTime && hour < m_morningPoint)
        {
            minutsNow = m_hourInMinuts * (hour - m_sunRiseTime) + m_dayTime.Minute();
            m_x = minutsNow / (m_morningMinuts1);
            m_lightPower = ((m_morninglightIntensity - m_lightIntensityAtNight) * m_x + m_lightIntensityAtNight) / m_lightIntensityAtMidDay;
        }
        else if (hour >= m_morningPoint && hour < 12)
        {
            minutsNow = m_hourInMinuts * (hour - m_sunRiseTime) + m_dayTime.Minute();
            m_x = minutsNow / ((12 - m_sunRiseTime) * m_hourInMinuts);
            m_y = -0.3f * (m_x - 1) * (m_x - 1) + 1;
            m_lightPower = ((m_lightIntensityAtMidDay - m_lightIntensityAtNight) * m_y + m_lightIntensityAtNight)/ m_lightIntensityAtMidDay;
        }
        else if (hour >= 12 && hour < m_afternoonPoint)
        {
            minutsNow = m_hourInMinuts * (hour - 12) + m_dayTime.Minute();
            m_x = minutsNow / (m_afternoonMinuts1 + m_afternoonMinuts2);
            m_y = -0.3f * (m_x) * (m_x) + 1;
            m_lightPower = ((m_lightIntensityAtMidDay - m_lightIntensityAtNight) * m_y + m_lightIntensityAtNight)/ m_lightIntensityAtMidDay;
        }
        else if (hour >= m_afternoonPoint && hour < m_sunSetTime)
        {
            minutsNow = m_hourInMinuts * (hour - m_afternoonPoint) + m_dayTime.Minute();
            m_x = minutsNow / (m_afternoonMinuts2);
            m_lightPower = (m_afternoonlightIntensity - (m_afternoonlightIntensity - m_lightIntensityAtNight) * m_x) / m_lightIntensityAtMidDay;
        }
    }
}
