﻿using System;
using Translation.Common.Helpers;
using Translation.Common.Models.Base;

namespace Translation.Common.Models.Requests.User
{
    public sealed class UserAcceptInviteRequest : BaseRequest
    {
        public Guid Token { get; }
        public string Email { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Password { get; }

        public UserAcceptInviteRequest(Guid token, string email, string firstName,
                                       string lastName, string password)
        {
            if (token.IsEmptyGuid())
            {
                ThrowArgumentException(nameof(token), token);
            }

            if (email.IsNotEmail())
            {
                ThrowArgumentException(nameof(email), email);
            }

            if (firstName.IsEmpty())
            {
                ThrowArgumentException(nameof(firstName), firstName);
            }

            if (lastName.IsEmpty())
            {
                ThrowArgumentException(nameof(lastName), lastName);
            }

            if (password.IsNotValidPassword())
            {
                ThrowArgumentException(nameof(password), "");
            }

            Token = token;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Password = password;
        }
    }
}

