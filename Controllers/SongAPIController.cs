using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MusicAPIwithoutDocker.Data;
using MusicAPIwithoutDocker.Models;
using MusicAPIwithoutDocker.Models.DTO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace MusicAPIwithoutDocker.Controllers
{
    [Route("api/SongAPI/[action]")]

    [ApiController]
    public class SongAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _appDbContext;
        public SongAPIController(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        // GET ALL SONGS LIST 
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult< IEnumerable<SongDTO>> GetSongs()
        {
            return Ok (_appDbContext.Songs.ToList());
        }

        // GET A SONG BASED ON ID 
        [HttpGet("{id:int}", Name ="GetSong")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<SongDTO> GetSong(int id )
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var song = _appDbContext.Songs.FirstOrDefault(u => u.Id == id);
            if (song == null)
            {
                return NotFound();
            }
            return Ok (song);
        }

        // CREATE A NEW SONG 
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<SongDTO> Create( [FromBody]SongDTO songDTO)
        {
            if (songDTO == null)
            {
                return BadRequest(songDTO);
            }
                     
            if (_appDbContext.Songs.FirstOrDefault( u => u.Name.ToLower() == songDTO.Name.ToLower())!= null)
            {
                ModelState.AddModelError("CustomNameError", "Song Name Already Exists");
                return BadRequest(ModelState);
            }
           Song model = new()
           {
               Id = songDTO.Id,
               Name = songDTO.Name,
               Artist = songDTO.Artist,
               Album = songDTO.Album,
               DateCreated = DateTime.Now
           };
            _appDbContext.Songs.Add(model);
            _appDbContext.SaveChanges();
            return CreatedAtRoute("GetSong", new { id = songDTO.Id }, songDTO);
        }

        [HttpDelete("{id:int}", Name = "DeleteSong")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete ( int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var song = _appDbContext.Songs.FirstOrDefault(u => u.Id == id);
            if (song == null)
            {
                return NotFound();
            }
            _appDbContext.Songs.Remove(song);
            _appDbContext.SaveChanges();
            return NoContent();
        }
        // UPDATE A SONG
        [HttpPut("{id:int}", Name = "UpdateSong")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]        
        public IActionResult UpdateSong (int id , [FromBody] SongDTO songDTO)
        {
            if ( songDTO == null || id != songDTO.Id)
            {
                return BadRequest();
            }
            Song model = new()
            {
                Id = songDTO.Id,
                Name = songDTO.Name,
                Artist = songDTO.Artist,
                Album = songDTO.Album,
                UpdatedBy = DateTime.Now
            };
            _appDbContext.Songs.Update(model);
            _appDbContext.SaveChanges();

            return NoContent();

        }
    }
}
