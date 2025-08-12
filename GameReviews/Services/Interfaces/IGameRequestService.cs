using GameReviews.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

public interface IGameRequestService
{
    Task SubmitRequestAsync(GameRequest request);
    Task<Dictionary<string, SelectList>> GetDropdownsAsync();
    Task<IEnumerable<GameRequest>> GetAllRequestsAsync();
    Task<GameRequest> GetRequestByIdAsync(int id);
    Task UpdateRequestStatusAsync(int id, string status, string adminResponse);
    Task<List<GameRequest>> GetAllRequestsWithUsersAsync();


}

