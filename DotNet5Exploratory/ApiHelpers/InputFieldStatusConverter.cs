using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DotNet5Exploratory.ApiHelpers
{
    public class InputFieldStatusConverter : JsonConverter
    {        
        public override bool CanConvert(Type objectType)
        {
            // Return false if the input itself is null
            if (objectType == null)
            {
                return false;
            }

            return (objectType.IsClass &&
                        objectType.GetInterfaces().Any(iType => iType == typeof(IHasFieldStatus)));
        }

        
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Return false when the objectType object is null 
            if (objectType == null)
            {
                return false;
            }

            var jsonObj = JObject.Load(reader);
            var targetObj = (IHasFieldStatus)Activator.CreateInstance(objectType);

            var dict = new Dictionary<string, FieldDeserializationStatus>();
            targetObj.FieldStatus = dict;

            foreach (PropertyInfo prop in objectType.GetProperties())
            {

                if (prop.CanWrite && prop.Name != "FieldStatus")
                {
                    // Check if the JSON input value exist or not 
                    if (jsonObj.TryGetValue(prop.Name, StringComparison.OrdinalIgnoreCase, out JToken value))
                    {

                        if (value.Type == JTokenType.Null)
                        {
                            // If the JSON input value explicitly provided as null then set the field status as 1
                            dict.Add(prop.Name, FieldDeserializationStatus.WasSetToNull);
                        }
                        else
                        {
                            // If the JSON input value has value other than null then set the field status as 2
                            // If any error while convert value to object, throw validation exception
                            try
                            {

                                prop.SetValue(targetObj, value.ToObject(prop.PropertyType, serializer));
                                dict.Add(prop.Name, FieldDeserializationStatus.HasValue);
                            }
                            catch (JsonReaderException exception)
                            {
                                throw new ValidationException(new ValidationResult(exception.Path, new string[] { exception.Path }), null, null);
                            }
                            // If the user gives JSON input as non-string value to string property, then conversion gets failed and the below catch block will be executed
                            catch (ArgumentException)
                            {
                                // If the exception message related to data type conversion e.g. from double to string or integer to string, then set the field status to the corresponding property
                                // and proceed further. And this conversion exception will be caught during validate the input model check done in RequestValidationAttribue class and thrown
                                dict.Add(prop.Name, FieldDeserializationStatus.HasValue);

                            }
                        }
                    }
                    else
                    {
                        // If the input tag is not specified in JSON input then set the field status as 0
                        dict.Add(prop.Name, FieldDeserializationStatus.WasNotPresent);
                    }
                }
            }

            return targetObj;
        }

        /// <summary>
        /// Enum used to assign the status value to required entity attributes 
        /// Status values are:  WasNotPresent – 0, WasSetToNull - 1,  HasValue  - 2
        /// </summary>
        public enum FieldDeserializationStatus
        {
            WasNotPresent = 0,
            WasSetToNull = 1,
            HasValue = 2
        }

        /// <summary>
        /// Abstract member of JsonConverter base class must be Override  
        /// </summary>
        public override bool CanWrite
        {
            get { return false; }
        }

        /// <summary>
        /// Abstract member of JsonConverter base class must be Override 
        /// The parameters passed to this method is not in use but present here due base class method signature.
        /// </summary>
        /// <param name="writer"> JsonWriter object</param>
        /// <param name="value"> Input object value</param>
        /// <param name="serializer"> JsonSerializer object</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
