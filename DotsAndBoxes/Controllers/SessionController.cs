using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotsAndBoxes.Common.CommonModels;
using DotsAndBoxes.Core.Models;
using DotsAndBoxes.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace DotsAndBoxes.Controllers
{
    //This controller is responsible for handling the "sessions"
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        //Each new game session, has a unique key that is used to map the client to the correct session
        private readonly IHubContext<GameHub> _gameHub;
        public SessionController(IHubContext<GameHub> hubContext)
        {

            _gameHub = hubContext;

        }
        //[HttpPost]
        //public async Task<IActionResult> Create([FromRoute]int dimension)
        //{

        //    await _gameHub.Clients.All.SendAsync("Create",);
          
        //}
        //When player draws a line we find the correct game session and validate the move accordingly
        //if all is well we return an ok status code with the new values
        //[HttpPost]
        //public async Task<IActionResult> Draw(Guid sessionKey,Line line)
        //{
            
            

        //}
    }
}