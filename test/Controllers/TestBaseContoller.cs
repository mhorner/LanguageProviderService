using System.Net;
using Core;
using Core.Interfaces;
using Data;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Tests.Controllers;

public class UnitOfWorkTest
{
    protected int HTTP_OK = (int)HttpStatusCode.OK;
    protected int HTTP_BADREQUEST = (int)HttpStatusCode.BadRequest;
    protected int HTTP_NOTFOUND = (int)HttpStatusCode.NotFound; 
    protected Data.UnitOfWork CreateUnitOfWork()
    {
        var builder = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        var dataContext = new DataContext(builder.Options);
        return new Data.UnitOfWork(dataContext);
    }
    
    protected static int? Cast(object result, out Message message)
    {
        message = null;
        if (result is OkObjectResult objectResult)
        {
            message = (Message) objectResult.Value;
        }
        return GetReturnStatusCode(result);
    }
    protected static int? Cast(object result, out IList<Message> message)
    {
        message = null;
        if (result is OkObjectResult objectResult)
        {
            message = ((IQueryable<Message>) objectResult.Value).ToList();
        }
        return GetReturnStatusCode(result);
    }

    private static int? GetReturnStatusCode(object result)
    {
        return result switch
        {
            OkObjectResult okObjectResult => okObjectResult.StatusCode,
            BadRequestObjectResult badRequestObjectResult => badRequestObjectResult.StatusCode,
            NotFoundResult notFoundResult => notFoundResult.StatusCode,
            _ => throw new Exception($"Unexpected result type returned, {result.GetType()}.")
        };
    }
}