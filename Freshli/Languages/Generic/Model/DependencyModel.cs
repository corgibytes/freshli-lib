using System;
using System.Collections.Generic;
using Freshli.Exceptions;
using Newtonsoft.Json;

namespace Freshli.Languages.Generic.Model {
  public class DependencyModel {
    [JsonProperty("name")] public string Name { get; set; }

    [JsonProperty("allows_prerelease")]
    public bool? AllowsPrerelease { get; set; }

    [JsonProperty("specs")]
    public IList<Spec> VersionSpecs { get; set; }

    public string ConvertSpecsToString() {
      return string.Join(',', VersionSpecs);
    }

    public static IList<Spec> ParseSpecsFromString(string aggregatedSpecs) {
      var specs = new List<Spec>();

      foreach (var specPair in aggregatedSpecs.Split(',')) {
        var splitSpec = specPair.Split(' ');
        if (splitSpec.Length != 2) {
          throw new VersionParseException(specPair);
        }

        var spec = new Spec {
          Op = GenericOperator.FromStringToOperator(splitSpec[0]),
          Version = splitSpec[1]
        };

        specs.Add(spec);
      }

      return specs;
    }

    public class Spec {
      [JsonProperty("operator")]
      [JsonConverter(typeof(OperatorConverter))]
      public GenericOperator.Operator Op { get; set; }
      [JsonProperty("version")] public string Version { get; set; }

      public override string ToString() {
        return GenericOperator.FromOperatorToString(Op) + " " + Version;
      }
    }

    private class OperatorConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
          throw new NotImplementedException("This functionality is not implemented in Freshli.");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var enumString = (string)reader.Value;

            return GenericOperator.FromStringToOperator(enumString);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }
    }
  }
}
