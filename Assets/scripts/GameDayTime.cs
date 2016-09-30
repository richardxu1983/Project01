﻿

public class GameDayTime {

    private int m_minInTick;             //多少个逻辑帧过游戏里的1分钟
    private int m_hourInMinuts = 60;     //多少游戏分钟是游戏1小时
    private int m_initHour = 3;          //初始的小时
    private int m_monthInDays = 15;      //一个月多少天
    private int m_dayInHours = 24;      //一天24小时
    private int m_yearInMonths = 12;    //一年12个月
    private float m_MinutsInDay;
    private float m_MinuteNowInDay;
    private int m_year;
    private int m_minute;
    private int m_hour;
    private int m_day;
    private int m_month;
    private int m_tick;

    public delegate void Notify();
    public event Notify notifier;

    //
    public GameDayTime(int minInTick, int hourInMinuts, int initHour, int monthInDays)
    {
        m_minInTick = minInTick;
        m_hourInMinuts = hourInMinuts;
        m_initHour = initHour;
        m_monthInDays = monthInDays;
    }

    //
    //init the time
    public void initTime()
    {
        m_minute = 0;
        m_tick = 0;
        m_day = 1;
        m_month = 1;
        m_year = 3001;
        m_hour = m_initHour;
        m_MinutsInDay = m_dayInHours * m_hourInMinuts;
        m_MinuteNowInDay = m_minute + m_hour * m_hourInMinuts;
    }

    //
    public float MinutsInDay()
    {
        return m_MinutsInDay;
    }

    //
    public float MinutNow()
    {
        return m_MinuteNowInDay;
    }

    //Step Tick Time
    public void StepTickTime()
    {
        m_tick++;
        if (m_tick >= m_minInTick)
        {
            m_tick = 0;
            m_minute++;
            
            if (m_minute >= m_hourInMinuts)
            {
                m_minute = 0;
                m_hour++;
                if (m_hour >= m_dayInHours)
                {
                    m_hour = 0;
                    m_day++;
                    if (m_day >= m_monthInDays)
                    {
                        m_day = 0;
                        m_month++;
                        if (m_month >= m_yearInMonths)
                        {
                            m_month = 0;
                            m_year++;
                        }
                    }
                }
            }
            m_MinuteNowInDay = m_minute + m_hour * m_hourInMinuts;
            notifier.Invoke();
        }
    }

    public string TimeString()
    {
        return m_hour.ToString("d2") + ":" + m_minute.ToString("d2")  + "\n" + m_month + " month - " + m_day + " , " + m_year;
    }

    //
    public int Year()
    {
        return m_year;
    }

    public int Month()
    {
        return m_month;
    }

    public int Day()
    {
        return m_day;
    }

    public int Hour()
    {
        return m_hour;
    }

    public int Minute()
    {
        return m_minute;
    }

    //
    public void DebugSetHour(int hour)
    {
        m_hour = hour;
    }

    //
    public void DebugAddHour(int hour)
    {
        m_hour += hour;
        if(m_hour>=24)
        {
            m_hour = 0;
        }
    }
}
