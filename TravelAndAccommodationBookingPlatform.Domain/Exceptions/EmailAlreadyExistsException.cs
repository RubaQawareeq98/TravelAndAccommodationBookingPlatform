namespace TravelAndAccommodationBookingPlatform.Domain.Exceptions;

public class EmailAlreadyExistsException(string message) : Exception(message);
