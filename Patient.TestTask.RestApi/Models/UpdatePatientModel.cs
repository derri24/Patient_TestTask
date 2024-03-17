using Patient.TestTask.RestApi.Entities;

namespace Patient.TestTask.RestApi.Models;

public class UpdatePatientModel
{
    public UpdatePatientNameModel Name { get; set; }
    public string Gender { get; set; }
    public DateTime BirthDate { get; set; }
    public bool Active { get; set; }
}

public class UpdatePatientNameModel
{
    public Guid Id { get; set; }
    public string Use { get; set; }
    public string Family { get; set; }
    public List<string> Given { get; set; }
}