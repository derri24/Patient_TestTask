using System.Globalization;
using System.Text.RegularExpressions;
using MongoDB.Driver;
using Patient.TestTask.RestApi.Configuration;
using Patient.TestTask.RestApi.Entities;
using Patient.TestTask.RestApi.Models;
using Patient.TestTask.RestApi.Responses;
using Patient.TestTask.RestApi.Services.Interfaces;
using Prakle.ManagementWarehouse.Api.Base.DataAccess;

namespace Patient.TestTask.RestApi.Services;

public class PatientService : IPatientService
{
    private IBaseRepository<Entities.Patient> _patientRepository;
    private const int NearDaysCount = 2;

    public PatientService(IBaseRepository<Entities.Patient> patientRepository)
    {
        _patientRepository = patientRepository;
    }

    private Gender ParseGender(string gender)
    {
        gender = gender.ToLower();
        if (gender == "male")
            return Gender.Male;
        if (gender == "female")
            return Gender.Female;
        if (gender == "other")
            return Gender.Other;
        return Gender.Unknown;
    }


    private List<FilterDefinition<Entities.Patient>> CreateDateFilters(List<string> dates)
    {
        var filters = new List<FilterDefinition<Entities.Patient>>();
        var filterBuilder = _patientRepository.FilterDefinitionBuilder;
        foreach (var date in dates)
        {
            var dateString = date.Remove(0, 2);
            var filterName = date.Remove(2, dateString.Length );

            DateTime.TryParse(dateString, out var dateTime);
            
            switch (filterName)
            {
                case "ne":
                {
                    var newDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0,
                        DateTimeKind.Utc);
                    var afterDayDateTime1 = newDateTime.AddDays(1);
                    var beforeDayDateTime1 = newDateTime.AddDays(-1);
                    filters.Add(filterBuilder.Or(
                        filterBuilder.Lte(x => x.BirthDate, beforeDayDateTime1),
                        filterBuilder.Gte(x => x.BirthDate, afterDayDateTime1)
                    ));
                    break;
                }
                case "eq":
                {
                    var newDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0,
                        DateTimeKind.Utc);
                    var afterDayDateTime = newDateTime.AddDays(1);
                    filters.Add(filterBuilder.And(
                        filterBuilder.Gte(x => x.BirthDate, newDateTime),
                        filterBuilder.Lt(x => x.BirthDate, afterDayDateTime)
                    ));
                    break;
                }
                case "lt":
                {
                    var newDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour,
                        dateTime.Minute, dateTime.Second, DateTimeKind.Utc);
                    filters.Add(filterBuilder.Lt(x => x.BirthDate, newDateTime));
                    break;
                }
                case "gt":
                {
                    var newDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour,
                        dateTime.Minute, dateTime.Second, DateTimeKind.Utc);
                    filters.Add(filterBuilder.Gt(x => x.BirthDate, newDateTime));
                    break;
                }
                case "le":
                {
                    var newDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour,
                        dateTime.Minute, dateTime.Second, DateTimeKind.Utc);
                    filters.Add(filterBuilder.Lte(x => x.BirthDate, newDateTime));
                    break;
                }
                case "ge":
                {
                    var newDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour,
                        dateTime.Minute, dateTime.Second, DateTimeKind.Utc);
                    filters.Add(filterBuilder.Gte(x => x.BirthDate, newDateTime));
                    break;
                }
                case "sa":
                {
                    var newDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0,
                        DateTimeKind.Utc);
                    newDateTime = newDateTime.AddDays(1);
                    filters.Add(filterBuilder.Gte(x => x.BirthDate, newDateTime));
                    break;
                }
                case "eb":
                {
                    var newDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0,
                        DateTimeKind.Utc);
                    filters.Add(filterBuilder.Lt(x => x.BirthDate, newDateTime));
                    break;
                }
                case "ap":
                {
                    var newDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0,
                        DateTimeKind.Utc);
                    var afterDayDateTime = newDateTime.AddDays(NearDaysCount);
                    var beforeDayDateTime = newDateTime.AddDays(-NearDaysCount);
                    filters.Add(filterBuilder.And(
                        filterBuilder.Gte(x => x.BirthDate, beforeDayDateTime),
                        filterBuilder.Lt(x => x.BirthDate, afterDayDateTime)
                    ));
                    break;
                }
            }
        }

        return filters;
    }

    private List<SearchPatientItemModel> ConvertEntitiesToModels(List<Entities.Patient> patients)
    {
        var patientsModel = new List<SearchPatientItemModel>();
        foreach (var patient in patients)
        {
            var patientModel = new SearchPatientItemModel()
            {
                Name = new SearchPatientItemNameModel()
                {
                    Family = patient.Name.Family,
                    Given = patient.Name.Given,
                    Use = patient.Name.Use,
                    Id = patient.Id
                },
                Active = patient.Active,
                Gender = patient.Gender.ToString().ToLower(),
                BirthDate = patient.BirthDate
            };
            patientsModel.Add(patientModel);
        }

        return patientsModel;
    }

    public async Task<SearchPatientsResponse> Search(List<string> dates)
    {
        var response = new SearchPatientsResponse();
        if (dates is null || dates.Count == 0)
        {
            var patients = (await _patientRepository.FilterByAsync(x => true)).OrderBy(x=>x.BirthDate).ToList();
            response.Patient = ConvertEntitiesToModels(patients);
            return response;
        }

        var filters = CreateDateFilters(dates);
        
        var patientsByFilter = (await _patientRepository.FilterByAsync(filters)).OrderBy(x=>x.BirthDate).ToList();
        response.Patient = ConvertEntitiesToModels(patientsByFilter);
        return response;
    }

    public async Task<GetPatientByIdResponse> GetPatientById(Guid id)
    {
        var response = new GetPatientByIdResponse();

        var patient = await _patientRepository.FindByIdAsync(id);

        var patientModel = new GetPatientByIdModel()
        {
            Name = new GetPatientByIdNameModel()
            {
                Id = patient.Id,
                Family = patient.Name.Family,
                Given = patient.Name.Given,
                Use = patient.Name.Use,
            },
            Active = patient.Active,
            Gender = patient.Gender.ToString().ToLower(),
            BirthDate = patient.BirthDate,
        };
        response.Patient = patientModel;
        return response;
    }

    public async Task<PatientStatusCode> Create(CreatePatientModel model)
    {
        if (model is null || model.Name is null ||
            string.IsNullOrEmpty(model.Name.Family) || model.BirthDate == default ||
            model.Name.Given == null || model.Name.Given.Count != 2)
            return PatientStatusCode.InvalidRequest;

        var patient = new Entities.Patient()
        {
            Name = new Name()
            {
                Family = model.Name.Family,
                Given = model.Name.Given,
                Use = model.Name.Use,
            },
            Active = model.Active,
            Gender = ParseGender(model.Gender),
            BirthDate = model.BirthDate,
        };

        await _patientRepository.InsertOneAsync(patient);
        return PatientStatusCode.Ok;
    }

    public async Task<PatientStatusCode> CreateMany(CreateManyPatientsModel model)
    {
        if (model is null || model.Patients is null)
            return PatientStatusCode.InvalidRequest;

        var patients = new List<Entities.Patient>();
        foreach (var patientModel in model.Patients)
        {
            if (patientModel is null || patientModel.Name is null || string.IsNullOrEmpty(patientModel.Name.Family) ||
                patientModel.BirthDate == default || patientModel.Name.Given == null ||
                patientModel.Name.Given.Count != 2)
                return PatientStatusCode.InvalidRequest;

            var patient = new Entities.Patient()
            {
                Name = new Name()
                {
                    Family = patientModel.Name.Family,
                    Given = patientModel.Name.Given,
                    Use = patientModel.Name.Use,
                },
                Active = patientModel.Active,
                Gender = ParseGender(patientModel.Gender),
                BirthDate = patientModel.BirthDate,
            };
            patients.Add(patient);
        }

        await _patientRepository.InsertManyAsync(patients);
        return PatientStatusCode.Ok;
    }

    public async Task<PatientStatusCode> Update(UpdatePatientModel model)
    {
        if (model is null || model.Name is null || string.IsNullOrEmpty(model.Name.Family)
            || model.BirthDate == default || model.Name.Id == default
            || model.Name.Given == null || model.Name.Given.Count != 2)
            return PatientStatusCode.InvalidRequest;

        var patient = await _patientRepository.FindOneAsync(x => x.Id == model.Name.Id);

        if (patient is null)
            return PatientStatusCode.PatientNotFound;

        patient.Name = new Name()
        {
            Family = model.Name.Family,
            Given = model.Name.Given,
            Use = model.Name.Use,
        };
        patient.Active = model.Active;
        patient.Gender = ParseGender(model.Gender);
        patient.BirthDate = model.BirthDate;

        await _patientRepository.ReplaceOneAsync(patient);
        return PatientStatusCode.Ok;
    }

    public async Task<PatientStatusCode> Delete(Guid id)
    {
        if (id == default)
            return PatientStatusCode.InvalidRequest;

        var patient = await _patientRepository.FindOneAsync(x => x.Id == id);
        if (patient is null)
            return PatientStatusCode.PatientNotFound;

        await _patientRepository.HardDeleteOneAsync(x => x.Id == id);
        return PatientStatusCode.Ok;
    }
}

public enum PatientStatusCode
{
    Ok,
    InvalidRequest,
    PatientNotFound,
}