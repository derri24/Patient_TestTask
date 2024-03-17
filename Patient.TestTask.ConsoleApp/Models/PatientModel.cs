using System;
using System.Collections.Generic;

namespace Patient.TestTask.ConsoleApp.Models;


public class PatientModel 
{
    public NameModel Name { get; set; }
    public string Gender { get; set; }
    public DateTime BirthDate { get; set; }
    public bool Active { get; set; }
}

public class NameModel
{
    public string Use { get; set; }
    public string Family { get; set; }
    public List<string> Given { get; set; }
}
