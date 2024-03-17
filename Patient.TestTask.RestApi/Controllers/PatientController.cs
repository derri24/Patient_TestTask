using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Patient.TestTask.RestApi.Models;
using Patient.TestTask.RestApi.Responses;
using Patient.TestTask.RestApi.Services;
using Patient.TestTask.RestApi.Services.Interfaces;

namespace Patient.TestTask.RestApi.Controllers;

[Route($"[controller]/[action]")]
[Tags("Рожденные в роддоме дети")]

public class PatientController : Controller
{
    private readonly IPatientService _patientService;

    public PatientController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    /// <summary>
    /// Поиск рожденных детей по дате рождения.
    /// </summary>
    /// <response code="200">Ok</response>
    /// <response code="500">Internal Server Error</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    
    [HttpGet]
    public async Task<SearchPatientsResponse> Search([FromQuery]List<string> dates)
    {
        return await _patientService.Search(dates);
    }
    
    /// <summary>
    /// Получение рожденного ребенка по идентификатору.
    /// </summary>
    /// <response code="200">Ok</response>
    /// <response code="500">Internal Server Error</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    
    [HttpGet]
    public async Task<GetPatientByIdResponse> GetPatientById(Guid id)
    {
        return await _patientService.GetPatientById(id);
    }

    /// <summary>
    /// Создание рожденного ребенка.
    /// </summary>
    /// <response code="200">Ok</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">Internal Server Error</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    
    [HttpPost]
    public async Task<StatusCodeResult> Create([FromBody, BindRequired] CreatePatientModel model)
    {
        var result = await _patientService.Create(model);
        if (result == PatientStatusCode.InvalidRequest)
            return StatusCode(400);
        return StatusCode(200); 
    }
    
    /// <summary>
    /// Создание нескольких рожденных детей.
    /// </summary>
    /// <response code="200">Ok</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">Internal Server Error</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    
    [HttpPost]
    public async Task<StatusCodeResult> CreateMany([FromBody, BindRequired] CreateManyPatientsModel model)
    {
        var result = await _patientService.CreateMany(model);
        if (result == PatientStatusCode.InvalidRequest)
            return StatusCode(400);
        return StatusCode(200); 
    }

  
    /// <summary>
    /// Обновление рожденного ребенка.
    /// </summary>
    /// <response code="200">Ok</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">Internal Server Error</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    
    [HttpPut]
    public async Task<StatusCodeResult> Update([FromBody, BindRequired] UpdatePatientModel model)
    {
        var result = await _patientService.Update(model);
        if (result == PatientStatusCode.InvalidRequest || result == PatientStatusCode.PatientNotFound)
            return StatusCode(400);
        return StatusCode(200); 
    }
    
    /// <summary>
    /// Удаление рожденного ребенка.
    /// </summary>
    /// <response code="200">Ok</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">Internal Server Error</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    
    [HttpDelete]
    public async Task<StatusCodeResult> Delete(Guid id)
    {
        var result =await _patientService.Delete(id);
        if (result == PatientStatusCode.PatientNotFound)
            return StatusCode(400);
        return StatusCode(200);
    }
}