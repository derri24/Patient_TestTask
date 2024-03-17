using Patient.TestTask.RestApi.Entities;

namespace Patient.TestTask.RestApi.Models;

public class SearchPatientItemModel
{
    public SearchPatientItemNameModel Name { get; set; }
    public string Gender { get; set; }
    public DateTime BirthDate { get; set; }
    public bool Active { get; set; }
}

public class SearchPatientItemNameModel
{
    public Guid Id { get; set; }
    public string Use { get; set; }
    public string Family { get; set; }
    public List<string> Given { get; set; }
}