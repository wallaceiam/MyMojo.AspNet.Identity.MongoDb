using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyMojo.AspNet.Identity.MongoDb
{

    /// <summary>
    /// The default implementation of <see cref="IdentityUser{TKey}"/> which uses a string as a primary key.
    /// </summary>
    public class IdentityUser : IdentityUser<string>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="IdentityUser"/>.
        /// </summary>
        /// <remarks>
        /// The Id property is initialized to from a new GUID string value.
        /// </remarks>
        public IdentityUser()
        {
            Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="IdentityUser"/>.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <remarks>
        /// The Id property is initialized to from a new GUID string value.
        /// </remarks>
        public IdentityUser(string userName) : this()
        {
            UserName = userName;
        }
    }

    /// <summary>
    /// Represents a user in the identity system
    /// </summary>
    /// <typeparam name="TKey">The type used for the primary key for the user.</typeparam>
    public class IdentityUser<TKey> : IdentityUser<TKey, IdentityUserClaim<TKey>, IdentityUserRole<TKey>, IdentityUserLogin<TKey>, IdentityUserToken<TKey>>
        where TKey : IEquatable<TKey>
    { }

    /// <summary>
    /// Represents a user in the identity system
    /// </summary>
    /// <typeparam name="TKey">The type used for the primary key for the user.</typeparam>
    /// <typeparam name="TUserClaim">The type representing a claim.</typeparam>
    /// <typeparam name="TUserRole">The type representing a user role.</typeparam>
    /// <typeparam name="TUserLogin">The type representing a user external login.</typeparam>
    public class IdentityUser<TKey, TUserClaim, TUserRole, TUserLogin, TUserToken> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="IdentityUser{TKey}"/>.
        /// </summary>
        public IdentityUser() { }

        /// <summary>
        /// Initializes a new instance of <see cref="IdentityUser{TKey}"/>.
        /// </summary>
        /// <param name="userName">The user name.</param>
        public IdentityUser(string userName) : this()
        {
            UserName = userName;
        }

        /// <summary>
        /// Gets or sets the primary key for this user.
        /// </summary>
        public virtual TKey Id { get; set; }

        /// <summary>
        /// Gets or sets the user name for this user.
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// Gets or sets the normalized user name for this user.
        /// </summary>
        public virtual string NormalizedUserName { get; set; }

        /// <summary>
        /// Gets or sets the email address for this user.
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// Gets or sets the normalized email address for this user.
        /// </summary>
        public virtual string NormalizedEmail { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if a user has confirmed their email address.
        /// </summary>
        /// <value>True if the email address has been confirmed, otherwise false.</value>
        public virtual bool EmailConfirmed { get; set; }

        /// <summary>
        /// Gets or sets a salted and hashed representation of the password for this user.
        /// </summary>
        public virtual string PasswordHash { get; set; }

        /// <summary>
        /// A random value that must change whenever a users credentials change (password changed, login removed)
        /// </summary>
        public virtual string SecurityStamp { get; set; }

        /// <summary>
        /// A random value that must change whenever a user is persisted to the store
        /// </summary>
        public virtual string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets a telephone number for the user.
        /// </summary>
        public virtual string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if a user has confirmed their telephone address.
        /// </summary>
        /// <value>True if the telephone number has been confirmed, otherwise false.</value>
        public virtual bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if two factor authentication is enabled for this user.
        /// </summary>
        /// <value>True if 2fa is enabled, otherwise false.</value>
        public virtual bool TwoFactorEnabled { get; set; }

        /// <summary>
        /// Gets or sets the date and time, in UTC, when any user lockout ends.
        /// </summary>
        /// <remarks>
        /// A value in the past means the user is not locked out.
        /// </remarks>
        public virtual DateTimeOffset? LockoutEnd { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if the user could be locked out.
        /// </summary>
        /// <value>True if the user could be locked out, otherwise false.</value>
        public virtual bool LockoutEnabled { get; set; }

        /// <summary>
        /// Gets or sets the number of failed login attempts for the current user.
        /// </summary>
        public virtual int AccessFailedCount { get; set; }

        /// <summary>
        /// Navigation property for the roles this user belongs to.
        /// </summary>
        public virtual ICollection<TUserRole> Roles { get; } = new List<TUserRole>();

        /// <summary>
        /// Navigation property for the claims this user possesses.
        /// </summary>
        public virtual ICollection<TUserClaim> Claims { get; } = new List<TUserClaim>();

        /// <summary>
        /// Navigation property for this users login accounts.
        /// </summary>
        public virtual ICollection<TUserLogin> Logins { get; } = new List<TUserLogin>();

        /// <summary>
        /// Navigation property for this users login accounts.
        /// </summary>
        public virtual ICollection<TUserToken> Tokens { get; } = new List<TUserToken>();

        /// <summary>
        /// Returns the username for this user.
        /// </summary>
        public override string ToString()
        {
            return UserName;
        }
    }


    public class IdentityUserClaim<TKey> where TKey : IEquatable<TKey>
    {
        public IdentityUserClaim()
        {

        }

        //
        // Summary:
        //     Gets or sets the claim type for this claim.
        public virtual string ClaimType { get; set; }
        //
        // Summary:
        //     Gets or sets the claim value for this claim.
        public virtual string ClaimValue { get; set; }
        //
        // Summary:
        //     Gets or sets the identifier for this user claim.
        public virtual int Id { get; set; }

        //
        // Summary:
        //     Reads the type and value from the Claim.
        //
        // Parameters:
        //   claim:
        public virtual void InitializeFromClaim(Claim claim)
        {
            this.ClaimType = claim.Type;

            this.ClaimValue = claim.Value;
        }
        //
        // Summary:
        //     Converts the entity into a Claim instance.
        public virtual Claim ToClaim()
        {
            return new Claim(ClaimType, ClaimValue);
        }
    }


    /// <summary>
    /// Represents the link between a user and a role.
    /// </summary>
    /// <typeparam name="TKey">The type of the primary key used for users and roles.</typeparam>
    public class IdentityUserRole<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Gets or sets the primary key of the role that is linked to the user.
        /// </summary>
        public virtual string Role { get; set; }
    }

    /// <summary>
    /// Represents a login and its associated provider for a user.
    /// </summary>
    /// <typeparam name="TKey">The type of the primary key of the user associated with this login.</typeparam>
    public class IdentityUserLogin<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Gets or sets the login provider for the login (e.g. facebook, google)
        /// </summary>
        public virtual string LoginProvider { get; set; }

        /// <summary>
        /// Gets or sets the unique provider identifier for this login.
        /// </summary>
        public virtual string ProviderKey { get; set; }

        /// <summary>
        /// Gets or sets the friendly name used in a UI for this login.
        /// </summary>
        public virtual string ProviderDisplayName { get; set; }
    }

    /// <summary>
    /// Represents an authentication token for a user.
    /// </summary>
    /// <typeparam name="TKey">The type of the primary key used for users.</typeparam>
    public class IdentityUserToken<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Gets or sets the LoginProvider this token is from.
        /// </summary>
        public virtual string LoginProvider { get; set; }

        /// <summary>
        /// Gets or sets the name of the token.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the token value.
        /// </summary>
        public virtual string Value { get; set; }
    }

}
