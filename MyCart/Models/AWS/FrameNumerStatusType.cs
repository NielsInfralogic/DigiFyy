﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DigiFyy.Models.AWS
{
    public enum FrameNumberStatusType { UIDNotRegistered = -1, NotRegistered = 0, Registered = 1, ReportedStolen = 2, Found = 3, Unknown = 9 };


    public enum OwnerRegisterErrorType { UIDRegisterToOtherUser = -1, UIDDoesNotExist = -2, UIDReportedStolen= -3, }
}
