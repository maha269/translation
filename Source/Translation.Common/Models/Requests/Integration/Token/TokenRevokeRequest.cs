﻿using System;

using Translation.Common.Helpers;
using Translation.Common.Models.Base;

namespace Translation.Common.Models.Requests.Integration.Token
{
    public class TokenRevokeRequest : BaseAuthenticatedRequest
    {
        public Guid IntegrationClientUid { get; set; }
        public Guid Token { get; }

        public TokenRevokeRequest(long currentUserId, Guid token, Guid integrationClientUid) : base(currentUserId)
        {
            if (integrationClientUid.IsEmptyGuid())
            {
                ThrowArgumentException(nameof(integrationClientUid), integrationClientUid);
            }

            if (token.IsEmptyGuid())
            {
                ThrowArgumentException(nameof(token), token);
            }

            Token = token;
            IntegrationClientUid = integrationClientUid;
        }
    }
}