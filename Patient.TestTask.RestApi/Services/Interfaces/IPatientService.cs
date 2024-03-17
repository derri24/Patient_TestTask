using Patient.TestTask.RestApi.Models;
using Patient.TestTask.RestApi.Responses;

namespace Patient.TestTask.RestApi.Services.Interfaces;

public interface IPatientService
{
    Task<SearchPatientsResponse> Search(List<string> dates);
    Task<GetPatientByIdResponse> GetPatientById(Guid id);
    Task<PatientStatusCode> Create(CreatePatientModel model);
    Task<PatientStatusCode> CreateMany(CreateManyPatientsModel model);
    Task<PatientStatusCode> Update(UpdatePatientModel model);
    Task<PatientStatusCode> Delete(Guid id);
}