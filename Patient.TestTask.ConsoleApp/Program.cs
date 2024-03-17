using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Patient.TestTask.ConsoleApp;
using Patient.TestTask.ConsoleApp.Models;

async Task InsertManyPatientsIntoDb(List<PatientModel> patients)
{
    
    var httpClient = new HttpClient();
    var createManyPatientsModel = new CreateManyPatientsModel();
    createManyPatientsModel.Patients = patients;
    
   var response = await httpClient.PostAsJsonAsync("http://patient.testtask.rest.api:80/Patient/CreateMany", createManyPatientsModel);
}

var patients = PatientGenerator.Generate();
await InsertManyPatientsIntoDb(patients);
Console.WriteLine("Рожденные дети созданны успешно.");