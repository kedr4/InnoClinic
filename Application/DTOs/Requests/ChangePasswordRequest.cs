using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Requests;

public class ChangePasswordRequest
{
    public string OldPassword { get; init; }
    public string NewPassword { get; init; }

    public ChangePasswordRequest(string oldPassword, string newPassword)
    {
        OldPassword = oldPassword;
        NewPassword = newPassword;
    }
}
