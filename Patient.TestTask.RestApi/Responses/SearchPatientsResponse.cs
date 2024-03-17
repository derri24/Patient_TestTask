using Patient.TestTask.RestApi.Models;

namespace Patient.TestTask.RestApi.Responses;

public class SearchPatientsResponse
{
    public List<SearchPatientItemModel> Patient { get; set; }
}

