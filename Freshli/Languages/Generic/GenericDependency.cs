using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Freshli.Languages.Generic {
  public class GenericDependency {
    [JsonProperty("name")] public string Name { get; set; }

    [JsonProperty("allows_prerelease")]
    public bool? AllowsPrerelease { get; set; }

    [JsonProperty("specs")]
    public IList<Spec> VersionSpecs { get; set; }

    public enum Operator {
      Equals,
      LessThan,
      LessThanOrEqualTo,
      GreaterThan,
      GreaterThanOrEqualTo,
      LatestMinor,
      Unknown
    }

    public static Operator FromStringToOperator(string op) {
      return op switch {
        "=" => Operator.Equals,
        "<" => Operator.LessThan,
        "<=" => Operator.LessThanOrEqualTo,
        ">" => Operator.GreaterThan,
        ">=" => Operator.GreaterThanOrEqualTo,
        "latest" => Operator.LatestMinor,
        _ => Operator.Unknown
      };
    }

    public class Spec {
      [JsonProperty("operator")]
      [JsonConverter(typeof(OperatorConverter))]
      public Operator Op { get; set; }
      [JsonProperty("version")] public string Version { get; set; }
    }

    private class OperatorConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
          throw new NotImplementedException("This functionality is not implemented in Freshli.");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var enumString = (string)reader.Value;

            return FromStringToOperator(enumString);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }
    }
  }
}
