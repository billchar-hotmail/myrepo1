using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace Angelo.Aegis.Api
{
    /// <summary>
    /// Api Base Controller 
    /// </summary>
    public class ApiController : Controller
    {
        /// <summary>
        /// Overload of "Create" action result that generates the resource path based on convention.
        /// (eg. POST new item to /api/items should result in an entry /api/items/{id})
        /// </summary>
        /// <param name="id">The Id of the newly created resource</param>
        /// <returns></returns>
        protected CreatedResult Created(object id)
        {
            return new CreatedResult(Request.Path + "/" + Convert.ToString(id), id);
        }

        /// <summary>
        /// Overload of "Bad Request" that accepts and formats Identity Errors. 
        /// </summary>
        /// <param name="errors">IdentityError Collection</param>
        /// <returns></returns>
        protected BadRequestObjectResult BadRequest(IEnumerable<IdentityError> errors)
        {
            return new BadRequestObjectResult(errors.Select(x => x.Description));
        }
    }
}
