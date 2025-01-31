﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TheCodeCamp.Data;
using TheCodeCamp.Models;

namespace TheCodeCamp.Controllers
{
    [RoutePrefix("api/camps")]
    public class CampsController : ApiController
    {
        private readonly ICampRepository _repository;
        private readonly IMapper _mapper;
        public CampsController(ICampRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [Route()]
        public async Task<IHttpActionResult> Get(bool includeTalks = false)
        {
            try
            {
                var result = await _repository.GetAllCampsAsync(includeTalks);
                
                // Mapping
                var mappedResult = _mapper.Map<IEnumerable<CampModel>>(result);

                return Ok(mappedResult);
            }
            catch
            {
                //TODO: Add logging
                return InternalServerError();
            }
        }

        [Route("{moniker}")]
        public async Task<IHttpActionResult> Get(string moniker, bool includeTalks = false)
        {
            try
            {
                var result = await _repository.GetCampAsync(moniker, includeTalks);
                if (result == null) return NotFound();

                var mappedResult = _mapper.Map<CampModel>(result);

                return Ok(mappedResult);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}