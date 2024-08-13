﻿using API.Models;
using API.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Services.RequestFriendshipService
{
    public class RequestFriendshipService : IRequestFriendshipService
    {
        private readonly AppDbContext _context;
        public RequestFriendshipService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<RequestUserDTO>>? GetAllReceivedRequests(int receiverID)
        {
            var temp = await _context.Users.FirstOrDefaultAsync(u => u.ID == receiverID);
            if (temp == null)
            {
                return null;
            }
            // Retrieve all friendship requests received by a specific user
            var requests = _context.Requests
                // Filter requests by the receiver's ID
                .Where(u => u.ReceiverID == receiverID)
                // Select only the SenderID and RequestedOn date for each request
                .Select(u => new { u.SenderID, u.RequestedOn })
                // Execute the query and convert the results to an array
                .ToArray();

            var senders = requests.Select(u => u.SenderID).ToArray();

            var users = _context.Users
                    .Where(u => senders.Contains(u.ID))
                    .Select(u => new { u.ID, u.Username });

            var result = from request in requests
                         join user in users on request.SenderID equals user.ID
                         select new RequestUserDTO
                         {
                             username = user.Username,
                             time = request.RequestedOn
                         };

            return result;
        }

        public async Task<IEnumerable<RequestUserDTO>>? GetAllSentRequests(int senderID)
        {
            var temp = await _context.Users.FirstOrDefaultAsync(u => u.ID == senderID);
            if (temp == null)
            {
                return null;
            }

            //get all requests
            var requests = _context.Requests
                .Where(u => u.SenderID == senderID)
                .Select(u => new { u.ReceiverID, u.RequestedOn })
                .ToArray();

            //select all receivers
            var receivers = requests.Select(u => u.ReceiverID);

            //select all users with receiverID
            var users = _context.Users
                .Where(u => receivers.Contains(u.ID))
                .Select(u => new { u.ID, u.Username });

            //now we join and return request user dto
            var result = from request in requests
                         join user in users on request.ReceiverID equals user.ID
                         select new RequestUserDTO
                         {
                             username = user.Username,
                             time = request.RequestedOn
                         };


            return result;
        }

        public async Task<string> RemoveRequest(int SenderID, int ReceiverID)
        {
            var request = await _context.Requests.FirstOrDefaultAsync(r => r.SenderID == SenderID && r.ReceiverID == ReceiverID);
            if (request == null)
            {
                return "request doesn't exist";
            }
            _context.Remove(request);
            await _context.SaveChangesAsync();

            return "success!";
        }


        public async Task<string> SendRequest(int SenderID, int ReceiverID)
        {
            var request = await _context.Requests.FirstOrDefaultAsync(r => r.SenderID == SenderID && r.ReceiverID == ReceiverID);
            if (request != null)
            {
                return "request already sent";
            }

            RequestedFriendship model = new RequestedFriendship
            {
                SenderID = SenderID,
                ReceiverID = ReceiverID,
                RequestedOn = DateTime.UtcNow
            };

            _context.Add(model);
            await _context.SaveChangesAsync();

            return "success!";
        }
    }
}
