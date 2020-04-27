using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using WCA.Domain.Abstractions;
using WCA.Domain.Models.Account;

namespace WCA.Domain.Actionstep
{
    public class ActionstepCredential : EntityBase, IEntityWithId
    {
        public int Id { get; set; }

        public Guid ConcurrencyStamp { get; set; } = Guid.NewGuid();

        // TODO: should probably use DPAPI
        [Required]
        public string AccessToken { get; set; }

        // TODO: should probably use DPAPI
        public string RefreshToken { get; set; }

        [Required]
        public DateTime AccessTokenExpiryUtc { get; set; }

        [Required]
        public DateTime RefreshTokenExpiryUtc { get; set; }

        [Required]
        public int ExpiresIn { get; set; }

        [Required]
        public DateTime ReceivedAtUtc { get; set; }

        [Required]
        public string TokenType { get; set; }

        [Required]
        public Uri ApiEndpoint { get; set; }

        [Required]
        [Display(Description = "The user who granted authorisation to Actionstep and therefore owns this credential")]
        public WCAUser Owner { get; set; }

        [Required]
        public ActionstepOrg ActionstepOrg { get; set; }

        public DateTime LockExpiresAtUtc { get; set; }

        public Guid LockId { get; set; }

        public JwtSecurityToken IdToken { get; set; }

        public DateTime? RevokedAtUtc { get; set; }

        public bool AccessTokenIsValidAndNotExpired(int minimumMinutesTokenMustBeValid)
        {
            if (string.IsNullOrEmpty(AccessToken) || RevokedAtUtc.HasValue)
            {
                return false;
            }

            return DateTime.UtcNow.AddMinutes(minimumMinutesTokenMustBeValid) < AccessTokenExpiryUtc;
        }

        public bool AccessTokenIsValidAndNotExpired()
        {
            return AccessTokenIsValidAndNotExpired(0);
        }

        public bool RefreshTokenIsValidAndNotExpired(int minimumMinutesTokenMustBeValid)
        {
            if (string.IsNullOrEmpty(RefreshToken) || RevokedAtUtc.HasValue)
            {
                return false;
            }

            return DateTime.UtcNow.AddMinutes(minimumMinutesTokenMustBeValid) < RefreshTokenExpiryUtc;
        }

        public bool RefreshTokenIsValidAndNotExpired()
        {
            return RefreshTokenIsValidAndNotExpired(0);
        }
    }
}