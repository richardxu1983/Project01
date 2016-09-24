

public class SunLight  {

    public float m_sunRiseTime;       //日出时间
    public float m_sunSetTime;       //日落时间
    public float m_morningPoint;     //上午强光照时间
    public float m_afternoonPoint;     //下午强光照时间
    public float m_lightIntensityAtNight;   //夜间光照强度
    public float m_lightIntensityAtMidDay;   //正午光照强度
    private float m_morningMinuts1;        //上午分钟数
    private float m_afternoonMinuts1;      //下午分钟数
    private float m_morningMinuts2;        //上午分钟数
    private float m_afternoonMinuts2;      //下午分钟数
    private float m_morninglightIntensity;  //上午光照分界点
    private float m_afternoonlightIntensity;  //下午光照分界点
    private float m_x = 0f;
    private float m_y = 0f;
    private float m_lightPower;
    private float m_lightIntensity;
    private float m_sunAngle;
    private int m_hourInMinuts = 60;     //多少游戏分钟是游戏1小时
    private GameDayTime m_dayTime;      //游戏时间类

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
    }

    public void init()
    {
        //计算分界点时间分钟数
        m_morningMinuts1 = (m_morningPoint - m_sunRiseTime) * m_hourInMinuts;
        m_afternoonMinuts1 = (m_afternoonPoint - 12) * m_hourInMinuts;
        m_morningMinuts2 = (12 - m_morningPoint) * m_hourInMinuts;
        m_afternoonMinuts2 = (m_sunSetTime - m_afternoonPoint) * m_hourInMinuts;

        //计算上午光照分界点强度
        m_x = (m_morningPoint - m_sunRiseTime) / (12 - m_sunRiseTime);
        m_y = -0.3f * (m_x - 1) * (m_x - 1) + 1;
        m_morninglightIntensity = m_y * (m_lightIntensityAtMidDay - m_lightIntensityAtNight) + m_lightIntensityAtNight;

        //计算下午光照分界点强度
        m_x = (m_afternoonPoint - 12) / (m_sunSetTime - 12);
        m_y = -0.3f * (m_x) * (m_x) + 1;
        m_afternoonlightIntensity = (m_lightIntensityAtMidDay - m_lightIntensityAtNight) * m_y + m_lightIntensityAtNight;

    }

    //
    public float LightPower()
    {
        return m_lightPower;
    }

    //
    public float LightIntensity()
    {
        return m_lightIntensity;
    }

    public float SunAngle()
    {
        return m_sunAngle;
    }


    //
    public void SunLightIntensity()
    {
        int hour = m_dayTime.Hour();
        float minutsNow = 0f;
        float x;
        float y;

        if (hour >= m_sunSetTime || hour < m_sunRiseTime)
        {
            m_lightPower = 0;
            m_sunAngle = 0;
            m_lightIntensity = m_lightIntensityAtNight;
            //Debug.Log("night time, m_light.intensity="+ m_light.intensity);
        }
        else if (hour >= m_sunRiseTime && hour < m_morningPoint)
        {
            minutsNow = m_hourInMinuts * (hour - m_sunRiseTime) + m_dayTime.Minute();
            x = minutsNow / (m_morningMinuts1);
            m_lightIntensity = (m_morninglightIntensity) * x + m_lightIntensityAtNight;
            m_sunAngle = 90 * minutsNow / (m_morningMinuts1 + m_morningMinuts2);
            //Debug.Log("morning time ,x = " + x + ", m_light.intensity="+ m_light.intensity+ ", m_morninglightIntensity="+ m_morninglightIntensity);
        }
        else if (hour >= m_morningPoint && hour < 12)
        {
            minutsNow = m_hourInMinuts * (hour - m_sunRiseTime) + m_dayTime.Minute();
            x = minutsNow / (m_morningMinuts1 + m_morningMinuts2);
            y = -0.3f * (x - 1) * (x - 1) + 1;
            m_lightIntensity = (m_lightIntensityAtMidDay - m_lightIntensityAtNight) * y + m_lightIntensityAtNight;
            m_sunAngle = 90 * x;
            //Debug.Log("morning time ,x = " + x + ", m_light.intensity=" + m_light.intensity);
        }
        else if (hour >= 12 && hour < m_afternoonPoint)
        {
            minutsNow = m_hourInMinuts * (hour - 12) + m_dayTime.Minute();
            x = minutsNow / (m_afternoonMinuts1 + m_afternoonMinuts2);
            y = -0.3f * (x) * (x) + 1;
            m_lightIntensity = (m_lightIntensityAtMidDay - m_lightIntensityAtNight) * y + m_lightIntensityAtNight;
            m_sunAngle = 90 * x + 90;
            //Debug.Log("afternoon time , m_light.intensity=" + m_light.intensity);
        }
        else if (hour >= m_afternoonPoint && hour < m_sunSetTime)
        {
            minutsNow = m_hourInMinuts * (hour - m_afternoonPoint) + m_dayTime.Minute();
            x = minutsNow / (m_afternoonMinuts2);
            m_lightIntensity = m_afternoonlightIntensity - (m_afternoonlightIntensity - m_lightIntensityAtNight) * x;
            minutsNow = m_hourInMinuts * (hour - 12) + m_dayTime.Minute();
            m_sunAngle = 90 * minutsNow / (m_afternoonMinuts2 + m_afternoonMinuts1) + 90;
            //Debug.Log("afternoon time , m_light.intensity=" + m_light.intensity);
        }
    }
}
