﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Responses;

public class CreateDoctorResponse
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public string? DoctorId { get; set; }
}

}
