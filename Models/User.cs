using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TodoApp.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        public string Email { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public bool IsVerified { get; set; } = false;
        public string? OtpCode { get; set; }
        public DateTime? OtpExpiry { get; set; }
        public string RefreshToken { get; set; } = null!;
     public DateTime RefreshTokenExpiryTime { get; set; }


    }
}
