﻿using System;
using System.Collections.Generic;

using Translation.Common.Helpers;
using Translation.Common.Models.Base;

namespace Translation.Common.Models.Requests.Label
{
    public sealed class LabelCreateListRequest : BaseAuthenticatedRequest
    {
        public Guid OrganizationUid { get; set; }
        public Guid ProjectUid { get; }
        public List<LabelListInfo> Labels { get; set; }

        public LabelCreateListRequest(long currentUserId, Guid organizationUid, Guid projectUid,
                                      List<LabelListInfo> labels) : base(currentUserId)
        {
            if (organizationUid.IsEmptyGuid())
            {
                ThrowArgumentException(nameof(organizationUid), organizationUid);
            }

            if (projectUid.IsEmptyGuid())
            {
                ThrowArgumentException(nameof(projectUid), projectUid);
            }

            OrganizationUid = organizationUid;
            ProjectUid = projectUid;
            Labels = labels;
        }
    }
}