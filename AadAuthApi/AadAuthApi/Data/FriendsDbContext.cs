using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AadAuthApi.Data
{
    public class FriendsDbContext : DbContext
    {
        public FriendsDbContext(DbContextOptions<FriendsDbContext> options)
        : base(options)
        { }

        public DbSet<Friend> Friends { get; set; }

        public async Task<Friend> AddFriend(Friend newFriend)
        {
            Friends.Add(newFriend);
            await SaveChangesAsync();
            return newFriend;
        }

        public async Task RemoveFriend(string aadId)
        {
            Friends.Remove(Friends.FirstOrDefault(f => f.AADID == aadId));
            await SaveChangesAsync();
        }

        public async Task DeleteEverything()
        {
            using var transaction = await Database.BeginTransactionAsync();
            await Database.ExecuteSqlRawAsync("DELETE FROM Friends");
            await transaction.CommitAsync();
        }
    }
}
