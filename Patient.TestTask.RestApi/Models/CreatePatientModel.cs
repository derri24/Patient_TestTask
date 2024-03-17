using Patient.TestTask.RestApi.Entities;

namespace Patient.TestTask.RestApi.Models;

public class CreatePatientModel
{
    public CreatePatientNameModel Name { get; set; }
    public string Gender { get; set; }
    public DateTime BirthDate { get; set; }
    public bool Active { get; set; }
}

public class CreatePatientNameModel
{
    public string Use { get; set; }
    public string Family { get; set; }
    public List<string> Given { get; set; }
}