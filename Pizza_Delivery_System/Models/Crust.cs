using System.Text.Json.Serialization;

namespace Pizza_Delivery_System.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Crust
    {
        StuffedCrust,
        CrackerCrust,
        FlatBreadCrust,
        ThinCrust,
        ThickCrust,
        CrispyCrust
    }
}
