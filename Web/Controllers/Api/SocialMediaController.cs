﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Models;
using Web.Models.Domain;

namespace Web.Controllers.Api
{
    [Route("api/[Controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Cors.EnableCors("PlanVotePolicy")]
    public class SocialMediaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SocialMediaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/SocialMediaAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SocialMedia>>> GetSocialMedia()
        {
            var socialMedia = await _context
                .SocialMedia
                .Select(socialMedia => new
                {
                    socialMedia.MediaName,
                    socialMedia.Message,
                    socialMedia.Link,
                })
                .ToListAsync();

            return Ok(new
            {
                socialMedia,
            });
        }

        // GET: api/SocialMediaAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SocialMedia>> GetSocialMedia(int id)
        {
            var socialMedia = await _context.SocialMedia.FindAsync(id);

            if (socialMedia == null)
            {
                return NotFound();
            }

            return socialMedia;
        }

        // PUT: api/SocialMediaAPI/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSocialMedia(int id, SocialMedia socialMedia)
        {
            if (id != socialMedia.ID)
            {
                return BadRequest();
            }

            _context.Entry(socialMedia).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SocialMediaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/SocialMediaAPI
        [HttpPost]
        public async Task<ActionResult<SocialMedia>> PostSocialMedia(SocialMedia socialMedia)
        {
            _context.SocialMedia.Add(socialMedia);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSocialMedia", new { id = socialMedia.ID }, socialMedia);
        }

        // DELETE: api/SocialMediaAPI/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SocialMedia>> DeleteSocialMedia(int id)
        {
            var socialMedia = await _context.SocialMedia.FindAsync(id);
            if (socialMedia == null)
            {
                return NotFound();
            }

            _context.SocialMedia.Remove(socialMedia);
            await _context.SaveChangesAsync();

            return socialMedia;
        }

        private bool SocialMediaExists(int id)
        {
            return _context.SocialMedia.Any(e => e.ID == id);
        }
    }
}
