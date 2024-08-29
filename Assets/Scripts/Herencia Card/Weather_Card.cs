using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weather Card", menuName = "Weather Card")]
public class Weather_Card : Card
{
    public EWeatherType WeatherType;
    public enum EWeatherType
    {
        Rain,
        Fog,
        Frost
    }
}
