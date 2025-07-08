using System.Net.Http.Headers;
using TravelAndAccommodationBookingPlatform.Domain.Enums;

namespace TAABP.integrationTests.Shared;

public abstract class TestAuthenticationHeader
{
    public static void SetTestAuthHeader(HttpClient client, Guid userId, UserRole role)
    {
        client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("TestAuthScheme", $"{userId}|{role.ToString()}");
    }
}
