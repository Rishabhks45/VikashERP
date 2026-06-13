using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace VikashERP.SharedKernel.Enums;

//Employee Attendance PRESENT', 'ABSENT', 'HALF_DAY', 'LEAVE' Enum

public enum EmployeeAttendance
{
    [Description("Present")]
    PRESENT = 1,
    [Description("Absent")]
    ABSENT = 2,
    [Description("Half Day")]
    HALF_DAY = 3,
    [Description("Leave")]
    LEAVE = 4
}


