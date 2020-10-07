using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DotNet5Exploratory.ApiHelpers.InputFieldStatusConverter;

namespace DotNet5Exploratory
{
    /// <summary>
    /// Interface to implement field status in required entities
    /// </summary>
    public interface IHasFieldStatus
    {        
        Dictionary<string, FieldDeserializationStatus> FieldStatus { get; set; }
    }
}
