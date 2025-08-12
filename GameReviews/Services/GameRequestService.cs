using GameReviews.Data;
using GameReviews.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class GameRequestService : IGameRequestService
{
    private readonly ApplicationDbContext _context;

    public GameRequestService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task SubmitRequestAsync(GameRequest request)
    {
        _context.GameRequests.Add(request);
        await _context.SaveChangesAsync();
    }

    public async Task<Dictionary<string, SelectList>> GetDropdownsAsync()
    {
        var genres = await _context.Genres
            .Select(g => new { g.Id, g.Name })
            .ToListAsync();

        var platforms = await _context.Platforms
            .Select(p => new { p.Id, p.Name })
            .ToListAsync();

        return new Dictionary<string, SelectList>
        {
            { "Genres", new SelectList(genres, "Id", "Name") },
            { "Platforms", new SelectList(platforms, "Id", "Name") }
        };
    }
    
    public async Task<IEnumerable<GameRequest>> GetAllRequestsAsync()
    {
        return await _context.GameRequests.Include(r => r.Genre).Include(r => r.Platform).ToListAsync();
    }

    public async Task<GameRequest> GetRequestByIdAsync(int id)
    {
        return await _context.GameRequests
            .Include(r => r.RequestedByUser)   
            .Include(r => r.Genre)             
            .Include(r => r.Platform)          
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task UpdateRequestStatusAsync(int id, string status, string adminResponse)
    {
        var request = await _context.GameRequests.FindAsync(id);
        if (request == null) return;

        request.Status = status;
        request.AdminResponse = adminResponse;
        request.ResponseDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }
    public async Task<List<GameRequest>> GetAllRequestsWithUsersAsync()
    {
        return await _context.GameRequests
            .Include(r => r.RequestedByUser) 
            .ToListAsync();
    }


}

