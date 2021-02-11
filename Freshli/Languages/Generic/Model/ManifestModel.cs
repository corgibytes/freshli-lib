using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Freshli.Languages.Generic.Model {
  public class ManifestModel {

    [JsonProperty("date_updated")]
    [JsonConverter(typeof(EpochTimeConverter))]
    public DateTime DateUpdated { get; set; }

    [JsonProperty("dependencies")]
    public IList<DependencyModel> Dependencies { get; set; }

    public class EpochTimeConverter : JsonConverter
    {
      public override bool CanConvert(Type objectType)
      {
        return objectType == typeof(double);
      }

      public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
      {
        var t = (double) reader.Value;
        return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(t);
      }

      public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
      {
        throw new NotImplementedException();
      }
    }
  }
}
