
namespace Ordering.Application.Dtos;
public record class AddressDto(
    string FirstName, 
    string LastName,
    string EmailAddress,
    string AddressLine, 
    string State,
    string Country,
    string ZipCode
    
    );
