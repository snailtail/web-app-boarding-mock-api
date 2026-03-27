using System.Text.Json.Serialization;

namespace MockServer.Models.Checklist;

public record OngoingEmployeeChecklists(List<OngoingEmployeeChecklist> Checklists)
{
    [JsonPropertyName("_meta")]
    public PagingMetaData? Meta { get; init; }
}
