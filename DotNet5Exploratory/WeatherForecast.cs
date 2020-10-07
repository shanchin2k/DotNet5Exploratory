using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using static DotNet5Exploratory.ApiHelpers.InputFieldStatusConverter;

namespace DotNet5Exploratory
{
    public class WeatherForecast : IHasFieldStatus
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public Dictionary<string, FieldDeserializationStatus> FieldStatus { get; set; }
    }
}
