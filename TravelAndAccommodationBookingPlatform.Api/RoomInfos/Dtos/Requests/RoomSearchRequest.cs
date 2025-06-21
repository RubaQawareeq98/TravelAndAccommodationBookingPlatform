namespace TravelAndAccommodationBookingPlatform.Api.RoomInfos.Dtos.Requests;

public class RoomSearchRequest
{
    public string? Filters { get; set; }
    public string? Sorts { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
    public List<Guid>? AmenitiesIds { get; set; }
}
