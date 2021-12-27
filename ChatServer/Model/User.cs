using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace ChatServer.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Salt { get; set; }

        public List<Conversation> Conversations { get; set; } = new();
        public List<Message> Messages { get; set; } = new();
        
        public User() { }

        public User(string username, string password)
        {
            Username = username;
            (Password, Salt) = GenerateHashedPassword(password);
        }

        public bool CheckPassword(string attemptedPassword)
        {
            string hashedAttemptedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: attemptedPassword,
                salt: Convert.FromBase64String(Salt),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 32
            ));
            
            return Password == hashedAttemptedPassword;
        }

        private (string, string) GenerateHashedPassword(string password)
        {
            var salt = new byte[16];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }

            string saltStr = Convert.ToBase64String(salt);

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 32
            ));

            return (hashed, saltStr);
        }

        protected bool Equals(User other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((User) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}